using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Neo4jClient;
using Neo4jClient.Cypher;
using PolyglotHeaven.Contracts.Types;
using PolyglotHeaven.Helpers;

namespace PolyglotHeaven.Web.Api
{
    [RoutePrefix("api/recommendation")]
    public class RecommendationsController : ApiController
    {
        private GraphClient _neo4jClient;

        public RecommendationsController()
        {
            _neo4jClient = Neo4jClientBuilder.Build();
        }

        [Route]
        //public HttpResponseMessage Get(List<OrderItem> items)
        public HttpResponseMessage Get(Guid? productId)
        {
            //            MATCH(n: Product) < -[:INCLUDES] - (o:Order),
            //	(o) -[:INCLUDES]->(n2: Product)
            //WHERE n.Id IN["d2add22c-f077-419d-b2eb-44f08307087f", "2a114455-4627-4b3f-b154-bf0c029abc71"] AND
            //   NOT(n2.Id IN["d2add22c-f077-419d-b2eb-44f08307087f", "2a114455-4627-4b3f-b154-bf0c029abc71"])
            //RETURN n2, count(*)

            var queryResult = _neo4jClient.Cypher
                .Match("(p1:Product)<-[:INCLUDES]-(o:Order), (o)-[:INCLUDES]->(p2:Product)")
                .Where("p1.Id IN {Ids} AND NOT(p2.Id IN {Ids})")
                .WithParams(new
                {
                    Ids = new List<Guid> { productId.Value }
                })
                .Return((p2, cnt) => new {
                    Product = Return.As<Service.Documents.Product>("p2"),
                    Count = Return.As<int>("cnt"),
                })
                .Results;


            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Neo4jClient;
using Neo4jClient.Cypher;
using PolyglotHeaven.Helpers;
using PolyglotHeaven.Service.Documents;

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
        [HttpPost]
        public HttpResponseMessage Get(RecommendationQuery query)
        {
            var queryResult = _neo4jClient.Cypher
                .Match("(p1:Product)--(o:Order)--(c:Customer)")
                .Match("(c)--(o2:Order)--(p2:Product)")
                .Where("p1.Id IN {Ids} AND NOT(p2.Id IN {Ids})")
                .WithParams(new
                {
                    Ids = query.ProductIds
                })
                .Match("(p2)--(o3:Order)")
                .Return((p2, o3) => new RecommendationResult
                {
                    Product = Return.As<Product>("p2"),
                    Cnt = Return.As<int>("COUNT(DISTINCT o3)"),
                })
                .OrderByDescending("Cnt");

            var x = queryResult.Results.ToList();
            return Request.CreateResponse(HttpStatusCode.OK, x);
        }
    }

    public class RecommendationQuery
    {
        public List<Guid> ProductIds { get; set; }
    }

    public class RecommendationResult
    {
        public Product Product { get; set; }
        public int Cnt { get; set; }
    }
}
﻿using System.Net.Http;
using System.Web.Http;
using Linky;
using Nest;
using PolyglotHeaven.Contracts.Commands;
using PolyglotHeaven.Helpers;

namespace PolyglotHeaven.Web.Api
{
    [RoutePrefix("api/customer")]
    public class CustomerController : BasePostEndpoint<CreateCustomer>
    {
        private IElasticClient _esClient;

        public CustomerController()
        {
            _esClient = ElasticClientBuilder.BuildClient();
        }

        [Route]
        [HttpGet]
        public HttpResponseMessage Get(string query = null)
        {
            var searchResult = _esClient.Search<Service.Documents.Customer>(sd => sd
                .Query(qd => qd
                    .Match(mqd => mqd.OnField(p => p.Name).Query(query))).Take(10));

            return Request.CreateResponse(searchResult.Documents);
        }

        [Route]
        [LinksFrom(typeof(IndexModel), "createcustomer")]
        public override HttpResponseMessage Post(CreateCustomer command)
        {
            return Execute(command);
        }
    }
}
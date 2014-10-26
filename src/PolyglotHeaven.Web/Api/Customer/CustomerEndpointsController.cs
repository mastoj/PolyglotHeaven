using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using PolyglotHeaven.Contracts.Commands;
using Linky;

namespace PolyglotHeaven.Web.Api.Customer
{
    [RoutePrefix("api/customer")]
    public class CreateCustomerController : BasePostEndpoint<CreateCustomer>
    {
        [Route]
        [HttpGet]
        public HttpResponseMessage Get()
        {
            return Request.CreateResponse(new List<Service.Documents.Customer>
            {
                new Service.Documents.Customer()
                {
                    Id = Guid.NewGuid(),
                    Name = "Tomas"
                },
                new Service.Documents.Customer()
                {
                    Id = Guid.NewGuid(),
                    Name = "Tomas"
                },
                new Service.Documents.Customer()
                {
                    Id = Guid.NewGuid(),
                    Name = "Tomas"
                },
                new Service.Documents.Customer()
                {
                    Id = Guid.NewGuid(),
                    Name = "Tomas"
                },
                new Service.Documents.Customer()
                {
                    Id = Guid.NewGuid(),
                    Name = "Tomas"
                }
            });
        }

        [Route]
        [LinksFrom(typeof(IndexModel), "createcustomer")]
        public override HttpResponseMessage Post(CreateCustomer command)
        {
            return Execute(command);
        }
    }
}
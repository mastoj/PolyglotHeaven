using System.Net.Http;
using System.Web.Http;
using PolyglotHeaven.Contracts.Commands;
using Linky;

namespace PolyglotHeaven.Web.Api.Customer
{
    [RoutePrefix("api/customer")]
    public class CreateCustomerEndpointController : BasePostEndpoint<CreateCustomer>
    {
        [Route]
        [LinksFrom(typeof(IndexModel), "createcustomer")]
        public override HttpResponseMessage Post(CreateCustomer command)
        {
            return Execute(command);
        }
    }
}
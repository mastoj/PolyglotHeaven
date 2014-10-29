using System.Net.Http;
using System.Web.Http;
using Linky;
using PolyglotHeaven.Contracts.Commands;

namespace PolyglotHeaven.Web.Api
{
    [RoutePrefix("api/order")]
    public class OrderEndpointController : BasePostEndpoint<PlaceOrder>
    {
        [Route]
        [LinksFrom(typeof(IndexModel), "placeorder")]
        public override HttpResponseMessage Post(PlaceOrder command)
        {
            return Execute(command);
        }
    }
}
using System.Net.Http;
using System.Web.Http;
using PolyglotHeaven.Contracts.Commands;
using Linky;

namespace PolyglotHeaven.Web.Api.Product
{
    [RoutePrefix("api/product")]
    public class CreateProductEndpointController : BasePostEndpoint<CreateProduct>
    {
        [Route]
        [LinksFrom(typeof(IndexModel), "createproduct")]
        public override HttpResponseMessage Post(CreateProduct command)
        {
            return Execute(command);
        }
    }
}
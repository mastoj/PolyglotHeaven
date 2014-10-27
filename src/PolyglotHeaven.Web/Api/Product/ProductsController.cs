using System.Net.Http;
using System.Web.Http;
using Nest;
using PolyglotHeaven.Contracts.Commands;
using Linky;
using PolyglotHeaven.Helpers;

namespace PolyglotHeaven.Web.Api.Product
{
    [RoutePrefix("api/product")]
    public class ProductsController : BasePostEndpoint<CreateProduct>
    {
        private IElasticClient _esClient;

        public ProductsController()
        {
            _esClient = ElasticClientBuilder.BuildClient();
        }

        [Route]
        [HttpGet]
        public HttpResponseMessage Get(string query = null)
        {
            var searchResult = _esClient.Search<Service.Documents.Product>(sd => sd.QueryString(query).Take(10));

            return Request.CreateResponse(searchResult.Documents);
        }

        [Route]
        [LinksFrom(typeof(IndexModel), "createproduct")]
        public override HttpResponseMessage Post(CreateProduct command)
        {
            return Execute(command);
        }
    }
}
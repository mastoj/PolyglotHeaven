using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Linky;

namespace PolyglotHeaven.Web.Api
{
    [RoutePrefix("api")]
    public class HomeController : ApiController
    {
        [Route]
        public HttpResponseMessage Get()
        {
            return Request.CreateResponse(HttpStatusCode.OK,
                new IndexModel
            {
                WelcomeMessage = "Hello to PolyglotHeaven"
            });
        }
    }

    public class IndexModel
    {
        public string WelcomeMessage { get; set; }
    }
}
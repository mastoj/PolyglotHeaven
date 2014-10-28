using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PolyglotHeaven.Domain;
using PolyglotHeaven.Infrastructure;

namespace PolyglotHeaven.Web.Api
{
    [RoutePrefix("api/commands")]
    public abstract class BasePostEndpoint<TCommand> : ApiController where TCommand : ICommand
    {
        public abstract HttpResponseMessage Post(TCommand command);

        private DomainEntry _domainEntry;

        private DomainEntry DomainEntry
        {
            get
            {
                _domainEntry = _domainEntry ?? ApplicationConfiguration.CreateDomainEntry();
                return _domainEntry;
            }
        }

        public HttpResponseMessage Execute(TCommand command)
        {
            try
            {
                DomainEntry.ExecuteCommand(command);
                return Request.CreateResponse(HttpStatusCode.Accepted, command);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
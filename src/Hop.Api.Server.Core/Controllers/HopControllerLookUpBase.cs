using Hop.Framework.Domain.Dispatcher;
using Hop.Framework.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Hop.Api.Server.Core.Controllers
{
    public class HopControllerLookUpBase<TController, TLookupFilter> : HopAuthorizedControllerBase<TController>
    {
        private readonly ILookUpService<TLookupFilter> _service;

        public HopControllerLookUpBase(ILookUpService<TLookupFilter> service,
            ILogger<TController> logProvider, IDispatcher dispatcher) : base(logProvider, dispatcher)
        {
            _service = service;
        }

        [Route("lookup")]
        public IActionResult Lookup(TLookupFilter filter)
        {
            return this.ReturnResponseFromResult(_service.LookupFilter(filter));
        }
    }
}

using Hop.Api.Server.Core.Dispatcher;
using Hop.Framework.Core.Log;
using Hop.Framework.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Hop.Api.Server.Core.Controllers
{
    public class HopControllerLookUpBase<TLookupFilter> : HopAuthorizedControllerBase
    {
        private readonly ILookUpService<TLookupFilter> _service;

        public HopControllerLookUpBase(ILookUpService<TLookupFilter> service,
            ILogProvider logProvider, IDispatcher dispatcher) : base(logProvider, dispatcher)
        {
            _service = service;
        }

        [Route("lookup")]
        public IActionResult Lookup(TLookupFilter filter)
        {
            return this.ReturnResponseFromResult(_service.LookupFilter(filter));
        }
    }

    public class HopControllerLookUpBaseAsync<TLookupFilter> : HopAuthorizedControllerBase
    {
        private readonly ILookUpServiceAsync<TLookupFilter> _service;

        public HopControllerLookUpBaseAsync(ILookUpServiceAsync<TLookupFilter> service,
            ILogProvider logProvider, IDispatcher dispatcher) : base(logProvider, dispatcher)
        {
            _service = service;
        }

        [Route("lookup")]
        [HttpGet]
        public async Task<IActionResult> Lookup(TLookupFilter filter)
        {
            return this.ReturnResponseFromResult(await _service.LookupFilter(filter));
        }
    }
}

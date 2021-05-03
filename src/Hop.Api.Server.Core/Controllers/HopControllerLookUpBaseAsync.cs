using Hop.Framework.Domain.Dispatcher;
using Hop.Framework.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Hop.Api.Server.Core.Controllers
{
    public class HopControllerLookUpBaseAsync<TController, TLookupFilter> : HopAuthorizedControllerBase<TController>
    {
        private readonly ILookUpServiceAsync<TLookupFilter> _service;

        public HopControllerLookUpBaseAsync(ILookUpServiceAsync<TLookupFilter> service,
            ILogger<TController> logger, IDispatcher dispatcher) : base(logger, dispatcher)
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

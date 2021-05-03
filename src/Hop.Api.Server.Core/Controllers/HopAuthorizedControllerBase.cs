using Hop.Framework.Domain.Dispatcher;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace Hop.Api.Server.Core.Controllers
{
    [Authorize]
    public class HopAuthorizedControllerBase<TController> : HopControllerBase<TController>
    {
        public HopAuthorizedControllerBase(ILogger<TController> logger, IDispatcher dispatcher)
            : base(logger, dispatcher)
        {
        }
    }
}

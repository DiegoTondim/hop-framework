using Hop.Api.Server.Core.Dispatcher;
using Hop.Framework.Core.Log;
using Microsoft.AspNetCore.Authorization;

namespace Hop.Api.Server.Core.Controllers
{
    [Authorize]
    public class HopAuthorizedControllerBase : HopControllerBase
    {
        public HopAuthorizedControllerBase(ILogProvider logProvider, IDispatcher dispatcher) : base(logProvider, dispatcher)
        {
        }
    }
}

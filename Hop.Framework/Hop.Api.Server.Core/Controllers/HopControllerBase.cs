using Hop.Api.Server.Core.Dispatcher;
using Hop.Framework.Core.Log;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hop.Api.Server.Core.Controllers
{
    [AllowAnonymous]
    public abstract class HopControllerBase : Controller
    {
        public ILogProvider LogProvider { get; }
        public IDispatcher Dispatcher { get; private set; }

        protected HopControllerBase(ILogProvider logProvider, IDispatcher dispatcher)
        {
            LogProvider = logProvider;
            Dispatcher = dispatcher;
        }
    }
}

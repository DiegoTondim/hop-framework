using Hop.Framework.Domain.Dispatcher;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Hop.Api.Server.Core.Controllers
{
    [AllowAnonymous]
    public abstract class HopControllerBase<TController> : Controller
    {
        public ILogger<TController> Logger { get; }
        public IDispatcher Dispatcher { get; private set; }

        protected HopControllerBase(ILogger<TController> logger, IDispatcher dispatcher)
        {
            Logger = logger;
            Dispatcher = dispatcher;
        }
    }
}

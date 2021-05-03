using Hop.Api.Server.Core.Controllers;
using Hop.Framework.Domain.Commands;
using Hop.Framework.Domain.Dispatcher;
using Hop.Net5WebApi.Application.Filters;
using Hop.Net5WebApi.Application.Services;
using Hop.Net5WebApi.Application.ViewModels;
using Hop.Net5WebApi.Domain.Commands;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace Hop.Net5WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : HopControllerCrudBaseWithLookUpAsync<PersonController, IPersonService,
        RegisterNewPersonCommand, UpdatePersonCommand, DeleteCommand<Guid>, Guid,
        PersonFilter, PersonReadViewModel, PersonFilter>
    {
        public PersonController(IPersonService service,
            ILogger<PersonController> logger,
            IDispatcher dispatcher)
            : base(service, logger, dispatcher)
        {
        }
    }
}

using Hop.Framework.Domain.Commands;
using Hop.Framework.Domain.Dispatcher;
using Hop.Framework.Domain.Repository;
using Hop.Framework.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Hop.Api.Server.Core.Controllers
{
    public class HopControllerCrudBaseWithLookUp<TController, TService, TNewCommand, TUpdateCommand, TRemoveCommand, TPrimaryKeyType, TFilter, TResponse, TPaginatedResponse, TLookUpFilter>
        : HopControllerLookUpBase<TController, TLookUpFilter>
        where TService : ICrudService<TNewCommand, TUpdateCommand, TRemoveCommand, TPrimaryKeyType, TFilter, TResponse, TPaginatedResponse>, ILookUpService<TLookUpFilter>
        where TNewCommand : CommandBase
        where TUpdateCommand : CommandBase
        where TRemoveCommand : DeleteCommand<TPrimaryKeyType>
        where TFilter : FilterBase
    {
        protected readonly TService Service;

        public HopControllerCrudBaseWithLookUp(TService service,
            ILogger<TController> logger, IDispatcher dispatcher) : base(service, logger, dispatcher)
        {
            Service = service;
        }

        [HttpPost]
        public IActionResult Post(TNewCommand command)
        {
            return this.ReturnResponseFromResult(Service.Create(command));
        }

        [HttpPut]
        public IActionResult Put(TUpdateCommand command)
        {
            return this.ReturnResponseFromResult(Service.Update(command));
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete(TPrimaryKeyType id)
        {
            return this.ReturnResponseFromResult(Service.Remove((TRemoveCommand)new DeleteCommand<TPrimaryKeyType>(id)));
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult Get(TPrimaryKeyType id)
        {
            return this.ReturnResponseFromResult(Service.GetById(id));
        }

        [HttpGet]
        public IActionResult Get([FromQuery] TFilter filter)
        {
            return this.ReturnResponseFromResultWithPaginatedData(Service.Filter(filter));
        }
    }
}

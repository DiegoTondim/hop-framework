using Hop.Api.Server.Core.Dispatcher;
using Hop.Framework.Core.Log;
using Hop.Framework.Domain.Commands;
using Hop.Framework.Domain.Repository;
using Hop.Framework.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Hop.Api.Server.Core.Controllers
{
    public class HopControllerCrudBase<TService, TNewCommand, TUpdateCommand, TRemoveCommand, TPrimaryKeyType, TFilter, TResponse, TPaginatedResponse>
        : HopAuthorizedControllerBase
        where TService : ICrudService<TNewCommand, TUpdateCommand, TRemoveCommand, TPrimaryKeyType, TFilter, TResponse, TPaginatedResponse>
        where TNewCommand : CommandBase
        where TUpdateCommand : CommandBase
        where TRemoveCommand : DeleteCommand<TPrimaryKeyType>
        where TFilter : FilterBase
    {
        protected readonly TService Service;

        public HopControllerCrudBase(TService service, ILogProvider logProvider, IDispatcher dispatcher) : base(logProvider, dispatcher)
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
        public IActionResult Get([FromQuery]TFilter filter)
        {
            return this.ReturnResponseFromResultWithPaginatedData(Service.Filter(filter));
        }
    }

    public class HopControllerCrudBaseWithLookUp<TService, TNewCommand, TUpdateCommand, TRemoveCommand, TPrimaryKeyType, TFilter, TResponse, TPaginatedResponse, TLookUpFilter>
        : HopControllerLookUpBase<TLookUpFilter>
        where TService : ICrudService<TNewCommand, TUpdateCommand, TRemoveCommand, TPrimaryKeyType, TFilter, TResponse, TPaginatedResponse>, ILookUpService<TLookUpFilter>
        where TNewCommand : CommandBase
        where TUpdateCommand : CommandBase
        where TRemoveCommand : DeleteCommand<TPrimaryKeyType>
        where TFilter : FilterBase
    {
        protected readonly TService Service;

        public HopControllerCrudBaseWithLookUp(TService service,
            ILogProvider logProvider, IDispatcher dispatcher) : base(service, logProvider, dispatcher)
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
        public IActionResult Get([FromQuery]TFilter filter)
        {
            return this.ReturnResponseFromResultWithPaginatedData(Service.Filter(filter));
        }
    }

    public class HopControllerCrudBaseAsync<TService, TNewCommand, TUpdateCommand, TRemoveCommand, TPrimaryKeyType, TFilter, TPaginatedResponse>
        : HopAuthorizedControllerBase
        where TService : ICrudServiceAsync<TNewCommand, TUpdateCommand, TRemoveCommand, TPrimaryKeyType, TFilter, TPaginatedResponse>
        where TNewCommand : CommandBase
        where TUpdateCommand : CommandBase
        where TRemoveCommand : DeleteCommand<TPrimaryKeyType>
        where TFilter : FilterBase
    {
        protected readonly TService Service;

        public HopControllerCrudBaseAsync(TService service,
            ILogProvider logProvider, IDispatcher dispatcher) : base(logProvider, dispatcher)
        {
            Service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]TNewCommand command)
        {
            return this.ReturnResponseFromResult(await Service.Create(command));
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody]TUpdateCommand command)
        {
            return this.ReturnResponseFromResult(await Service.Update(command));
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(TPrimaryKeyType id)
        {
            return this.ReturnResponseFromResult(await Service.Remove((TRemoveCommand)new DeleteCommand<TPrimaryKeyType>(id)));
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(TPrimaryKeyType id)
        {
            return this.ReturnResponseFromResult(await Service.GetById(id));
        }

        [HttpGet]
        public IActionResult Get([FromQuery]TFilter filter)
        {
            return this.ReturnResponseFromResultWithPaginatedData(Service.Filter(filter));
        }
    }

    public class HopControllerCrudBaseWithLookUpAsync<TService, TNewCommand, TUpdateCommand, TRemoveCommand, TPrimaryKeyType, TFilter, TPaginatedResponse, TLookUpFilter>
        : HopControllerLookUpBaseAsync<TLookUpFilter>
        where TService : ICrudServiceAsync<TNewCommand, TUpdateCommand, TRemoveCommand, TPrimaryKeyType, TFilter, TPaginatedResponse>, ILookUpServiceAsync<TLookUpFilter>
        where TNewCommand : CommandBase
        where TUpdateCommand : CommandBase
        where TRemoveCommand : DeleteCommand<TPrimaryKeyType>
        where TFilter : FilterBase
    {
        protected readonly TService Service;

        public HopControllerCrudBaseWithLookUpAsync(TService service,
            ILogProvider logProvider, IDispatcher dispatcher) : base(service, logProvider, dispatcher)
        {
            Service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]TNewCommand command)
        {
            return this.ReturnResponseFromResult(await Service.Create(command));
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody]TUpdateCommand command)
        {
            return this.ReturnResponseFromResult(await Service.Update(command));
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(TPrimaryKeyType id)
        {
            return this.ReturnResponseFromResult(await Service.Remove((TRemoveCommand)new DeleteCommand<TPrimaryKeyType>(id)));
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(TPrimaryKeyType id)
        {
            return this.ReturnResponseFromResult(await Service.GetById(id));
        }

        [HttpGet]
        public IActionResult Get([FromQuery]TFilter filter)
        {
            return this.ReturnResponseFromResultWithPaginatedData(Service.Filter(filter));
        }
    }
}

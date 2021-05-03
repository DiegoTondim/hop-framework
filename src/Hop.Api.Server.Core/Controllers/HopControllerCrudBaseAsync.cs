using Hop.Framework.Domain.Commands;
using Hop.Framework.Domain.Dispatcher;
using Hop.Framework.Domain.Repository;
using Hop.Framework.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Hop.Api.Server.Core.Controllers
{
    public class HopControllerCrudBaseAsync<TController, TService, TNewCommand, TUpdateCommand, TRemoveCommand, TPrimaryKeyType, TFilter, TPaginatedResponse>
        : HopAuthorizedControllerBase<TController>
        where TService : ICrudServiceAsync<TNewCommand, TUpdateCommand, TRemoveCommand, TPrimaryKeyType, TFilter, TPaginatedResponse>
        where TNewCommand : CommandBase
        where TUpdateCommand : CommandBase
        where TRemoveCommand : DeleteCommand<TPrimaryKeyType>
        where TFilter : FilterBase
    {
        protected readonly TService Service;

        public HopControllerCrudBaseAsync(TService service,
            ILogger<TController> logProvider, IDispatcher dispatcher) : base(logProvider, dispatcher)
        {
            Service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TNewCommand command)
        {
            return this.ReturnResponseFromResult(await Service.Create(command));
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] TUpdateCommand command)
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
        public IActionResult Get([FromQuery] TFilter filter)
        {
            return this.ReturnResponseFromResultWithPaginatedData(Service.Filter(filter));
        }
    }
}

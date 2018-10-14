using Hop.Framework.Core.Messaging;
using Hop.Framework.Domain.Commands;
using Hop.Framework.Domain.Handlers;
using Hop.Framework.Domain.Results;
using System;
using System.Threading.Tasks;

namespace Hop.Api.Server.Core.Dispatcher
{
    public class Dispatcher : IDispatcher
    {
        private IPublisher Publisher
        {
            get
            {
                return (IPublisher)_provider.GetService(typeof(IPublisher));
            }
        }
        private readonly IServiceProvider _provider;

        public Dispatcher(IServiceProvider provider)
        {
            _provider = provider;
        }

        public async Task<Result> Send(IEvent @event)
        {
            Publisher.Publish(@event);
            return await Task.FromResult(Result.Ok);
        }

        public async Task<Result> Execute<T>(T command) where T : CommandBase
        {
            return await ((ICommandHandlerWithResultAsync<T>)_provider.GetService(typeof(ICommandHandlerWithResultAsync<T>))).Execute(command);
        }
    }

    public interface IDispatcher
    {
        Task<Result> Send(IEvent @event);
        Task<Result> Execute<T>(T command) where T : CommandBase;
    }
}

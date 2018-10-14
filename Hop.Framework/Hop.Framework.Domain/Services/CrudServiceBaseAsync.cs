using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Hop.Framework.Domain.Commands;
using Hop.Framework.Domain.Handlers;
using Hop.Framework.Domain.Models;
using Hop.Framework.Domain.Notification;
using Hop.Framework.Domain.Repository;
using Hop.Framework.Domain.Results;
using Hop.Framework.Domain.Validation;

namespace Hop.Framework.Domain.Services
{
    public abstract class CrudServiceBaseAsync<TNewCommand, TUpdateCommand, TRemoveCommand, TEntity, TPrimaryKeyType, TFilter, TResponse, TPaginatedResponse, TRemoveCommandValidation>
         : CrudServiceBaseAsync<TNewCommand, TUpdateCommand, TRemoveCommand, TEntity, TPrimaryKeyType, TFilter, TResponse, TPaginatedResponse>
         where TEntity : class, IEntity<TPrimaryKeyType>
         where TNewCommand : CommandBase
         where TUpdateCommand : CommandBase
         where TRemoveCommand : DeleteCommand<TPrimaryKeyType>
         where TRemoveCommandValidation : IValidation<DeleteCommand<TPrimaryKeyType>>
         where TFilter : FilterBase
    {
        private readonly TRemoveCommandValidation _removeValidation;

        protected CrudServiceBaseAsync(IRepository<TEntity, TPrimaryKeyType> repository,
            IDomainNotificationHandler domainNotification, IValidation<TNewCommand> newValidation, IValidation<TUpdateCommand> updateValidation,
            IUnityOfWork uow, TRemoveCommandValidation removeValidation)
            : base(repository, domainNotification, newValidation, updateValidation, uow)
        {
            _removeValidation = removeValidation;
        }

        public async Task<Result> Execute(DeleteCommand<TPrimaryKeyType> command)
        {
            if (Validate(command, _removeValidation).IsValid)
            {
                Repository.Remove(command.Id);
                await Commit();
            }
            return await Return();
        }

        public async Task<Result> Remove(TPrimaryKeyType id)
        {
            await Execute(new DeleteCommand<TPrimaryKeyType>(id));
            return await Return();
        }
    }

    public abstract class CrudServiceBaseAsync<TNewCommand, TUpdateCommand, TRemoveCommand, TEntity, TPrimaryKeyType, TFilter, TResponse, TPaginatedResponse>
        : CommandHandlerBaseAsync, ICrudServiceAsync<TNewCommand, TUpdateCommand, TRemoveCommand, TPrimaryKeyType, TFilter, TPaginatedResponse>,
        IConverterPaginatedService<TPaginatedResponse, TEntity, TPrimaryKeyType, TFilter>
        where TNewCommand : CommandBase
        where TUpdateCommand : CommandBase
        where TRemoveCommand : DeleteCommand<TPrimaryKeyType>
        where TEntity : class, IEntity<TPrimaryKeyType>
        where TFilter : FilterBase
    {
        protected readonly IRepository<TEntity, TPrimaryKeyType> Repository;
        private readonly IValidation<TNewCommand> _newValidation;
        private readonly IValidation<TUpdateCommand> _updateValidation;

        protected CrudServiceBaseAsync(IRepository<TEntity, TPrimaryKeyType> repository, IDomainNotificationHandler domainNotification,
            IValidation<TNewCommand> newValidation, IValidation<TUpdateCommand> updateValidation, IUnityOfWork uow)
            : base(domainNotification, uow)
        {
            Repository = repository;
            _newValidation = newValidation;
            _updateValidation = updateValidation;
        }

        public async Task<Result> Execute(TNewCommand command)
        {
            if (Validate(command, _newValidation).IsValid)
            {
                var entity = await Convert(command);
                Repository.AddOrUpdate(entity);
                if (await Commit())
                {
                    return await Return(await ConvertToResponse(entity));
                }
            }
            return await Return();
        }

        public async Task<Result> Execute(TUpdateCommand command)
        {
            if (Validate(command, _updateValidation).IsValid)
            {
                var entity = await Convert(command);
                Repository.AddOrUpdate(entity);
                if (await Commit())
                {
                    return await Return(await ConvertToResponse(entity));
                }
            }
            return await Return();
        }

        public async Task<Result> Execute(DeleteCommand<TPrimaryKeyType> command)
        {
            Repository.Remove(command.Id);
            await Commit();
            return await Return();
        }

        public async Task<Result> GetById(TPrimaryKeyType id)
        {
            var response = await ConvertToResponse(await Repository.GetByIdAsync(id));
            return await Return(response);
        }

        public virtual ResultWithPaginatedData<TPaginatedResponse> Filter(TFilter filters)
        {
            var filter = FilterExpression(filters);
            var query = Repository.GetAll();

            //Filtro não deve ser obrigatorio.
            if (filter != null)
            {
                query = query.Where(filter);
            }

            var data = query.Order(filters.Order, GetOrderByExpression(filters))
                .Paginate(filters.PerPage, filters.CurrentPage, ConvertToPaginatedResponse);
            return Return(data);
        }

        public virtual Func<TEntity, object> GetOrderByExpression(TFilter filter)
        {
            return null;
        }

        public virtual Expression<Func<TEntity, bool>> FilterExpression(TFilter filter)
        {
            return null;
        }

        public async Task<Result> Create(TNewCommand command)
        {
            var result = await this.Execute(command);
            if (result.Success)
            {
                //Não dar wait de proposito, para o evento não toimar tempo de processamento do CRUD
                OnCreated(command, result);
            }

            return result;
        }

        public virtual async Task<bool> OnCreated(TNewCommand command, Result result)
        {
            //To be overriden
            return await Task.FromResult(true);
        }

        public async Task<Result> Update(TUpdateCommand command)
        {
            var result = await this.Execute(command);
            if (result.Success)
            {
                //Não dar wait de proposito, para o evento não toimar tempo de processamento do CRUD
                OnUpdated(command, result);
            }

            return result;
        }

        public virtual async Task<bool> OnUpdated(TUpdateCommand command, Result result)
        {
            //To be overriden
            return await Task.FromResult(true);
        }

        public async Task<Result> Remove(TRemoveCommand command)
        {
            var result = await this.Execute(command);
            if (result.Success)
            {
                //Não dar wait de proposito, para o evento não toimar tempo de processamento do CRUD
                OnRemoved(command, result);
            }

            return result;
        }

        public virtual async Task<bool> OnRemoved(TRemoveCommand command, Result result)
        {
            //To be overriden
            return await Task.FromResult(true);
        }

        protected abstract Task<TEntity> Convert(TNewCommand command);
        protected abstract Task<TEntity> Convert(TUpdateCommand command);
        public abstract TPaginatedResponse ConvertToPaginatedResponse(TEntity entity);
        public abstract Task<TResponse> ConvertToResponse(TEntity entity);
    }
}

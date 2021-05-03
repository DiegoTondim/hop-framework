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
    public abstract class CrudServiceBase<TNewCommand, TUpdateCommand, TRemoveCommand, TEntity, TPrimaryKeyType, TFilter, TResponse, TPaginatedResponse, TRemoveCommandValidation>
        : CrudServiceBase<TNewCommand, TUpdateCommand, TRemoveCommand, TEntity, TPrimaryKeyType, TFilter, TResponse, TPaginatedResponse>
        where TEntity : class, IEntity<TPrimaryKeyType>
        where TNewCommand : CommandBase
        where TUpdateCommand : CommandBase
        where TRemoveCommand : DeleteCommand<TPrimaryKeyType>
        where TRemoveCommandValidation : IValidation<DeleteCommand<TPrimaryKeyType>>
        where TFilter : FilterBase
    {
        private readonly TRemoveCommandValidation _removeValidation;

        protected CrudServiceBase(IRepository<TEntity, TPrimaryKeyType> repository,
            IDomainNotificationHandler domainNotification, IValidation<TNewCommand> newValidation, IValidation<TUpdateCommand> updateValidation,
            IUnitOfWork uow, TRemoveCommandValidation removeValidation)
            : base(repository, domainNotification, newValidation, updateValidation, uow)
        {
            _removeValidation = removeValidation;
        }


        public Result Execute(DeleteCommand<TPrimaryKeyType> command)
        {
            if (Validate(command, _removeValidation).IsValid)
            {
                Repository.Remove(command.Id);
                Commit();
            }
            return Return();
        }

        public Result Remove(TPrimaryKeyType id)
        {
            Execute(new DeleteCommand<TPrimaryKeyType>(id));
            return Return();
        }
    }

    public abstract class CrudServiceBase<TNewCommand, TUpdateCommand, TRemoveCommand, TEntity, TPrimaryKeyType, TFilter, TResponse, TPaginatedResponse>
        : CommandHandlerBase, ICrudService<TNewCommand, TUpdateCommand, TRemoveCommand, TPrimaryKeyType, TFilter, TResponse, TPaginatedResponse>,
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

        protected CrudServiceBase(IRepository<TEntity, TPrimaryKeyType> repository, IDomainNotificationHandler domainNotification,
            IValidation<TNewCommand> newValidation, IValidation<TUpdateCommand> updateValidation, IUnitOfWork uow)
            : base(domainNotification, uow)
        {
            Repository = repository;
            _newValidation = newValidation;
            _updateValidation = updateValidation;
        }

        public Result Execute(TNewCommand command)
        {
            if (Validate(command, _newValidation).IsValid)
            {
                var entity = Convert(command);
                Repository.AddOrUpdate(entity);
                if (Commit())
                {
                    return Return(ConvertToResponse(entity));
                }
            }
            return Return();
        }

        public Result Execute(TUpdateCommand command)
        {
            if (Validate(command, _updateValidation).IsValid)
            {
                var entity = Convert(command);
                Repository.AddOrUpdate(entity);
                if (Commit())
                {
                    return Return(ConvertToResponse(entity));
                }
            }
            return Return();
        }

        public Result Execute(DeleteCommand<TPrimaryKeyType> command)
        {
            Repository.Remove(command.Id);
            Commit();
            return Return();
        }

        public Result GetById(TPrimaryKeyType id)
        {
            return Return(ConvertToResponse(Repository.GetById(id)));
        }

        public virtual ResultWithPaginatedData<TPaginatedResponse> Filter(TFilter filters)
        {
            var filter = FilterExpression(filters);
            var query = Repository.GetAll();

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

        public Result Create(TNewCommand command)
        {
            var result = this.Execute(command);
            if (result.Success)
            {
                OnCreated(command, result);
            }
            return result;
        }

        public virtual async Task<bool> OnCreated(TNewCommand command, Result result)
        {
            return await Task.FromResult(true);
        }

        public Result Update(TUpdateCommand command)
        {
            var result = this.Execute(command);
            if (result.Success)
            {
                OnUpdated(command, result);
            }
            return result;
        }

        public virtual async Task<bool> OnUpdated(TUpdateCommand command, Result result)
        {
            return await Task.FromResult(true);
        }

        public Result Remove(TRemoveCommand command)
        {
            var result = this.Execute(command);
            if (result.Success)
            {
                OnRemoved(command, result);
            }
            return result;
        }

        public virtual async Task<bool> OnRemoved(TRemoveCommand command, Result result)
        {
            return await Task.FromResult(true);
        }

        protected abstract TEntity Convert(TNewCommand command);
        protected abstract TEntity Convert(TUpdateCommand command);
        public abstract TPaginatedResponse ConvertToPaginatedResponse(TEntity entity);
        public abstract TResponse ConvertToResponse(TEntity entity);
    }
}

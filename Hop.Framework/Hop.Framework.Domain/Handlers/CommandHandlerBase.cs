using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hop.Framework.Domain.Commands;
using Hop.Framework.Domain.Notification;
using Hop.Framework.Domain.Repository;
using Hop.Framework.Domain.Results;
using Hop.Framework.Domain.Validation;

namespace Hop.Framework.Domain.Handlers
{
    public abstract class CommandHandlerBase
    {
        private readonly IDomainNotificationHandler _notifications;
        private readonly IUnityOfWork _unityOfWork;

        protected CommandHandlerBase(IDomainNotificationHandler notifications, IUnityOfWork unityOfWork)
        {
            _notifications = notifications;
            _unityOfWork = unityOfWork;
        }

        protected ValidationResult Validate<T, TValidation>(T command, TValidation validation) where T : CommandBase where TValidation : IValidation<T>
        {
            var result = validation.Validate(command);
            foreach (var validationMessage in result.Messages)
            {
                _notifications.Handle(new DomainNotification(validationMessage.Type.ToString(), validationMessage.Message));
            }
            return result;
        }

        public ResultWithPaginatedData<TViewModel> Return<TViewModel>(int currentPage, int total, int perPage, IEnumerable<TViewModel> data)
        {
            var result = new ResultWithPaginatedData<TViewModel>(currentPage, total, perPage, data);
            return result;
        }

        public ResultWithPaginatedData<TViewModel> Return<TViewModel>(PaginatedData<TViewModel> data)
        {
            var result = new ResultWithPaginatedData<TViewModel>(data);
            return result;
        }

        public Result Return()
        {
            var result = Result.Ok;
            if (_notifications.HasNotificationsWithKey("Error"))
            {
                result = Result.Error;
            }
            result.AddMessages(_notifications.GetNotifications());
            return result;
        }

        public Result Return(object value)
        {
            var result = Return();
            result.AddValue(value);
            return result;
        }

        public bool Commit()
        {
            if (_notifications.HasNotificationsWithKey("Error")) return false;

            if (_unityOfWork.SaveAndCommit() > 0)
            {
                return true;
            }

            _notifications.Handle(new DomainNotification("Error", "We had a problem during saving your data."));
            return false;
        }
    }

    public abstract class CommandHandlerBaseAsync
    {
        private readonly IDomainNotificationHandler _notifications;
        private readonly IUnityOfWork _unityOfWork;

        protected CommandHandlerBaseAsync(IDomainNotificationHandler notifications, IUnityOfWork unityOfWork)
        {
            _notifications = notifications;
            _unityOfWork = unityOfWork;
        }

        protected ValidationResult Validate<T, TValidation>(T command, TValidation validation) where T : CommandBase where TValidation : IValidation<T>
        {
            var result = validation.Validate(command);
            foreach (var validationMessage in result.Messages)
            {
                _notifications.Handle(new DomainNotification(validationMessage.Type.ToString(), validationMessage.Message));
            }
            return result;
        }

        public ResultWithPaginatedData<TViewModel> Return<TViewModel>(int currentPage, int total, int perPage, IEnumerable<TViewModel> data)
        {
            var result = new ResultWithPaginatedData<TViewModel>(currentPage, total, perPage, data);
            return result;
        }

        public ResultWithPaginatedData<TViewModel> Return<TViewModel>(PaginatedData<TViewModel> data)
        {
            var result = new ResultWithPaginatedData<TViewModel>(data);
            return result;
        }

        public async Task<Result<TViewModel>> ReturnAsync<TViewModel>(TViewModel data)
        {
            var result = new Result<TViewModel>(data);
            return await Task.FromResult(result);
        }

        public async Task<Result> Return()
        {
            var result = Result.Ok;
            if (_notifications.HasNotificationsWithKey("Error"))
            {
                result = Result.Error;
            }
            result.AddMessages(_notifications.GetNotifications());
            return await Task.FromResult(result);
        }

        public async Task<Result> Return(object value)
        {
            var result = await Return();
            result.AddValue(value);
            return result;
        }

        public async Task<bool> Commit()
        {
            if (_notifications.HasNotificationsWithKey("Error")) return false;

            if ((await _unityOfWork.SaveAndCommitAsync()) > 0)
            {
                return true;
            }

            _notifications.Handle(new DomainNotification("Error", "We had a problem during saving your data."));
            return false;
        }

        public async Task<bool> CommitWithResult()
        {
            if (_notifications.HasNotificationsWithKey("Error")) return false;

            var result = await _unityOfWork.SaveAndCommitAsyncWithSaveResult();
            int qtd = 0;
            if (int.TryParse(result, out qtd) && qtd > 0)
            {
                return true;
            }

            _notifications.Handle(new DomainNotification("Error", result));
            return false;
        }
    }
}

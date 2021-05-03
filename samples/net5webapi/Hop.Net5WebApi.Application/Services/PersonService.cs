using Hop.Framework.Domain.Commands;
using Hop.Framework.Domain.Notification;
using Hop.Framework.Domain.Repository;
using Hop.Framework.Domain.Results;
using Hop.Framework.Domain.Services;
using Hop.Framework.Domain.Validation;
using Hop.Net5WebApi.Application.Filters;
using Hop.Net5WebApi.Application.ViewModels;
using Hop.Net5WebApi.Domain.Commands;
using Hop.Net5WebApi.Domain.Entities;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Hop.Net5WebApi.Application.Services
{
    public class PersonService : CrudServiceBaseAsync<RegisterNewPersonCommand, UpdatePersonCommand, DeleteCommand<Guid>,
        PersonEntity, Guid, PersonFilter, PersonReadViewModel, PersonReadViewModel>, IPersonService
    {
        public PersonService(IValidation<RegisterNewPersonCommand> registerNewPersonValidation, IValidation<UpdatePersonCommand> updatePersonValidation,
            IDomainNotificationHandler notifications, IUnitOfWork uow,
            IRepositoryWithGuidKey<PersonEntity> repository)
            : base(repository, notifications, registerNewPersonValidation, updatePersonValidation, uow)
        {
        }

        protected override async Task<PersonEntity> Convert(RegisterNewPersonCommand command)
        {
            var entity = new PersonEntity(command.Name);
            return await Task.FromResult(entity);
        }

        public override Expression<Func<PersonEntity, bool>> FilterExpression(PersonFilter filter)
        {
            if (!string.IsNullOrWhiteSpace(filter?.Name))
            {
                return x => x.Name.Contains(filter.Name);
            }

            return null;
        }

        protected override async Task<PersonEntity> Convert(UpdatePersonCommand command)
        {
            return await Task.FromResult(new PersonEntity(command.Name));
        }

        public override PersonReadViewModel ConvertToPaginatedResponse(PersonEntity entity)
        {
            return new PersonReadViewModel(entity.Id, entity.Name);
        }

        public override async Task<PersonReadViewModel> ConvertToResponse(PersonEntity entity)
        {
            return await Task.FromResult(new PersonReadViewModel(entity.Id, entity.Name));
        }

        public async Task<Result> LookupFilter(PersonFilter filter)
        {
            var items = await Repository.FindAsync(x => x.Name.Contains(filter.Name));
            return await Return(items.ToList().Select(ConvertToLookupResponse));
        }

        public PersonReadViewModel ConvertToLookupResponse(PersonEntity entity)
        {
            return new PersonReadViewModel(entity.Name);
        }
    }
}

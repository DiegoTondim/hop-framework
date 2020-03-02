using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Hop.Framework.Core.Bootstrapper;
using Hop.Framework.Core.IoC;
using Hop.Framework.Domain.Commands;
using Hop.Framework.Domain.Models;
using Hop.Framework.Domain.Module;
using Hop.Framework.Domain.Notification;
using Hop.Framework.Domain.Repository;
using Hop.Framework.Domain.Results;
using Hop.Framework.Domain.Services;
using Hop.Framework.Domain.Validation;
using Hop.Framework.FluentValidation;
using Hop.Framework.UnitTests.DI;
using NSubstitute;
using NUnit.Framework;

namespace Hop.Framework.Domain.Tests.Async
{
    [TestFixture]
    public class ServiceAsyncTest
    {
        public ServiceAsyncTest()
        {
            
        }

        [Test]
        public void Should_Execute_Create_Person_Command()
        {
            using (ServiceResolver.BeginScope(ScopeType.Thread))
            {
                var service = ServiceResolver.Container.Resolve<IPersonService>();
                var result = service.Create(new RegisterNewPersonCommand("John")).Result;

                Assert.AreEqual(false, ServiceResolver.Container.Resolve<IDomainNotificationHandler>().HasNotifications());
                Assert.AreEqual(true, result.Success);
            }
        }

        [Test]
        public void Should_Execute_Create_Person_Command_And_Return_Validation_Error()
        {
            using (ServiceResolver.BeginScope(ScopeType.Thread))
            {
                var service = ServiceResolver.Container.Resolve<IPersonService>();
                var result = service.Create(new RegisterNewPersonCommand(null)).Result;

                Assert.AreEqual(true, ServiceResolver.Container.Resolve<IDomainNotificationHandler>().HasNotifications());
                Assert.AreEqual(false, result.Success);
            }
        }

        [Test]
        public void Should_Execute_Create_Person_Command_And_Return_Info_Message()
        {
            using (ServiceResolver.BeginScope(ScopeType.Thread))
            {
                var service = ServiceResolver.Container.Resolve<IPersonService>();
                var result = service.Create(new RegisterNewPersonCommand("James")).Result;

                Assert.AreEqual(true,
                    ServiceResolver.Container.Resolve<IDomainNotificationHandler>().HasNotifications());
                Assert.AreEqual(true, result.Success);
            }
        }

        [Test]
        public void Should_Search_Part_Of_Name_Of_Person_And_Found_Two()
        {
            using (ServiceResolver.BeginScope(ScopeType.Thread))
            {
                var service = ServiceResolver.Container.Resolve<IPersonService>();
                var result = service.Filter(new PersonFilter
                {
                    Name = "J"
                });

                Assert.AreEqual(true, result.Success);
                Assert.AreEqual(2, result.Value.Total);
            }
        }

        [Test]
        public void Should_Search_Part_Of_Name_Of_Person_And_Not_Found()
        {
            using (ServiceResolver.BeginScope(ScopeType.Thread))
            {
                var service = ServiceResolver.Container.Resolve<IPersonService>();
                var result = service.Filter(new PersonFilter
                {
                    Name = "Paul"
                });

                Assert.AreEqual(true, result.Success);
                Assert.AreEqual(0, result.Value.Total);
            }
        }

        [Test]
        public void Should_Search_Person_By_ID_And_Found_One()
        {
            using (ServiceResolver.BeginScope(ScopeType.Thread))
            {
                var service = ServiceResolver.Container.Resolve<IPersonService>();
                var result = service.GetById(Guid.Empty).Result;

                Assert.AreEqual(true, result.Success);
                Assert.IsNotNull(result.Value);
            }
        }
    }

    public class PersonDomain : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsNew => Id != Guid.Empty;

        public PersonDomain(string name)
        {
            Name = name;
        }
    }

    public class PersonService : CrudServiceBaseAsync<RegisterNewPersonCommand, UpdatePersonCommand, DeleteCommand<Guid>,
        PersonDomain, Guid, PersonFilter, PersonReadViewModel, PersonReadViewModel>, IPersonService
    {
        public PersonService(IValidation<RegisterNewPersonCommand> registerNewPersonValidation, IValidation<UpdatePersonCommand> updatePersonValidation,
            IDomainNotificationHandler notifications, IUnityOfWork uow,
            IRepositoryWithGuidKey<PersonDomain> repository)
            : base(repository, notifications, registerNewPersonValidation, updatePersonValidation, uow)
        {
        }

        protected override async Task<PersonDomain> Convert(RegisterNewPersonCommand command)
        {
            var entity = new PersonDomain(command.Name);
            return await Task.FromResult(entity);
        }

        public override Expression<Func<PersonDomain, bool>> FilterExpression(PersonFilter filter)
        {
            if (!string.IsNullOrWhiteSpace(filter?.Name))
            {
                return x => x.Name.Contains(filter.Name);
            }

            return null;
        }

        protected override async Task<PersonDomain> Convert(UpdatePersonCommand command)
        {
            return await Task.FromResult(new PersonDomain(command.Name));
        }

        public override PersonReadViewModel ConvertToPaginatedResponse(PersonDomain entity)
        {
            return new PersonReadViewModel(entity.Id, entity.Name);
        }

        public override async Task<PersonReadViewModel> ConvertToResponse(PersonDomain entity)
        {
            return await Task.FromResult(new PersonReadViewModel(entity.Id, entity.Name));
        }

        public async Task<Result> LookupFilter(PersonFilter filter)
        {
            var items = await Repository.FindAsync(x => x.Name.Contains(filter.Name));
            return await Return(items.ToList().Select(ConvertToLookupResponse));
        }

        public PersonReadViewModel ConvertToLookupResponse(PersonDomain entity)
        {
            return new PersonReadViewModel(entity.Name);
        }
    }

    public interface IPersonService : ICrudServiceAsync<RegisterNewPersonCommand, UpdatePersonCommand, DeleteCommand<Guid>, Guid, PersonFilter, PersonReadViewModel>,
        ILookUpServiceAsync<PersonDomain, PersonFilter, PersonReadViewModel, Guid>
    {
    }

    public class PersonFilter : FilterBase
    {
        public string Name { get; set; }
        public override int PerPage => 10;
    }

    public class PersonViewModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }

        public PersonViewModel(string name)
        {
            Name = name;
        }
        public PersonViewModel(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }

    public class PersonReadViewModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }

        public PersonReadViewModel(string name)
        {
            Name = name;
        }
        public PersonReadViewModel(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }

    public class RegisterNewPersonCommand : PersonCommand
    {

        public RegisterNewPersonCommand(string name) : base(name)
        {
        }
    }

    public class UpdatePersonCommand : PersonCommand
    {

        public UpdatePersonCommand(string name) : base(name)
        {
        }
    }

    public abstract class PersonCommand : CommandBase
    {
        public string Name { get; set; }

        protected PersonCommand(string name)
        {
            Name = name;
        }
    }

    public class CommandModule : DependencyModule
    {
        public override void Load(IContainer container)
        {
            container.Register<IPersonService, PersonService>();
            container.Register<IValidation<RegisterNewPersonCommand>, RegisterNewPersonValidator>();
            container.Register<IValidation<UpdatePersonCommand>, UpdatePersonValidator>();
            container.Register<IUnityOfWork, UOW>();
            var repository = Substitute.For<IRepositoryWithGuidKey<PersonDomain>>();
            repository.GetById(Guid.Empty).ReturnsForAnyArgs(new PersonDomain("John"));
            repository.GetByIdAsync(Guid.Empty).ReturnsForAnyArgs(new PersonDomain("John"));
            repository.GetAll().ReturnsForAnyArgs(new List<PersonDomain>()
            {
                new PersonDomain("John"),
                new PersonDomain("Maria"),
                new PersonDomain("Javin"),
                new PersonDomain("Maria"),
            }.AsQueryable());
            container.Register<IRepositoryWithGuidKey<PersonDomain>>((a) => repository);
        }
    }

    public class RegisterNewPersonValidator : PersonValidator<RegisterNewPersonCommand>
    {
        public RegisterNewPersonValidator()
        {
            ValidateName();
        }
    }

    public class UpdatePersonValidator : PersonValidator<UpdatePersonCommand>
    {
        public UpdatePersonValidator()
        {
            ValidateName();
        }
    }

    public class PersonValidator<T> : ValidationBase<T> where T : PersonCommand
    {
        protected void ValidateName()
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Please ensure you have entered the Name")
                .Must(x => x != "James").WithMessage("James is not a good Name!").WithSeverity(Severity.Info)
                .Length(2, 150).WithMessage("The Name must have between 2 and 150 characters")
                .WithSeverity(Severity.Error);
        }
    }

    public class UOW : IUnityOfWork
    {
        public void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
        }

        public int Save()
        {
            return 1;
        }

        public int SaveAndCommit()
        {
            return 1;
        }

        public Task<int> SaveAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.FromResult(1);
        }

        public Task<int> SaveAndCommitAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.FromResult(1);
        }

        public void Commit()
        {
        }

        public void Rollback()
        {
        }

        public Task<string> SaveAndCommitAsyncWithSaveResult(CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }
    }
}

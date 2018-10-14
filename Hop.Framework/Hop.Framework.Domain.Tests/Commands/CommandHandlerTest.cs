using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Hop.Framework.Core.Bootstrapper;
using Hop.Framework.Core.IoC;
using Hop.Framework.Domain.Commands;
using Hop.Framework.Domain.Handlers;
using Hop.Framework.Domain.Module;
using Hop.Framework.Domain.Notification;
using Hop.Framework.Domain.Repository;
using Hop.Framework.Domain.Results;
using Hop.Framework.Domain.Validation;
using Hop.Framework.FluentValidation;
using Hop.Framework.UnitTests.DI;
using NUnit.Framework;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Hop.Framework.Domain.Tests.Commands
{

    [TestFixture]
    public class CommandHandlerTest
    {
        public CommandHandlerTest()
        {
            Bootstrapper
                .Configure()
                .UserUnitTestsContainer()
                .ThreadLifestyle()
                .UseDomainNotifications()
                .RegisterModule<CommandModule>()
                .Build();
        }

        [Test]
        public void Should_Execute_Create_Person_Command()
        {
            using (ServiceResolver.BeginScope(ScopeType.Thread))
            {
                var service = ServiceResolver.Container.Resolve<IPersonService>();
                var result = service.Execute(new RegisterNewPersonCommand("John"));

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
                var result = service.Execute(new RegisterNewPersonCommand(null));

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
                var result = service.Execute(new RegisterNewPersonCommand("James"));

                Assert.AreEqual(true, ServiceResolver.Container.Resolve<IDomainNotificationHandler>().HasNotifications());
                Assert.AreEqual(true, result.Success);
            }
        }
    }

    public class PersonDomain
    {
        public Guid Id { get; set; }
    }

    public class PersonService : CommandHandlerBase, IPersonService
    {
        private readonly IValidation<RegisterNewPersonCommand> _registerNewPersonValidation;

        public PersonService(IValidation<RegisterNewPersonCommand> registerNewPersonValidation,
            IDomainNotificationHandler notifications, IUnityOfWork uow)
            : base(notifications, uow)
        {
            _registerNewPersonValidation = registerNewPersonValidation;
        }

        public Result Execute(RegisterNewPersonCommand command)
        {
            PersonDomain entity = null;
            if (Validate(command, _registerNewPersonValidation).IsValid)
            {
                if (Commit())
                {
                    entity = new PersonDomain();
                }
            }
            return Return(command);
        }
    }

    public interface IPersonService : ICommandHandlerWithResult<RegisterNewPersonCommand>
    {
    }

    public class PersonViewModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }

        public PersonViewModel(string name)
        {
            Name = name;
        }
    }

    public class RegisterNewPersonCommand : PersonCommand
    {

        public RegisterNewPersonCommand(string name) : base(name)
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
            container.Register<IUnityOfWork, UOW>();
        }
    }

    public class RegisterNewPersonValidator : PersonValidator<RegisterNewPersonCommand>
    {
        public RegisterNewPersonValidator()
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

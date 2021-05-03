using FluentValidation;
using Hop.Framework.Domain.Commands;
using Hop.Framework.Domain.Handlers;
using Hop.Framework.Domain.Notification;
using Hop.Framework.Domain.Repository;
using Hop.Framework.Domain.Results;
using Hop.Framework.Domain.Validation;
using Hop.Framework.FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Hop.Framework.Domain.Tests.Commands
{

    [TestFixture]
    public class CommandHandlerTest
    {
        private readonly ServiceProvider _serviceProvider;

        public CommandHandlerTest()
        {
            _serviceProvider = new ServiceCollection()
                .AddDomainNotifications()
                .AddScoped<IPersonService, PersonService>()
                .AddScoped<IValidation<RegisterNewPersonCommand>, RegisterNewPersonValidator>()
                .AddScoped<IUnityOfWork, UOW>()
                .BuildServiceProvider();
        }

        [Test]
        public void Should_Execute_Create_Person_Command()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IPersonService>();
                var result = service.Execute(new RegisterNewPersonCommand("John"));

                Assert.AreEqual(false, scope.ServiceProvider.GetRequiredService<IDomainNotificationHandler>().HasNotifications());
                Assert.AreEqual(true, result.Success);
            }
        }

        [Test]
        public void Should_Execute_Create_Person_Command_And_Return_Validation_Error()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IPersonService>();
                var result = service.Execute(new RegisterNewPersonCommand(null));

                Assert.AreEqual(true, scope.ServiceProvider.GetRequiredService<IDomainNotificationHandler>().HasNotifications());
                Assert.AreEqual(false, result.Success);
            }
        }

        [Test]
        public void Should_Execute_Create_Person_Command_And_Return_Info_Message()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IPersonService>();
                var result = service.Execute(new RegisterNewPersonCommand("James"));

                Assert.AreEqual(true, scope.ServiceProvider.GetRequiredService<IDomainNotificationHandler>().HasNotifications());
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

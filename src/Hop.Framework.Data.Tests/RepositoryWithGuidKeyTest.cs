using Hop.Framework.Core.User;
using Hop.Framework.Domain.Repository;
using Hop.Framework.EFCore.Repository;
using Hop.Framework.EFCore.Tests.Infra.Context;
using Hop.Framework.EFCore.Tests.Infra.Models;
using Hop.Framework.EFCore.Tests.Infra.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Linq;

namespace Hop.Framework.EFCore.Tests
{
    [TestFixture]
	public class RepositoryWithGuidKeyTest
	{
		private IUserContextService GetUserContextService()
		{
			var service = new Core.User.UserContextService();
			service.Set(new UserContext());
			return service;
		}

		[Test]
		public void Repository_Can_Insert()
		{
			using (var context = new InMemoryContext())
			{
				IRepository<TabelaTesteWithGuidKey, Guid> repository = new TabelaTesteWithGuidKeyRepository(context, GetUserContextService());
				var uow = new UnityOfWork(context, Substitute.For<ILogger<UnityOfWork>>());

				repository.AddOrUpdate(new TabelaTesteWithGuidKey()
				{
					Propriedade = Guid.NewGuid().ToString()
				});

				var alteracoes = uow.SaveAndCommit();
				Assert.IsTrue(alteracoes == 1);

				var chave = repository.GetAll().FirstOrDefault();
				Assert.NotNull(chave);
				Assert.IsTrue(chave.Id != Guid.Empty);

				var registro = repository.GetById(chave.Id);
				Assert.NotNull(registro);

				registro.Propriedade = "NewValue";
				uow.SaveAndCommit();

				registro = repository.GetById(chave.Id);
				Assert.NotNull(registro);
				Assert.AreEqual(registro.Propriedade, "NewValue");
			}
		}

		[Test]
		public void Repository_Can_InsertAsync()
		{
			using (var context = new InMemoryContext())
			{
				IRepository<TabelaTesteWithGuidKey, Guid> repository = new TabelaTesteWithGuidKeyRepository(context, GetUserContextService());
				var uow = new UnityOfWork(context, Substitute.For<ILogger<UnityOfWork>>());

				var result = repository.AddOrUpdateAsync(new TabelaTesteWithGuidKey()
				{
					Propriedade = Guid.NewGuid().ToString()
				}).Result;

				var alteracoes = uow.SaveAndCommitAsync().Result;
				Assert.IsTrue(alteracoes == 1);

				var chave = repository.FindAsync(p => p.Propriedade == result.Propriedade).Result.FirstOrDefault();
				Assert.NotNull(chave);
				Assert.IsTrue(chave.Id != Guid.Empty);

				var registro = repository.GetByIdAsync(chave.Id).Result;
				Assert.NotNull(registro);

				registro.Propriedade = "NewValue";
				var saveResult = uow.SaveAndCommitAsync().Result;

				registro = repository.GetByIdAsync(chave.Id).Result;
				Assert.NotNull(registro);
				Assert.AreEqual(registro.Propriedade, "NewValue");
			}
		}

		[Test]
		public void Repository_Can_Update_Attached()
		{
			using (var context = new InMemoryContext())
			{
				IRepository<TabelaTesteWithGuidKey, Guid> repository = new TabelaTesteWithGuidKeyRepository(context, GetUserContextService());

				var uow = new UnityOfWork(context, Substitute.For<ILogger<UnityOfWork>>());

				repository.AddOrUpdate(new TabelaTesteWithGuidKey()
				{
					Propriedade = Guid.NewGuid().ToString()
				});

				var alteracoes = uow.SaveAndCommit();
				Assert.IsTrue(alteracoes == 1);

				var registro = repository.GetAll().FirstOrDefault();
				Assert.NotNull(registro);
				Assert.IsTrue(registro.Id != Guid.Empty);

				registro.Propriedade = "NewValue_123";

				repository.AddOrUpdate(registro);
				uow.SaveAndCommit();
				Assert.IsTrue(alteracoes == 1);
				Assert.AreEqual(registro.Propriedade, "NewValue_123");
			}
		}

		[Test]
		public void Repository_Can_Update_AttachedAsync()
		{
			using (var context = new InMemoryContext())
			{
				IRepository<TabelaTesteWithGuidKey, Guid> repository = new TabelaTesteWithGuidKeyRepository(context, GetUserContextService());
				var uow = new UnityOfWork(context, Substitute.For<ILogger<UnityOfWork>>());

				var result = repository.AddOrUpdateAsync(new TabelaTesteWithGuidKey()
				{
					Propriedade = Guid.NewGuid().ToString()
				}).Result;

				var alteracoes = uow.SaveAndCommitAsync().Result;
				Assert.IsTrue(alteracoes == 1);

				var registro = repository.FindAsync(p => p.Propriedade == result.Propriedade).Result.FirstOrDefault();
				Assert.NotNull(registro);
				Assert.IsTrue(registro.Id != Guid.Empty);

				registro.Propriedade = "NewValue_123";

				var saveResult = repository.AddOrUpdateAsync(registro).Result;
				var contextSaveResult = uow.SaveAndCommitAsync().Result;
				Assert.IsTrue(alteracoes == 1);
				Assert.AreEqual(registro.Propriedade, "NewValue_123");
			}
		}

		[Test]
		public void Repository_Can_Update_Detached()
		{
			using (var context = new InMemoryContext())
			{
				IRepository<TabelaTesteWithGuidKey, Guid> repository = new TabelaTesteWithGuidKeyRepository(context, GetUserContextService());
				var uow = new UnityOfWork(context, Substitute.For<ILogger<UnityOfWork>>());

				repository.AddOrUpdate(new TabelaTesteWithGuidKey()
				{
					Propriedade = Guid.NewGuid().ToString()
				});

				var alteracoes = uow.SaveAndCommit();
				Assert.IsTrue(alteracoes == 1);

				var chave = repository.GetAll().FirstOrDefault();
				Assert.NotNull(chave);
				Assert.IsTrue(chave.Id != Guid.Empty);

				var registro = new TabelaTesteWithGuidKey()
				{
					Id = chave.Id,
					Propriedade = "NewValue_123"
				};

				repository.AddOrUpdate(registro);
				uow.SaveAndCommit();
				Assert.IsTrue(alteracoes == 1);

				registro = repository.GetById(chave.Id);
				Assert.NotNull(registro);
				Assert.AreEqual(registro.Propriedade, "NewValue_123");
			}
		}

		[Test]
		public void Repository_Can_Update_DetachedAsync()
		{
			using (var context = new InMemoryContext())
			{
				IRepository<TabelaTesteWithGuidKey, Guid> repository = new TabelaTesteWithGuidKeyRepository(context, GetUserContextService());
				var uow = new UnityOfWork(context, Substitute.For<ILogger<UnityOfWork>>());

				var result = repository.AddOrUpdateAsync(new TabelaTesteWithGuidKey()
				{
					Propriedade = Guid.NewGuid().ToString()
				}).Result;

				var alteracoes = uow.SaveAndCommitAsync().Result;
				Assert.IsTrue(alteracoes == 1);

				var chave = repository.FindAsync(p => p.Propriedade == result.Propriedade).Result.FirstOrDefault();
				Assert.NotNull(chave);
				Assert.IsTrue(chave.Id != Guid.Empty);

				var registro = new TabelaTesteWithGuidKey()
				{
					Id = chave.Id,
					Propriedade = "NewValue_123"
				};

				registro = repository.AddOrUpdateAsync(registro).Result;
				var contextSaveResult = uow.SaveAndCommitAsync().Result;
				Assert.IsTrue(alteracoes == 1);

				registro = repository.GetByIdAsync(registro.Id).Result;
				Assert.NotNull(registro);
				Assert.AreEqual(registro.Propriedade, "NewValue_123");
			}
		}

		[Test]
		public void Repository_Can_Remove_ByEntity()
		{
			using (var context = new InMemoryContext())
			{
				IRepository<TabelaTesteWithGuidKey, Guid> repository = new TabelaTesteWithGuidKeyRepository(context, GetUserContextService());
				var uow = new UnityOfWork(context, Substitute.For<ILogger<UnityOfWork>>());

				repository.AddOrUpdate(new TabelaTesteWithGuidKey()
				{
					Propriedade = Guid.NewGuid().ToString()
				});

				var alteracoes = uow.SaveAndCommit();
				Assert.IsTrue(alteracoes == 1);

				var registro = repository.GetAll().FirstOrDefault();
				Assert.NotNull(registro);
				Assert.IsTrue(registro.Id != Guid.Empty);

				repository.Remove(registro);
				uow.SaveAndCommit();
				Assert.IsTrue(alteracoes == 1);

				registro = repository.Find(p => p.Id == registro.Id).FirstOrDefault();
				Assert.Null(registro);
			}
		}

		[Test]
		public void Repository_Can_Remove_ByEntityAsync()
		{
			using (var context = new InMemoryContext())
			{
				IRepository<TabelaTesteWithGuidKey, Guid> repository = new TabelaTesteWithGuidKeyRepository(context, GetUserContextService());
				var uow = new UnityOfWork(context, Substitute.For<ILogger<UnityOfWork>>());

				var result = repository.AddOrUpdateAsync(new TabelaTesteWithGuidKey()
				{
					Propriedade = Guid.NewGuid().ToString()
				}).Result;

				var alteracoes = uow.SaveAndCommitAsync().Result;
				Assert.IsTrue(alteracoes == 1);

				var registro = repository.FindAsync(p => p.Propriedade == result.Propriedade).Result.FirstOrDefault();
				Assert.NotNull(registro);
				Assert.IsTrue(registro.Id != Guid.Empty);

				var removeResult = repository.RemoveAsync(registro).Result;
				var saveResult = uow.SaveAndCommitAsync().Result;
				Assert.IsTrue(alteracoes == 1);

				registro = repository.FindAsync(p => p.Id == registro.Id).Result.FirstOrDefault();
				Assert.Null(registro);
			}
		}

		[Test]
		public void Repository_Can_Remove_ById()
		{
			using (var context = new InMemoryContext())
			{
				IRepository<TabelaTesteWithGuidKey, Guid> repository = new TabelaTesteWithGuidKeyRepository(context, GetUserContextService());
				var uow = new UnityOfWork(context, Substitute.For<ILogger<UnityOfWork>>());

				var registro = new TabelaTesteWithGuidKey()
				{
					Propriedade = Guid.NewGuid().ToString()
				};

				repository.AddOrUpdate(registro);

				var alteracoes = uow.SaveAndCommit();
				Assert.IsTrue(alteracoes == 1);

				repository.Remove(registro.Id);
				uow.SaveAndCommit();
				Assert.IsTrue(alteracoes == 1);

				registro = repository.Find(p => p.Id == registro.Id).FirstOrDefault();
				Assert.Null(registro);
			}
		}

		[Test]
		public void Repository_Can_Remove_ByIdAsync()
		{
			using (var context = new InMemoryContext())
			{
				IRepository<TabelaTesteWithGuidKey, Guid> repository = new TabelaTesteWithGuidKeyRepository(context, GetUserContextService());
				var uow = new UnityOfWork(context, Substitute.For<ILogger<UnityOfWork>>());

				var registro = new TabelaTesteWithGuidKey()
				{
					Propriedade = Guid.NewGuid().ToString()
				};

				var saveResult = repository.AddOrUpdateAsync(registro).Result;

				var alteracoes = uow.SaveAndCommitAsync().Result;
				Assert.IsTrue(alteracoes == 1);

				var removeResult = repository.RemoveAsync(registro.Id).Result;
				var saveRemoveResult = uow.SaveAndCommitAsync().Result;
				Assert.IsTrue(alteracoes == 1);

				registro = repository.FindAsync(p => p.Id == registro.Id).Result.FirstOrDefault();
				Assert.Null(registro);
			}
		}

		[Test]
		public void Read_Only_Repository_Can_Not_Write()
		{
			using (var context = new InMemoryContext())
			{
				var readOnlyRepository = new TabelaTesteReadOnlyWithGuidKeyRepository(context);
				context.TabelaTesteWithGuidKeys.Add(new TabelaTesteWithGuidKey()
				{
					Propriedade = Guid.NewGuid().ToString()
				});

				var alteracoes = context.SaveChanges();
				Assert.IsTrue(alteracoes == 1);

				var chave = context.TabelaTesteWithGuidKeys.FirstOrDefault();
				Assert.NotNull(chave);
				Assert.IsTrue(chave.Id != Guid.Empty);

				var registro = readOnlyRepository.GetById(chave.Id);
				Assert.NotNull(registro);

				registro.Propriedade = "NewValue";
				context.SaveChanges();

				registro = readOnlyRepository.GetById(chave.Id);
				Assert.NotNull(registro);
				Assert.AreNotEqual(registro.Propriedade, "NewValue");
			}
		}

		[Test]
		public void Read_Only_Repository_Can_Not_WriteAsync()
		{
			using (var context = new InMemoryContext())
			{ 
				var readOnlyRepository = new TabelaTesteReadOnlyWithGuidKeyRepository(context);
				context.TabelaTesteWithGuidKeys.Add(new TabelaTesteWithGuidKey()
				{
					Propriedade = Guid.NewGuid().ToString()
				});

				var alteracoes = context.SaveChangesAsync().Result;
				Assert.IsTrue(alteracoes == 1);

				var chave = context.TabelaTesteWithGuidKeys.FirstOrDefaultAsync().Result;
				Assert.NotNull(chave);
				Assert.IsTrue(chave.Id != Guid.Empty);

				var registro = readOnlyRepository.GetByIdAsync(chave.Id).Result;
				Assert.NotNull(registro);

				registro.Propriedade = "NewValue";
				var saveResult = context.SaveChangesAsync().Result;

				registro = readOnlyRepository.GetByIdAsync(chave.Id).Result;
				Assert.NotNull(registro);
				Assert.AreNotEqual(registro.Propriedade, "NewValue");
			}
		}

		[Test]
		public void Repository_Can_Remove_Logically_ById()
		{
			using (var context = new InMemoryContext())
			{
				IRepository<TabelaTesteSoftDeleteWithGuidKey, Guid> repository = new TabelaTesteWithGuidKeySoftDeleteRepository(context, GetUserContextService());
				var uow = new UnityOfWork(context, Substitute.For<ILogger<UnityOfWork>>());

				var registro = new TabelaTesteSoftDeleteWithGuidKey()
				{
					Propriedade = Guid.NewGuid().ToString()
				};

				var saveResult = repository.AddOrUpdateAsync(registro).Result;

				var alteracoes = uow.SaveAndCommitAsync().Result;
				Assert.IsTrue(alteracoes == 1);

				var removeResult = repository.RemoveAsync(registro.Id).Result;
				var saveRemoveResult = uow.SaveAndCommitAsync().Result;
				Assert.IsTrue(alteracoes == 1);
				var entidade = repository.FindAsync(p => p.Id == registro.Id).Result.FirstOrDefault();
				Assert.Null(entidade);
			}
		}
	}
}
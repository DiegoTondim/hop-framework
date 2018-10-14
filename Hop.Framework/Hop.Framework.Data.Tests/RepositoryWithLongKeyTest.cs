using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using Hop.Framework.Core.Log;
using Hop.Framework.EFCore.Context;
using Hop.Framework.EFCore.Repository;
using Hop.Framework.EFCore.Tests.Infra;
using Hop.Framework.EFCore.Tests.Infra.Context;
using Hop.Framework.EFCore.Tests.Infra.Models;
using Hop.Framework.EFCore.Tests.Infra.Repositories;
using Hop.Framework.Domain.Repository;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using NUnit.Framework;

namespace Hop.Framework.EFCore.Tests
{
    [TestFixture]
    public class RepositoryWithLongKeyTest
    {
        [Test]
        public void Repository_Can_Insert()
        {
            using (var context = new InMemoryContext())
            using(IRepository<TabelaTesteWithLongKey, long> repository = new TabelaTesteWithLongKeyRepository(context))
            {
                var uow = new UnityOfWork(context, Substitute.For<ILogProvider>());

                repository.AddOrUpdate(new TabelaTesteWithLongKey()
                {
                    Propriedade = Guid.NewGuid().ToString()
                });

                var alteracoes = uow.SaveAndCommit();
                Assert.IsTrue(alteracoes == 1);

                var chave = repository.GetAll().FirstOrDefault();
                Assert.NotNull(chave);
                Assert.IsTrue(chave.Id > 0);

                var registro = repository.GetById(chave.Id);
                Assert.NotNull(registro);

                registro.Propriedade = "Alterada";
                uow.SaveAndCommit();

                registro = repository.GetById(chave.Id);
                Assert.NotNull(registro);
                Assert.AreEqual(registro.Propriedade, "Alterada");
            }
        }

        [Test]
        public void Repository_Can_InsertAsync()
        {
            using (var context = new InMemoryContext())
            using (IRepository<TabelaTesteWithLongKey, long> repository = new TabelaTesteWithLongKeyRepository(context))
            {
                var uow = new UnityOfWork(context, Substitute.For<ILogProvider>());

                var result = repository.AddOrUpdateAsync(new TabelaTesteWithLongKey()
                {
                    Propriedade = Guid.NewGuid().ToString()
                }).Result;

                var alteracoes = uow.SaveAndCommitAsync().Result;
                Assert.IsTrue(alteracoes == 1);

                var chave = repository.FindAsync(p => p.Propriedade == result.Propriedade).Result.FirstOrDefault();
                Assert.NotNull(chave);
                Assert.IsTrue(chave.Id > 0);

                var registro = repository.GetByIdAsync(chave.Id).Result;
                Assert.NotNull(registro);

                registro.Propriedade = "Alterada";
                var saveResult = uow.SaveAndCommitAsync().Result;

                registro = repository.GetByIdAsync(chave.Id).Result;
                Assert.NotNull(registro);
                Assert.AreEqual(registro.Propriedade, "Alterada");
            }
        }

        [Test]
        public void Repository_Can_Update_Attached()
        {
            using (var context = new InMemoryContext())
            using (IRepository<TabelaTesteWithLongKey, long> repository = new TabelaTesteWithLongKeyRepository(context))
            {
                var uow = new UnityOfWork(context, Substitute.For<ILogProvider>());

                repository.AddOrUpdate(new TabelaTesteWithLongKey()
                {
                    Propriedade = Guid.NewGuid().ToString()
                });

                var alteracoes = uow.SaveAndCommit();
                Assert.IsTrue(alteracoes == 1);

                var registro = repository.GetAll().FirstOrDefault();
                Assert.NotNull(registro);
                Assert.IsTrue(registro.Id > 0);

                registro.Propriedade = "Alterada_123";

                repository.AddOrUpdate(registro);
                uow.SaveAndCommit();
                Assert.IsTrue(alteracoes == 1);
                Assert.AreEqual(registro.Propriedade, "Alterada_123");
            }
        }

        [Test]
        public void Repository_Can_Update_AttachedAsync()
        {
            using (var context = new InMemoryContext())
            using (IRepository<TabelaTesteWithLongKey, long> repository = new TabelaTesteWithLongKeyRepository(context))
            {
                var uow = new UnityOfWork(context, Substitute.For<ILogProvider>());

                var result = repository.AddOrUpdateAsync(new TabelaTesteWithLongKey()
                {
                    Propriedade = Guid.NewGuid().ToString()
                }).Result;

                var alteracoes = uow.SaveAndCommitAsync().Result;
                Assert.IsTrue(alteracoes == 1);

                var registro = repository.FindAsync(p => p.Propriedade == result.Propriedade).Result.FirstOrDefault();
                Assert.NotNull(registro);
                Assert.IsTrue(registro.Id > 0);

                registro.Propriedade = "Alterada_123";

                var saveResult = repository.AddOrUpdateAsync(registro).Result;
                var contextSaveResult = uow.SaveAndCommitAsync().Result;
                Assert.IsTrue(alteracoes == 1);
                Assert.AreEqual(registro.Propriedade, "Alterada_123");
            }
        }

        [Test]
        public void Repository_Can_Update_Detached()
        {
            using (var context = new InMemoryContext())
            using (IRepository<TabelaTesteWithLongKey, long> repository = new TabelaTesteWithLongKeyRepository(context))
            {
                var uow = new UnityOfWork(context, Substitute.For<ILogProvider>());

                repository.AddOrUpdate(new TabelaTesteWithLongKey()
                {
                    Propriedade = Guid.NewGuid().ToString()
                });

                var alteracoes = uow.SaveAndCommit();
                Assert.IsTrue(alteracoes == 1);

                var chave = repository.GetAll().FirstOrDefault();
                Assert.NotNull(chave);
                Assert.IsTrue(chave.Id > 0);

                var registro = new TabelaTesteWithLongKey()
                {
                    Id = chave.Id,
                    Propriedade = "Alterada_123"
                };

                repository.AddOrUpdate(registro);
                uow.SaveAndCommit();
                Assert.IsTrue(alteracoes == 1);

                registro = repository.GetById(chave.Id);
                Assert.NotNull(registro);
                Assert.AreEqual(registro.Propriedade, "Alterada_123");
            }
        }

        [Test]
        public void Repository_Can_Update_DetachedAsync()
        {
            using (var context = new InMemoryContext())
            using (IRepository<TabelaTesteWithLongKey, long> repository = new TabelaTesteWithLongKeyRepository(context))
            {
                var uow = new UnityOfWork(context, Substitute.For<ILogProvider>());

                var result = repository.AddOrUpdateAsync(new TabelaTesteWithLongKey()
                {
                    Propriedade = Guid.NewGuid().ToString()
                }).Result;

                var alteracoes = uow.SaveAndCommitAsync().Result;
                Assert.IsTrue(alteracoes == 1);

                var chave = repository.FindAsync(p => p.Propriedade == result.Propriedade).Result.FirstOrDefault();
                Assert.NotNull(chave);
                Assert.IsTrue(chave.Id > 0);

                var registro = new TabelaTesteWithLongKey()
                {
                    Id = chave.Id,
                    Propriedade = "Alterada_123"
                };

                registro = repository.AddOrUpdateAsync(registro).Result;
                var contextSaveResult = uow.SaveAndCommitAsync().Result;
                Assert.IsTrue(alteracoes == 1);

                registro = repository.GetByIdAsync(registro.Id).Result;
                Assert.NotNull(registro);
                Assert.AreEqual(registro.Propriedade, "Alterada_123");
            }
        }

        [Test]
        public void Repository_Can_Remove_ByEntity()
        {
            using (var context = new InMemoryContext())
            using (IRepository<TabelaTesteWithLongKey, long> repository = new TabelaTesteWithLongKeyRepository(context))
            {
                var uow = new UnityOfWork(context, Substitute.For<ILogProvider>());

                repository.AddOrUpdate(new TabelaTesteWithLongKey()
                {
                    Propriedade = Guid.NewGuid().ToString()
                });

                var alteracoes = uow.SaveAndCommit();
                Assert.IsTrue(alteracoes == 1);

                var registro = repository.GetAll().FirstOrDefault();
                Assert.NotNull(registro);
                Assert.IsTrue(registro.Id > 0);

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
            using (IRepository<TabelaTesteWithLongKey, long> repository = new TabelaTesteWithLongKeyRepository(context))
            {
                var uow = new UnityOfWork(context, Substitute.For<ILogProvider>());

                var result = repository.AddOrUpdateAsync(new TabelaTesteWithLongKey()
                {
                    Propriedade = Guid.NewGuid().ToString()
                }).Result;

                var alteracoes = uow.SaveAndCommitAsync().Result;
                Assert.IsTrue(alteracoes == 1);

                var registro = repository.FindAsync(p => p.Propriedade == result.Propriedade).Result.FirstOrDefault();
                Assert.NotNull(registro);
                Assert.IsTrue(registro.Id > 0);

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
            using (IRepository<TabelaTesteWithLongKey, long> repository = new TabelaTesteWithLongKeyRepository(context))
            {
                var uow = new UnityOfWork(context, Substitute.For<ILogProvider>());

                var registro = new TabelaTesteWithLongKey()
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
            using (IRepository<TabelaTesteWithLongKey, long> repository = new TabelaTesteWithLongKeyRepository(context))
            {
                var uow = new UnityOfWork(context, Substitute.For<ILogProvider>());

                var registro = new TabelaTesteWithLongKey()
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
            using (var readOnlyRepository = new TabelaTesteReadOnlyWithLongKeyRepository(context))
            {
                context.TabelaTesteWithLongKeys.Add(new TabelaTesteWithLongKey()
                {
                    Propriedade = Guid.NewGuid().ToString()
                });

                var alteracoes = context.SaveChanges();
                Assert.IsTrue(alteracoes == 1);

                var chave = context.TabelaTesteWithLongKeys.FirstOrDefault();
                Assert.NotNull(chave);
                Assert.IsTrue(chave.Id > 0);

                var registro = readOnlyRepository.GetById(chave.Id);
                Assert.NotNull(registro);

                registro.Propriedade = "Alterada";
                context.SaveChanges();

                registro = readOnlyRepository.GetById(1);
                Assert.NotNull(registro);
                Assert.AreNotEqual(registro.Propriedade, "Alterada");
            }
        }

        [Test]
        public void Read_Only_Repository_Can_Not_WriteAsync()
        {
            using (var context = new InMemoryContext())
            using (var readOnlyRepository = new TabelaTesteReadOnlyWithLongKeyRepository(context))
            {
                context.TabelaTesteWithLongKeys.Add(new TabelaTesteWithLongKey()
                {
                    Propriedade = Guid.NewGuid().ToString()
                });

                var alteracoes = context.SaveChangesAsync().Result;
                Assert.IsTrue(alteracoes == 1);

                var chave = context.TabelaTesteWithLongKeys.FirstOrDefaultAsync().Result;
                Assert.NotNull(chave);
                Assert.IsTrue(chave.Id > 0);

                var registro = readOnlyRepository.GetByIdAsync(chave.Id).Result;
                Assert.NotNull(registro);

                registro.Propriedade = "Alterada";
                var saveResult = context.SaveChangesAsync().Result;

                registro = readOnlyRepository.GetByIdAsync(1).Result;
                Assert.NotNull(registro);
                Assert.AreNotEqual(registro.Propriedade, "Alterada");
            }
        }
    }
}
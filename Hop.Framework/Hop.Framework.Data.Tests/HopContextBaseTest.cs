using System;
using System.Linq;
using System.Threading.Tasks;
using Hop.Framework.EFCore.Tests.Infra;
using Hop.Framework.EFCore.Tests.Infra.Context;
using Hop.Framework.EFCore.Tests.Infra.Models;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Hop.Framework.EFCore.Tests
{
    [TestFixture]
    public class HopContextBaseTest
    {
        [Test]
        public void Context_With_Long_and_Read_and_Write()
        {
            using (var context = new InMemoryContext())
            {
                var propName = Guid.NewGuid().ToString();

                context.TabelaTesteWithLongKeys.Add(new TabelaTesteWithLongKey()
                {
                    Propriedade = propName
                });

                var alteracoes = context.SaveChanges();
                Assert.IsTrue(alteracoes == 1);

                var registro = context.TabelaTesteWithLongKeys.FirstOrDefault(p => p.Propriedade == propName);
                Assert.NotNull(registro);
                Assert.IsTrue(registro.Id > 0);
            }
        }

        [Test]
        public void Context_With_Long_and_Read_and_WriteAsync()
        {
            using (var context = new InMemoryContext())
            {
                var propName = Guid.NewGuid().ToString();

                context.TabelaTesteWithLongKeys.Add(new TabelaTesteWithLongKey()
                {
                    Propriedade = propName
                });

                var alteracoes = context.SaveChangesAsync().Result;
                Assert.IsTrue(alteracoes == 1);

                var registro = context.TabelaTesteWithLongKeys.FirstOrDefaultAsync(p => p.Propriedade == propName).Result;
                Assert.NotNull(registro);
                Assert.IsTrue(registro.Id > 0);
            }
        }

        [Test]
        public void Context_With_Guid_and_Read_and_Write()
        {
            using (var context = new InMemoryContext())
            {
                var propName = Guid.NewGuid().ToString();

                context.TabelaTesteWithGuidKeys.Add(new TabelaTesteWithGuidKey()
                {
                    Propriedade = propName
                });

                var alteracoes = context.SaveChanges();
                Assert.IsTrue(alteracoes == 1);

                var registro = context.TabelaTesteWithGuidKeys.FirstOrDefault(p => p.Propriedade == propName);
                Assert.NotNull(registro);

                Assert.IsFalse(registro.Id == Guid.Empty);
            }
        }

        [Test]
        public void Context_With_Guid_and_Read_and_WriteAsync()
        {
            using (var context = new InMemoryContext())
            {
                var propName = Guid.NewGuid().ToString();

                context.TabelaTesteWithGuidKeys.Add(new TabelaTesteWithGuidKey()
                {
                    Propriedade = propName
                });

                var alteracoes = context.SaveChangesAsync().Result;
                Assert.IsTrue(alteracoes == 1);

                var registro = context.TabelaTesteWithGuidKeys.FirstOrDefaultAsync(p => p.Propriedade == propName).Result;
                Assert.NotNull(registro);
                Assert.IsFalse(registro.Id == Guid.Empty);
            }
        }

        [Test]
        public void Context_Should_Audit()
        {
            using (var context = new InMemoryContext())
            {
                var propName = Guid.NewGuid().ToString();

                var model = new TabelaTesteWithAudit()
                {
                    Propriedade = propName
                };

                context.TabelaTesteWithAudits.Add(model);

                var alteracoes = context.SaveChanges();
                Assert.IsTrue(alteracoes == 1);
                Assert.IsTrue(model.CreatedBy == UserContext.UserGuid);
                Assert.IsTrue(model.CreatedDate.Date == DateTime.Today);

                var registro = context.TabelaTesteWithAudits.FirstOrDefault();
                Assert.NotNull(registro);

                registro.Propriedade += "_alterada";

                alteracoes = context.SaveChanges();
                Assert.IsTrue(alteracoes == 1);
                Assert.IsTrue(model.UpdatedBy == UserContext.UserGuid);
                Assert.IsTrue(model.UpdatedDate.HasValue);
                Assert.IsTrue(model.UpdatedDate.Value.Date == DateTime.Today);
            }
        }

        [Test]
        public void Context_Should_AuditAsync()
        {
            using (var context = new InMemoryContext())
            {
                var propName = Guid.NewGuid().ToString();

                var model = new TabelaTesteWithAudit()
                {
                    Propriedade = propName
                };

                var addResult = context.TabelaTesteWithAudits.AddAsync(model).Result;

                var alteracoes = context.SaveChangesAsync().Result;
                Assert.IsTrue(alteracoes == 1);
                Assert.IsTrue(model.CreatedBy == UserContext.UserGuid);
                Assert.IsTrue(model.CreatedDate.Date == DateTime.Today);

                model = context.TabelaTesteWithAudits.FirstOrDefaultAsync().Result;
                Assert.NotNull(model);

                model.Propriedade += "_alterada";

                alteracoes = context.SaveChangesAsync().Result;
                Assert.IsTrue(alteracoes == 1);
                Assert.IsTrue(model.UpdatedBy == UserContext.UserGuid);
                Assert.IsTrue(model.UpdatedDate.HasValue);
                Assert.IsTrue(model.UpdatedDate.Value.Date == DateTime.Today);
            }
        }
    }
}
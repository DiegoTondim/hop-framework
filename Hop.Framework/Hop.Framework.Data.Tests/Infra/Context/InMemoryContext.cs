using Hop.Framework.Core.Date;
using Hop.Framework.EFCore.Context;
using Hop.Framework.EFCore.Tests.Infra.Models;
using Microsoft.EntityFrameworkCore;

namespace Hop.Framework.EFCore.Tests.Infra.Context
{
	public class InMemoryContext : HopContextBase
	{
		public InMemoryContext() : base(new UserContextService(), new DateProvider(), GenerateDefaultOptions())
		{

		}
		public DbSet<TabelaTesteSoftDeleteWithGuidKey> TabelaTesteSoftDeleteWithGuidKeys { get; set; }
		public DbSet<TabelaTesteWithLongKey> TabelaTesteWithLongKeys { get; set; }
		public DbSet<TabelaTesteWithGuidKey> TabelaTesteWithGuidKeys { get; set; }
		public DbSet<TabelaTesteWithAudit> TabelaTesteWithAudits { get; set; }

		private static DbContextOptions<InMemoryContext> GenerateDefaultOptions()
		{
			return new DbContextOptionsBuilder<InMemoryContext>()
				.UseInMemoryDatabase(databaseName: "Add_writes_to_database")
				.Options;
		}
	}
}
using Hop.Framework.Core.User;
using Hop.Framework.EFCore.Context;
using Hop.Net5WebApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hop.Net5WebApi.Infra.Contexts
{
    public class DbContext : HopContextBase
    {
        public DbContext() : base(new UserContextService(), GenerateDefaultOptions())
        {

        }
        public DbSet<PersonEntity> Person { get; set; }

        private static DbContextOptions<DbContext> GenerateDefaultOptions()
        {
            return new DbContextOptionsBuilder<DbContext>()
                .UseInMemoryDatabase(databaseName: "Add_writes_to_database")
                .Options;
        }
    }
}

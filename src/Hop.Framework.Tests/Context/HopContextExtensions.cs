using Microsoft.EntityFrameworkCore;

namespace Hop.Framework.UnitTests.Context
{
    public static class HopContextExtensions
    {
        public static DbContextOptions<T> InMemoryDefaultOptions<T>(string databaseName) where T : DbContext
        {
            return new DbContextOptionsBuilder<T>()
                .UseInMemoryDatabase(databaseName: databaseName)
                .Options;
        }
    }
}

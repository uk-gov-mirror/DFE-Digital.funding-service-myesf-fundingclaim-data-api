using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pds.FundingClaim.Repositories.Data;

namespace Pds.FundingClaim.Repositories.Tests
{
    public class InMemoryDbContextFactory
    {
        public PdsContext GetPdsDbContext()
        {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            var options = new DbContextOptionsBuilder<PdsContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryPdsDatabase")
                .UseInternalServiceProvider(serviceProvider)
                .Options;

            var dbContext = new PdsContext(options);

            return dbContext;
        }
    }
}
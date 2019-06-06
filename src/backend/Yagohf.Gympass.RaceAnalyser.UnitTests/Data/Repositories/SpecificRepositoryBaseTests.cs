using Microsoft.EntityFrameworkCore;
using Yagohf.Gympass.RaceAnalyser.Data.Context;

namespace Yagohf.Gympass.RaceAnalyser.UnitTests.Data.Repositories
{
    public class SpecificRepositoryBaseTests
    {
        protected RaceAnalyserContext CreateContext(string dbName = null)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<RaceAnalyserContext>()
               .UseInMemoryDatabase(databaseName: dbName ?? "DB_CREATION_TEST")
               .Options;

            //Act.
            RaceAnalyserContext context = new RaceAnalyserContext(options);
            return context;
        }
    }
}

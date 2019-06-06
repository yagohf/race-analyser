using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yagohf.Gympass.RaceAnalyser.Data.Context;
using Yagohf.Gympass.RaceAnalyser.Model.Entities;

namespace Yagohf.Gympass.RaceAnalyser.UnitTests.Data.Context
{
    [TestClass]
    public class RaceAnalyserContextTests
    {
        [TestMethod]
        public void Testar_Criacao()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<RaceAnalyserContext>()
               .UseInMemoryDatabase(databaseName: "DB_CREATION_TEST")
               .Options;

            //Act.
            RaceAnalyserContext context = new RaceAnalyserContext(options);
            var setRaces = context.Set<Race>(); //Forçar a inicialização (OnModelCreating)

            //Assert.
            Assert.IsNotNull(context);
            Assert.IsNotNull(setRaces);
        }
    }
}

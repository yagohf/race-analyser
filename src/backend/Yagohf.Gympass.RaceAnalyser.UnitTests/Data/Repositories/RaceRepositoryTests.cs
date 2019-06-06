using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yagohf.Gympass.RaceAnalyser.Data.Repositories;

namespace Yagohf.Gympass.RaceAnalyser.UnitTests.Data.Repositories
{
    [TestClass]
    public class RaceRepositoryTests : SpecificRepositoryBaseTests
    {
        [TestMethod]
        public void Testar_Instance()
        {
            //Arrange.

            //Act.
            RaceRepository repository = new RaceRepository(this.CreateContext());

            //Assert.
            Assert.IsNotNull(repository);
        }
    }
}

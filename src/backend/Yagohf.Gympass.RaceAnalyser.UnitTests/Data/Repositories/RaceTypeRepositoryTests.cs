using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yagohf.Gympass.RaceAnalyser.Data.Repositories;

namespace Yagohf.Gympass.RaceAnalyser.UnitTests.Data.Repositories
{
    [TestClass]
    public class RaceTypeRepositoryTests : SpecificRepositoryBaseTests
    {
        [TestMethod]
        public void Testar_Instance()
        {
            //Arrange.

            //Act.
            RaceTypeRepository repository = new RaceTypeRepository(this.CreateContext());

            //Assert.
            Assert.IsNotNull(repository);
        }
    }
}

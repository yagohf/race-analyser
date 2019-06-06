using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yagohf.Gympass.RaceAnalyser.Data.Repositories;

namespace Yagohf.Gympass.RaceAnalyser.UnitTests.Data.Repositories
{
    [TestClass]
    public class LapRepositoryTests : SpecificRepositoryBaseTests
    {
        [TestMethod]
        public void Testar_Instance()
        {
            //Arrange.

            //Act.
            LapRepository repository = new LapRepository(this.CreateContext());

            //Assert.
            Assert.IsNotNull(repository);
        }
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yagohf.Gympass.RaceAnalyser.Data.Repositories;

namespace Yagohf.Gympass.RaceAnalyser.UnitTests.Data.Repositories
{
    [TestClass]
    public class DriverResultRepositoryTests : SpecificRepositoryBaseTests
    {
        [TestMethod]
        public void Testar_Instance()
        {
            //Arrange.

            //Act.
            DriverResultRepository repository = new DriverResultRepository(this.CreateContext());

            //Assert.
            Assert.IsNotNull(repository);
        }
    }
}

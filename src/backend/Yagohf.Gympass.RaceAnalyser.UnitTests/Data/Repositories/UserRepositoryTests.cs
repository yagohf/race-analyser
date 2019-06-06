using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yagohf.Gympass.RaceAnalyser.Data.Repositories;

namespace Yagohf.Gympass.RaceAnalyser.UnitTests.Data.Repositories
{
    [TestClass]
    public class UserRepositoryTests : SpecificRepositoryBaseTests
    {
        [TestMethod]
        public void Testar_Instance()
        {
            //Arrange.

            //Act.
            UserRepository repository = new UserRepository(this.CreateContext());

            //Assert.
            Assert.IsNotNull(repository);
        }
    }
}

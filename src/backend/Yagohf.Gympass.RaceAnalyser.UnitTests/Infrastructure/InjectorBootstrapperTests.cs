using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yagohf.Gympass.RaceAnalyser.Injector;

namespace Yagohf.Gympass.RaceAnalyser.UnitTests.Infrastructure
{
    [TestClass]
    public class InjectorBootstrapperTests
    {
        [TestMethod]
        public void Testar_RegisterServices()
        {
            //Arrange.
            IServiceCollection serviceCollection = new ServiceCollection();

            //Act.
            InjectorBootstrapper.RegisterServices(serviceCollection, TestUtil.GetConfiguration());
            var serviceProvider = serviceCollection.BuildServiceProvider();

            //Assert.
            Assert.IsNotNull(serviceProvider);
        }
    }
}

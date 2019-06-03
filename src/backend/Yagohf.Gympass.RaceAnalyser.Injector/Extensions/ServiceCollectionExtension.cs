using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Yagohf.Gympass.RaceAnalyser.Injector.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void AddInjectorBootstrapper(this IServiceCollection services, IConfiguration configuration)
        {
            InjectorBootstrapper.RegisterServices(services, configuration);
        }
    }
}

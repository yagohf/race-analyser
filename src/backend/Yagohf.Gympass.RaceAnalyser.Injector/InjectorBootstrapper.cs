using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Yagohf.Gympass.RaceAnalyser.Data.Context;
using Yagohf.Gympass.RaceAnalyser.Data.Interface.Queries;
using Yagohf.Gympass.RaceAnalyser.Data.Interface.Repositories;
using Yagohf.Gympass.RaceAnalyser.Data.Queries;
using Yagohf.Gympass.RaceAnalyser.Data.Repositories;
using Yagohf.Gympass.RaceAnalyser.Services.Domain;
using Yagohf.Gympass.RaceAnalyser.Services.Helper;
using Yagohf.Gympass.RaceAnalyser.Services.Interface.Domain;
using Yagohf.Gympass.RaceAnalyser.Services.Interface.Helper;
using Yagohf.Gympass.RaceAnalyser.Services.MappingProfiles;

namespace Yagohf.Gympass.RaceAnalyser.Injector
{
    public static class InjectorBootstrapper
    {
        private const string RACEANALYSERDB_CONNECTION_STRING = "RaceAnalyserDB";

        public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
        {
            //Context EF Core
            services.AddDbContext<RaceAnalyserContext>(opt =>
            {
                opt.UseSqlServer(configuration.GetConnectionString(RACEANALYSERDB_CONNECTION_STRING));
            });

            //Data - Repository
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRaceRepository, RaceRepository>();
            services.AddScoped<IRaceTypeRepository, RaceTypeRepository>();

            //Data - Query
            services.AddScoped<IUserQuery, UserQuery>();
            services.AddScoped<IRaceQuery, RaceQuery>();

            //Service - Helper
            services.AddScoped<ITokenHelper, TokenHelper>();

            //Service - Domain
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRaceService, RaceService>();

            //Automapper
            MapperConfiguration mapperConfiguration = new MapperConfiguration(mConfig =>
            {
                mConfig.AddProfile(new ServiceMapperProfile());
            });

            IMapper mapper = mapperConfiguration.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Yagohf.Gympass.RaceAnalyser.Api.Infrastructure.Filters;
using Yagohf.Gympass.RaceAnalyser.Infrastructure.Configuration;
using Yagohf.Gympass.RaceAnalyser.Injector.Extensions;

namespace Yagohf.Gympass.RaceAnalyser.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Adicionar suporte a CORS (Cross-Origin Resource Sharing).
            services.AddCors(corsOptions =>
            {
                corsOptions.AddPolicy("CorsPolicy",
                   builder => builder.AllowAnyOrigin()
                              .AllowAnyMethod()
                              .AllowAnyHeader()
                              .AllowCredentials());
            });

            services.AddMvc(config =>
            {
                //Filters.
                config.Filters.Add<ApiExceptionFilter>();
            });

            //Adicionar injeção de dependência delegada para outra camada.
            services.AddInjectorBootstrapper(this.Configuration);

            //Setar configurações fortemente tipadas.
            var authSection = Configuration.GetSection("Authentication");
            services.Configure<AuthenticationSettings>(authSection);

            //Setar configurações fortemente tipadas.
            var raceFileSettingsSection = Configuration.GetSection("RaceFile");
            services.Configure<RaceFileSettings>(raceFileSettingsSection);

            //Configurar autenticação com JWT.
            AuthenticationSettings authSettings = authSection.Get<AuthenticationSettings>();
            byte[] encriptionKey = Encoding.ASCII.GetBytes(authSettings.EncriptionKey);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(encriptionKey),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            //Swagger.
            services.AddSwaggerGen(cfg =>
            {
                cfg.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info()
                {
                    Title = "RaceAnalyser API",
                    Version = "v1",
                    Description = "API da aplicação RaceAnalyser, utilizada para analisar resultados de corridas."
                });

                cfg.IncludeXmlComments(BuildPathSwaggerXmlDocument());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();
            app.UseCors("CorsPolicy");
            app.UseMvc();
            app.UseSwagger((c) =>
            {
                //Tratamento para setar o basepath no Swagger.
                string basepath = "/api/v1";
                c.PreSerializeFilters.Add((swaggerDoc, httpReq) => swaggerDoc.BasePath = basepath);

                c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
                {
                    IDictionary<string, PathItem> paths = new Dictionary<string, PathItem>();
                    foreach (var path in swaggerDoc.Paths)
                    {
                        paths.Add(path.Key.Replace(basepath, ""), path.Value);
                    }

                    swaggerDoc.Paths = paths;
                });
            });

            app.UseSwaggerUI(cfg =>
            {
                cfg.SwaggerEndpoint("/swagger/v1/swagger.json", "RaceAnalyser API - v1");
            });
        }

        #region [ Helpers ]
        private string BuildPathSwaggerXmlDocument()
        {
            string applicationPath = PlatformServices.Default.Application.ApplicationBasePath;
            string applicationName = PlatformServices.Default.Application.ApplicationName;
            string xmlConfigFileFullPath = Path.Combine(applicationPath, $"{applicationName}.xml");

            return xmlConfigFileFullPath;
        }
        #endregion
    }
}

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using System;
using System.IO;

namespace Yagohf.Gympass.RaceAnalyser.Api
{
    public class Program
    {
        private const string CONFIG_FILE_NAME = "appsettings.json";
        private const string LOGDB_CONNECTION_STRING = "LogDB";

        public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
          .SetBasePath(Directory.GetCurrentDirectory())
          .AddJsonFile(CONFIG_FILE_NAME, optional: false, reloadOnChange: true)
          .AddEnvironmentVariables()
          .Build();


        public static void Main(string[] args)
        {
            ConfigurarSerilog();

            try
            {
                Log.Information("Main - Startando aplicação...");
                BuildWebHost(args).Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Main - Aplicação encontrou uma exceção e encerrou a execução...");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            var webHostBuilder = WebHost.CreateDefaultBuilder(args)
                .UseSerilog()
                .UseStartup<Startup>()
                .UseConfiguration(Configuration)
                .Build();

            return webHostBuilder;
        }

        #region [ Helpers ]
        private static void ConfigurarSerilog()
        {
            var serilogColumnOptions = new ColumnOptions();

            //Remover coluna de XML.
            serilogColumnOptions.Store.Remove(StandardColumn.Properties);

            //Adicionar coluna com objeto JSON.
            serilogColumnOptions.Store.Add(StandardColumn.LogEvent);

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .Enrich.FromLogContext()
                .WriteTo.MSSqlServer(
                    connectionString: Configuration.GetConnectionString(LOGDB_CONNECTION_STRING),
                    tableName: Configuration.GetSection("Serilog:LogTableName").Value ?? "Log",
                    autoCreateSqlTable: false,
                    columnOptions: serilogColumnOptions)
                .CreateLogger();
        }
        #endregion
    }
}

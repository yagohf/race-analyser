using Microsoft.Extensions.Configuration;
using System.IO;

namespace Yagohf.Gympass.RaceAnalyser.UnitTests
{
    public static class TestUtil
    {
        private static IConfiguration _configuration;

        static TestUtil()
        {
            _configuration = new ConfigurationBuilder()
                      .SetBasePath(Directory.GetCurrentDirectory())
                      .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                      .AddEnvironmentVariables()
                      .Build();
        }

        public static IConfiguration GetConfiguration()
        {
            return _configuration;
        }
    }
}

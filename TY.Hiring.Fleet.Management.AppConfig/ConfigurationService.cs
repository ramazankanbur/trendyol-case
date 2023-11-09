using Microsoft.Extensions.Configuration;
using TY.Hiring.Fleet.Management.Model.Enums;

namespace TY.Hiring.Fleet.Management.AppConfig
{
    public static class ConfigurationService
    {
        private static IConfiguration _configuration;

        public static void Configure(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public static string GetConfiguration(string key)
        {
            return _configuration.GetSection(key).Value;
        }

        public static string GetConnectionString()
        {
            var hostEnv = GetEnv();

            switch (hostEnv)
            {
                case HostEnv.Test:
                    return _configuration.GetConnectionString("Sqlite");
                case HostEnv.Preprod:
                case HostEnv.Prod:
                    return _configuration.GetConnectionString("LocalDb");
                default:
                    return default;
            }
        }

        public static HostEnv GetEnv()
        {
            return Enum.Parse<HostEnv>(GetConfiguration("HostEnv"));
        }
    }
}

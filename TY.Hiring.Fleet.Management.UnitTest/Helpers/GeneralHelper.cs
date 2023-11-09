using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TY.Hiring.Fleet.Management.UnitTest.Helpers
{
    public static class GeneralHelper
    {
        private static bool ConfigurationsBuildStatus = false;

        public static void BuildConfigurations(Action<ConfigurationBuilder> action)
        {
            if (ConfigurationsBuildStatus) return;
            ConfigurationBuilder configurationBuilder = new();
            action(configurationBuilder);
            ConfigurationsBuildStatus = true;
        }

        public static IServiceProvider GetDefaultServiceProvider(Action<IServiceCollection>? configureServices = null)
        {
            ServiceCollection serviceCollectionBuilder = new();

            configureServices?.Invoke(serviceCollectionBuilder);

            ServiceProvider serviceProvider = serviceCollectionBuilder.BuildServiceProvider();

            return serviceProvider;
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TY.Hiring.Fleet.Management.AppConfig;
using TY.Hiring.Fleet.Management.Model.Enums;

namespace TY.Hiring.Fleet.Management.Data.ORM.EF
{
    public static class ServiceExtensions
    {
        public static void AddDataLayer(this IServiceCollection services)
        {
            services.AddScoped<DbContext, TYHiringFleetManagementContext>();

            var hostEnv = ConfigurationService.GetEnv();

            switch (hostEnv)
            {
                case HostEnv.Dev:
                    services.AddDbContext<TYHiringFleetManagementContext>(option => option.UseInMemoryDatabase("FleetManagement"));
                    break;
                case HostEnv.Test:
                    services.AddDbContext<TYHiringFleetManagementContext>(option => option.UseSqlite(ConfigurationService.GetConnectionString()));
                    break;
                case HostEnv.Preprod:
                case HostEnv.Prod:
                    services.AddDbContext<TYHiringFleetManagementContext>(option => option.UseSqlServer(ConfigurationService.GetConnectionString()));
                    break;
                default:
                    services.AddDbContext<TYHiringFleetManagementContext>(option => option.UseSqlServer(ConfigurationService.GetConnectionString()));
                    break;
            }
        }
    }
}

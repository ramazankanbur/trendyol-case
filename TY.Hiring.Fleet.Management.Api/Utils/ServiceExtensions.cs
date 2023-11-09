using TY.Hiring.Fleet.Management.AppConfig;
using TY.Hiring.Fleet.Management.Data.ORM.EF.Repository;
using TY.Hiring.Fleet.Management.Data.ORM.EF.UOW;
using TY.Hiring.Fleet.Management.Service;
using TY.Hiring.Fleet.Management.Service.Interface;

namespace TY.Hiring.Fleet.Management.Api.Utils
{
    public static class ServiceExtensions
    {
        public static void AddAppServices(this IServiceCollection services)
        {
            services.AddScoped<IPackageStatusTypeService, PackageStatusTypeService>();

            services.AddScoped<ISackStatusTypeService, SackStatusTypeService>();

            services.AddScoped<IDeliveryPointTypeService, DeliveryPointTypeService>();

            services.AddScoped<IPackageService, PackageService>();

            services.AddScoped<ISackService, SackService>();

            services.AddScoped<IVehicleService, VehicleService>();

            services.AddScoped<ILogService, LogService>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        }
    }
}

using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using TY.Hiring.Fleet.Management.AppConfig;
using TY.Hiring.Fleet.Management.Data.ORM.EF;
using TY.Hiring.Fleet.Management.Model.Enums;

namespace TY.Hiring.Fleet.Management.Api.Utils
{
    public static class MsSqlDbInitializer
    {
        public static void InitializeDb(WebApplication app)
        {
            var hostEnv = ConfigurationService.GetEnv();
            switch (hostEnv)
            {
                case HostEnv.Test:
                    using (var serviceScope = app.Services.CreateScope())
                    using (var context = serviceScope.ServiceProvider.GetService<TYHiringFleetManagementContext>()!)
                    {
                        if ((context.Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator).Exists())
                        {
                            context.Database.EnsureDeleted();
                        }
                        if (!context.Database.EnsureCreated())
                            if (!context.Database.CanConnect())
                                throw new Exception("Database was not created!");
                    }
                    break;
                default:
                    break;
            }
        }
    }
}

using AutoMapper;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using TY.Hiring.Fleet.Management.AppConfig;
using TY.Hiring.Fleet.Management.Data.ORM.EF.UOW;
using TY.Hiring.Fleet.Management.UnitTest.Helpers;
using TY.Hiring.Fleet.Management.Mapper;

namespace TY.Hiring.Fleet.Management.UnitTest.Services
{
    internal class ServiceTestBase
    {
        protected readonly IServiceProvider services;
        protected IMapper mapper;
        protected Mock<IUnitOfWork> unitOfWorkMock;

        public ServiceTestBase()
        {
            // some services we may need are registering
            PrepareServiceProvider(ref services);

            // overriding settings with appsettings.json file which is in test assembly
            GeneralHelper.BuildConfigurations(configurationBuilder =>
            {
                configurationBuilder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                ConfigurationService.Configure(configurationBuilder.Build());
            });
        }

        protected void PrepareServiceProvider(ref IServiceProvider services)
        {
            services = GeneralHelper.GetDefaultServiceProvider(serviceCollection =>
            {
                serviceCollection.AddAutoMapper(typeof(EntryPoint));
            });

            mapper = services.GetService<IMapper>();

            unitOfWorkMock = GetUnitOfWorkMock();

        }

        protected static Mock<IUnitOfWork> GetUnitOfWorkMock()
        {
            var uowMock = new Mock<IUnitOfWork>();
            var dbTransactionMock = new Mock<IDbContextTransaction>();
            uowMock.Setup(x => x.BeginNewTransaction()).Returns(Task.FromResult(dbTransactionMock.Object));
            uowMock.Setup(x => x.RollBackTransaction(dbTransactionMock.Object)).Returns(Task.FromResult(true));
            uowMock.Setup(x => x.TransactionCommit(dbTransactionMock.Object)).Returns(Task.CompletedTask);
            uowMock.Setup(x => x.SaveChangesAsync()).Returns(Task.FromResult(1));
            return uowMock;
        }
    }
}

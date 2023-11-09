using MockQueryable.Moq;
using Moq;
using TY.Hiring.Fleet.Management.Data.ORM.EF.Models;
using TY.Hiring.Fleet.Management.Data.ORM.EF.Repository;
using TY.Hiring.Fleet.Management.Model.Models.Dtos;
using TY.Hiring.Fleet.Management.Service;

namespace TY.Hiring.Fleet.Management.UnitTest.Services
{
    internal class LogServiceTest : ServiceTestBase
    {
        #region dependencies
        private Mock<IRepository<Log>> logRepositoryMock;
        #endregion

        public LogServiceTest()
        {
            InitializeMockObjects();
        }

        private void InitializeMockObjects()
        {
            logRepositoryMock = new Mock<IRepository<Log>>();
        }

        [SetUp]
        public void Setup()
        {
            unitOfWorkMock.Setup(x => x.GetRepository<Log>()).Returns(logRepositoryMock.Object);
        }

        [Test]
        public async Task CreateList_ShouldReturnTrue_WhenLogCreate()
        {
            var mockLogListRequest = new List<LogDTO>() { new LogDTO() { Barcode = "PackageBarcode" }, new LogDTO() { Barcode = "SackBarcode" } };

            var logServie = new LogService(unitOfWorkMock.Object, mapper);

            var createResult = await logServie.CreateList(mockLogListRequest);

            Assert.That(createResult, Is.True);
        }

        [Test]
        public async Task GetLogsAsnyc_ShouldReturnLogs_WhenGetLogs()
        {
            var page = 1;
            var size = 50;
            var mockDatas = new List<Log>() { new Log() { Barcode = "PackageBarcode" } }.AsQueryable().BuildMock();

            logRepositoryMock.Setup(x => x.GetAll()).Returns(mockDatas).Verifiable();

            var logService = new LogService(unitOfWorkMock.Object, mapper);
            
            var result = await logService.GetLogsAsnyc(page,size);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Has.Count.AtLeast(1));
            Assert.That(result, Has.Count.AtMost(size));
        }
    }
}

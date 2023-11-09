using MockQueryable.Moq;
using Moq;
using TY.Hiring.Fleet.Management.Data.ORM.EF.Models;
using TY.Hiring.Fleet.Management.Data.ORM.EF.Repository;
using TY.Hiring.Fleet.Management.Service;

namespace TY.Hiring.Fleet.Management.UnitTest.Services
{
    internal class SackServiceTest : ServiceTestBase
    {
        #region dependencies
        private Mock<IRepository<Sack>> sackRepositoryMock;
        #endregion

        public SackServiceTest()
        {
            InitializeMockObjects();
        }

        private void InitializeMockObjects()
        {
            sackRepositoryMock = new Mock<IRepository<Sack>>();
        }

        [SetUp]
        public void Setup()
        {
            unitOfWorkMock.Setup(x => x.GetRepository<Sack>()).Returns(sackRepositoryMock.Object);
        }

        [Test]
        public async Task GetSacksAsnyc_ShouldReturnSacks_WhenGetSacks()
        {
            var page = 1;
            var size = 50;

            var mockDatas = new List<Sack>() { new Sack() { Barcode = "SackBarcode" } }.AsQueryable().BuildMock();

            sackRepositoryMock.Setup(x => x.GetAll()).Returns(mockDatas).Verifiable();

            var sackService = new SackService(unitOfWorkMock.Object, mapper);

            var result = await sackService.GetSacksAsnyc(page,size);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Has.Count.AtLeast(1));
            Assert.That(result, Has.Count.AtMost(size));
        }
    }
}

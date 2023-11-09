using MockQueryable.Moq;
using Moq; 
using TY.Hiring.Fleet.Management.Data.ORM.EF.Models;
using TY.Hiring.Fleet.Management.Data.ORM.EF.Repository;
using TY.Hiring.Fleet.Management.Service;

namespace TY.Hiring.Fleet.Management.UnitTest.Services
{
    internal class PackageServiceTest : ServiceTestBase
    {
        #region dependencies
        private Mock<IRepository<Package>> packageRepositoryMock;
        #endregion

        public PackageServiceTest()
        {
            InitializeMockObjects();
        }

        private void InitializeMockObjects()
        {
            packageRepositoryMock = new Mock<IRepository<Package>>();
        }

        [SetUp]
        public void Setup()
        {
            unitOfWorkMock.Setup(x => x.GetRepository<Package>()).Returns(packageRepositoryMock.Object);
        }

        [Test]
        public async Task GetPackagesAsnyc_ShouldReturnPackages_WhenGetPackages()
        {
            var mockDatas = new List<Package>() { new Package() { Barcode = "PackageBarcode" } }.AsQueryable().BuildMock();

            packageRepositoryMock.Setup(x => x.GetAll()).Returns(mockDatas).Verifiable();

            var packageService = new PackageService(unitOfWorkMock.Object, mapper);

            var result = await packageService.GetPackagesAsnyc();

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Has.Count.AtLeast(1));
        }
    }
}

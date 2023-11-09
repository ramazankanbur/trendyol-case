using MockQueryable.Moq;
using Moq;
using TY.Hiring.Fleet.Management.Data.ORM.EF.Models;
using TY.Hiring.Fleet.Management.Data.ORM.EF.Repository;
using TY.Hiring.Fleet.Management.Service;

namespace TY.Hiring.Fleet.Management.UnitTest.Services
{
    internal class PackageStatusTypeServiceTest : ServiceTestBase
    {
        #region dependencies
        private Mock<IRepository<PackageStatusType>> packageStatusTypeRepositoryMock;

        #endregion

        public PackageStatusTypeServiceTest()
        {
            InitializeMockObjects();
        }

        private void InitializeMockObjects()
        {
            packageStatusTypeRepositoryMock = new Mock<IRepository<PackageStatusType>>();
        }


        [Test]
        public async Task GetPackageStatusTypesById_ShouldReturnNull_WhenPackageStatusTypeIdNotExistsInDb()
        {
            var notExistIdInDb = -1;

            var mockDatas = new List<PackageStatusType>().AsQueryable().BuildMock();
            packageStatusTypeRepositoryMock.Setup(x => x.GetAll()).Returns(mockDatas).Verifiable();
            unitOfWorkMock.Setup(x => x.GetRepository<PackageStatusType>()).Returns(packageStatusTypeRepositoryMock.Object);

            var packageStatusTypeService = new PackageStatusTypeService(unitOfWorkMock.Object, mapper);
            
            var result = await packageStatusTypeService.GetPackageStatusTypesByIdAsnyc(notExistIdInDb);

            Assert.That(result, Is.Null);
        }

    }
}

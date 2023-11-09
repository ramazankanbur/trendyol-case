using MockQueryable.Moq;
using Moq;
using TY.Hiring.Fleet.Management.Data.ORM.EF.Models;
using TY.Hiring.Fleet.Management.Data.ORM.EF.Repository;
using TY.Hiring.Fleet.Management.Service;

namespace TY.Hiring.Fleet.Management.UnitTest.Services
{
    internal class SackStatusTypeServiceTest : ServiceTestBase
    {
        #region dependencies
        private Mock<IRepository<SackStatusType>> sackStatusTypeRepositoryMock;
        #endregion

        public SackStatusTypeServiceTest()
        {
            InitializeMockObjects();
        }

        private void InitializeMockObjects()
        {
            sackStatusTypeRepositoryMock = new Mock<IRepository<SackStatusType>>();
        }

        [SetUp]
        public void Setup()
        {
            unitOfWorkMock.Setup(x => x.GetRepository<SackStatusType>()).Returns(sackStatusTypeRepositoryMock.Object);
        }

        [Test]
        public async Task GetSackStatusTypesAsnyc_ShouldReturnSackStatusTypes_WhenGetSackStatusTypes()
        {
            var mockDatas = new List<SackStatusType>() { new SackStatusType() {  Name = "Unload"} }.AsQueryable().BuildMock();

            sackStatusTypeRepositoryMock.Setup(x => x.GetAll()).Returns(mockDatas).Verifiable();

            var sackStatusTypeService = new SackStatusTypeService(unitOfWorkMock.Object, mapper);

            var result = await sackStatusTypeService.GetSackStatusTypesAsnyc();

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Has.Count.AtLeast(1));
        }
    }
}

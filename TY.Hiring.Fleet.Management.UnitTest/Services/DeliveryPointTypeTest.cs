using MockQueryable.Moq;
using Moq;
using TY.Hiring.Fleet.Management.Data.ORM.EF.Models;
using TY.Hiring.Fleet.Management.Data.ORM.EF.Repository;
using TY.Hiring.Fleet.Management.Service;

namespace TY.Hiring.Fleet.Management.UnitTest.Services
{
    internal class DeliveryPointTypeTest : ServiceTestBase
    {
        #region dependencies
        private Mock<IRepository<DeliveryPointType>> deliveryPointTypeRepositoryMock;
        #endregion

        public DeliveryPointTypeTest()
        {
            InitializeMockObjects();
        }

        private void InitializeMockObjects()
        {
            deliveryPointTypeRepositoryMock = new Mock<IRepository<DeliveryPointType>>();
        }

        [SetUp]
        public void Setup()
        {
            unitOfWorkMock.Setup(x => x.GetRepository<DeliveryPointType>()).Returns(deliveryPointTypeRepositoryMock.Object);
        }
         
        [Test]
        public async Task GetDeliveryPointTypeAsnyc_ShouldReturnDeliveryPointType_WhenGetDeliveryPointType()
        {
            var mockDatas = new List<DeliveryPointType>() { new DeliveryPointType() {  Name = "Branch" } }.AsQueryable().BuildMock();

            deliveryPointTypeRepositoryMock.Setup(x => x.GetAll()).Returns(mockDatas).Verifiable();

            var deliveryPointTypeService = new DeliveryPointTypeService(unitOfWorkMock.Object, mapper);

            var result = await deliveryPointTypeService.GetDeliveryPointTypesAsnyc();

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Has.Count.AtLeast(1));
        }
    }
}

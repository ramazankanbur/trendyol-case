using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using TY.Hiring.Fleet.Management.Data.ORM.EF.Models;
using TY.Hiring.Fleet.Management.Data.ORM.EF.Repository;
using TY.Hiring.Fleet.Management.Model.Models.Requests;
using TY.Hiring.Fleet.Management.Service;
using enums = TY.Hiring.Fleet.Management.Model.Enums;

namespace TY.Hiring.Fleet.Management.UnitTest.Services
{
    internal class VehicleServiceTest : ServiceTestBase
    {
        #region dependencies
        private Mock<IRepository<Sack>> sackRepositoryMock;
        private Mock<IRepository<Package>> packageRepositoryMock;
        private Mock<IRepository<SackPackage>> sackPackageRepositoryMock;
        private Mock<IRepository<Log>> logRepositoryMock;
        #endregion

        public VehicleServiceTest()
        {
            InitializeMockObjects();
        }

        private void InitializeMockObjects()
        {
            sackRepositoryMock = new Mock<IRepository<Sack>>();
            packageRepositoryMock = new Mock<IRepository<Package>>();
            sackPackageRepositoryMock = new Mock<IRepository<SackPackage>>();
            logRepositoryMock = new Mock<IRepository<Log>>();
        }

        [SetUp]
        public void Setup()
        {
            unitOfWorkMock.Setup(x => x.GetRepository<Package>()).Returns(packageRepositoryMock.Object);
            unitOfWorkMock.Setup(x => x.GetRepository<Sack>()).Returns(sackRepositoryMock.Object);
            unitOfWorkMock.Setup(x => x.GetRepository<SackPackage>()).Returns(sackPackageRepositoryMock.Object);
            unitOfWorkMock.Setup(x => x.GetRepository<Log>()).Returns(logRepositoryMock.Object);
        }


        private DistributeRequest GetMockDistributeRequest(string barcode, enums.DeliveryPointType deliveryPointType)
        {
            var packageDelivery = new DistributeRequest.Delivery() { Barcode = barcode };
            var deliveryList = new List<DistributeRequest.Delivery>() { packageDelivery };
            var routeItem = new DistributeRequest.RouteItem() { Deliveries = deliveryList, DeliveryPoint = deliveryPointType.GetHashCode() };
            var routeList = new List<DistributeRequest.RouteItem>() { routeItem };
            var mockDistributeRequest = new DistributeRequest() { Route = routeList };

            return mockDistributeRequest;
        }

        #region Branch tests
        [Test]
        public async Task DistributeDeliveries_ShouldReturnLoaded_WhenTryingToUnloadSackAtTheBranch()
        {
            //arrange
            var mockDistributeRequest = GetMockDistributeRequest("SackBarcode", enums.DeliveryPointType.Branch);

            var mockForSackResult = new List<Sack>() {
                new Sack() {
                    Barcode= "SackBarcode",
                    DeliveryPointTypeId = enums.DeliveryPointType.Branch.GetHashCode()
                }
            }.AsQueryable().BuildMock();

            sackRepositoryMock.Setup(x => x.GetAll()).Returns(mockForSackResult);

            var vehicleService = new VehicleService(unitOfWorkMock.Object);
            //action
            var result = await vehicleService.DistributeDeliveries(mockDistributeRequest);

            Assert.That(result, Is.Not.Null);
            var assertVal = result.Route.FirstOrDefault()?.Deliveries.FirstOrDefault()?.State;
            Assert.That(enums.SackStatusType.Loaded.GetHashCode(), Is.EqualTo(assertVal));
        }

        [Test]
        public async Task DistributeDeliveries_ShouldReturnUnLoaded_WhenTryingToUnloadIndividualPackageAtTheBranch()
        {
            //arrange
            var mockDistributeRequest = GetMockDistributeRequest("PackageBarcode", enums.DeliveryPointType.Branch);

            var mockForPackageResult = new List<Package>() {
                new Package() {
                    Barcode= "PackageBarcode",
                    DeliveryPointTypeId = enums.DeliveryPointType.Branch.GetHashCode()
                }
            }.AsQueryable().BuildMock();

            packageRepositoryMock.Setup(x => x.GetAll()).Returns(mockForPackageResult);

            var vehicleService = new VehicleService(unitOfWorkMock.Object);
            //action
            var result = await vehicleService.DistributeDeliveries(mockDistributeRequest);

            Assert.That(result, Is.Not.Null);
            var assertVal = result.Route.FirstOrDefault()?.Deliveries.FirstOrDefault()?.State;
            Assert.That(enums.PackageStatusType.Unloaded.GetHashCode(), Is.EqualTo(assertVal));
        }

        [Test]
        public async Task DistributeDeliveries_ShouldReturnLoaded_WhenTryingToUnloadPackageInSackAtTheBranch()
        {
            //arrange
            var mockDistributeRequest = GetMockDistributeRequest("PackageBarcode", enums.DeliveryPointType.Branch);

            var mockForPackageResult = new List<Package>() {
                new Package() {
                     Id = 1,
                    Barcode= "PackageBarcode",
                    DeliveryPointTypeId = enums.DeliveryPointType.Branch.GetHashCode()
                }
            }.AsQueryable().BuildMock();

            var mockForSackPackageResult = new List<SackPackage>() {
                new SackPackage(){ Id = 1, SackId =1, PackageId =1, Sack = new Sack(){ Id = 1, Barcode="SackBarcode" }, Package = new Package(){ Id = 1, Barcode="PackageBarcode" } }
            }.AsQueryable().BuildMock();

            packageRepositoryMock.Setup(x => x.GetAll()).Returns(mockForPackageResult);
            sackPackageRepositoryMock.Setup(x => x.GetAll()).Returns(mockForSackPackageResult);


            var vehicleService = new VehicleService(unitOfWorkMock.Object);
            //action
            var result = await vehicleService.DistributeDeliveries(mockDistributeRequest);

            Assert.That(result, Is.Not.Null);
            var assertVal = result.Route.FirstOrDefault()?.Deliveries.FirstOrDefault()?.State;
            Assert.That(enums.PackageStatusType.Loaded.GetHashCode(), Is.EqualTo(assertVal));
        }
        #endregion

        #region Distribution Centre tests
        [Test]
        public async Task DistributeDeliveries_ShouldReturnUnLoaded_WhenTryingToUnloadSackAtTheDistributionCentre()
        {
            //arrange
            var mockDistributeRequest = GetMockDistributeRequest("SackBarcode", enums.DeliveryPointType.DistributionCentre);

            var mockForSackResult = new List<Sack>() {
                new Sack() {
                    Barcode= "SackBarcode",
                    DeliveryPointTypeId = enums.DeliveryPointType.DistributionCentre.GetHashCode()
                }
            }.AsQueryable().BuildMock();

            sackRepositoryMock.Setup(x => x.GetAll()).Returns(mockForSackResult);

            var vehicleService = new VehicleService(unitOfWorkMock.Object);
            //action
            var result = await vehicleService.DistributeDeliveries(mockDistributeRequest);

            Assert.That(result, Is.Not.Null);
            var assertVal = result.Route.FirstOrDefault()?.Deliveries.FirstOrDefault()?.State;
            Assert.That(enums.SackStatusType.Unloaded.GetHashCode(), Is.EqualTo(assertVal));
        }

        [Test]
        public async Task DistributeDeliveries_ShouldReturnUnLoaded_WhenTryingToUnloadIndividualPackageAtTheDistributionCentre()
        {
            //arrange
            var mockDistributeRequest = GetMockDistributeRequest("PackageBarcode", enums.DeliveryPointType.DistributionCentre);

            var mockForPackageResult = new List<Package>() {
                new Package() {
                    Barcode= "PackageBarcode",
                    DeliveryPointTypeId = enums.DeliveryPointType.DistributionCentre.GetHashCode()
                }
            }.AsQueryable().BuildMock();

            packageRepositoryMock.Setup(x => x.GetAll()).Returns(mockForPackageResult);

            var vehicleService = new VehicleService(unitOfWorkMock.Object);
            //action
            var result = await vehicleService.DistributeDeliveries(mockDistributeRequest);

            Assert.That(result, Is.Not.Null);
            var assertVal = result.Route.FirstOrDefault()?.Deliveries.FirstOrDefault()?.State;
            Assert.That(enums.PackageStatusType.Unloaded.GetHashCode(), Is.EqualTo(assertVal));
        }

        [Test]
        public async Task DistributeDeliveries_ShouldReturnUnLoaded_WhenTryingToUnloadPackageInSackAtTheDistributionCentre()
        {
            //arrange
            var mockDistributeRequest = GetMockDistributeRequest("PackageBarcode", enums.DeliveryPointType.DistributionCentre);

            var mockForPackageResult = new List<Package>() {
                new Package() {
                    Id = 1,
                    Barcode= "PackageBarcode",
                    DeliveryPointTypeId = enums.DeliveryPointType.DistributionCentre.GetHashCode()
                }
            }.AsQueryable().BuildMock();

            var mockForSackPackageResult = new List<SackPackage>() {
                new SackPackage(){ 
                    Id = 1, 
                    SackId =1, 
                    PackageId = 1, 
                    Sack = new Sack(){ Id = 1, Barcode="SackBarcode" }, 
                    Package = new Package(){ Id = 1, Barcode="PackageBarcode" } }
            }.AsQueryable().BuildMock();

            packageRepositoryMock.Setup(x => x.GetAll()).Returns(mockForPackageResult);
            sackPackageRepositoryMock.Setup(x => x.GetAll()).Returns(mockForSackPackageResult);

            var vehicleService = new VehicleService(unitOfWorkMock.Object);
            //action
            var result = await vehicleService.DistributeDeliveries(mockDistributeRequest);

            Assert.That(result, Is.Not.Null);
            var assertVal = result.Route.FirstOrDefault()?.Deliveries.FirstOrDefault()?.State;
            Assert.That(enums.PackageStatusType.Unloaded.GetHashCode(), Is.EqualTo(assertVal));
        }
        #endregion

        #region Transfer Centre tests
        [Test]
        public async Task DistributeDeliveries_ShouldReturnUnLoaded_WhenTryingToUnloadSackAtTheTransferCentre()
        {
            //arrange
            var mockDistributeRequest = GetMockDistributeRequest("SackBarcode", enums.DeliveryPointType.TransferCentre);

            var mockForSackResult = new List<Sack>() {
                new Sack() {
                    Barcode= "SackBarcode",
                    DeliveryPointTypeId = enums.DeliveryPointType.TransferCentre.GetHashCode()
                }
            }.AsQueryable().BuildMock();

            sackRepositoryMock.Setup(x => x.GetAll()).Returns(mockForSackResult);

            var vehicleService = new VehicleService(unitOfWorkMock.Object);
            //action
            var result = await vehicleService.DistributeDeliveries(mockDistributeRequest);

            Assert.That(result, Is.Not.Null);
            var assertVal = result.Route.FirstOrDefault()?.Deliveries.FirstOrDefault()?.State;
            Assert.That(enums.SackStatusType.Unloaded.GetHashCode(), Is.EqualTo(assertVal));
        }

        [Test]
        public async Task DistributeDeliveries_ShouldReturnLoaded_WhenTryingToUnloadIndividualPackageAtTheTransferCentre()
        {
            //arrange
            var mockDistributeRequest = GetMockDistributeRequest("PackageBarcode", enums.DeliveryPointType.TransferCentre);

            var mockForPackageResult = new List<Package>() {
                new Package() {
                    Barcode="PackageBarcode",
                    DeliveryPointTypeId = enums.DeliveryPointType.TransferCentre.GetHashCode()
                }
            }.AsQueryable().BuildMock();

            var mockForSackPackageResult = new List<SackPackage>() {
                new SackPackage(){ 
                    Id = 1, 
                    SackId =1, 
                    PackageId =1, 
                    Sack = new Sack(){ Id = 1, Barcode="SackBarcode" }, 
                    Package = new Package(){ Id = 1, Barcode="PackageBarcode" } 
                }
            }.AsQueryable().BuildMock();

            packageRepositoryMock.Setup(x => x.GetAll()).Returns(mockForPackageResult);
            sackPackageRepositoryMock.Setup(x => x.GetAll()).Returns(mockForSackPackageResult);

            var vehicleService = new VehicleService(unitOfWorkMock.Object);
            //action
            var result = await vehicleService.DistributeDeliveries(mockDistributeRequest);

            Assert.That(result, Is.Not.Null);
            var assertVal = result.Route.FirstOrDefault()?.Deliveries.FirstOrDefault()?.State;
            Assert.That(enums.PackageStatusType.Loaded.GetHashCode(), Is.EqualTo(assertVal));
        }

        [Test]
        public async Task DistributeDeliveries_ShouldReturnUnLoaded_WhenTryingToUnloadPackageInSackAtTheTransferCentre()
        {
            //arrange
            var mockDistributeRequest = GetMockDistributeRequest("PackageBarcode", enums.DeliveryPointType.TransferCentre);

            var mockForPackageResult = new List<Package>() {
                new Package() {
                    Id = 1,
                    Barcode= "PackageBarcode",
                    DeliveryPointTypeId = enums.DeliveryPointType.TransferCentre.GetHashCode()
                }
            }.AsQueryable().BuildMock();

            var mockForSackPackageResult = new List<SackPackage>() {
                new SackPackage(){
                    Id = 1,
                    SackId =1,
                    PackageId = 1,
                    Sack = new Sack(){ Id = 1, Barcode="SackBarcode" },
                    Package = new Package(){ Id = 1, Barcode="PackageBarcode" } }
            }.AsQueryable().BuildMock();

            packageRepositoryMock.Setup(x => x.GetAll()).Returns(mockForPackageResult);
            sackPackageRepositoryMock.Setup(x => x.GetAll()).Returns(mockForSackPackageResult);

            var vehicleService = new VehicleService(unitOfWorkMock.Object);
            //action
            var result = await vehicleService.DistributeDeliveries(mockDistributeRequest);

            Assert.That(result, Is.Not.Null);
            var assertVal = result.Route.FirstOrDefault()?.Deliveries.FirstOrDefault()?.State;
            Assert.That(enums.PackageStatusType.Unloaded.GetHashCode(), Is.EqualTo(assertVal));
        }
        #endregion

    }
}

using TY.Hiring.Fleet.Management.Data.ORM.EF.Models;
using TY.Hiring.Fleet.Management.Data.ORM.EF.UOW;
using enums = TY.Hiring.Fleet.Management.Model.Enums;
using TY.Hiring.Fleet.Management.Model.Models.Constants;
using TY.Hiring.Fleet.Management.Model.Models.Dtos;
using TY.Hiring.Fleet.Management.Model.Models.Requests;
using TY.Hiring.Fleet.Management.Service.Interface;
using Microsoft.EntityFrameworkCore;
using TY.Hiring.Fleet.Management.Data.ORM.EF.Repository;
using TY.Hiring.Fleet.Management.Service.ParameterModels;

namespace TY.Hiring.Fleet.Management.Service
{
    public class VehicleService : IVehicleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Sack> _sackRepository;
        private readonly IRepository<Package> _packageRepository;
        private readonly IRepository<SackPackage> _sackPackageRepository;
        public VehicleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _sackRepository = _unitOfWork.GetRepository<Sack>();
            _packageRepository = _unitOfWork.GetRepository<Package>();
            _sackPackageRepository = _unitOfWork.GetRepository<SackPackage>();
        }

        public async Task<DistributeDTO> DistributeDeliveries(DistributeRequest data)
        {
            return await ProcessDeliveries(data);
        }

        private async Task<DistributeDTO> ProcessDeliveries(DistributeRequest data)
        {
            var response = new DistributeDTO();

            var transaction = await _unitOfWork.BeginNewTransaction();
            try
            {
                // deliveries barcodes merged to touch db one time
                var mergedDeliveries = data.Route.SelectMany(x => x.Deliveries).Select(x => x.Barcode).ToList();

                var deliveryDataFromDb = GetParameterToUnloadProcessFromDb(mergedDeliveries);

                foreach (var route in data.Route)
                {
                    var deliveryPoint = (enums.DeliveryPointType)route.DeliveryPoint;
                    var deliveries = route.Deliveries;

                    var distributionResult = new List<DeliveryDTO>();

                    switch (deliveryPoint)
                    {

                        case enums.DeliveryPointType.Branch:
                            distributionResult = BranchUnloadProcess(deliveryDataFromDb, deliveries);
                            LogToNotUnloadDeliveries(distributionResult.Where(x => !x.IsUnloaded), deliveryPoint);
                            break;

                        case enums.DeliveryPointType.DistributionCentre:
                            distributionResult = DistributionCentreUnloadProcess(deliveryDataFromDb, deliveries);
                            LogToNotUnloadDeliveries(distributionResult.Where(x => !x.IsUnloaded), deliveryPoint);
                            break;

                        case enums.DeliveryPointType.TransferCentre:
                            distributionResult = TransferCentreUnloadProcess(deliveryDataFromDb, deliveries);
                            LogToNotUnloadDeliveries(distributionResult.Where(x => !x.IsUnloaded), deliveryPoint);
                            break;

                        default:
                            break;
                    }

                    var deliveryResult = new List<DistributeDTO.Delivery>();

                    deliveries.ForEach(x =>
                    {
                        var delivery = new DistributeDTO.Delivery() { Barcode = x.Barcode, State = distributionResult.FirstOrDefault(dr => dr.Barcode == x.Barcode).State };
                        deliveryResult.Add(delivery);
                    });

                    response.Route.Add(new DistributeDTO.RouteItem()
                    {
                        DeliveryPoint = route.DeliveryPoint,
                        Deliveries = deliveryResult
                    });
                }
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.TransactionCommit(transaction);
            }
            catch (Exception)
            {
                if (transaction != null)
                {
                    await _unitOfWork.RollBackTransaction(transaction);
                }
                throw;
            }
            finally
            {
                _unitOfWork.Dispose();
                if (transaction != default)
                {
                    _unitOfWork.Dispose();
                }
            }
            return response;
        }

        private UnloadProcessDbResultParameterModel GetParameterToUnloadProcessFromDb(List<string> barcodeList)
        {
            var packageDbResult = _packageRepository.GetAll().Where(x => barcodeList.Contains(x.Barcode)).ToList();
            var sackDbResult = _sackRepository.GetAll().Where(x => barcodeList.Contains(x.Barcode)).ToList();
            var sackPackagesDbResult = _sackPackageRepository.GetAll().Include(x => x.Package).Include(x => x.Sack).Where(x => barcodeList.Contains(x.Package.Barcode) || barcodeList.Contains(x.Sack.Barcode)).ToList();


            return new UnloadProcessDbResultParameterModel()
            {
                Packages = packageDbResult,
                SackPackages = sackPackagesDbResult,
                Sacks = sackDbResult
            };
        }

        private List<DeliveryDTO> BranchUnloadProcess(UnloadProcessDbResultParameterModel deliveryDataFromDb, List<DistributeRequest.Delivery> deliveries)
        {
            var response = new List<DeliveryDTO>();

            var packagesToUnload = deliveryDataFromDb.Packages
                .Where(x => deliveries.Any(d => d.Barcode == x.Barcode) &&
                x.DeliveryPointTypeId == enums.DeliveryPointType.Branch.GetHashCode() &&
                // Branch: Only packages can be unloaded. Sacks and packages in sacks can not be unloaded
                CheckIfPackageInSack(x, deliveryDataFromDb.SackPackages) == default)
                .ToList();

            var unloadPackageResult = PackageProcess(packagesToUnload, enums.PackageStatusType.Unloaded);
            response.AddRange(unloadPackageResult);

            // not matching delivery process
            var notMatchDeliveryBracodes = deliveries.Select(x => x.Barcode).Except(packagesToUnload.Select(x => x.Barcode));
            var notMatchDeliveries = deliveries.Where(x => notMatchDeliveryBracodes.Contains(x.Barcode));
            var motMatchPackages = deliveryDataFromDb.Packages.Where(x => notMatchDeliveries.Any(nmp => nmp.Barcode == x.Barcode)).ToList();
            var motMatchSacks = deliveryDataFromDb.Sacks.Where(x => notMatchDeliveries.Any(nmp => nmp.Barcode == x.Barcode)).ToList();

            var notUnloadPackageResult = PackageProcess(motMatchPackages, enums.PackageStatusType.Loaded);
            response.AddRange(notUnloadPackageResult);

            var notUnloadedSackResult = SackProcess(motMatchSacks, enums.SackStatusType.Loaded);
            response.AddRange(notUnloadedSackResult);

            return response;
        }

        private List<DeliveryDTO> DistributionCentreUnloadProcess(UnloadProcessDbResultParameterModel deliveryDataFromDb, List<DistributeRequest.Delivery> deliveries)
        {
            var response = new List<DeliveryDTO>();

            var packagesToUnload = deliveryDataFromDb.Packages
                .Where(x => x.DeliveryPointTypeId == enums.DeliveryPointType.DistributionCentre.GetHashCode() && deliveries.Any(d => d.Barcode == x.Barcode))
                .ToList();

            var unloadPackagesWithSackResult = UnloadPackagesWithSack(packagesToUnload, deliveryDataFromDb.SackPackages);
            response.AddRange(unloadPackagesWithSackResult);

            var sackToUnload = deliveryDataFromDb.Sacks
              .Where(x => x.DeliveryPointTypeId == enums.DeliveryPointType.DistributionCentre.GetHashCode() && deliveries.Any(d => d.Barcode == x.Barcode))
              .ToList();

            var unloadSackResult = UnloadSackWithPackages(sackToUnload, deliveryDataFromDb.SackPackages);
            response.AddRange(unloadSackResult);

            // not matching delivery process
            var notMatchDeliveryBracodes = deliveries.Select(x => x.Barcode).Except(sackToUnload.Select(x => x.Barcode).Union(packagesToUnload.Select(x => x.Barcode)));
            var notMatchDeliveries = deliveries.Where(x => notMatchDeliveryBracodes.Contains(x.Barcode));
            var motMatchPackages = deliveryDataFromDb.Packages.Where(x => notMatchDeliveries.Any(nmp => nmp.Barcode == x.Barcode)).ToList();
            var motMatchSacks = deliveryDataFromDb.Sacks.Where(x => notMatchDeliveries.Any(nmp => nmp.Barcode == x.Barcode)).ToList();

            var notMatchPackagesResult = PackageProcess(motMatchPackages, enums.PackageStatusType.Loaded);
            response.AddRange(notMatchPackagesResult);

            var notMatchSacksResult = SackProcess(motMatchSacks, enums.SackStatusType.Loaded);
            response.AddRange(notMatchSacksResult);

            return response;
        }

        private List<DeliveryDTO> TransferCentreUnloadProcess(UnloadProcessDbResultParameterModel deliveryDataFromDb, List<DistributeRequest.Delivery> deliveries)
        {
            var response = new List<DeliveryDTO>();

            var packagesToUnload = deliveryDataFromDb.Packages
                .Where(x =>
                x.DeliveryPointTypeId == enums.DeliveryPointType.TransferCentre.GetHashCode() &&
                // Branch: Only packages in sack can be unloaded. individual packages can not be unloaded
                CheckIfPackageInSack(x, deliveryDataFromDb.SackPackages) != default)
                .ToList();

            var unloadPackagesWithSackResult = UnloadPackagesWithSack(packagesToUnload, deliveryDataFromDb.SackPackages);
            response.AddRange(unloadPackagesWithSackResult);

            var sackToUnload = deliveryDataFromDb.Sacks
                .Where(x => x.DeliveryPointTypeId == enums.DeliveryPointType.TransferCentre.GetHashCode() && deliveries.Any(d => d.Barcode == x.Barcode))
                .ToList();

            var unloadSackResult = UnloadSackWithPackages(sackToUnload, deliveryDataFromDb.SackPackages);
            response.AddRange(unloadSackResult);

            // not Matching package process
            var notMatchDeliveryBracodes = deliveries.Select(x => x.Barcode).Except(sackToUnload.Select(x => x.Barcode).Union(packagesToUnload.Select(x => x.Barcode)));
            var notMatchDeliveries = deliveries.Where(x => notMatchDeliveryBracodes.Contains(x.Barcode));

            var motMatchPackages = deliveryDataFromDb.Packages.Where(x => notMatchDeliveries.Any(nmp => nmp.Barcode == x.Barcode)).ToList();
            var motMatchSacks = deliveryDataFromDb.Sacks.Where(x => notMatchDeliveries.Any(nmp => nmp.Barcode == x.Barcode)).ToList();

            var notMatchPackagesResult = PackageProcess(motMatchPackages, enums.PackageStatusType.Loaded);
            response.AddRange(notMatchPackagesResult);

            var notMatchSacksResult = SackProcess(motMatchSacks, enums.SackStatusType.Loaded);
            response.AddRange(notMatchSacksResult);

            return response;
        }

        private List<DeliveryDTO> UnloadSackWithPackages(List<Sack> sacksToUnload, List<SackPackage> sackPackages)
        {
            var response = new List<DeliveryDTO>();

            sacksToUnload
                .ForEach(s =>
                {
                    var modifiedDate = DateTime.Now;
                    var deliveryToAdd = new DeliveryDTO() { Barcode = s.Barcode, State = enums.SackStatusType.Unloaded.GetHashCode() };

                    UptateSack(s, enums.SackStatusType.Unloaded, modifiedDate);

                    // If the sack itself is unloaded from the vehicle before the shipments inside the sack are unloaded, the sack and all the shipments inside the sack will switch to "unloaded".
                    var packagesInSackToUnload = sackPackages.Where(x => x.SackId == s.Id).Select(x => x.Package).ToList();

                    packagesInSackToUnload.ForEach(p =>
                    {
                        UpdatePackage(p, enums.PackageStatusType.Unloaded, modifiedDate);
                    });

                    response.Add(deliveryToAdd);
                });

            return response;
        }

        private List<DeliveryDTO> UnloadPackagesWithSack(List<Package> packagesToUnload, List<SackPackage> sackPackages)
        {
            var response = new List<DeliveryDTO>();

            packagesToUnload
                 .ForEach(x =>
                 {
                     var modifiedDate = DateTime.Now;
                     var deliveryToAdd = new DeliveryDTO() { Barcode = x.Barcode, State = enums.PackageStatusType.Unloaded.GetHashCode() };

                     UpdatePackage(x, enums.PackageStatusType.Unloaded, modifiedDate);

                     // As the packages in the sack are unloaded one by one, the sack also goes into the "unloaded" state
                     // when all of the packages have been successfully unloaded.
                     var sack = CheckIfPackageInSack(x, sackPackages);
                     if (sack != default)
                     {
                         // checking if current package is last to be unloaded
                         if (!sackPackages.Any(sp => sp.Package.PackageStatusTypeId == enums.PackageStatusType.Loaded.GetHashCode()))
                         {
                             UptateSack(sack, enums.SackStatusType.Unloaded, modifiedDate);
                         }
                     }
                     response.Add(deliveryToAdd);
                 });

            return response;
        }

        private List<DeliveryDTO> SackProcess(List<Sack> sacksToUnload, enums.SackStatusType sackStatusType)
        {
            var response = new List<DeliveryDTO>();

            sacksToUnload
                .ForEach(s =>
                {
                    var modifiedDate = DateTime.Now;
                    var deliveryToAdd = new DeliveryDTO() { Barcode = s.Barcode, IsUnloaded = sackStatusType == enums.SackStatusType.Unloaded, State = sackStatusType.GetHashCode() };

                    UptateSack(s, sackStatusType, modifiedDate);

                    response.Add(deliveryToAdd);
                });

            return response;
        }

        private List<DeliveryDTO> PackageProcess(List<Package> packagesToUnload, enums.PackageStatusType packageStatusType)
        {
            var response = new List<DeliveryDTO>();

            packagesToUnload
                .ForEach(p =>
                {
                    var deliveryToAdd = new DeliveryDTO() { Barcode = p.Barcode, IsUnloaded = packageStatusType == enums.PackageStatusType.Unloaded, State = packageStatusType.GetHashCode() };

                    UpdatePackage(p, packageStatusType);

                    response.Add(deliveryToAdd);
                });

            return response;
        }

        private void LogToNotUnloadDeliveries(IEnumerable<DeliveryDTO> deliveries, enums.DeliveryPointType deliveryPointType)
        {
            if (deliveries.ToList().Count > 0)
            {
                var logRepository = _unitOfWork.GetRepository<Log>();

                deliveries
                    .Where(x => !x.IsUnloaded)
                    .ToList()
                    .ForEach(x =>
                    {
                        var logToAdd = new Log()
                        {
                            Barcode = x.Barcode,
                            DeliveryPointId = deliveryPointType.GetHashCode(),
                            LogName = DeliveryLogName.Unload,
                            Message = $"Delivery with {x.Barcode} barcode is not compatible with delivery point {deliveryPointType}."
                        };
                        logRepository.Add(logToAdd);
                    });
            }
        }

        private Sack? CheckIfPackageInSack(Package package, List<SackPackage> sackPackages)
        {
            var sackWithPackage = sackPackages.FirstOrDefault(x => x.PackageId == package.Id);
            if (sackWithPackage != default)
            {
                return sackWithPackage.Sack;
            }
            return default;
        }

        private void UpdatePackage(Package package, enums.PackageStatusType packageStatusType, DateTime? modifiedDate = null)
        {
            if (!modifiedDate.HasValue)
            {
                modifiedDate = DateTime.Now;
            }

            package.PackageStatusTypeId = packageStatusType.GetHashCode();
            package.ModifiedAt = modifiedDate;
            package.ModifiedBy = "System";
        }

        private void UptateSack(Sack sack, enums.SackStatusType sackStatusType, DateTime? modifiedDate = null)
        {
            if (!modifiedDate.HasValue)
            {
                modifiedDate = DateTime.Now;
            }

            sack.SackStatusTypeId = sackStatusType.GetHashCode();
            sack.ModifiedAt = modifiedDate;
            sack.ModifiedBy = "System";
        }
    }
}

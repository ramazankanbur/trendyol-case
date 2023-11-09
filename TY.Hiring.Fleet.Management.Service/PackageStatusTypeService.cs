using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using TY.Hiring.Fleet.Management.Data.ORM.EF.Models;
using TY.Hiring.Fleet.Management.Data.ORM.EF.UOW;
using TY.Hiring.Fleet.Management.Model.Models;
using TY.Hiring.Fleet.Management.Model.Models.Dtos;
using TY.Hiring.Fleet.Management.Service.Interface;

namespace TY.Hiring.Fleet.Management.Service
{
    public class PackageStatusTypeService : IPackageStatusTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PackageStatusTypeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Create(PackageStatusTypeDTO data)
        {
            var packageStatusTypeRepository = _unitOfWork.GetRepository<PackageStatusType>();

            var packageStatusTypeToAdd = _mapper.Map<PackageStatusType>(data);

            packageStatusTypeRepository.Add(packageStatusTypeToAdd);

            await _unitOfWork.SaveChangesAsync();

            int response = packageStatusTypeToAdd.Id;

            return response;
        }

        public async Task<List<PackageStatusTypeDTO>> GetPackageStatusTypesAsnyc()
        {
            var response = new List<PackageStatusTypeDTO>();


            var packageStatusTypeRepository = _unitOfWork.GetRepository<PackageStatusType>();

            var basePackageStatusTypeQuery = packageStatusTypeRepository.GetAll().Where(x => x.IsActive);

            var rowCount = await basePackageStatusTypeQuery.CountAsync();

            var packageStatusTypeList = await basePackageStatusTypeQuery
                .ProjectTo<PackageStatusTypeDTO>(_mapper.ConfigurationProvider)
                .AsNoTracking()
                .ToListAsync();

            response = packageStatusTypeList;

            return response;
        }

        public async Task<PackageStatusTypeDTO> GetPackageStatusTypesByIdAsnyc(int id)
        {
            var response = new PackageStatusTypeDTO();

            var packageStatusTypeRepository = _unitOfWork.GetRepository<PackageStatusType>();

            var packageStatusType = new PackageStatusTypeDTO();

            packageStatusType = await packageStatusTypeRepository
                .GetAll()
                .Where(x => x.IsActive)
                .ProjectTo<PackageStatusTypeDTO>(_mapper.ConfigurationProvider)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            response = packageStatusType;

            return response;
        }
    }
}

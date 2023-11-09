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
    public class PackageService : IPackageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PackageService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ListResult<PackageDTO>> GetPackagesAsnyc(int page = 1, int size = 50)
        {
            var response = new ListResult<PackageDTO>();

            page = page < 1 ? 1 : page;
            size = size < 1 ? 50 : size;

            var packageRepository = _unitOfWork.GetRepository<Package>();

            var basePackageQuery = packageRepository.GetAll().Where(x => x.IsActive);

            var rowCount = await basePackageQuery.CountAsync();

            var packageList = await basePackageQuery
                .Skip((page - 1) * size)
                .Take(size)
                .ProjectTo<PackageDTO>(_mapper.ConfigurationProvider)
                .AsNoTracking()
                .ToListAsync();

            response.List = packageList;
            response.Size = size;
            response.Page = page;
            response.Count = rowCount;

            return response;
        }
    }
}

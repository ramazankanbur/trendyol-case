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
    public class SackService : ISackService
    {
     
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SackService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;

        }
       
        public async Task<ListResult<SackDTO>> GetSacksAsnyc(int page = 1, int size = 50)
        {
            var response = new ListResult<SackDTO>();

            page = page < 1 ? 1 : page;
            size = size < 1 ? 50 : size;

            var sackRepository = _unitOfWork.GetRepository<Sack>();

            var basesackQuery = sackRepository.GetAll().Where(x => x.IsActive == true);

            var rowCount = await basesackQuery.CountAsync();

            var sackList = await basesackQuery
                .Skip((page - 1) * size)
                .Take(size)
                .ProjectTo<SackDTO>(_mapper.ConfigurationProvider)
                .AsNoTracking()
                .ToListAsync();

            response.List = sackList;
            response.Size = size;
            response.Page = page;
            response.Count = rowCount;

            return response;
        }

        public async Task<List<SackPackageDTO>> GetPackagesBySackBarcode(string sackBarcode)
        {
            var response = new List<SackPackageDTO>();

            var sackRepository = _unitOfWork.GetRepository<SackPackage>();

            var sackList = await sackRepository
                .GetAll()
                .Where(x => x.IsActive && x.Sack.Barcode == sackBarcode)
                .ProjectTo<SackPackageDTO>(_mapper.ConfigurationProvider)
                .AsNoTracking()
                .ToListAsync();

            response = sackList;

            return response;
        } 
    }
}

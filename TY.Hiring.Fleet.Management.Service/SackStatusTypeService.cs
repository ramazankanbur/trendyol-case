using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using TY.Hiring.Fleet.Management.Data.ORM.EF.Models;
using TY.Hiring.Fleet.Management.Data.ORM.EF.UOW;
using TY.Hiring.Fleet.Management.Model.Models.Dtos;
using TY.Hiring.Fleet.Management.Service.Interface;

namespace TY.Hiring.Fleet.Management.Service
{
    public class SackStatusTypeService : ISackStatusTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SackStatusTypeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<SackStatusTypeDTO>> GetSackStatusTypesAsnyc()
        {
            var response = new List<SackStatusTypeDTO>();

            var sackStatusTypeRepository = _unitOfWork.GetRepository<SackStatusType>();

            var sackStatusTypeList = await sackStatusTypeRepository
                .GetAll()
                .Where(x => x.IsActive)
                .ProjectTo<SackStatusTypeDTO>(_mapper.ConfigurationProvider)
                .AsNoTracking()
                .ToListAsync();

            response = sackStatusTypeList;

            return response;
        }
    }
}

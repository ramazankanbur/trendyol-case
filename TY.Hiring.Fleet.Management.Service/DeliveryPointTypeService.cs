using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using TY.Hiring.Fleet.Management.Data.ORM.EF.Models;
using TY.Hiring.Fleet.Management.Data.ORM.EF.UOW;
using TY.Hiring.Fleet.Management.Model.Models.Dtos;
using TY.Hiring.Fleet.Management.Service.Interface;

namespace TY.Hiring.Fleet.Management.Service
{
    public class DeliveryPointTypeService : IDeliveryPointTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DeliveryPointTypeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<DeliveryPointTypeDTO>> GetDeliveryPointTypesAsnyc()
        {
            var response = new List<DeliveryPointTypeDTO>();

            var deliveryPointTypeRepository = _unitOfWork.GetRepository<DeliveryPointType>();
             
            var deliveryPointTypeList = await deliveryPointTypeRepository
                .GetAll()
                .Where(x => x.IsActive)
                .ProjectTo<DeliveryPointTypeDTO>(_mapper.ConfigurationProvider)
                .AsNoTracking()
                .ToListAsync();

            response = deliveryPointTypeList;

            return response;
        }
    }
}

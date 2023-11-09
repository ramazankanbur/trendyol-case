using TY.Hiring.Fleet.Management.Model.Models.Dtos;

namespace TY.Hiring.Fleet.Management.Service.Interface
{
    public interface IDeliveryPointTypeService
    {
        Task<List<DeliveryPointTypeDTO>> GetDeliveryPointTypesAsnyc();

    }
}

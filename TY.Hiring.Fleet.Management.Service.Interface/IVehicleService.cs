using TY.Hiring.Fleet.Management.Model.Models.Dtos;
using TY.Hiring.Fleet.Management.Model.Models.Requests;

namespace TY.Hiring.Fleet.Management.Service.Interface
{
    public interface IVehicleService
    {
        Task<DistributeDTO> DistributeDeliveries(DistributeRequest data);
    }
}

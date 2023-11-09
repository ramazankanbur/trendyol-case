using Microsoft.AspNetCore.Mvc;
using TY.Hiring.Fleet.Management.Model.Models;
using TY.Hiring.Fleet.Management.Model.Models.Dtos;
using TY.Hiring.Fleet.Management.Model.Models.Requests;
using TY.Hiring.Fleet.Management.Service.Interface;

namespace TY.Hiring.Fleet.Management.Api.Controllers
{
    [Route("v1/vehicles/{plate}")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;
        public VehicleController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        [HttpPost("Distribute")]
        public async Task<DataResult<DistributeDTO>> DistributeDeliveries(DistributeRequest distributeRequest, string plate)
        {
            var response = new DataResult<DistributeDTO>();

            var serviceResult = await _vehicleService.DistributeDeliveries(distributeRequest);

            serviceResult.Vehicle = plate;
            response.Result = serviceResult;

            return response;
        }
    }
}

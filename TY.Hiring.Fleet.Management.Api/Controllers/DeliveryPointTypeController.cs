using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TY.Hiring.Fleet.Management.Model.Models;
using TY.Hiring.Fleet.Management.Model.Models.Dtos;
using TY.Hiring.Fleet.Management.Service.Interface;

namespace TY.Hiring.Fleet.Management.Api.Controllers
{
    [Route("v1")]
    [ApiController]
    public class DeliveryPointTypeController : ControllerBase
    {
        private readonly IDeliveryPointTypeService _deliveryPointService;
        private readonly ILogger<DeliveryPointTypeController> _logger;
        public DeliveryPointTypeController(IDeliveryPointTypeService deliveryPointService, ILogger<DeliveryPointTypeController> logger)
        {
            _deliveryPointService = deliveryPointService;
            _logger = logger;
        }

        [HttpGet("DeliveryPoints")]
        public async Task<DataResult<List<DeliveryPointTypeDTO>>> GetDeliveryPoints()
        {
            var response = new DataResult<List<DeliveryPointTypeDTO>>();

            var serviceResult = await _deliveryPointService.GetDeliveryPointTypesAsnyc();

            response.Result = serviceResult;

            return response;
        }
    }
}

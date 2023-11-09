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
    public class SackStatusTypeController : ControllerBase
    {
        private readonly ISackStatusTypeService _sackStatusTypeService;
        private readonly ILogger<SackStatusTypeController> _logger;
        public SackStatusTypeController(ISackStatusTypeService sackStatusTypeService, ILogger<SackStatusTypeController> logger)
        {
            _sackStatusTypeService = sackStatusTypeService;
            _logger = logger;
        }

        [HttpGet("SackStatusTypes")]
        public async Task<DataResult<List<SackStatusTypeDTO>>> GetSackStatusTypes()
        {
            var response = new DataResult<List<SackStatusTypeDTO>>();

            var serviceResult = await _sackStatusTypeService.GetSackStatusTypesAsnyc();

            response.Result = serviceResult;

            return response;
        }
    }
}

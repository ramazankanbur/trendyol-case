using Microsoft.AspNetCore.Mvc;
using TY.Hiring.Fleet.Management.Model.Models;
using TY.Hiring.Fleet.Management.Model.Models.Dtos;
using TY.Hiring.Fleet.Management.Service.Interface;
using System.Net;

namespace TY.Hiring.Fleet.Management.Api.Controllers
{
    [Route("v1")]
    [ApiController]
    public class PackageStatusTypeController : ControllerBase
    {
        private readonly IPackageStatusTypeService _packageStatusService;
        private readonly ILogger<PackageStatusTypeController> _logger;
        public PackageStatusTypeController(IPackageStatusTypeService packageStatusService, ILogger<PackageStatusTypeController> logger)
        {
            _packageStatusService = packageStatusService;
            _logger = logger;
        }

        [HttpGet("PackageStatusTypes")]
        public async Task<DataResult<List<PackageStatusTypeDTO>>> GetPackageStatusTypes()
        {
            var response = new DataResult<List<PackageStatusTypeDTO>>();

            var serviceResult = await _packageStatusService.GetPackageStatusTypesAsnyc();

            response.Result = serviceResult;

            return response;
        }
    }
}


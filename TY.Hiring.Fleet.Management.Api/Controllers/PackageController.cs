using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TY.Hiring.Fleet.Management.Model.Models;
using TY.Hiring.Fleet.Management.Model.Models.Dtos;
using TY.Hiring.Fleet.Management.Service.Interface;

namespace TY.Hiring.Fleet.Management.Api.Controllers
{
    [Route("v1")]
    [ApiController]
    public class PackageController : ControllerBase
    {
        private readonly IPackageService _packageService;
        public PackageController(IPackageService packageService)
        {
            _packageService = packageService;
        }

        [HttpGet("Packages")]
        public async Task<DataResult<ListResult<PackageDTO>>> GetPackages(int page, int size)
        {
            var response = new DataResult<ListResult<PackageDTO>>();

            var serviceResult = await _packageService.GetPackagesAsnyc(page: page, size: size);

            response.Result = serviceResult;

            return response;
        }
    }
}

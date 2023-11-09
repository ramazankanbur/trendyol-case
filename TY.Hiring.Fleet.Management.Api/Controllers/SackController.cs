using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TY.Hiring.Fleet.Management.Model.Models;
using TY.Hiring.Fleet.Management.Model.Models.Dtos;
using TY.Hiring.Fleet.Management.Service.Interface;

namespace TY.Hiring.Fleet.Management.Api.Controllers
{
    [Route("v1")]
    [ApiController]
    public class SackController : ControllerBase
    {
        private readonly ISackService _sackService;
        public SackController(ISackService sackService)
        {
            _sackService = sackService;
        }

        [HttpGet("GetSacks")]
        public async Task<DataResult<ListResult<SackDTO>>> GetPackages(int page, int size)
        {
            var response = new DataResult<ListResult<SackDTO>>();

            var serviceResult = await _sackService.GetSacksAsnyc(page: page, size: size);

            response.Result = serviceResult;

            return response;
        }

        [HttpGet("GetPackagesBySackBarcode")]
        public async Task<DataResult<List<SackPackageDTO>>> GetPackages(string sackBarcode)
        {
            var response = new DataResult<List<SackPackageDTO>>();

            var serviceResult = await _sackService.GetPackagesBySackBarcode(sackBarcode);

            response.Result = serviceResult;

            return response;
        }

    }
}

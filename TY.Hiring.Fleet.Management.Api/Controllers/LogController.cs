using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TY.Hiring.Fleet.Management.Model.Models;
using TY.Hiring.Fleet.Management.Model.Models.Dtos;
using TY.Hiring.Fleet.Management.Service.Interface;

namespace TY.Hiring.Fleet.Management.Api.Controllers
{
    [Route("v1")]
    [ApiController]
    public class LogController : ControllerBase
    {
        private readonly ILogService _logService;
        public LogController(ILogService logService)
        {
            _logService = logService;
        }

        [HttpGet("Logs")]
        public async Task<DataResult<ListResult<LogDTO>>> GetLogs(int page, int size)
        {
            var response = new DataResult<ListResult<LogDTO>>();

            var serviceResult = await _logService.GetLogsAsnyc(page: page, size: size);

            response.Result = serviceResult;

            return response;
        }
    }
}

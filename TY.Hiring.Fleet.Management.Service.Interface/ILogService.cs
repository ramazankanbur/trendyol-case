using TY.Hiring.Fleet.Management.Model.Models;
using TY.Hiring.Fleet.Management.Model.Models.Dtos;

namespace TY.Hiring.Fleet.Management.Service.Interface
{
    public interface ILogService
    {
        Task<bool> CreateList(List<LogDTO> data);
        Task<ListResult<LogDTO>> GetLogsAsnyc(int page = 1, int size = 50);
    }
}

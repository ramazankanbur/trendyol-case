using TY.Hiring.Fleet.Management.Model.Models;
using TY.Hiring.Fleet.Management.Model.Models.Dtos;

namespace TY.Hiring.Fleet.Management.Service.Interface
{
    public interface ISackService
    {
        Task<ListResult<SackDTO>> GetSacksAsnyc(int page = 1, int size = 50);
        Task<List<SackPackageDTO>> GetPackagesBySackBarcode(string sackBarcode);
    }
}

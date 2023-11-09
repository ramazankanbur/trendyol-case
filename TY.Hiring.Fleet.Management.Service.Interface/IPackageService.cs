using TY.Hiring.Fleet.Management.Model.Models;
using TY.Hiring.Fleet.Management.Model.Models.Dtos;

namespace TY.Hiring.Fleet.Management.Service.Interface
{
    public interface IPackageService
    {
        Task<ListResult<PackageDTO>> GetPackagesAsnyc( int page = 1, int size = 50);
    }
}

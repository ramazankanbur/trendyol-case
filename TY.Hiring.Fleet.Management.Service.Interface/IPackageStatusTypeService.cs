using TY.Hiring.Fleet.Management.Model.Models;
using TY.Hiring.Fleet.Management.Model.Models.Dtos;

namespace TY.Hiring.Fleet.Management.Service.Interface
{
    public interface IPackageStatusTypeService
    {
        Task<int> Create(PackageStatusTypeDTO data);

        Task<List<PackageStatusTypeDTO>> GetPackageStatusTypesAsnyc();

        Task<PackageStatusTypeDTO> GetPackageStatusTypesByIdAsnyc(int id);
    }
}

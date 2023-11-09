using TY.Hiring.Fleet.Management.Model.Models.Dtos;

namespace TY.Hiring.Fleet.Management.Service.Interface
{
    public interface ISackStatusTypeService
    {
        Task<List<SackStatusTypeDTO>> GetSackStatusTypesAsnyc();
    }
}

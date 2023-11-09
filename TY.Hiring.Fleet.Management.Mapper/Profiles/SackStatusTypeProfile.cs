using AutoMapper;
using TY.Hiring.Fleet.Management.Data.ORM.EF.Models;
using TY.Hiring.Fleet.Management.Model.Models.Dtos;

namespace TY.Hiring.Fleet.Management.Mapper.Profiles
{
    public class SackStatusTypeProfile : Profile
    {
        public SackStatusTypeProfile()
        {
            CreateMap<SackStatusType, SackStatusTypeDTO>();
        }
    }
}

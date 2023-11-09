using AutoMapper;
using TY.Hiring.Fleet.Management.Data.ORM.EF.Models;
using TY.Hiring.Fleet.Management.Model.Models.Dtos;

namespace TY.Hiring.Fleet.Management.Mapper.Profiles
{
    public class SackProfile : Profile
    {
        public SackProfile()
        {
            CreateMap<Sack, SackDTO>()
                 .ForMember(dto => dto.SackStatus, p => p.MapFrom(s => ((Model.Enums.PackageStatusType)s.SackStatusTypeId).ToString()))
                .ForMember(dto => dto.DeliveryPoint, p => p.MapFrom(s => ((Model.Enums.DeliveryPointType)s.DeliveryPointTypeId).ToString()));
        }
    }
}

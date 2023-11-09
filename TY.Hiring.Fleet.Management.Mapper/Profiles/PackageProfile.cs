using AutoMapper;
using TY.Hiring.Fleet.Management.Data.ORM.EF.Models;
using TY.Hiring.Fleet.Management.Model.Models.Dtos;

namespace TY.Hiring.Fleet.Management.Mapper.Profiles
{
    public class PackageProfile : Profile
    {
        public PackageProfile()
        {
            CreateMap<Package, PackageDTO>()
                .ForMember(dto => dto.PackageStatus, p => p.MapFrom(s => ((Model.Enums.PackageStatusType)s.PackageStatusTypeId).ToString()))
                .ForMember(dto => dto.DeliveryPoint, p => p.MapFrom(s => ((Model.Enums.DeliveryPointType)s.DeliveryPointTypeId).ToString()));
        }
    }
}

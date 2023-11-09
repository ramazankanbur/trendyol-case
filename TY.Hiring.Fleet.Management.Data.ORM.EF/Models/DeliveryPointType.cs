using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TY.Hiring.Fleet.Management.Data.ORM.EF.Models
{
    public class DeliveryPointType : BaseEntity
    {
        public string Name { get; set; }
    }

    public class DeliveryPointTypeEntityConfiguration : IEntityTypeConfiguration<DeliveryPointType>
    {
        public void Configure(EntityTypeBuilder<DeliveryPointType> entity)
        {
            entity.HasData(
               new DeliveryPointType() { Id = 1, Name = "Branch" },
               new DeliveryPointType() { Id = 2, Name = "DistributionCentre" },
               new DeliveryPointType() { Id = 3, Name = "TransferCentre" }
               );
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TY.Hiring.Fleet.Management.Data.ORM.EF.Models
{
    public class SackStatusType : BaseEntity
    {
        public string Name { get; set; }
    }

    public class SackStatusTypeEntityConfiguration : IEntityTypeConfiguration<SackStatusType>
    {
        public void Configure(EntityTypeBuilder<SackStatusType> entity)
        {
            entity.HasData(
              new SackStatusType() { Id = 1, Name = "Created" },
              new SackStatusType() { Id = 3, Name = "Loaded" },
              new SackStatusType() { Id = 4, Name = "Unloaded" }
            );
        }
    }
}

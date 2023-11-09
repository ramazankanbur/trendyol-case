using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TY.Hiring.Fleet.Management.Data.ORM.EF.Models
{
    public class PackageStatusType : BaseEntity
    {
        public string Name { get; set; }
    }

    public class PackageStatusTypeEntityConfiguration : IEntityTypeConfiguration<PackageStatusType>
    {
        public void Configure(EntityTypeBuilder<PackageStatusType> entity)
        {
            entity.HasData(
                new PackageStatusType() { Id = 1, Name = "Created" },
                new PackageStatusType() { Id = 2, Name = "LoadedIntoStack" },
                new PackageStatusType() { Id = 3, Name = "Loaded" },
                new PackageStatusType() { Id = 4, Name = "Unloaded" }
                );

        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TY.Hiring.Fleet.Management.Data.ORM.EF.Models
{
    public class SackPackage : BaseEntity, IAuditEntity
    {
        public int SackId { get; set; }
        public Sack Sack { get; set; }

        public int PackageId { get; set; }
        public Package Package { get; set; }


        public string CreatedBy { get; set; } = "System";
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string ModifiedBy { get; set; } = "";
        public DateTime? ModifiedAt { get; set; }

    }

    public class SackPackageEntityConfiguration : IEntityTypeConfiguration<SackPackage>
    {
        public void Configure(EntityTypeBuilder<SackPackage> entity)
        {
            entity.HasData(
                new SackPackage() { Id = 1, SackId = 1, PackageId = 6 },
                new SackPackage() { Id = 2, SackId = 1, PackageId = 10 },
                new SackPackage() { Id = 3, SackId = 2, PackageId = 13 },
                new SackPackage() { Id = 4, SackId = 2, PackageId = 14 }
            );

            entity.HasKey(x => new { x.SackId, x.PackageId });
            entity.HasOne(x => x.Sack).WithMany(x => x.SackPackages).HasForeignKey(x => x.SackId).OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}

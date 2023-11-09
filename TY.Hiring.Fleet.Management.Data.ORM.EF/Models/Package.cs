using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TY.Hiring.Fleet.Management.Data.ORM.EF.Models
{
    public class Package : BaseEntity, IAuditEntity
    {
        public string Barcode { get; set; }

        public int DeliveryPointTypeId { get; set; }
        public DeliveryPointType DeliveryPointType { get; set; }

        public int PackageStatusTypeId { get; set; }
        public PackageStatusType PackageStatusType { get; set; }

        public int Desi { get; set; }

        public string CreatedBy { get; set; } = "System";
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string ModifiedBy { get; set; } = "";
        public DateTime? ModifiedAt { get; set; }
    }

    public class PackageEntityConfiguration : IEntityTypeConfiguration<Package>
    {
        public void Configure(EntityTypeBuilder<Package> entity)
        {
            entity.HasData(
                new Package() { Id = 1, Barcode = "P7988000121", DeliveryPointTypeId = 1, PackageStatusTypeId = 1, Desi = 5 },
                new Package() { Id = 2, Barcode = "P7988000122", DeliveryPointTypeId = 1, PackageStatusTypeId = 1, Desi = 5 },
                new Package() { Id = 3, Barcode = "P7988000123", DeliveryPointTypeId = 1, PackageStatusTypeId = 1, Desi = 9 },
                new Package() { Id = 4, Barcode = "P8988000120", DeliveryPointTypeId = 2, PackageStatusTypeId = 1, Desi = 33 },
                new Package() { Id = 5, Barcode = "P8988000121", DeliveryPointTypeId = 2, PackageStatusTypeId = 1, Desi = 17 },
                new Package() { Id = 6, Barcode = "P8988000122", DeliveryPointTypeId = 2, PackageStatusTypeId = 1, Desi = 26 },
                new Package() { Id = 7, Barcode = "P8988000123", DeliveryPointTypeId = 2, PackageStatusTypeId = 1, Desi = 35 },
                new Package() { Id = 8, Barcode = "P8988000124", DeliveryPointTypeId = 2, PackageStatusTypeId = 1, Desi = 1 },
                new Package() { Id = 9, Barcode = "P8988000125", DeliveryPointTypeId = 2, PackageStatusTypeId = 1, Desi = 200 },
                new Package() { Id = 10, Barcode = "P8988000126", DeliveryPointTypeId = 2, PackageStatusTypeId = 1, Desi = 50 },
                new Package() { Id = 11, Barcode = "P9988000126", DeliveryPointTypeId = 3, PackageStatusTypeId = 1, Desi = 15 },
                new Package() { Id = 12, Barcode = "P9988000127", DeliveryPointTypeId = 3, PackageStatusTypeId = 1, Desi = 16 },
                new Package() { Id = 13, Barcode = "P9988000128", DeliveryPointTypeId = 3, PackageStatusTypeId = 1, Desi = 55 },
                new Package() { Id = 14, Barcode = "P9988000129", DeliveryPointTypeId = 3, PackageStatusTypeId = 1, Desi = 28 },
                new Package() { Id = 15, Barcode = "P9988000130", DeliveryPointTypeId = 3, PackageStatusTypeId = 1, Desi = 17 }
            );
        }
    }
}

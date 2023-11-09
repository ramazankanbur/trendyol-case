using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TY.Hiring.Fleet.Management.Data.ORM.EF.Models
{
    public class Sack : BaseEntity, IAuditEntity
    {
        public string Barcode { get; set; }

        public int DeliveryPointTypeId { get; set; }
        public DeliveryPointType DeliveryPointType { get; set; }

        public int SackStatusTypeId { get; set; }
        public SackStatusType SackStatusType { get; set; }

        public string CreatedBy { get; set; } = "System";
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string ModifiedBy { get; set; } = "";
        public DateTime? ModifiedAt { get; set; }

        public ICollection<SackPackage> SackPackages{ get; set; }
    }

    public class SackEntityConfiguration : IEntityTypeConfiguration<Sack>
    {
        public void Configure(EntityTypeBuilder<Sack> entity)
        {
            entity.HasData(
                new Sack() { Id = 1, Barcode = "C725799", DeliveryPointTypeId = 2, SackStatusTypeId = 1 },
                new Sack() { Id = 2, Barcode = "C725800", DeliveryPointTypeId = 3, SackStatusTypeId = 1 }
            );
        }
    }
}


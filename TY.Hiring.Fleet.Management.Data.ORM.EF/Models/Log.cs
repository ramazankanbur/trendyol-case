using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TY.Hiring.Fleet.Management.Data.ORM.EF.Models
{
    public class Log : BaseEntity, IAuditEntity
    {
        public string Message { get; set; }
        // Load, Unload ..
        public string LogName { get; set; }
        public int DeliveryPointId { get; set; }        
        public string Barcode { get; set; }

        public string CreatedBy { get; set; } = "System";
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string ModifiedBy { get; set; } = "";
        public DateTime? ModifiedAt { get; set; }
    }
    public class LogEntityConfiguration : IEntityTypeConfiguration<Log>
    {
        public void Configure(EntityTypeBuilder<Log> entity) { }
    }
}

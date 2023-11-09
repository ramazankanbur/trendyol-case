using Microsoft.EntityFrameworkCore;
using TY.Hiring.Fleet.Management.Data.ORM.EF.Models;

namespace TY.Hiring.Fleet.Management.Data.ORM.EF
{
    public class TYHiringFleetManagementContext : DbContext
    {
        public TYHiringFleetManagementContext(DbContextOptions<TYHiringFleetManagementContext> options) : base(options) { }

        public virtual DbSet<PackageStatusType> PackageStatusType { get; set; }
        public virtual DbSet<SackStatusType> SackStatusType { get; set; }
        public virtual DbSet<DeliveryPointType> DeliveryPointType { get; set; }
        public virtual DbSet<Package> Package { get; set; }
        public virtual DbSet<Sack> Sack { get; set; }
        public virtual DbSet<SackPackage> SackPackages { get; set; }
        public virtual DbSet<Log> Log { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PackageStatusTypeEntityConfiguration());
            modelBuilder.ApplyConfiguration(new SackStatusTypeEntityConfiguration());
            modelBuilder.ApplyConfiguration(new DeliveryPointTypeEntityConfiguration());
            modelBuilder.ApplyConfiguration(new SackEntityConfiguration());
            modelBuilder.ApplyConfiguration(new PackageEntityConfiguration());
            modelBuilder.ApplyConfiguration(new SackPackageEntityConfiguration());
            modelBuilder.ApplyConfiguration(new LogEntityConfiguration());
        }
    }
}

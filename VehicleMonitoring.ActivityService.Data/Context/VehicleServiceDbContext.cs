using Microsoft.EntityFrameworkCore;
using VehicleMonitoring.ActivityService.DomainModels;

namespace VehicleMonitoring.ActivityService.Data
{
    public class ActivityServiceDbContext : DbContext
    {
        public ActivityServiceDbContext(DbContextOptions<ActivityServiceDbContext> options) : base(options)
        {
        }

        public virtual DbSet<VehicleActivity> VehicleActivities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VehicleActivity>()
                .Property(e => e.VehicleId)
                .IsUnicode(false);
        }
    }
}
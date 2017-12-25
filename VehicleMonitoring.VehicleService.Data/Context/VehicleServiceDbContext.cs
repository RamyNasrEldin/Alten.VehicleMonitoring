using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using VehicleMonitoring.VehicleService.DomainModels;

namespace VehicleMonitoring.VehicleService.Data
{
    public class VehicleServiceDbContext : DbContext
    {
        public VehicleServiceDbContext(DbContextOptions<VehicleServiceDbContext> options) : base(options)
        {
           
        }

        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Vehicle> Vehicles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Customer>()
                .Property(e => e.Address)
                .IsUnicode(false);

            modelBuilder.Entity<Vehicle>()
                .Property(e => e.Id)
                .IsUnicode(false);

            modelBuilder.Entity<Vehicle>()
                .Property(e => e.RegNo)
                .IsUnicode(false);
        }
    }
}
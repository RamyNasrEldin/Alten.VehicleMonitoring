using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VehicleMonitoring.VehicleService.DomainModels;

namespace VehicleMonitoring.VehicleService.Data.SampleData
{
    public class DbInitializer
    {
        private VehicleServiceDbContext _context;
        
        public DbInitializer(VehicleServiceDbContext context)
        {
            this._context = context;
        }

        public async Task Initialize()
        {
            if (_context == null)
            {
                return;
            }
            _context.Database.EnsureCreated();

            // Look for any vehicles.
            if (_context.Vehicles.Any())
            {
                return;   // DB has been seeded
            }

            _context.Customers.AddRange(new List<Customer>() {
               new Customer { Id = Guid.Parse("EFB499FA-B179-4B99-9539-6925751F1FB6"), Name = "Kalles Grustransporter AB", Address = "Cementvägen 8, 111 11 Södertälje", IsActive = true, CreatedOn = DateTime.Now, IsDeleted = false },
               new Customer { Id = Guid.Parse("A0860071-B1B8-4663-AAD6-6D75A6C92D47"), Name = "Johans Bulk AB", Address = "Balkvägen 12, 222 22 Stockholm", IsActive = true, CreatedOn = DateTime.Now, IsDeleted = false },
               new Customer { Id = Guid.Parse("D47CEC83-BCE8-46ED-B77D-D33D457319F7"), Name = "Haralds Värdetransporter AB", Address = "Budgetvägen 1, 333 33 Uppsala", IsActive = true, CreatedOn = DateTime.Now, IsDeleted = false }
               });

            _context.Vehicles.AddRange(new List<Vehicle>() {
                new Vehicle { Id = "YS2R4X20005399401", RegNo = "ABC123", CustomerId = Guid.Parse("EFB499FA-B179-4B99-9539-6925751F1FB6"), CurrentStatus = true, IsActive = true, IsDeleted = false, LastUpdateTime = DateTime.Now, CreatedOn = DateTime.Now },
                new Vehicle { Id = "VLUR4X20009093588", RegNo = "DEF456", CustomerId = Guid.Parse("EFB499FA-B179-4B99-9539-6925751F1FB6"), CurrentStatus = false, IsActive = true, IsDeleted = false, LastUpdateTime = DateTime.Now, CreatedOn = DateTime.Now },
                new Vehicle { Id = "VLUR4X20009048066", RegNo = "GHI789", CustomerId = Guid.Parse("EFB499FA-B179-4B99-9539-6925751F1FB6"), CurrentStatus = true, IsActive = true, IsDeleted = false, LastUpdateTime = DateTime.Now, CreatedOn = DateTime.Now },

                new Vehicle { Id = "YS2R4X20005388011", RegNo = "JKL012", CustomerId = Guid.Parse("A0860071-B1B8-4663-AAD6-6D75A6C92D47"), CurrentStatus = false, IsActive = true, IsDeleted = false, LastUpdateTime = DateTime.Now, CreatedOn = DateTime.Now },
                new Vehicle { Id = "YS2R4X20005387949", RegNo = "MNO345", CustomerId = Guid.Parse("A0860071-B1B8-4663-AAD6-6D75A6C92D47"), CurrentStatus = true, IsActive = true, IsDeleted = false, LastUpdateTime = DateTime.Now, CreatedOn = DateTime.Now },

                new Vehicle { Id = "YS2R4X20005387765", RegNo = "PQR678", CustomerId = Guid.Parse("D47CEC83-BCE8-46ED-B77D-D33D457319F7"), CurrentStatus = true, IsActive = true, IsDeleted = false, LastUpdateTime = DateTime.Now, CreatedOn = DateTime.Now },
                new Vehicle { Id = "YS2R4X20005387055", RegNo = "STU901", CustomerId = Guid.Parse("D47CEC83-BCE8-46ED-B77D-D33D457319F7"), CurrentStatus = false, IsActive = true, IsDeleted = false, LastUpdateTime = DateTime.Now, CreatedOn = DateTime.Now }
                });

            await _context.SaveChangesAsync();
        }
    }
}

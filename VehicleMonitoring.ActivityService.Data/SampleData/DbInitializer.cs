using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VehicleMonitoring.ActivityService.DomainModels;

namespace VehicleMonitoring.ActivityService.Data.SampleData
{
    public class DbInitializer
    {
        private ActivityServiceDbContext _context;
        public DbInitializer(ActivityServiceDbContext context)
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
            //if (_context.VehicleActivities.Any())
            //{
            //    return;   // DB has been seeded
            //}
            
            //_context.VehicleActivities.AddRange(new List<VehicleActivity>() {
            //    new VehicleActivity { ID = 1, VehicleId = "ABC123", Status = true, EntryDate = DateTime.Now },
            //    new VehicleActivity { ID = 2, VehicleId = "ABC123", Status = false, EntryDate = DateTime.Now.AddMinutes(1) },
            //    new VehicleActivity { ID = 3, VehicleId = "ABC123", Status = true, EntryDate = DateTime.Now.AddMinutes(1) },


            //    new VehicleActivity { ID = 4, VehicleId = "DEF456", Status = true, EntryDate = DateTime.Now },
            //    new VehicleActivity { ID = 5, VehicleId = "DEF456", Status = true, EntryDate = DateTime.Now.AddMinutes(1) },
            //    new VehicleActivity { ID = 6, VehicleId = "DEF456", Status = false, EntryDate = DateTime.Now.AddMinutes(1) },

            //    new VehicleActivity { ID = 7, VehicleId = "GHI789", Status = true, EntryDate = DateTime.Now },
            //    new VehicleActivity { ID = 8, VehicleId = "GHI789", Status = true, EntryDate = DateTime.Now.AddMinutes(1) },
            //    new VehicleActivity { ID = 9, VehicleId = "GHI789", Status = false, EntryDate = DateTime.Now.AddMinutes(1) },

            //    new VehicleActivity { ID = 10, VehicleId = "JKL012", Status = false, EntryDate = DateTime.Now },
            //    new VehicleActivity { ID = 11, VehicleId = "JKL012", Status = false, EntryDate = DateTime.Now.AddMinutes(1) },
            //    new VehicleActivity { ID = 12, VehicleId = "JKL012", Status = true, EntryDate = DateTime.Now.AddMinutes(1) },

            //    new VehicleActivity { ID = 13, VehicleId = "MNO345", Status = false, EntryDate = DateTime.Now },
            //    new VehicleActivity { ID = 14, VehicleId = "MNO345", Status = false, EntryDate = DateTime.Now.AddMinutes(1) },
            //    new VehicleActivity { ID = 15, VehicleId = "MNO345", Status = true, EntryDate = DateTime.Now.AddMinutes(1) },

            //    new VehicleActivity { ID = 16, VehicleId = "PQR678", Status = false, EntryDate = DateTime.Now },
            //    new VehicleActivity { ID = 17, VehicleId = "PQR678", Status = false, EntryDate = DateTime.Now.AddMinutes(1) },
            //    new VehicleActivity { ID = 18, VehicleId = "PQR678", Status = true, EntryDate = DateTime.Now.AddMinutes(1) },

            //    new VehicleActivity { ID = 19, VehicleId = "STU901", Status = false, EntryDate = DateTime.Now },
            //    new VehicleActivity { ID = 20, VehicleId = "STU901", Status = false, EntryDate = DateTime.Now.AddMinutes(1) },
            //    new VehicleActivity { ID = 21, VehicleId = "STU901", Status = true, EntryDate = DateTime.Now.AddMinutes(1) },
            //    });

            //await _context.SaveChangesAsync();
        }
    }
}

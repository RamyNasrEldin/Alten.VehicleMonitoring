using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VehicleMonitoring.Core.UnitOfWork;
using VehicleMonitoring.VehicleService.DTO;

namespace VehicleMonitoring.VehicleService.Infrastructure.UnitOfWork
{
    public interface IVehicleServiceUOW : IUnitOfWork
    {
        List<LookupDTO> GetCustomersLookup();
        List<CustomerDTO> GetCustomersVehicles();
        List<CustomerDTO> GetCustomersVehiclesByStatus(bool status);
        CustomerDTO GetCustomerVehiclesByCustomerId(Guid customerID);
        Task<bool> UpdateVehicleStatus(string VehicleId, bool Status);
    }
}

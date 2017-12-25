using VehicleMonitoring.Core.UnitOfWork;
using VehicleMonitoring.ActivityService.DTO;
using System.Threading.Tasks;

namespace VehicleMonitoring.ActivityService.Infrastructure.UnitOfWork
{
    public interface IVehicleActivityServiceUOW : IUnitOfWork
    {
        void SaveVehicleActivityTransaction(VehicleActivityDTO vehicleActivityDTO);
        Task<bool> SaveVehicleActivityTransactionAsync(VehicleActivityDTO vehicleActivityDTO);
    }
}

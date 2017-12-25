using System.Threading.Tasks;
using VehicleMonitoring.Core.UnitOfWork;

namespace VehicleMonitoring.ListenerService.Infrastructure.UnitOfWork
{
    public interface IListenerServiceUOW: IUnitOfWork
    {
        Task<bool> NotifyRealTimeClients(string vehicleId, bool status);
        Task<bool> ReportVehicleStatusRecieved(string vehicleId, bool status);
    }
}

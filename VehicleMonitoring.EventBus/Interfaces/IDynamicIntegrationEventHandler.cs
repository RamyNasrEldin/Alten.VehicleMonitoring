using System.Threading.Tasks;

namespace VehicleMonitoring.EventBus.Interfaces
{
    public interface IDynamicIntegrationEventHandler
    {
        Task Handle(dynamic eventData);
    }
}

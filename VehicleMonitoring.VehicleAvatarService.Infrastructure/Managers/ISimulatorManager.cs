using System.Threading.Tasks;

namespace VehicleMonitoring.VehicleAvatarService.Infrastructure.Managers
{
    public interface ISimulatorManager
    {

        Task PushRandomMessagesAsync();
        void PushRandomMessages();
    }
}

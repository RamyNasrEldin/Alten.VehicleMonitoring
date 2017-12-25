using Microsoft.AspNetCore.SignalR;
using VehicleMonitoring.SignalR.Notifications;

namespace VehicleMonitoring.SignalR.HubManagers
{
    public class VehicleMonitoringHub : Hub
    {
        public void Send(VehicleStatusNotification notification)
        {
            // Call the broadcastMessage method to update clients.
            Clients.All.InvokeAsync("vehicleStatusChanged", notification);
            


        }
    }
}
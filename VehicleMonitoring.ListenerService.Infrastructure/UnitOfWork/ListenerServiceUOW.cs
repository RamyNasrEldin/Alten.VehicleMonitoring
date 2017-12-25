using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using VehicleMonitoring.EventBus.Interfaces;
using VehicleMonitoring.ListenerService.Infrastructure.EventHandlers;
using VehicleMonitoring.Messaging.Events;
using VehicleMonitoring.SignalR.HubManagers;
using VehicleMonitoring.SignalR.Notifications;

namespace VehicleMonitoring.ListenerService.Infrastructure.UnitOfWork
{
    public class ListenerServiceUOW : IListenerServiceUOW
    {
        #region Data Members 
        private bool disposed = false;
        private IEventBus _eventBus;
        private ILogger<ListenerServiceUOW> _logger;
        private IHubContext<VehicleMonitoringHub> _hubContext;

        #endregion
        #region Constructor
        public ListenerServiceUOW(ILoggerFactory loggerFactory, IHubContext<VehicleMonitoringHub> hubContext, IEventBus eventBus)
        {
            _hubContext = hubContext;
            _eventBus = eventBus;
            _logger = loggerFactory?.CreateLogger<ListenerServiceUOW>();
            if (_logger == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }
        }
        #endregion
        #region Public Methods
        public async Task<bool> NotifyRealTimeClients(string vehicleId, bool status)
        {
            var notification = new VehicleStatusNotification(vehicleId,status);
            await _hubContext.Clients.All.InvokeAsync("vehicleStatusChanged", notification);
            return true;
        }

        public async Task<bool> ReportVehicleStatusRecieved(string vehicleId, bool status)
        {
            var @eventRecieved = new VehicleStatusRecievedIntegrationEvent(vehicleId, status);
            _eventBus.Publish(@eventRecieved);
            return true;
        }
        #endregion
        #region Private Methods

        #endregion
        #region Disposing 
        protected virtual void Dispose(bool disposing)
        {

            if (!this.disposed)
            {
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        #endregion
    }
}

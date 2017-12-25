using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VehicleMonitoring.EventBus.Interfaces;
using VehicleMonitoring.ListenerService.Infrastructure.UnitOfWork;
using VehicleMonitoring.Messaging.Events;
using VehicleMonitoring.SignalR.HubManagers;

namespace VehicleMonitoring.ListenerService.Infrastructure.EventHandlers
{
    public class VehicleStatusChangedIntegrationEventHandler : IIntegrationEventHandler<VehicleStatusChangedIntegrationEvent>
    {
        #region Data Memebers
        IListenerServiceUOW _uow;
        private ILogger<VehicleStatusChangedIntegrationEventHandler> _logger;
        #endregion
        #region CTOR
        public VehicleStatusChangedIntegrationEventHandler(IListenerServiceUOW uow, ILoggerFactory loggerFactory)
        {
            _uow = uow;
            _logger = loggerFactory?.CreateLogger<VehicleStatusChangedIntegrationEventHandler>();
            if (_logger == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }
        }
        #endregion
        public async Task Handle(VehicleStatusChangedIntegrationEvent @event)
        {
            try
            {
                List<Task> taskList = new List<Task>();
                taskList.Add(Task.Run<bool>(() =>
                {
                    return _uow.NotifyRealTimeClients(@event.VehicleId, @event.Status);
                }));
                taskList.Add(Task.Run<bool>(() =>
                {
                    return _uow.ReportVehicleStatusRecieved(@event.VehicleId, @event.Status);
                }));

                await Task.WhenAll(taskList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

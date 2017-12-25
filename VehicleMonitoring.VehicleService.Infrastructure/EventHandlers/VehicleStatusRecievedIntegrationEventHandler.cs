using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using VehicleMonitoring.EventBus.Interfaces;
using VehicleMonitoring.Messaging.Events;
using VehicleMonitoring.VehicleService.Infrastructure.UnitOfWork;

namespace VehicleMonitoring.ListenerService.Infrastructure.EventHandlers
{
    public class VehicleStatusRecievedIntegrationEventHandler : IIntegrationEventHandler<VehicleStatusRecievedIntegrationEvent>
    {
        #region Data Memebers
        IVehicleServiceUOW _uow;
        private IEventBus _eventBus;
        private ILogger<VehicleStatusRecievedIntegrationEventHandler> _logger;
        #endregion
        #region CTOR
        public VehicleStatusRecievedIntegrationEventHandler(IVehicleServiceUOW uow, IEventBus eventBus, ILoggerFactory loggerFactory)
        {
            _uow = uow;
            _eventBus = eventBus;
            _logger = loggerFactory?.CreateLogger<VehicleStatusRecievedIntegrationEventHandler>();
            if (_logger == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }
        }
        #endregion
        public async Task Handle(VehicleStatusRecievedIntegrationEvent @event)
        {
            try
            {
                await _uow.UpdateVehicleStatus(@event.VehicleId, @event.Status);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                throw ex;
            }
        }
    }
}

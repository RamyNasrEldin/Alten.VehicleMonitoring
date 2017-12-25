using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VehicleMonitoring.ActivityService.DTO;
using VehicleMonitoring.ActivityService.Infrastructure.UnitOfWork;
using VehicleMonitoring.EventBus.Interfaces;
using VehicleMonitoring.Messaging.Events;

namespace VehicleMonitoring.ActivityService.Infrastructure.EventHandlers
{
    public class VehicleStatusChangedIntegrationEventHandler : IIntegrationEventHandler<VehicleStatusChangedIntegrationEvent>
    {
        #region Data Memebers
        IVehicleActivityServiceUOW _uow;
        #endregion
        #region CTOR
        public VehicleStatusChangedIntegrationEventHandler(IVehicleActivityServiceUOW uow)
        {
            _uow = uow;
        }
        #endregion

        public async Task Handle(VehicleStatusChangedIntegrationEvent @event)
        {
            try
            {
                var newActivity = new VehicleActivityDTO(@event.VehicleId, @event.Status);
                await _uow.SaveVehicleActivityTransactionAsync(newActivity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

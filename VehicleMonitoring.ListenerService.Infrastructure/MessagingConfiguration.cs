using System;
using System.Collections.Generic;
using System.Text;
using VehicleMonitoring.EventBus.Interfaces;
using VehicleMonitoring.ListenerService.Infrastructure.Interfaces;

namespace VehicleMonitoring.ListenerService.Infrastructure
{
    public class MessagingConfiguration : IMessagingConfiguration
    {

        #region Data Members
        private IEventBus _EventBus;
        IApplicationBuilder app;
        #endregion
        public MessagingConfiguration(IEventBus eventBus)
        {
            _EventBus = eventBus;
        }
        public void SubscribeToVehicleStatusChangedIntegrationEvent()
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
            eventBus.Subscribe<ProductPriceChangedIntegrationEvent>(
                ProductPriceChangedIntegrationEventHandler);
        }
    }
}

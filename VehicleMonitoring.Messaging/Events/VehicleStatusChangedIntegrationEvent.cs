using System;
using System.Collections.Generic;
using System.Text;
using VehicleMonitoring.EventBus.Events;

namespace VehicleMonitoring.Messaging.Events
{
    public class VehicleStatusChangedIntegrationEvent : IntegrationEvent
    {
        #region Data Members
        public string VehicleId { get; private set; }
        public bool Status { get; private set; }
        #endregion

        #region CTOR
        public VehicleStatusChangedIntegrationEvent(string vehicleId, bool status)
        {
            this.VehicleId = vehicleId;
            this.Status = status;
        }
        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Text;
using VehicleMonitoring.EventBus.Events;

namespace VehicleMonitoring.Messaging.Events
{
    public class VehicleStatusRecievedIntegrationEvent : IntegrationEvent
    {
        #region Data Members
        public string VehicleId { get; private set; }
        public bool Status { get; private set; }
        #endregion

        #region CTOR
        public VehicleStatusRecievedIntegrationEvent(string vehicleId, bool status)
        {
            this.VehicleId = vehicleId;
            this.Status = status;
        }
        #endregion
    }
}

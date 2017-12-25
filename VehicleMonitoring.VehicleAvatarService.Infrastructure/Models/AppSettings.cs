using System;
using System.Collections.Generic;
using System.Text;

namespace VehicleMonitoring.VehicleAvatarService.Simulator
{
    public class EventBusAppSettings
    {
        public string ConnectionString { get; set; }
        public string EventBusConnection { get; set; }
        public string EventBusUserName { get; set; }
        public string EventBusPassword { get; set; }
        public int EventBusRetryCount { get; set; }
    }

    public class GeneralAppSettings
    {
        public double VehicleStatusPushingInterval  { get; set; }
    }
}

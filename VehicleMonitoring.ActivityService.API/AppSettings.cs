﻿namespace VehicleMonitoring.ActivityService.API
{
    public class EventBusAppSettings
    {
        public string QueueName { get; set; }
        public string EventBusConnection { get; set; }
        public string EventBusUserName { get; set; }
        public string EventBusPassword { get; set; }
        public string SubscriptionClientName { get; set; }
        public int EventBusRetryCount { get; set; }        
    }
    public class GeneralAppSettings
    {
        public string ConnectionString { get; set; }
    }
}

using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VehicleMonitoring.EventBus.Interfaces;
using VehicleMonitoring.Messaging.Events;

namespace VehicleMonitoring.VehicleAvatarService.Infrastructure.Managers
{
    public class SimulatorManager : ISimulatorManager
    {
        #region Data Memebers
        private IEventBus _eventBus;
        private ILogger<SimulatorManager> _logger;
        private List<string> Avatars;
        #endregion
        public SimulatorManager(IEventBus eventBus, ILoggerFactory loggerFactory)
        {
            _eventBus = eventBus;
            _logger = loggerFactory?.CreateLogger<SimulatorManager>();
            if (_logger == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }
            LoadAvatars();
        }
        #region CTOR

        #endregion

        #region Public Operations
        public async Task PushRandomMessagesAsync()
        {
            List<Task> taskList = new List<Task>();
            Random gen = new Random();
            foreach (var avatar in Avatars)
            {
                taskList.Add(Task.Run(() =>
                {
                    try
                    {
                        //here we suppose to ping the vehicle by it's TCP IP/port
                        //and return true or false based on the ping result
                        //But for demo purpose we just generate random value for each vehicle status.
                        int prob = gen.Next(100);
                        var status = prob <= 40;
                        var @event = new VehicleStatusChangedIntegrationEvent(avatar, status);
                        _eventBus.Publish(@event);

                        using (_logger.BeginScope("Messaging Avatar"))
                        {
                            _logger.LogInformation($"New message published from Vehicle: {avatar} with status: {status} at {DateTime.Now.ToLongTimeString()}");
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogCritical(ex.Message);
                    }
                }));
            }
            await Task.WhenAll(taskList);
        }

        public void PushRandomMessages()
        {
            try
            {
                Random gen = new Random();
                foreach (var avatar in Avatars)
                {
                    //here we suppose to ping the vehicle by it's TCP IP/port
                    //and return true or false based on the ping result
                    //But for demo purpose we just generate random value for each vehicle status.
                    int prob = gen.Next(100);
                    var status = prob <= 40;
                    var @event = new VehicleStatusChangedIntegrationEvent(avatar, status);
                    _eventBus.Publish(@event);

                    using (_logger.BeginScope("Messaging Avatar"))
                    {
                        _logger.LogInformation($"New message published from Vehicle: {avatar} with status: {status} at {DateTime.Now.ToLongTimeString()}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);

            }
        }
        #endregion

        #region Helper methods
        private void LoadAvatars() => Avatars = new List<string>
            {
                 "YS2R4X20005399401",
                "VLUR4X20009093588",
                "VLUR4X20009048066",
                "YS2R4X20005388011",
                "YS2R4X20005387949",
                "YS2R4X20005387765",
                "YS2R4X20005387055"
            };
        
        #endregion

    }
}

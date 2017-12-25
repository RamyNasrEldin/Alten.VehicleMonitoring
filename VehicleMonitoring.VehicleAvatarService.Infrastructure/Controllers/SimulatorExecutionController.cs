using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using System.Timers;
using VehicleMonitoring.VehicleAvatarService.Infrastructure.Managers;

namespace VehicleMonitoring.VehicleAvatarService.Simulator
{
    public class SimulatorExecutionController : ISimulatorExecutionController
    {
        #region Data Members
        ISimulatorManager _manager;
        Timer _simulatorTimer;
        GeneralAppSettings _config;
        #endregion

        #region CTOR
        public SimulatorExecutionController(ISimulatorManager manager, IOptions<GeneralAppSettings> config)
        {
            _manager = manager;
            _config = config.Value;
            SetupTimer();
        }


        #endregion

        #region Public Operations
        public bool StartExecution()
        {
            try
            {
                _simulatorTimer.Start();
                return true;
            }
            catch (Exception ex)
            {
                //Log the exception
                return false;
            }

        }
        #endregion

        #region Helper Methods
        private void SetupTimer()
        {
            _simulatorTimer = new Timer
            {
                Interval = _config.VehicleStatusPushingInterval > 0 ? _config.VehicleStatusPushingInterval : 60000
            };
            _simulatorTimer.Elapsed += new ElapsedEventHandler(SimulatorTimer_Elapsed);
        }
        private void SimulatorTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _simulatorTimer.Enabled = false;
            StartPushingAsync().Wait();
            _simulatorTimer.Enabled = true;
        }

        private async Task StartPushingAsync()
        {
            await _manager.PushRandomMessagesAsync();
        }
        private void StartPushing()
        {
            _manager.PushRandomMessages();
        }
        #endregion
    }
}

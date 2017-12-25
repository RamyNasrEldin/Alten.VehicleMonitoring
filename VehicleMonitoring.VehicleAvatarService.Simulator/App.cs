using Autofac;
using System;
using System.Collections.Generic;
using System.Text;
using VehicleMonitoring.EventBus.Interfaces;

namespace VehicleMonitoring.VehicleAvatarService.Simulator
{
    public interface IApp
    {
        void Run();
    }
    public class App : IApp
    {
        private ISimulatorExecutionController _executionController;
        public App(ISimulatorExecutionController executionController)
        {
            _executionController = executionController;
        }


        public void Run()
        {
            _executionController.StartExecution();
            Console.WriteLine("\n************* Simulator is running *****************\n");
        }
    }
}

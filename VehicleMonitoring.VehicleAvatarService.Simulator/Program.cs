using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace VehicleMonitoring.VehicleAvatarService.Simulator
{
    class Program
    {

        static void Main(string[] args)
        {
            try
            {
                var serviceCollection = new ServiceCollection();
                ContainerConfig.ConfigureServices(serviceCollection);
                var serviceProvider = serviceCollection.BuildServiceProvider();
                var _logger = serviceProvider.GetRequiredService<ILogger<Program>>();
                Console.WriteLine("Hello, Welcome to Alten Simulator.\n");
                try
                {
                    var application = serviceProvider.GetService<App>();
                    application.Run();
                }
                catch (Exception ex)
                {
                    _logger.LogCritical(ex.Message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}

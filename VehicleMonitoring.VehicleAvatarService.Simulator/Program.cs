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

                //new Startup().ConfigureServices(serviceCollection);
                ContainerConfig.ConfigureServices(serviceCollection);
                var serviceProvider = serviceCollection.BuildServiceProvider();

                var _logger = serviceProvider.GetRequiredService<ILogger<Program>>();
                Console.WriteLine("Hello, Welcome to Alten Simulator,\nDo you want to start sending random status values for the fleet vehicles?");
                var result = Console.ReadKey();
                Console.WriteLine("\n");
                if (result.Key == ConsoleKey.Y)
                {
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
                else
                {
                    Console.WriteLine("\n******          Thank you for using our simulator         ****** \nPlease press any key to exit");
                    Console.ReadKey();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}

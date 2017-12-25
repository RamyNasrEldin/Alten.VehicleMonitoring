using VehicleMonitoring.EventBusRabbitMQ;
using VehicleMonitoring.EventBus.Interfaces;
using VehicleMonitoring.VehicleAvatarService.Infrastructure.Managers;
using Microsoft.Extensions.Logging;
using VehicleMonitoring.EventBus.Subscriptions;
using RabbitMQ.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.Extensions.Options;
using Autofac;
using System.Collections.Generic;

namespace VehicleMonitoring.VehicleAvatarService.Simulator
{
    public static class ContainerConfig
    {
        private static int retryCount = 5;
        public static void ConfigureServices(IServiceCollection serviceCollection)
        {
            // add configured instance of logging
            serviceCollection.AddSingleton(new LoggerFactory()
                .AddConsole()
                .AddDebug());

            serviceCollection.AddLogging(config => config.AddConsole());
            
            // build configuration
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("app-settings.json", false)
                .Build();
            serviceCollection.AddOptions();
            serviceCollection.Configure<EventBusAppSettings>(configurationBuilder.GetSection("EventBusConfiguration"));
            serviceCollection.Configure<GeneralAppSettings>(configurationBuilder.GetSection("GeneralConfiguration"));

            serviceCollection.AddSingleton<ISimulatorManager, SimulatorManager>();
            serviceCollection.AddSingleton<ISimulatorExecutionController, SimulatorExecutionController>();
            serviceCollection.AddSingleton<IApp, App>();
            serviceCollection.AddSingleton<ILoggerFactory, LoggerFactory>();
            serviceCollection.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();
            
            serviceCollection.AddSingleton<IRabbitMQPersistentConnection>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();
                var _config = sp.GetRequiredService<IOptions<EventBusAppSettings>>().Value;

                var factory = new ConnectionFactory()
                {
                    HostName = _config.EventBusConnection
                };
                
                if (!string.IsNullOrEmpty(_config.EventBusUserName))
                {
                    factory.UserName = _config.EventBusUserName;
                }
                
                if (!string.IsNullOrEmpty(_config.EventBusPassword))
                {
                    factory.Password = _config.EventBusPassword;
                }

                var retryCount = 5;
                if (_config.EventBusRetryCount>0)
                {
                    retryCount = _config.EventBusRetryCount;
                }

                return new DefaultRabbitMQPersistentConnection(factory, logger, retryCount);
            });

            serviceCollection.AddSingleton<IEventBus, RabbitMQBus>(sp =>
            {
                var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
                var logger = sp.GetRequiredService<ILogger<RabbitMQBus>>();
                
                var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();
                
                return new RabbitMQBus(rabbitMQPersistentConnection, logger, null, eventBusSubcriptionsManager, null, retryCount);
            });

            serviceCollection.AddSingleton<App>();
        }
    }
}

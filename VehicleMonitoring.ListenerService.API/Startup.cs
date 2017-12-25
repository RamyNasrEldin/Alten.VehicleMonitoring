using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.IO;
using VehicleMonitoring.Core.FilterAttributes;
using VehicleMonitoring.EventBus.Interfaces;
using VehicleMonitoring.EventBus.Subscriptions;
using VehicleMonitoring.EventBusRabbitMQ;
using VehicleMonitoring.ListenerService.Infrastructure.EventHandlers;
using VehicleMonitoring.ListenerService.Infrastructure.UnitOfWork;
using VehicleMonitoring.Messaging.Events;
using VehicleMonitoring.SignalR.HubManagers;

namespace VehicleMonitoring.ListenerService.API
{
    public class Startup
    {
        public EventBusAppSettings _config;
        public Startup(IConfiguration configuration)
        {
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            
            services.AddSignalR();
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false)
                .Build();
            
            services.Configure<EventBusAppSettings>(configurationBuilder.GetSection("EventBusConfiguration"));
            services.Configure<GeneralAppSettings>(configurationBuilder.GetSection("GeneralConfiguration"));

            var generalSettings = services.BuildServiceProvider().GetRequiredService<IOptions<GeneralAppSettings>>().Value;
            services.AddSingleton<IListenerServiceUOW, ListenerServiceUOW>();

            services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();
                _config = sp.GetRequiredService<IOptions<EventBusAppSettings>>().Value;

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
                if (_config.EventBusRetryCount > 0)
                {
                    retryCount = _config.EventBusRetryCount;
                }

                return new DefaultRabbitMQPersistentConnection(factory, logger, retryCount);
            });

            RegisterEventBus(services);
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            services.AddOptions();
            services.AddMvc(options =>
            {
                options.Filters.Add(new ApiExceptionFilter());
            });
            var container = new ContainerBuilder();
            container.Populate(services);

            return new AutofacServiceProvider(container.Build());

        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
           
            app.UseCors("CorsPolicy");
            app.UseSignalR(routes =>
            {
                routes.MapHub<VehicleMonitoringHub>("vehicleMonitoring");
            });
            
            app.UseMvc();
            
            ConfigureEventBus(app);
        }

        private void ConfigureEventBus(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
            eventBus.Subscribe<VehicleStatusChangedIntegrationEvent, VehicleStatusChangedIntegrationEventHandler>();
        }

        private void RegisterEventBus(IServiceCollection services)
        {
            services.AddSingleton<IEventBus, RabbitMQBus>(sp =>
            {
                var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
                var iLifetimeScope = sp.GetRequiredService<ILifetimeScope>();
                var logger = sp.GetRequiredService<ILogger<RabbitMQBus>>();
                var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

                var retryCount = 5;
                if (_config.EventBusRetryCount > 0)
                {
                    retryCount = _config.EventBusRetryCount;
                }
                
                return new RabbitMQBus(rabbitMQPersistentConnection, logger, iLifetimeScope, eventBusSubcriptionsManager, _config.SubscriptionClientName, retryCount);
            });

            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();

            services.AddSingleton<VehicleStatusChangedIntegrationEventHandler>();
        }
    }
}

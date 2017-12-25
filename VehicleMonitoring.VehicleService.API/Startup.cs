using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.IO;
using VehicleMonitoring.Core.FilterAttributes;
using VehicleMonitoring.Core.Repository;
using VehicleMonitoring.EventBus.Interfaces;
using VehicleMonitoring.EventBus.Subscriptions;
using VehicleMonitoring.EventBusRabbitMQ;
using VehicleMonitoring.ListenerService.Infrastructure.EventHandlers;
using VehicleMonitoring.Messaging.Events;
using VehicleMonitoring.VehicleService.Data;
using VehicleMonitoring.VehicleService.Data.SampleData;
using VehicleMonitoring.VehicleService.Infrastructure.Repository;
using VehicleMonitoring.VehicleService.Infrastructure.UnitOfWork;

namespace VehicleMonitoring.VehicleService.API
{
    public class Startup
    {
        public EventBusAppSettings _config;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            
        }

        public IConfiguration Configuration { get; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false)
                .Build();
            
            services.Configure<EventBusAppSettings>(configurationBuilder.GetSection("EventBusConfiguration"));
            services.Configure<GeneralAppSettings>(configurationBuilder.GetSection("GeneralConfiguration"));

            var generalSettings = services.BuildServiceProvider().GetRequiredService<IOptions<GeneralAppSettings>>().Value;

            services.AddDbContext<VehicleServiceDbContext>(options => options.UseSqlite(generalSettings.ConnectionString));
            services.AddTransient<DbInitializer>();

            // Add service and create Policy with options
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

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
          
            services.AddMvc(options =>
            {
                options.Filters.Add(new ApiExceptionFilter());
            });
            services.AddOptions();
            services.AddTransient<DbContext, VehicleServiceDbContext>();
            services.AddTransient<IRepositoryProvider, RepositoryProvider>();
            services.AddTransient<IVehicleServiceUOW, VehicleServiceUOW>();
            services.AddTransient<IRepositoryFactory, RepositoryFactory>();
            services.AddTransient<IRepositoryProvider, RepositoryProvider>();

            var container = new ContainerBuilder();
            container.Populate(services);

            return new AutofacServiceProvider(container.Build());
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory,
                              DbInitializer DbSeeder)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
           
            app.UseCors("CorsPolicy");
            app.UseMvc();
            ConfigureEventBus(app);
            DbSeeder.Initialize().Wait();
        }

        private void ConfigureEventBus(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
            eventBus.Subscribe<VehicleStatusRecievedIntegrationEvent, VehicleStatusRecievedIntegrationEventHandler>();
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

            services.AddSingleton<VehicleStatusRecievedIntegrationEventHandler>();
        }
    }
}

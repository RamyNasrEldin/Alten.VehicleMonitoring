using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using VehicleMonitoring.ActivityService.Infrastructure.EventHandlers;
using VehicleMonitoring.EventBus.Interfaces;
using VehicleMonitoring.EventBus.Subscriptions;
using VehicleMonitoring.EventBusRabbitMQ;
using VehicleMonitoring.Messaging.Events;
using Autofac.Extensions.DependencyInjection;
using System;
using Microsoft.Extensions.Options;
using System.IO;
using VehicleMonitoring.ActivityService.Infrastructure.UnitOfWork;
using VehicleMonitoring.ActivityService.Data;
using VehicleMonitoring.ActivityService.Data.SampleData;
using Microsoft.EntityFrameworkCore;
using VehicleMonitoring.Core.Repository;
using VehicleMonitoring.ActivityService.Infrastructure.Repository;
using System.Collections.Generic;
using VehicleMonitoring.Core.FilterAttributes;

namespace VehicleMonitoring.ActivityService.API
{
    public class Startup
    {

        public EventBusAppSettings _config;
        public static IEventBusSubscriptionsManager _eventBusSubcriptionsManager;
        public Startup(IConfiguration configuration)
        {
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
           
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false)
                .Build();
            services.AddOptions();
            services.Configure<EventBusAppSettings>(configurationBuilder.GetSection("EventBusConfiguration"));
            services.Configure<GeneralAppSettings>(configurationBuilder.GetSection("GeneralConfiguration"));
            
            var generalSettings = services.BuildServiceProvider().GetRequiredService<IOptions<GeneralAppSettings>>().Value;

            services.AddDbContext<ActivityServiceDbContext>(options => options.UseSqlite(generalSettings.ConnectionString)); 
            
            services.AddSingleton<DbInitializer>();
            services.AddSingleton<DbContext, ActivityServiceDbContext>();
            services.AddSingleton<IRepositoryProvider, RepositoryProvider>();
            services.AddSingleton<IVehicleActivityServiceUOW, VehicleActivityServiceUOW>();
            services.AddSingleton<IRepositoryFactory, RepositoryFactory>();
            services.AddSingleton<IRepositoryProvider, RepositoryProvider>();

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
            services.AddMvc(options =>
            {
                options.Filters.Add(new ApiExceptionFilter());
            });
            services.AddOptions();

            var container = new ContainerBuilder();
            container.Populate(services);

            return new AutofacServiceProvider(container.Build());

        }
        private void ConfigureEventBus(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
            eventBus.Subscribe<VehicleStatusChangedIntegrationEvent, VehicleStatusChangedIntegrationEventHandler>();
        }
        private void RegisterEventBus(IServiceCollection services)
        {
            // = services.Resolve<IEventBusSubscriptionsManager>();//app.ApplicationServices.GetRequiredService<IEventBusSubscriptionsManager>();
            services.AddSingleton<IEventBus, RabbitMQBus>(sp =>
            {
                var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
                var iLifetimeScope = sp.GetRequiredService<ILifetimeScope>();
                var logger = sp.GetRequiredService<ILogger<RabbitMQBus>>();
                if (_eventBusSubcriptionsManager == null)
                    _eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();
                  var retryCount = 5;
                if (_config.EventBusRetryCount > 0)
                {
                    retryCount = _config.EventBusRetryCount;
                }
                
                return new RabbitMQBus(rabbitMQPersistentConnection, logger, iLifetimeScope, _eventBusSubcriptionsManager, _config.SubscriptionClientName, retryCount);
            });

            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();
            services.AddSingleton<VehicleStatusChangedIntegrationEventHandler>();
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
            DbSeeder.Initialize().Wait();
            ConfigureEventBus(app);
        }


    }
}

FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY *.sln ./
COPY VehicleMonitoring.ListenerService.API/VehicleMonitoring.ListenerService.API.csproj VehicleMonitoring.ListenerService.API/
COPY VehicleMonitoring.ListenerService.Infrastructure/VehicleMonitoring.ListenerService.Infrastructure.csproj VehicleMonitoring.ListenerService.Infrastructure/
COPY VehicleMonitoring.Core/VehicleMonitoring.Core.csproj VehicleMonitoring.Core/
COPY VehicleMonitoring.EventBusRabbitMQ/VehicleMonitoring.EventBusRabbitMQ.csproj VehicleMonitoring.EventBusRabbitMQ/
COPY VehicleMonitoring.EventBus/VehicleMonitoring.EventBus.csproj VehicleMonitoring.EventBus/
COPY VehicleMonitoring.SignalR/VehicleMonitoring.SignalR.csproj VehicleMonitoring.SignalR/
COPY VehicleMonitoring.Messaging/VehicleMonitoring.Messaging.csproj VehicleMonitoring.Messaging/
RUN dotnet restore
COPY . .
WORKDIR /src/VehicleMonitoring.ListenerService.API
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "VehicleMonitoring.ListenerService.API.dll"]

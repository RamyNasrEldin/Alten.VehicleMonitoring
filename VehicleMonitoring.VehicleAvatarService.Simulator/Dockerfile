FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY *.sln ./
COPY VehicleMonitoring.VehicleAvatarService.Simulator/VehicleMonitoring.VehicleAvatarService.Simulator.csproj VehicleMonitoring.VehicleAvatarService.Simulator/
COPY VehicleMonitoring.VehicleAvatarService.Infrastructure/VehicleMonitoring.VehicleAvatarService.Infrastructure.csproj VehicleMonitoring.VehicleAvatarService.Infrastructure/
COPY VehicleMonitoring.EventBusRabbitMQ/VehicleMonitoring.EventBusRabbitMQ.csproj VehicleMonitoring.EventBusRabbitMQ/
COPY VehicleMonitoring.EventBus/VehicleMonitoring.EventBus.csproj VehicleMonitoring.EventBus/
COPY VehicleMonitoring.Messaging/VehicleMonitoring.Messaging.csproj VehicleMonitoring.Messaging/
RUN dotnet restore
COPY . .
WORKDIR /src/VehicleMonitoring.VehicleAvatarService.Simulator
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "VehicleMonitoring.VehicleAvatarService.Simulator.dll"]

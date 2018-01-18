FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY *.sln ./
COPY VehicleMonitoring.ActivityService.API/VehicleMonitoring.ActivityService.API.csproj VehicleMonitoring.ActivityService.API/
COPY VehicleMonitoring.ActivityService.Infrastructure/VehicleMonitoring.ActivityService.Infrastructure.csproj VehicleMonitoring.ActivityService.Infrastructure/
COPY VehicleMonitoring.Repository/VehicleMonitoring.Repository.csproj VehicleMonitoring.Repository/
COPY VehicleMonitoring.Core/VehicleMonitoring.Core.csproj VehicleMonitoring.Core/
COPY VehicleMonitoring.ActivityService.DTO/VehicleMonitoring.ActivityService.DTO.csproj VehicleMonitoring.ActivityService.DTO/
COPY VehicleMonitoring.ActivityService.DomainModels/VehicleMonitoring.ActivityService.DomainModels.csproj VehicleMonitoring.ActivityService.DomainModels/
COPY VehicleMonitoring.ActivityService.Data/VehicleMonitoring.ActivityService.Data.csproj VehicleMonitoring.ActivityService.Data/
COPY VehicleMonitoring.Messaging/VehicleMonitoring.Messaging.csproj VehicleMonitoring.Messaging/
COPY VehicleMonitoring.EventBus/VehicleMonitoring.EventBus.csproj VehicleMonitoring.EventBus/
COPY VehicleMonitoring.EventBusRabbitMQ/VehicleMonitoring.EventBusRabbitMQ.csproj VehicleMonitoring.EventBusRabbitMQ/
RUN dotnet restore
COPY . .
WORKDIR /src/VehicleMonitoring.ActivityService.API
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "VehicleMonitoring.ActivityService.API.dll"]

FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY *.sln ./
COPY VehicleMonitoring.VehicleService.API/VehicleMonitoring.VehicleService.API.csproj VehicleMonitoring.VehicleService.API/
COPY VehicleMonitoring.VehicleService.Infrastructure/VehicleMonitoring.VehicleService.Infrastructure.csproj VehicleMonitoring.VehicleService.Infrastructure/
COPY VehicleMonitoring.VehicleService.Data/VehicleMonitoring.VehicleService.Data.csproj VehicleMonitoring.VehicleService.Data/
COPY VehicleMonitoring.VehicleService.DomainModels/VehicleMonitoring.VehicleService.DomainModels.csproj VehicleMonitoring.VehicleService.DomainModels/
COPY VehicleMonitoring.Repository/VehicleMonitoring.Repository.csproj VehicleMonitoring.Repository/
COPY VehicleMonitoring.Core/VehicleMonitoring.Core.csproj VehicleMonitoring.Core/
COPY VehicleMonitoring.VehicleService.DTO/VehicleMonitoring.VehicleService.DTO.csproj VehicleMonitoring.VehicleService.DTO/
COPY VehicleMonitoring.Messaging/VehicleMonitoring.Messaging.csproj VehicleMonitoring.Messaging/
COPY VehicleMonitoring.EventBus/VehicleMonitoring.EventBus.csproj VehicleMonitoring.EventBus/
COPY VehicleMonitoring.EventBusRabbitMQ/VehicleMonitoring.EventBusRabbitMQ.csproj VehicleMonitoring.EventBusRabbitMQ/
COPY VehicleMonitoring.SignalR/VehicleMonitoring.SignalR.csproj VehicleMonitoring.SignalR/
RUN dotnet restore
COPY . .
WORKDIR /src/VehicleMonitoring.VehicleService.API
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "VehicleMonitoring.VehicleService.API.dll"]

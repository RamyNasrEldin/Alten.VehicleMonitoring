FROM microsoft/aspnetcore:2.0 AS base
RUN apt-get update && \
    apt-get install -y wget && \
    apt-get install -y gnupg2 && \
    wget -qO- https://deb.nodesource.com/setup_6.x | bash - && \
    apt-get install -y build-essential nodejs
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY *.sln ./
COPY VehicleMonitoring.WebSPA/VehicleMonitoring.WebSPA.csproj VehicleMonitoring.WebSPA/
RUN dotnet restore
COPY . .
WORKDIR /src/VehicleMonitoring.WebSPA
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "VehicleMonitoring.WebSPA.dll"]

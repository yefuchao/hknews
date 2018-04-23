FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY *.sln ./
COPY src/HKExNews.Background/HKExNews.Background.csproj src/HKExNews.Background/
COPY src/BuildingBlocks/EventBusRabbitMQ/EventBusRabbitMQ/EventBusRabbitMQ.csproj src/BuildingBlocks/EventBusRabbitMQ/EventBusRabbitMQ/
COPY src/BuildingBlocks/EventBus/EventBus/EventBus.csproj src/BuildingBlocks/EventBus/EventBus/
RUN dotnet restore
COPY . .
WORKDIR /src/src/HKExNews.Background
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "HKExNews.Background.dll"]

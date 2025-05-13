FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY Microservices/NotificationService/ Microservices/NotificationService/

COPY Shared/ Shared/

WORKDIR /src/Microservices/NotificationService

RUN dotnet restore "NotificationService.csproj"
RUN dotnet build "NotificationService.csproj" -c Release -o /app/build
FROM build AS publish

WORKDIR /src/Microservices/NotificationService

RUN dotnet publish "NotificationService.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

COPY --from=publish /app/publish .
EXPOSE 8084
ENTRYPOINT ["dotnet", "NotificationService.dll"]

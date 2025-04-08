FROM mcr.microsoft.com/dotnet/sdk:8.0 AS buil

WORKDIR /src

COPY Microservices/AnalyticsService/ ./

RUN dotnet restore "AnalyticsService.csproj"
RUN dotnet build "AnalyticsService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AnalyticsService.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

COPY --from=publish /app/publish .

EXPOSE 8081

ENTRYPOINT ["dotnet", "AnalyticsService.dll"]
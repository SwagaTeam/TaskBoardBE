FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY Microservices/ProjectService/ Microservices/ProjectService/

COPY Shared/ Shared/

WORKDIR /src/Microservices/ProjectService

RUN dotnet restore "ProjectService.csproj"
RUN dotnet build "ProjectService.csproj" -c Release -o /app/build
FROM build AS publish

RUN dotnet publish "ProjectService.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

WORKDIR /app

COPY --from=publish /app/publish .

EXPOSE 8082

ENTRYPOINT ["dotnet", "ProjectService.dll"]

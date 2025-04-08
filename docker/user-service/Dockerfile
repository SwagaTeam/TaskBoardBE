FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY Microservices/UserService/ ./

RUN dotnet restore "UserService.csproj"
RUN dotnet build "UserService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UserService.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

COPY --from=publish /app/publish .

EXPOSE 8083

ENTRYPOINT ["dotnet", "UserService.dll"]

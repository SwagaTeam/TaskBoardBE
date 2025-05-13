FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY TaskBoardAPI/ TaskBoardAPI/

COPY Shared/ Shared/

WORKDIR /src/TaskBoardAPI

RUN dotnet restore "TaskBoardAPI.csproj"
RUN dotnet build "TaskBoardAPI.csproj" -c Release -o /app/build
FROM build AS publish

RUN dotnet publish "TaskBoardAPI.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

WORKDIR /app

COPY --from=publish /app/publish .

EXPOSE 8082

ENTRYPOINT ["dotnet", "TaskBoardAPI.dll"]

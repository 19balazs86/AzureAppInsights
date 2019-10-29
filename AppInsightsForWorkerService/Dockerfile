FROM mcr.microsoft.com/dotnet/core/runtime:3.0-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["AppInsightsForWorkerService/AppInsightsForWorkerService.csproj", "AppInsightsForWorkerService/"]
RUN dotnet restore "AppInsightsForWorkerService/AppInsightsForWorkerService.csproj"
COPY . .
WORKDIR "/src/AppInsightsForWorkerService"
RUN dotnet build "AppInsightsForWorkerService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AppInsightsForWorkerService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AppInsightsForWorkerService.dll"]
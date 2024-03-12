FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["DataService/DataService.csproj", "DataService/"]
COPY ["Domain/Domain.csproj", "Domain/"]
RUN dotnet restore "DataService/DataService.csproj"
COPY . .
WORKDIR "/src/DataService"
RUN dotnet build "DataService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DataService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DataService.dll"]

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Neo4j/Neo4j.csproj", "Neo4j/"]
RUN dotnet restore "Neo4j/Neo4j.csproj"
COPY . .
WORKDIR "/src/Neo4j"
RUN dotnet build "Neo4j.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Neo4j.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Neo4j.dll"]

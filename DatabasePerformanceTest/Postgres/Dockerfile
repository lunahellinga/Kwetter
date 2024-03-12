FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Postgres/Postgres.csproj", "Postgres/"]
RUN dotnet restore "Postgres/Postgres.csproj"
COPY . .
WORKDIR "/src/Postgres"
RUN dotnet build "Postgres.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Postgres.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY Postgres/root.crt /cert/root.crt
ENTRYPOINT ["dotnet", "Postgres.dll"]

FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["PayloadGenerator/PayloadGenerator.csproj", "PayloadGenerator/"]
RUN dotnet restore "PayloadGenerator/PayloadGenerator.csproj"
COPY . .
WORKDIR "/src/PayloadGenerator"
RUN dotnet build "PayloadGenerator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PayloadGenerator.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PayloadGenerator.dll"]

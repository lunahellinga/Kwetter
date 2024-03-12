FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["MentionService/MentionService.csproj", "MentionService/"]
COPY ["Domain/Domain.csproj", "Domain/"]
RUN dotnet restore "MentionService/MentionService.csproj"
COPY . .
WORKDIR "/src/MentionService"
RUN dotnet build "MentionService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MentionService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MentionService.dll"]

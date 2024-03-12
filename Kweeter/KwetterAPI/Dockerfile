FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["KwetterAPI/KwetterAPI.csproj", "KwetterAPI/"]
COPY ["Domain/Domain.csproj", "Domain/"]
RUN dotnet restore "KwetterAPI/KwetterAPI.csproj"
COPY . .
WORKDIR "/src/KwetterAPI"
RUN dotnet build "KwetterAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "KwetterAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "KwetterAPI.dll"]

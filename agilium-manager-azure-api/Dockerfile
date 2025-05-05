#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/back/API/agilium.api.manager/agilium.api.manager.csproj", "src/back/API/agilium.api.manager/"]
COPY ["src/back/API/agilium.api.business/agilium.api.business.csproj", "src/back/API/agilium.api.business/"]
COPY ["src/back/API/agilium.api.infra/agilium.api.infra.csproj", "src/back/API/agilium.api.infra/"]
RUN dotnet restore "./src/back/API/agilium.api.manager/./agilium.api.manager.csproj"
COPY . .
WORKDIR "/src/src/back/API/agilium.api.manager"
RUN dotnet build "./agilium.api.manager.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./agilium.api.manager.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "agilium.api.manager.dll"]
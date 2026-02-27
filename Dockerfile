# ================================
# 1) STAGE DE BUILD (.NET 5)
# ================================
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src

COPY . .

WORKDIR "/src/agilum.mvc.web"
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish

# ================================
# 2) RUNTIME (.NET 5)
# ================================
FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 80
ENTRYPOINT ["dotnet", "agilum.mvc.web.dll"]
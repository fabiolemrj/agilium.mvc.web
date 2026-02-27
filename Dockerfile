# ================================
# 1) STAGE DE BUILD
# ================================
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src

COPY ["agilium.mvc.web/agilium.mvc.web.csproj", "agilium.mvc.web/"]
RUN dotnet restore "agilium.mvc.web/agilium.mvc.web.csproj"

COPY . .
WORKDIR "/src/agilium.mvc.web"
RUN dotnet publish -c Release -o /app/publish

# ================================
# 2) RUNTIME (.NET Core 3.1)
# ================================
FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 80
ENTRYPOINT ["dotnet", "agilum.mvc.web.dll"]
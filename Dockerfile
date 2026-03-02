# ================================
# 1) BUILD (.NET Core 3.1)
# ================================
FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src

COPY . .

WORKDIR "/src/agilum.mvc.web"
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish


# ================================
# 2) RUNTIME (.NET Core 3.1)
# ================================
FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS runtime

# Dependências nativas do SkiaSharp
RUN apt-get update && apt-get install -y \
    libfontconfig1 \
    libfreetype6 \
    libharfbuzz0b \
    libpng16-16 \
    libjpeg62-turbo \
    libgif7 \
    libwebp7 \
    libx11-6 \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /app

# 🚀 COPIA O BUILD CORRETAMENTE
COPY --from=build /app/publish .

EXPOSE 80
ENTRYPOINT ["dotnet", "agilum.mvc.web.dll"]
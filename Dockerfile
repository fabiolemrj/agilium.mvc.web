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

# Corrigir repositórios buster arquivados
RUN sed -i 's|deb.debian.org|archive.debian.org|g' /etc/apt/sources.list && \
    sed -i 's|security.debian.org|archive.debian.org|g' /etc/apt/sources.list && \
    sed -i '/deb.*buster-updates/s/^/#/' /etc/apt/sources.list && \
    apt-get update

# Instalar dependências do SkiaSharp
RUN apt-get install -y \
    libfontconfig1 \
    libfreetype6 \
    libharfbuzz0b \
    libpng16-16 \
    libjpeg62-turbo \
    libgif7 \
    libwebp6 \
    libx11-6 \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 80
ENTRYPOINT ["dotnet", "agilum.mvc.web.dll"]
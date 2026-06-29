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

# Corrigir repositórios Debian Buster arquivados
RUN sed -i 's|deb.debian.org|archive.debian.org|g' /etc/apt/sources.list && \
    sed -i 's|security.debian.org|archive.debian.org|g' /etc/apt/sources.list && \
    sed -i '/deb.*buster-updates/s/^/#/' /etc/apt/sources.list && \
    apt-get update

# Instalar libgdiplus (obrigatório para System.Drawing no Linux)
RUN apt-get install -y --allow-unauthenticated libgdiplus && \
    rm -rf /var/lib/apt/lists/*

WORKDIR /app
COPY --from=build /app/publish .

# Script de entrada que usa a variável PORT do Render (fallback para 80)
RUN echo '#!/bin/bash\n\
export ASPNETCORE_URLS="http://0.0.0.0:${PORT:-80}"\n\
exec dotnet agilum.mvc.web.dll' > /app/entrypoint.sh && \
    chmod +x /app/entrypoint.sh

EXPOSE 80
ENV ASPNETCORE_ENVIRONMENT=Production
ENV RENDER=true
ENTRYPOINT ["/app/entrypoint.sh"]
# ================================
# 1) BUILD (.NET Core 3.1)
# ================================
FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src

# Copia apenas os .csproj primeiro para cache de restore
COPY agilum.mvc.web/agilum.mvc.web.csproj agilum.mvc.web/
COPY agilium-manager-azure-business/agilium-manager-azure-business.csproj agilium-manager-azure-business/
COPY agilium-manager-git-azure-infra/agilium-manager-git-azure-infra.csproj agilium-manager-git-azure-infra/

RUN dotnet restore agilum.mvc.web/agilum.mvc.web.csproj

# Copia o restante e publica
COPY . .
WORKDIR "/src/agilum.mvc.web"
RUN dotnet publish -c Release -o /app/publish --no-restore


# ================================
# 2) RUNTIME (.NET Core 3.1) - ARM friendly
# ================================
FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS runtime

# Corrigir repositórios Debian Buster arquivados
RUN sed -i 's|deb.debian.org|archive.debian.org|g' /etc/apt/sources.list && \
    sed -i 's|security.debian.org|archive.debian.org|g' /etc/apt/sources.list && \
    sed -i '/deb.*buster-updates/s/^/#/' /etc/apt/sources.list && \
    apt-get update && \
    apt-get install -y --no-install-recommends libgdiplus && \
    rm -rf /var/lib/apt/lists/*

# Link simbólico para libgdiplus (compatibilidade ARM)
RUN ln -sf /usr/lib/arm-linux-gnueabihf/libgdiplus.so /usr/lib/libgdiplus.so 2>/dev/null || true

WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 80
ENTRYPOINT ["dotnet", "agilum.mvc.web.dll"]
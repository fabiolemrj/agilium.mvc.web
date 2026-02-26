# ================================
# 1) STAGE DE BUILD
# ================================
FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src

# Copiar todos os arquivos da solução
COPY . .

# Restaurar dependências
WORKDIR "/src/agilum.mvc.web"
RUN dotnet restore

# Build
RUN dotnet publish -c Release -o /app/publish


# ================================
# 2) STAGE DE RUNTIME
# ================================
FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS runtime
WORKDIR /app

# Copiar a publicação do stage anterior
COPY --from=build /app/publish .

# Porta padrão do ASP.NET Core
EXPOSE 80
EXPOSE 443

# Iniciar aplicação
ENTRYPOINT ["dotnet", "agilum.mvc.web.dll"]
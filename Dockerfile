# Etapa 1 — Build
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# copia somente o csproj primeiro
COPY ["agilium.mvc.web/agilium.mvc.web.csproj", "agilium.mvc.web/"]

# restaura dependências
RUN dotnet restore "agilium.mvc.web/agilium.mvc.web.csproj"

# copia o restante do conteúdo
COPY . .

# publica
WORKDIR "/src/agilium.mvc.web"
RUN dotnet publish -c Release -o /app/publish

# Etapa 2 — Runtime (AspNet)
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "agilium.mvc.web.dll"]
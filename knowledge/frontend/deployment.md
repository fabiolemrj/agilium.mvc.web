# Deploy do Frontend

## Objetivo

Documentar o processo de build, configuração e publicação do frontend do Agilium Manager.

---

# Visão Geral

O frontend MVC é publicado junto com a aplicação .NET Core. O build inclui a compilação das Views Razor e a cópia de assets estáticos de `wwwroot/`. O deploy pode ser feito via Docker, Render cloud ou IIS. O ambiente Render possui configurações específicas (sem HTTPS redirection, porta via `PORT`).

---

# Organização

### Build

```bash
dotnet publish agilum.mvc.web/ -c Release -o ./publish
```

### Docker

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:3.1
WORKDIR /app
COPY ./publish .
ENTRYPOINT ["dotnet", "agilum.mvc.web.dll"]
```

### Render Cloud

```csharp
// Program.cs
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
webBuilder.UseUrls($"http://0.0.0.0:{port}");

// Startup.cs
var isRender = Environment.GetEnvironmentVariable("RENDER") != null;
if (!isRender)
{
    app.UseHttpsRedirection();
    app.UseHsts();
}
```

---

# Principais Conceitos

- **dotnet publish**: Compila Razor Views + copia wwwroot
- **Docker**: Container Linux com .NET Core 3.1
- **Render**: Cloud PaaS com HTTPS gerenciado no proxy
- **IIS**: Suporte via `web.config` ou ANCM
- **Variáveis de ambiente**: `PORT`, `RENDER`, connection strings

---

# Fluxos Relacionados

- `docs/fluxos/fluxo-configuracao.md` — Render cloud

---

# Componentes Relacionados

- `Program.cs` — Configuração de porta e URLs
- `Startup.cs` — Detecção de ambiente Render
- `Dockerfile` — Containerização
- `appsettings.json` — Configurações de ambiente

---

# APIs Relacionadas

- N/A

---

# Boas Práticas

- Usar variáveis de ambiente para configurações sensíveis
- Não commitar `appsettings.Production.json` com secrets
- Minificar assets em produção (já feito nos plugins)
- Testar deploy em ambiente similar ao produção
- Configurar HTTPS adequadamente (Render vs self-hosted)

---

# ADRs Relacionadas

Consultar:

`knowledge/decisions.md`

---

# Documentação Relacionada

- `docs/fluxos/fluxo-configuracao.md` — Configuração de deploy
- `knowledge/deployment.md` — Deploy geral

---

# Documentação Oficial

`docs/frontend/`

---

# Fluxo Recomendado para Agentes de IA

1. Verificar `Program.cs` para configuração de porta
2. Verificar `Startup.cs` para detecção de ambiente
3. Verificar `Dockerfile` para containerização
4. Usar `dotnet publish` para build de produção
5. Configurar variáveis de ambiente para conexões e secrets

---

# Resumo

Deploy via `dotnet publish`, suporte a Docker, Render cloud e IIS. Render gerencia HTTPS no proxy (sem UseHttpsRedirection). Porta configurável via variável de ambiente `PORT`.

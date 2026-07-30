# 📋 Relatório de Configuração de Secrets — agilum.mvc.web

**Projeto:** `agilum.mvc.web.csproj`  
**UserSecretsId:** `aspnet-agilum.mvc.web-55113476-1654-4186-9786-f0f9ccf66b0c`  
**Data:** 13/07/2026

---

## ✅ Situação Atual — Ambiente Local

As secrets abaixo **já foram criadas** com valores placeholder.  
⚠️ **Substitua os valores placeholder** pelos dados reais do seu ambiente.

| # | Chave | Valor Placeholder |
| --- | --- | --- |
| 1 | `ConnectionStrings:ConnectionDb` | `Server=mysql.agiliumadm.com.br;Database=agiliumadm;Uid=agiliumadm_add1;Pwd=agl1234` |
| 2 | `ConnectionStrings:dbIdentityContextConnection` | `Server=localhost;Port=3306;Database=AgiliumIdentityDb;User=root;Password=SUA_SENHA;` |
| 3 | `EmailSettings:PrimaryDomain` | `smtp.gmail.com` |
| 4 | `EmailSettings:PrimaryPort` | `587` |
| 5 | `EmailSettings:UsernameEmail` | `seu-email@gmail.com` |
| 6 | `EmailSettings:UsernamePassword` | `SUA_SENHA_APP` |
| 7 | `EmailSettings:FromEmail` | `seu-email@gmail.com` |
| 8 | `EmailSettings:ToEmail` | `destino@email.com` |
| 9 | `EmailSettings:CcEmail` | `cc@email.com` |

---

## 🖥️ Parte 1 — Como Configurar no Ambiente Local (Windows)

### Pré-requisitos

- .NET SDK instalado (versão 3.1 ou superior)
- PowerShell ou CMD

### Passo a passo para criar/atualizar secrets

Abra o terminal na pasta do projeto:

```powershell
cd C:\AgiliumManagerMVC\agilum.mvc.web
```

Execute os comandos abaixo **substituindo os valores** pelos dados reais:

```powershell
# ─── Connection Strings ──────────────────────────────────────
dotnet user-secrets set "ConnectionStrings:ConnectionDb" `
  "Server=SEU_SERVIDOR;Port=3306;Database=SEU_BANCO;User=SEU_USUARIO;Password=SUA_SENHA;"

dotnet user-secrets set "ConnectionStrings:dbIdentityContextConnection" `
  "Server=SEU_SERVIDOR;Port=3306;Database=SEU_BANCO_IDENTITY;User=SEU_USUARIO;Password=SUA_SENHA;"

# ─── Email Settings ──────────────────────────────────────────
dotnet user-secrets set "EmailSettings:PrimaryDomain" "smtp.gmail.com"
dotnet user-secrets set "EmailSettings:PrimaryPort" "587"
dotnet user-secrets set "EmailSettings:UsernameEmail" "seu-email@gmail.com"
dotnet user-secrets set "EmailSettings:UsernamePassword" "sua-senha-app"
dotnet user-secrets set "EmailSettings:FromEmail" "seu-email@gmail.com"
dotnet user-secrets set "EmailSettings:ToEmail" "destino@email.com"
dotnet user-secrets set "EmailSettings:CcEmail" "cc@email.com"
```

### Para listar as secrets existentes

```powershell
dotnet user-secrets list
```

### Para remover uma secret

```powershell
dotnet user-secrets remove "Nome:DaChave"
```

### Para limpar TODAS as secrets

```powershell
dotnet user-secrets clear
```

### Local físico do arquivo secrets.json (backup/inspeção)

```text
%APPDATA%\Microsoft\UserSecrets\aspnet-agilum.mvc.web-55113476-1654-4186-9786-f0f9ccf66b0c\secrets.json
```

---

## ☁️ Parte 2 — Como Configurar no Render.com

No Render.com **não existe** o `dotnet user-secrets`.  
A configuração é feita via **Variáveis de Ambiente** no painel do serviço.

> 📌 O código do projeto (`Startup.cs` e `IdentityConfig.cs`) já está preparado para ler de variáveis de ambiente como fallback.

### Passo a passo no Render.com

1. Acesse o [Dashboard do Render](https://dashboard.render.com)
2. Selecione o **Web Service** do projeto `agilum.mvc.web`
3. No menu lateral, clique em **Environment**
4. Na seção **Environment Variables**, adicione as variáveis abaixo:

### Variáveis de ambiente necessárias

| Key | Value (exemplo) |
| --- | --- |
| `RENDER` | `true` |
| `PORT` | `5000` |
| `ASPNETCORE_ENVIRONMENT` | `Production` |
| `ConnectionStrings__ConnectionDb` | `Server=SEU_HOST_MYSQL;Port=3306;Database=AgiliumDb;User=USUARIO;Password=SENHA;` |
| `ConnectionStrings__dbIdentityContextConnection` | `Server=SEU_HOST_MYSQL;Port=3306;Database=AgiliumIdentityDb;User=USUARIO;Password=SENHA;` |
| `ConnectionStrings__versaobd-major` | `8` |
| `ConnectionStrings__versaobd-minor` | `0` |
| `ConnectionStrings__versaobd-build` | `19` |
| `EmailSettings__PrimaryDomain` | `smtp.gmail.com` |
| `EmailSettings__PrimaryPort` | `587` |
| `EmailSettings__UsernameEmail` | `seu-email@gmail.com` |
| `EmailSettings__UsernamePassword` | `sua-senha-app` |
| `EmailSettings__FromEmail` | `seu-email@gmail.com` |
| `EmailSettings__ToEmail` | `destino@email.com` |
| `EmailSettings__CcEmail` | `cc@email.com` |

> ⚠️ **Importante:** No Render.com, o separador de seção é `__` (dois underscores).  
> Ex: `ConnectionStrings:ConnectionDb` no local → `ConnectionStrings__ConnectionDb` no Render.

### Explicação do funcionamento

O método `ObterConnectionString` no `Startup.cs` tenta obter o valor nesta ordem:

```text
1. Configuration (appsettings.json + user-secrets)
2. Environment.GetEnvironmentVariable("NOME_DIRETO")
3. Environment.GetEnvironmentVariable("ConnectionStrings__NOME")
```

O IdentityConfig também tem o mesmo fallback.  
Isso significa que, mesmo sem `user-secrets` no Render, as variáveis de ambiente são lidas corretamente.

### Variável RENDER

Quando `RENDER=true` está definida, o projeto automaticamente:

- **Desabilita HSTS** (Render gerencia HTTPS no proxy)
- **Desabilita redirecionamento HTTPS** (evita loop de redirect)
- Usa a porta da variável `PORT`

---

## 📊 Resumo dos Formatos

| Ambiente | Método | Separador | Exemplo |
| --- | --- | --- | --- |
| **Local (dev)** | `dotnet user-secrets` | `:` (dois pontos) | `ConnectionStrings:ConnectionDb` |
| **Render.com** | Environment Variables | `__` (duplo underscore) | `ConnectionStrings__ConnectionDb` |

---

## 🔒 Observações de Segurança

- ❌ **Nunca** faça commit de secrets no repositório Git
- ✅ O `secrets.json` fica fora da pasta do projeto (no perfil do usuário)
- ✅ No Render.com, as variáveis de ambiente são criptografadas em repouso
- ✅ Para Gmail, use uma [senha de app](https://support.google.com/accounts/answer/185833) em vez da senha da conta

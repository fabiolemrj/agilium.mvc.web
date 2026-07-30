# Configuração

## Objetivo

Documentar os fluxos de configuração do Agilium Manager: seleção de empresa, e-mail, PDV, licenciamento, integrações e parâmetros do sistema.

---

## Visão Geral

As configurações usam padrão CHAVE/VALOR por empresa no banco de dados, acessadas via `IUtilDapperRepository.ConfigRetornaValor()`. A seleção de empresa é forçada pós-login via `EmpresaSelecionadaMiddleware`.

---

## Fluxo Principal

### Seleção de Empresa

```
Pós-Login → /Empresa/SelecionarEmpresa
      │
      ▼
EmpresaController.ObterListasEmpresasPorUsuario()
      │
      ▼
POST: armazena EmpresaUsuarioViewModel na Session
      │
      ▼
EmpresaSelecionadaMiddleware → Toda requisição exige empresa
```

### Configuração de E-mail

```
ConfigController → CHAVE/VALOR
      │
      ├── MAIL_EMAIL, MAIL_SMTP, MAIL_POP
      ├── MAIL_PORTA_SMTP, MAIL_PORTA_POP
      ├── MAIL_REMETENTE, MAIL_SENHA
      │
      ▼
ServiceEmail.ObterConfigEmail(idEmpresa)
      │
      └── EmailSettings → SendEmailAsync()
```

### Licenciamento

```
HomeController.Licenca()
      │
      ▼
LicencaService.ObterPorIdEmpresa(idEmpresa)
      ├── Licença existe? Válida? Chaves OK?
      └── Não → Bloquear acesso
```

### Deploy Render

```
Program.cs: PORT env var → UseUrls()
Startup.cs: RENDER env var → sem HttpsRedirection/Hsts
```

---

## Regras de Negócio

Consultar:

`docs/business-rules/`

---

## Módulos Envolvidos

- `knowledge/business/empresas.md`
- `knowledge/business/configuracoes.md`
- `knowledge/business/licenciamento.md`
- `knowledge/business/integracoes.md`

---

## APIs Relacionadas

- `agilum.mvc.web/Controllers/ConfigController.cs`
- `agilum.mvc.web/Controllers/EmpresaController.cs`
- `agilum.mvc.web/Controllers/HomeController.cs` — `Licenca()`

---

## Banco de Dados

- Tabela de configurações CHAVE/VALOR por empresa
- `appsettings.json` — versões do sistema, conexões

---

## ADRs Relacionadas

Consultar:

`knowledge/decisions.md`

---

## Documentação Relacionada

- `docs/fluxos/fluxo-configuracao.md` — Documentação oficial detalhada

---

## Documentação Oficial

`docs/fluxos/fluxo-configuracao.md`

---

## Fluxo Recomendado para Agentes de IA

1. Verificar `EmpresaSelecionadaMiddleware` para fluxo de seleção
2. Verificar `IUtilDapperRepository.ConfigRetornaValor()` para consulta de config
3. Verificar `ConfigController` para interface MVC
4. Verificar `LicencaService` para validação de licença
5. Verificar `Program.cs` e `Startup.cs` para deploy Render

---

## Resumo

Configurações CHAVE/VALOR por empresa no banco. Seleção de empresa pós-login. E-mail via ServiceEmail. Licenciamento com chaves de ativação. Suporte a deploy Render cloud.

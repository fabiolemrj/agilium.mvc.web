# Configurações

## Objetivo

Módulo responsável pelas configurações do sistema: parâmetros por empresa, e-mail, PDV, integrações e constantes do sistema.

---

# Visão Geral

As configurações do Agilium Manager são gerenciadas via padrão CHAVE/VALOR no banco de dados, acessadas por `IConfigService` e `IUtilDapperRepository.ConfigRetornaValor()`. Configurações são escopadas por empresa. Exemplos: `CONTA_REALIZARCONTROLE`, `VENDAS_DOC_FISCAL_PADRAO`, `PDV_PREVENDA`, `MAIL_EMAIL`.

---

# Responsabilidades

- Gerenciamento de configurações chave/valor por empresa
- Configuração de e-mail (SMTP, POP, remetente, senha)
- Configuração do PDV (impressoras, vias, mensagens)
- Configurações fiscais (regime tributário, IE, CNAE)
- Configurações financeiras (conta bancária, taxas, juros)
- Configurações de integração (cardápio digital, marketplace)
- Versionamento do sistema e banco de dados

---

# Principais Entidades

- `ConfigChaveValor` — Chave/valor por empresa (via `IConfigService`)
- `EmailSettings` — Configurações de e-mail (PrimaryDomain, Port, Username, Password)

---

# Fluxos Relacionados

- `docs/fluxos/fluxo-configuracao.md` — Fluxos de configuração
- `docs/fluxos/fluxo-autenticacao.md` — Seleção de empresa

---

# APIs Relacionadas

- `agilum.mvc.web/Controllers/ConfigController.cs`
- `agilium-manager-azure-api/V1/ConfigController.cs`

---

# Regras de Negócio

Consultar:

`docs/business-rules/`

---

# Banco de Dados

Consultar:

`docs/database/`

---

# ADRs Relacionadas

Consultar:

`knowledge/decisions.md`

---

# Documentação Relacionada

- `docs/padroes/dapper.md` — `IUtilDapperRepository.ConfigRetornaValor()`
- `docs/padroes/services.md` — `ConfigService`
- `knowledge/business/empresas.md` — Escopo por empresa

---

# Documentação Oficial

`docs/business/configuracoes/`

---

# Fluxo Recomendado para Agentes de IA

1. Verificar `IConfigService` para interface
2. Verificar `IUtilDapperRepository.ConfigRetornaValor(chave, idEmpresa)` para consulta
3. Verificar `ConfigController.cs` para interface MVC
4. Verificar `EmailSettings` e `ServiceEmail` para e-mail
5. Verificar `appsettings.json` para versões do sistema

---

# Resumo

Configurações usam padrão CHAVE/VALOR por empresa no banco de dados. Acesso via `ConfigRetornaValor()`. Cobre e-mail, PDV, fiscal, financeiro e integrações.

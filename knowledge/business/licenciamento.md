# Licenciamento

## Objetivo

Módulo responsável pela validação e controle de licenças de uso do Agilium Manager por empresa.

---

# Visão Geral

O Agilium Manager utiliza um sistema de licenciamento para controlar o acesso por empresa. A licença é validada no `HomeController.Licenca()` e verifica existência, validade e chaves de ativação. O serviço `LicencaService` é injetado em todos os controllers via `MainController`.

---

# Responsabilidades

- Validação de licença por empresa
- Verificação de expiração
- Validação de chaves de ativação (K1...K7)
- Bloqueio de acesso quando licença inválida
- Renovação de licença

---

# Principais Entidades

- `Licenca` — Dados da licença (empresa, validade, chaves)

---

# Fluxos Relacionados

- `docs/fluxos/fluxo-configuracao.md` — Fluxo de licenciamento

---

# APIs Relacionadas

- `agilum.mvc.web/Controllers/HomeController.cs` — `Licenca()` action
- `agilum.mvc.web/Controllers/LicencaController.cs`

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

- `docs/fluxos/fluxo-configuracao.md` — Verificação de licença
- `docs/padroes/services.md` — `LicencaService`

---

# Documentação Oficial

`docs/business/licenciamento/`

---

# Fluxo Recomendado para Agentes de IA

1. Verificar `ILicencaService` para interface
2. Verificar `LicencaService` para lógica de validação
3. Verificar `HomeController.Licenca()` para ponto de entrada
4. Verificar `MainController` — `ILicencaService` injetado na base

---

# Resumo

O licenciamento controla o acesso por empresa via validação de licença, data de expiração e chaves de ativação. É verificado no acesso ao sistema e injetado em todos os controllers.

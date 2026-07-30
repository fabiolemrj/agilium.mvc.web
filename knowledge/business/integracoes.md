# Integrações

## Objetivo

Módulo responsável pelas integrações externas do Agilium Manager: cardápio digital, marketplace (site mercado), APIs externas e serviços de terceiros.

---

# Visão Geral

O Agilium Manager possui integrações com sistemas externos para expandir suas funcionalidades. A principal integração é com o **Cardápio Digital** (API externa para cardápio online). Também há integração com **marketplace** (Site Mercado) e suporte a deploy em cloud (Render).

---

# Responsabilidades

- Integração com Cardápio Digital (sincronização de produtos, preços e fotos)
- Integração com marketplace (Site Mercado)
- Configuração de conexões externas (connection strings, API base URLs)
- Sincronização bidirecional de dados
- Deploy em cloud (Render)

---

# Principais Entidades/Serviços

- `IntegracaoCardapioService` — Sincronização com cardápio digital
- `ProdutoSiteMercado` — Produtos no marketplace
- `PedidoSitemercado` — Pedidos originados do marketplace
- `MoedaSiteMercado` — Cotações do marketplace
- `CardapioDigital` — Configuração de conexão (ConnectionString, ApiBaseUrl)

---

# Fluxos Relacionados

- `docs/fluxos/fluxo-configuracao.md` — Configuração de integrações
- `docs/fluxos/fluxo-produto.md` — Sincronização de produto com cardápio

---

# APIs Relacionadas

- `agilium-manager-azure-api/V1/` — APIs REST

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

- `docs/padroes/services.md` — `IntegracaoCardapioService`
- `knowledge/business/produtos.md` — Origem da sincronização
- `knowledge/business/configuracoes.md` — Configuração de integrações
- `docs/fluxos/fluxo-configuracao.md` — Deploy Render

---

# Documentação Oficial

`docs/business/integracoes/`

---

# Fluxo Recomendado para Agentes de IA

1. Verificar `IIntegracaoCardapioService` para interface de cardápio
2. Verificar `IntegracaoCardapioService` para implementação (HTTP client)
3. Verificar `appsettings.json` — seção `CardapioDigital`
4. Verificar `ProdutoSiteMercado` e `PedidoSitemercado` models
5. Verificar `Program.cs` e `Startup.cs` para configuração Render

---

# Resumo

As principais integrações são Cardápio Digital (sincronização de produto/preço/foto via API HTTP) e marketplace. A configuração de conexão é feita via `appsettings.json`.

# Compras

## Objetivo

Módulo responsável pela gestão de compras, desde a criação do pedido até a efetivação com entrada no estoque, incluindo importação de XML NF-e e cadastro automático de produtos.

---

# Visão Geral

O módulo de Compras gerencia o ciclo de vida de uma compra: Aberta → Efetivada (ou Cancelada). A efetivação é atômica e gera entrada no estoque + lançamentos contábeis. A principal fonte de dados é a importação de XML de NF-e. Itens sem produto associado podem ter cadastro automático.

---

# Responsabilidades

- Criação e edição de compras (CRUD)
- Importação de XML NF-e (desserialização e preenchimento automático)
- Cadastro automático de produtos a partir de itens de compra
- Efetivação atômica (estoque + contábil)
- Cancelamento com reversão de estoque e contábil
- Geração de código sequencial (GerarCodigoCompra)

---

# Principais Entidades

- `Compra` — Registro principal (CDCOMPRA, STCOMPRA, dados fiscais)
- `CompraItem` — Itens da compra (produto, quantidade, valores, dados fiscais)
- `CompraFiscal` — XML da NF-e e status de manifesto

---

# Fluxos Relacionados

- `docs/fluxos/fluxo-compra.md` — Fluxo completo
- `docs/fluxos/fluxo-estoque.md` — Entrada por compra

---

# APIs Relacionadas

- `agilum.mvc.web/Controllers/CompraController.cs` — Endpoints: `/compra/novo`, `/compra/importar`, `/compra/efetivar`, etc.
- `agilium-manager-azure-api/V1/CompraController.cs`

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

- `docs/padroes/dapper.md` — Dapper para operações de compra
- `docs/padroes/services.md` — `CompraService`
- `knowledge/business/estoque.md` — Destino da entrada

---

# Documentação Oficial

`docs/business/compras/`

---

# Fluxo Recomendado para Agentes de IA

1. Verificar `CompraController.cs` para endpoints MVC
2. Verificar `CompraService.EfetivarCompra()` para lógica de efetivação
3. Verificar `CompraService.ImportarCompraDeXmlNfe()` para importação NF-e
4. Verificar `CompraService.RealizarCadastroProdutoAutomatico()` para cadastro automático
5. Verificar `CompraService.CancelarCompra()` para cancelamento com reversão
6. Consultar `docs/fluxos/fluxo-compra.md` para fluxo completo

---

# Resumo

Compras seguem o ciclo Aberta → Efetivada/Cancelada. A importação de XML NF-e é o principal meio de entrada de dados. A efetivação é atômica: atualiza estoque, custo médio, preço de venda e gera lançamentos contábeis.

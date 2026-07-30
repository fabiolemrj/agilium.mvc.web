# Módulos de Negócio

## Objetivo

Resumo dos módulos de negócio do Agilium Manager, suas responsabilidades e integrações dentro do sistema.

---

# Visão Geral

O Agilium Manager é um sistema ERP voltado ao mercado brasileiro, organizado em módulos de negócio independentes que cobrem: gestão de clientes, estoque, compras, vendas (PDV), financeiro, caixa, fiscal, licenciamento e configurações.

Cada módulo é documentado em um arquivo dedicado dentro desta pasta.

---

# Responsabilidades

- Servir como índice para os módulos de negócio
- Facilitar a navegação de agentes de IA entre módulos
- Vincular cada módulo às regras de negócio, ADRs e documentação relacionada

---

# Módulos

| Módulo | Arquivo | Descrição |
|--------|---------|-----------|
| Clientes | `clientes.md` | Cadastro de clientes, limites de crédito, histórico |
| Empresas | `empresas.md` | Multi-empresa, seleção, configurações por empresa |
| Usuários | `usuarios.md` | Cadastro, perfis, permissões, autenticação |
| Produtos | `produtos.md` | Catálogo, classificação fiscal, preços, códigos de barras |
| Categorias | `categorias.md` | Grupos, subgrupos, marcas, departamentos |
| Estoque | `estoque.md` | Movimentações, inventário, saldo, rastreabilidade |
| Compras | `compras.md` | Pedidos de compra, importação NF-e, efetivação |
| Fornecedores | `fornecedores.md` | Cadastro, histórico de compras |
| Pedidos | `pedidos.md` | Pedidos de venda, conversão para venda |
| Vendas | `vendas.md` | Vendas PDV, pré-venda, cancelamento, NFC-e |
| Caixa | `caixa.md` | Abertura, fechamento, sangria, suprimento |
| Financeiro | `financeiro.md` | Contas a pagar/receber, plano de contas, consolidação |
| Pagamentos | `pagamentos.md` | Formas de pagamento, moedas, vales |
| Fiscal | `fiscal.md` | CFOP, NCM, CEST, CST, IBPT, emissão de notas |
| Licenciamento | `licenciamento.md` | Ativação, chaves, validação de licença |
| Configurações | `configuracoes.md` | Parâmetros do sistema, e-mail, PDV, integrações |
| Relatórios | `relatorios.md` | Relatórios gerenciais, vendas, financeiro |
| Integrações | `integracoes.md` | Cardápio digital, marketplace, APIs externas |

---

# Fluxos Relacionados

- `docs/fluxos/` — Fluxos detalhados de cada módulo

---

# APIs Relacionadas

- `agilium-manager-azure-api/` — API REST (v1)
- `agilium-pdv-azure-api/` — API do PDV

---

# Regras de Negócio

Consultar: `docs/business-rules/`

---

# Banco de Dados

Consultar: `docs/database/`

---

# ADRs Relacionadas

Consultar: `knowledge/decisions.md`

---

# Documentação Relacionada

- `knowledge/architecture.md` — Arquitetura do sistema
- `knowledge/domain.md` — Modelo de domínio
- `knowledge/fluxos.md` — Fluxos documentados
- `knowledge/glossary.md` — Glossário de termos

---

# Fluxo Recomendado para Agentes de IA

1. Ler este README para visão geral
2. Acessar o arquivo do módulo desejado
3. Consultar as regras de negócio em `docs/business-rules/`
4. Consultar ADRs em `knowledge/decisions.md`
5. Verificar fluxos em `docs/fluxos/`
6. Verificar banco de dados em `docs/database/`

---

# Resumo

Os 18 módulos de negócio do Agilium Manager cobrem toda a operação de um ERP. Cada módulo é independente mas integrado aos demais via serviços compartilhados e regras de negócio cross-cutting.

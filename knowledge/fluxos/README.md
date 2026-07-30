# Fluxos

## Objetivo

Índice dos fluxos de negócio documentados do Agilium Manager. Cada fluxo é um domínio funcional com regras, pré-condições, exceções e integrações entre módulos.

---

## Fluxos Disponíveis

| Fluxo | Arquivo | Descrição |
|-------|---------|-----------|
| Autenticação | `autenticacao.md` | Login, logout, claims, permissões, lockout |
| Caixa | `caixa.md` | Abertura, fechamento, sangria, suprimento |
| Compra | `compra.md` | Criação, importação NF-e, efetivação, cancelamento |
| Configuração | `configuracao.md` | Empresa, e-mail, PDV, licenciamento, integrações |
| Estoque | `estoque.md` | Entrada, saída, inventário, ajustes, custo médio |
| Financeiro | `financeiro.md` | Contas a pagar/receber, plano de contas, lançamentos |
| Produto | `produto.md` | Cadastro, classificação fiscal, preços, composições |
| Venda | `venda.md` | Realização, pré-venda, cancelamento, NFC-e |
| Troubleshooting | `troubleshooting.md` | Problemas comuns e soluções |

---

## Como Navegar

```
Fluxo (knowledge/fluxos/)
      ↓
Regras de Negócio (docs/business-rules/)
      ↓
Arquitetura (knowledge/architecture.md)
      ↓
APIs (knowledge/api.md)
      ↓
Banco de Dados (docs/database/)
      ↓
Implementação (docs/fluxos/ — documentação oficial detalhada)
```

---

## Documentação Relacionada

- `docs/fluxos/` — Documentação oficial detalhada de cada fluxo
- `docs/business-rules/` — Regras de negócio
- `knowledge/business/` — Módulos de negócio
- `knowledge/architecture.md` — Arquitetura do sistema

---

## Fluxo Recomendado para Agentes de IA

1. Ler este README para identificar o fluxo desejado
2. Acessar o arquivo do fluxo em `knowledge/fluxos/`
3. Consultar regras de negócio em `docs/business-rules/`
4. Verificar a documentação oficial detalhada em `docs/fluxos/`
5. Verificar ADRs em `knowledge/decisions.md`
6. Verificar banco de dados em `docs/database/`

---

## Resumo

Os fluxos documentam os principais processos de negócio do Agilium Manager. Cada fluxo conecta regras de negócio, arquitetura, APIs e banco de dados em um documento navegável por agentes de IA.

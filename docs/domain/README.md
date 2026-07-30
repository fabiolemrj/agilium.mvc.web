# Domínios do Agilium Manager

## Objetivo

Documentar todos os domínios de negócio do sistema **Agilium Manager**, descrevendo a responsabilidade, entidades, fluxos, regras de negócio e integrações de cada módulo.

---

## Índice de Domínios

| # | Módulo | Arquivo | Descrição |
|---|--------|---------|-----------|
| 1 | Empresas | [empresa.md](./empresa.md) | Organização central, raiz do negócio |
| 2 | Usuários | [usuario.md](./usuario.md) | Contas de acesso, autenticação, perfis e permissões |
| 3 | Produtos | [produto.md](./produto.md) | Itens comercializados, classificação fiscal e preços |
| 4 | Clientes | [cliente.md](./cliente.md) | Clientes PF/PJ, contatos e preços diferenciados |
| 5 | Fornecedores | [fornecedor.md](./fornecedor.md) | Fornecedores de produtos e serviços |
| 6 | Funcionários | [funcionario.md](./funcionario.md) | Vendedores e operadores do sistema |
| 7 | Vendas | [venda.md](./venda.md) | Transações do PDV, itens, pagamentos e dados fiscais |
| 8 | Compras | [compra.md](./compra.md) | Aquisição de produtos, importação de NFe |
| 9 | Estoque | [estoque.md](./estoque.md) | Controle de locais, saldos e movimentações |
| 10 | Caixa | [caixa.md](./caixa.md) | Abertura, fechamento e movimentações do caixa |
| 11 | Turno | [turno.md](./turno.md) | Período operacional do PDV |
| 12 | Financeiro | [financeiro.md](./financeiro.md) | Contas a pagar/receber, plano de contas, moedas |
| 13 | Fiscal | [fiscal.md](./fiscal.md) | Tabelas tributárias (CFOP, CST, NCM, CEST, IBPT) |
| 14 | Pedidos | [pedido.md](./pedido.md) | Pedidos de venda e integração com marketplace |
| 15 | Devoluções | [devolucao.md](./devolucao.md) | Devolução de produtos e reintegração ao estoque |
| 16 | Inventário | [inventario.md](./inventario.md) | Contagem física e ajuste de estoque |
| 17 | Perdas | [perda.md](./perda.md) | Baixas por quebra, validade ou furto |
| 18 | Vales | [vale.md](./vale.md) | Emissão e consumo de vales-crédito |
| 19 | PDV | [ponto-venda.md](./ponto-venda.md) | Terminais de ponto de venda |
| 20 | Formas de Pagamento | [forma-pagamento.md](./forma-pagamento.md) | Meios de pagamento aceitos |
| 21 | Unidades de Medida | [unidade.md](./unidade.md) | Unidades para quantificação de produtos |
| 22 | Licenças | [licenca.md](./licenca.md) | Controle de licenciamento do software |
| 23 | Configurações | [config.md](./config.md) | Parâmetros globais e por empresa |
| 24 | Logs | [log.md](./log.md) | Registro de eventos e erros do sistema |

---

## Estrutura de Cada Documento

Cada domínio segue o padrão:

1. **Objetivo** — o que o módulo representa
2. **Responsabilidades** — lista do que o módulo faz
3. **Fluxo Geral** — fluxograma textual das operações
4. **Principais Entidades** — tabela com entidades do domínio
5. **Dependências** — outros módulos dos quais depende
6. **Regras de Negócio** — validações e restrições
7. **Serviços Envolvidos** — classes de serviço
8. **Controllers Relacionados** — controllers MVC/API
9. **Checklist** — itens a verificar antes de alterações

---

## Mapa de Dependências

```
Empresa ←── Usuário, Funcionário, Config, Licença, PDV, Unidade
    │
    ├── Produto ←── Fiscal (NCM, CEST, CFOP, CST)
    │      ├── Estoque
    │      ├── Compra ←── Fornecedor
    │      └── Pedido ←── Cliente
    │
    ├── Turno
    │     └── Caixa ←── Moeda
    │           └── Venda ←── FormaPagamento, Vale
    │                 ├── Financeiro (ContaReceber)
    │                 └── Estoque (baixa)
    │
    └── Financeiro (ContaPagar, PlanoConta, CategoriaFinanceira)
```

---

## Convenções

- Nomes em **português** refletindo os termos de negócio
- Serviços seguem o padrão `{Entidade}Service` em `agilium-manager-azure-business/Services/`
- Controllers seguem o padrão `{Entidade}Controller` em `agilum.mvc.web/Controllers/`
- Toda regra de negócio deve estar na camada Business, nunca em Controllers

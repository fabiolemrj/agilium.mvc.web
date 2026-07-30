# Módulo Vendas

## Objetivo

O módulo **Vendas** representa o processo de comercialização de produtos/serviços no PDV (Ponto de Venda). Registra todas as transações de venda, incluindo itens, formas de pagamento, moedas, dados fiscais, cancelamentos e espelhos de venda.

---

# Responsabilidades

- Registro de vendas
- Registro de itens da venda
- Registro de pagamentos
- Registro de moedas utilizadas
- Emissão de dados fiscais da venda
- Cancelamento de vendas
- Espelhamento de venda (histórico)
- Vendas temporárias (pré-venda)
- Integração com Caixa e Turno

---

# Fluxo Geral

```
Abrir Turno

↓

Abrir Caixa

↓

Iniciar Venda

↓

Adicionar Itens

↓

Selecionar Forma de Pagamento

↓

Finalizar Venda

↓

Registrar Dados Fiscais

↓

Movimentar Estoque

↓

Fechar Venda
```

---

# Tipos de Venda

| Tipo | Descrição |
|------|-----------|
| Venda | Venda normal finalizada |
| VendaTemporaria | Venda em aberto (pré-venda) |
| VendaCancelada | Venda cancelada |
| VendaEspelho | Cópia de segurança da venda |

---

# Dependências

- Empresa
- Turno
- Caixa
- Produto
- Cliente
- Estoque
- FormaPagamento
- Moeda
- Funcionario (vendedor)

---

# Principais Entidades

| Entidade | Descrição |
|----------|-----------|
| Venda | Registro principal da venda |
| VendaItem | Itens vendidos |
| VendaMoeda | Moedas utilizadas no pagamento |
| VendaFiscal | Dados fiscais da venda |
| VendaCancelada | Registro de cancelamento |
| VendaEspelho | Cópia espelho da venda |
| VendaTemporaria | Venda em aberto |
| VendaTemporariaItem | Itens da venda temporária |
| VendaTemporariaMoeda | Moedas da venda temporária |
| VendaTemporariaEspelho | Espelho da venda temporária |
| PedidoVenda | Pedido convertido em venda |
| PedidoVendaItem | Itens do pedido convertido |

---

# Regras de Negócio

## Abertura

- Venda só pode ser iniciada com Turno aberto
- Venda só pode ser iniciada com Caixa aberto
- Usuário deve ter permissão para realizar vendas

## Itens

- Quantidade deve ser maior que zero
- Produto deve estar ativo
- Estoque deve ter saldo suficiente (quando aplicável)
- Preço do item deve ser válido

## Pagamento

- Valor pago deve ser >= valor total
- Troco calculado automaticamente
- Múltiplas formas de pagamento por venda
- Múltiplas moedas por venda

## Cancelamento

- Venda cancelada gera estorno no estoque
- Venda cancelada gera registro fiscal de cancelamento
- Histórico é preservado no espelho

---

# Situações da Venda

| Situação | Descrição |
|----------|-----------|
| Aberta | Venda em andamento |
| Finalizada | Venda concluída |
| Cancelada | Venda cancelada |
| Pendente | Aguardando confirmação |

---

# Serviços Envolvidos

- VendaService
- EstoqueService
- CaixaService
- TurnoService
- ProdutoService
- FormaPagamentoService

---

# Controllers Relacionados

- VendaController (`agilum.mvc.web/Controllers/VendaController.cs`)

---

# Integração com Outros Módulos

```
Turno → Caixa → Venda → Estoque (baixa)
                    ↓
                Financeiro (receita)
                    ↓
                Fiscal (documentos)
```

---

# Boas Práticas

- Sempre validar Turno e Caixa abertos antes de iniciar venda
- Registrar espelho de toda venda finalizada
- Manter histórico de cancelamentos
- Não alterar vendas já finalizadas (cancelar e refazer)

---

# Checklist

☐ Turno aberto

☐ Caixa aberto

☐ Produtos ativos e com estoque

☐ Preços válidos

☐ Pagamento conferido

☐ Troco calculado

☐ Dados fiscais registrados

☐ Estoque atualizado

☐ Espelho gerado

---

# Conclusão

O módulo **Vendas** é o coração operacional do PDV. Toda venda impacta Estoque, Financeiro e Fiscal simultaneamente, exigindo integridade transacional em todas as operações.

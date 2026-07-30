# Módulo Estoque

## Objetivo

O módulo **Estoque** controla a localização, quantidade e movimentações dos produtos, garantindo a rastreabilidade das entradas (compras), saídas (vendas), devoluções, perdas e inventários.

---

# Responsabilidades

- Cadastro de locais de estoque
- Vínculo de produtos aos estoques (EstoqueProduto)
- Registro de movimentações (EstoqueHistorico)
- Atualização de saldo em tempo real
- Integração com Vendas (baixa), Compras (entrada), Devoluções, Perdas

---

# Fluxo de Movimentação

```
Compra Efetivada → Entrada no Estoque
Venda Finalizada → Saída do Estoque
Devolução        → Entrada no Estoque
Perda            → Saída do Estoque
Inventário       → Ajuste de Saldo
```

---

# Principais Entidades

| Entidade | Descrição |
|----------|-----------|
| Estoque | Local/cadastro de estoque |
| EstoqueProduto | Vínculo produto x estoque (saldo) |
| EstoqueHistorico | Histórico de movimentações |

---

# Dependências

- Empresa
- Produto
- Venda
- Compra
- Devolucao
- Perda
- Inventario

---

# Regras de Negócio

- Saldo não pode ficar negativo (salvo configuração específica)
- Toda movimentação gera histórico
- Estoque é por empresa

---

# Serviços Envolvidos

- EstoqueService

---

# Controllers Relacionados

- EstoqueController (`agilum.mvc.web/Controllers/EstoqueController.cs`)

---

# Checklist

☐ Estoque vinculado à empresa

☐ Produto vinculado ao estoque

☐ Saldo inicial definido

☐ Histórico de movimentações ativo

---

# Conclusão

O módulo **Estoque** garante a integridade dos saldos de produtos. Toda operação de entrada ou saída deve gerar o respectivo registro histórico para rastreabilidade.

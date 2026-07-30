# Módulo Caixa

## Objetivo

O módulo **Caixa** gerencia a abertura, fechamento e movimentações financeiras do caixa no PDV, incluindo sangrias, suprimentos e controle de moedas.

---

# Responsabilidades

- Abertura de caixa
- Fechamento de caixa
- Registro de movimentações (CaixaMovimento)
- Controle de moedas no caixa (CaixaMoeda)
- Sangria (retirada de valores)
- Suprimento (entrada de valores)
- Conferência de saldo
- Vínculo com Turno

---

# Fluxo Geral

```
Abrir Turno

↓

Abrir Caixa (informar saldo inicial)

↓

Operar (Vendas, Sangrias, Suprimentos)

↓

Fechar Caixa (conferir saldo)

↓

Fechar Turno
```

---

# Situações do Caixa

| Situação | Descrição |
|----------|-----------|
| Aberto | Caixa em operação |
| Fechado | Caixa encerrado |
| Em conferência | Fechamento em andamento |

---

# Principais Entidades

| Entidade | Descrição |
|----------|-----------|
| Caixa | Registro principal de abertura/fechamento |
| CaixaMovimento | Movimentações (venda, sangria, suprimento) |
| CaixaMoeda | Saldo por tipo de moeda |

---

# Dependências

- Empresa
- Turno
- Venda
- Moeda

---

# Regras de Negócio

- Caixa só pode ser aberto com Turno aberto
- Um Turno pode ter múltiplos Caixas
- Fechamento exige conferência de saldo
- Vendas só podem ocorrer com Caixa aberto
- Toda movimentação é registrada com data/hora

---

# Serviços Envolvidos

- CaixaService

---

# Controllers Relacionados

- CaixaController (`agilum.mvc.web/Controllers/CaixaController.cs`)

---

# Checklist

☐ Turno aberto antes do caixa

☐ Saldo inicial informado

☐ Moedas configuradas

☐ Movimentações registradas

☐ Conferência realizada no fechamento

---

# Conclusão

O módulo **Caixa** é o controle financeiro imediato do PDV. A integridade entre saldo declarado, movimentações e fechamento é essencial para a auditoria financeira da operação.

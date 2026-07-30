# Módulo Turno

## Objetivo

O módulo **Turno** representa o período operacional do PDV. Cada turno agrupa um conjunto de operações (vendas, aberturas de caixa) realizadas em um determinado período por um operador.

---

# Responsabilidades

- Abertura de turno
- Fechamento de turno
- Vínculo com empresa
- Vínculo com caixas
- Controle de preços por turno (TurnoPreco)

---

# Fluxo Geral

```
Abrir Turno

↓

Operar (Abrir Caixa → Vendas → Fechar Caixa)

↓

Fechar Turno
```

---

# Principais Entidades

| Entidade | Descrição |
|----------|-----------|
| Turno | Registro do turno |
| TurnoPreco | Preços diferenciados por turno |

---

# Dependências

- Empresa
- Caixa
- Produto (TurnoPreco)

---

# Regras de Negócio

- Apenas um turno ativo por empresa
- Turno deve ser fechado para abrir novo
- Fechamento do turno exige que todos os caixas estejam fechados

---

# Serviços Envolvidos

- TurnoService

---

# Controllers Relacionados

- TurnoController (`agilum.mvc.web/Controllers/TurnoController.cs`)

---

# Checklist

☐ Empresa selecionada

☐ Turno anterior fechado

☐ Data/hora de abertura registrada

☐ Caixas fechados antes do fechamento do turno

---

# Conclusão

O módulo **Turno** organiza a operação do PDV em períodos, facilitando a conferência e auditoria das movimentações realizadas em cada plantão.

# Módulo Inventário

## Objetivo

O módulo **Inventário** gerencia o processo de contagem física de produtos no estoque, permitindo ajustes de saldo entre o estoque físico e o estoque registrado no sistema.

---

# Responsabilidades

- Abertura de inventário
- Registro de itens contados (InventarioItem)
- Comparação entre quantidade registrada x contada
- Ajuste de saldo no estoque
- Fechamento de inventário

---

# Principais Entidades

| Entidade | Descrição |
|----------|-----------|
| Inventario | Registro principal |
| InventarioItem | Itens do inventário |

---

# Dependências

- Empresa
- Estoque
- Produto

---

# Situações

| Situação | Descrição |
|----------|-----------|
| Aberto | Inventário em contagem |
| Finalizado | Contagem concluída, ajustes aplicados |
| Cancelado | Inventário cancelado |

---

# Serviços Envolvidos

- InventarioService
- EstoqueService
- ProdutoService

---

# Controllers Relacionados

- InventarioController

---

# Checklist

☐ Estoque selecionado

☐ Itens contados

☐ Divergências analisadas

☐ Ajustes aplicados no estoque

---

# Conclusão

O módulo **Inventário** é essencial para a acuracidade do estoque, permitindo corrigir divergências entre o saldo físico e o sistêmico de forma controlada.

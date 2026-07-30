# Módulo Devoluções

## Objetivo

O módulo **Devoluções** gerencia o processo de devolução de produtos por clientes, incluindo controle de itens devolvidos, motivos e reintegração ao estoque.

---

# Responsabilidades

- Registro de devoluções
- Registro de itens devolvidos
- Controle de motivos de devolução (MotivoDevolucao)
- Reintegração ao estoque
- Controle de situação

---

# Principais Entidades

| Entidade | Descrição |
|----------|-----------|
| Devolucao | Registro principal |
| DevolucaoItem | Itens devolvidos |
| MotivoDevolucao | Catálogo de motivos |

---

# Dependências

- Empresa
- Venda (origem)
- Cliente
- Produto
- Estoque

---

# Situações

| Situação | Descrição |
|----------|-----------|
| Aberta | Devolução em andamento |
| Finalizada | Devolução concluída, estoque atualizado |
| Cancelada | Devolução cancelada |

---

# Serviços Envolvidos

- DevolucaoService
- EstoqueService
- VendaService

---

# Controllers Relacionados

- DevolucaoController

---

# Checklist

☐ Venda de origem identificada

☐ Itens conferidos

☐ Motivo registrado

☐ Estoque atualizado

---

# Conclusão

O módulo **Devoluções** fecha o ciclo da venda, permitindo reverter operações e reintegrar produtos ao estoque de forma rastreável.

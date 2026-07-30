# Módulo Perdas

## Objetivo

O módulo **Perdas** registra baixas de produtos por perda, quebra, validade vencida ou outros motivos operacionais, atualizando o saldo de estoque.

---

# Responsabilidades

- Registro de perdas
- Registro de itens da perda
- Baixa no estoque
- Classificação por tipo de perda

---

# Principais Entidades

| Entidade | Descrição |
|----------|-----------|
| Perda | Registro principal |

---

# Dependências

- Empresa
- Produto
- Estoque

---

# Tipos de Perda

| Tipo | Descrição |
|------|-----------|
| Quebra | Danos físicos |
| Validade | Produto vencido |
| Furto | Desaparecimento |
| Administrativa | Uso interno/consumo |
| Outros | Demais motivos |

---

# Serviços Envolvidos

- PerdaService
- EstoqueService

---

# Controllers Relacionados

- PerdaController

---

# Checklist

☐ Produto identificado

☐ Quantidade informada

☐ Tipo de perda classificado

☐ Estoque atualizado

---

# Conclusão

O módulo **Perdas** garante que as saídas de estoque não relacionadas a vendas sejam adequadamente registradas e classificadas.

# Módulo Vales

## Objetivo

O módulo **Vales** gerencia a emissão e o consumo de vales (créditos) que podem ser utilizados como forma de pagamento em vendas.

---

# Responsabilidades

- Emissão de vales
- Consumo de vales em vendas
- Controle de saldo do vale
- Controle de situação

---

# Principais Entidades

| Entidade | Descrição |
|----------|-----------|
| Vale | Registro do vale |

---

# Dependências

- Empresa
- Cliente (opcional)
- Venda (consumo)

---

# Situações

| Situação | Descrição |
|----------|-----------|
| Disponível | Vale ativo com saldo |
| Consumido | Vale totalmente utilizado |
| Cancelado | Vale cancelado |
| Vencido | Fora da validade |

---

# Serviços Envolvidos

- ValeService

---

# Controllers Relacionados

- ValeController

---

# Checklist

☐ Valor do vale definido

☐ Cliente vinculado (se aplicável)

☐ Saldo controlado

☐ Consumo registrado na venda

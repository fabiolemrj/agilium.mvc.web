# Módulo Compras

## Objetivo

O módulo **Compras** gerencia o processo de aquisição de produtos e mercadorias, incluindo entrada de notas fiscais (NFe), itens de compra, controle fiscal e integração com Estoque e Financeiro.

---

# Responsabilidades

- Registro de compras
- Registro de itens da compra
- Importação de XML de NFe (Nota Fiscal Eletrônica)
- Cadastro automático de produtos a partir da NFe
- Controle de situação da compra (Aberta, Efetivada, Cancelada)
- Cálculo de impostos (ICMS, IPI, PIS, COFINS)
- Atualização de preço de venda sugerido
- Integração com Estoque (entrada)

---

# Fluxo Geral

```
Nova Compra

↓

Informar Fornecedor / Dados

↓

Importar XML NFe (opcional)

↓

Adicionar Itens

↓

Conferir Valores Fiscais

↓

Efetivar Compra

↓

Atualizar Estoque

↓

Atualizar Preços
```

---

# Situações da Compra

| Situação | Descrição |
|----------|-----------|
| Aberta | Compra em edição |
| Efetivada | Compra finalizada, estoque atualizado |
| Cancelada | Compra cancelada |

---

# Dependências

- Fornecedor
- Empresa
- Turno
- Produto
- Estoque
- Unidade
- Tabelas fiscais (CFOP, CST, CSOSN, CEST)

---

# Principais Entidades

| Entidade | Descrição |
|----------|-----------|
| Compra | Registro principal |
| CompraItem | Itens da compra |
| CompraFiscal | Dados fiscais da compra |
| NotaFiscalInutil | NF inutilizada |

---

# Regras de Negócio

## Cadastro

- Fornecedor é obrigatório
- Empresa é obrigatória
- CFOP é obrigatório
- Data da compra deve ser válida

## Itens

- Produto deve ser informado (ou cadastrado automaticamente via NFe)
- Quantidade > 0
- Valor unitário > 0
- Valores fiscais devem ser consistentes (base de cálculo, alíquotas)

## Importação NFe

- XML deve ser válido e assinado
- Dados do XML preenchem automaticamente itens e valores fiscais
- Cadastro automático de produtos: se produto não existe, é criado

## Efetivação

- Ao efetivar, estoque é atualizado
- Preço de venda sugerido é calculado
- Dados contábeis são registrados

---

# Serviços Envolvidos

- CompraService (`agilium-manager-azure-business/Services/CompraService.cs`)
- FornecedorService
- ProdutoService
- EstoqueService
- TabelaAuxiliarFiscalService

---

# Controllers Relacionados

- CompraController (`agilum.mvc.web/Controllers/CompraController.cs`)

---

# Permissões (idTag)

| Ação | idTag |
|------|-------|
| Listar Compras | 2066 |
| Criar Compra | 2067 |
| Cancelar Compra | 2068 |
| Editar / Importar XML | 2070 |
| Efetivar Compra | 2072 |

---

# Boas Práticas

- Sempre validar XML antes de importar
- Conferir valores fiscais após importação
- Não efetivar compra com itens pendentes
- Manter rastreabilidade do XML original

---

# Checklist

☐ Fornecedor selecionado

☐ CFOP correto

☐ XML validado (se importado)

☐ Itens conferidos

☐ Valores fiscais consistentes

☐ Estoque atualizado após efetivação

☐ Preços recalculados

---

# Conclusão

O módulo **Compras** é a porta de entrada de mercadorias no sistema. A integração com NFe e o cadastro automático de produtos reduzem o trabalho manual, mas exigem validação rigorosa dos dados fiscais importados.

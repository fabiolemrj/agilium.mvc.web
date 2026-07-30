# Módulo Produtos

## Objetivo

O módulo **Produtos** gerencia todo o cadastro de itens comercializados pela empresa, incluindo informações comerciais, fiscais, tributárias, preços, composições, códigos de barras e integração com marketplace.

É um dos módulos centrais do sistema, sendo referenciado por Vendas, Compras, Estoque e Pedidos.

---

# Responsabilidades

O módulo é responsável por:

- Cadastro de produtos
- Classificação (grupo, subgrupo, departamento, marca)
- Cadastro de códigos de barras
- Cadastro de preços
- Cadastro de produtos compostos (composição)
- Informações fiscais (NCM, CEST, CFOP, CST, CSOSN, IBPT)
- Cadastro de fotos
- Integração com marketplace (Site Mercado)
- Controle de ativo/inativo
- Disponibilização para vendas e compras

---

# Fluxo Geral

```
Cadastrar Produto

↓

Classificar (Grupo / Departamento / Marca)

↓

Definir Informações Fiscais

↓

Definir Preços

↓

Vincular Códigos de Barras

↓

Disponibilizar para Operação
```

---

# Papel do Produto no Sistema

```
Produto

├── Compra (Item de Compra)
├── Venda (Item de Venda)
├── Estoque (Movimentações)
├── Pedido (Item de Pedido)
├── Devolução
├── Perda
├── Inventário
├── Marketplace
└── Cardápio Digital
```

---

# Dependências

- GrupoProduto
- SubGrupoProduto
- ProdutoDepartamento
- ProdutoMarca
- Unidade
- NCM
- CEST
- CFOP
- CST
- CSOSN
- IBPT
- Estoque
- Empresa

---

# Principais Informações

- Código do Produto
- Nome
- Descrição
- Grupo / SubGrupo
- Departamento / Marca
- Unidade de Medida
- Código de Barras (múltiplos)
- Preço de Custo
- Preço de Venda
- Margem de Lucro
- NCM
- CEST
- CFOP
- Origem do Produto
- Situação (Ativo/Inativo)
- Tipo (Produto/Serviço/Composto)
- Foto

---

# Regras de Negócio

## Cadastro

- Código do produto deve ser único por empresa
- Nome é obrigatório
- Unidade de medida é obrigatória
- NCM é obrigatório para produtos fiscais
- Pelo menos um código de barras deve ser informado

## Preço

- Preço de venda deve ser maior que zero
- Preço de custo afeta margem de lucro
- Produtos podem ter preços diferenciados por cliente (ClientePreco)
- Produtos podem ter preços por turno (TurnoPreco)

## Estoque

- Produto pode estar vinculado a múltiplos estoques (EstoqueProduto)
- Quantidade em estoque afeta disponibilidade para venda

---

# Principais Entidades Relacionadas

- Produto
- ProdutoCodigoBarra
- ProdutoComposicao
- ProdutoDepartamento
- ProdutoFoto
- ProdutoMarca
- ProdutoPreco
- ProdutoSiteMercado
- GrupoProduto
- SubGrupoProduto
- ClientePreco
- TurnoPreco

---

# Serviços Envolvidos

- ProdutoService (`agilium-manager-azure-business/Services/ProdutoService.cs`)
- TabelaAuxiliarFiscalService
- EstoqueService
- UnidadeService
- ProdutoDapper (consultas otimizadas)

---

# Controllers Relacionados

- ProdutoController (`agilum.mvc.web/Controllers/ProdutoController.cs`)

---

# Integrações

- **Cardápio Digital**: exportação de produtos via `IntegracaoCardapioService`
- **Site Mercado**: produtos com anúncios em marketplace
- **NFe**: informações fiscais utilizadas na emissão de documentos fiscais

---

# Boas Práticas

- Centralizar regras de negócio no `ProdutoService`
- Usar `ProdutoDapper` para consultas complexas (evitar N+1 no EF Core)
- Validar NCM e CEST antes da gravação
- Manter histórico de preços
- Não excluir fisicamente produtos com histórico de vendas

---

# Checklist

☐ Código único por empresa

☐ Nome informado

☐ Unidade de medida definida

☐ Classificação fiscal preenchida

☐ Preço de venda > 0

☐ Código de barras cadastrado

☐ Estoque vinculado

☐ Integrações verificadas

---

# Conclusão

O módulo **Produtos** é central para a operação comercial, conectando-se diretamente a Vendas, Compras, Estoque e emissão de documentos fiscais. Sua correta configuração fiscal é essencial para a conformidade tributária do sistema.

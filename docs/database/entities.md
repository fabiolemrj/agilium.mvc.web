# Entidades de Persistência

## Objetivo

Documentar as entidades persistidas pelo Agilium Manager, descrevendo sua estrutura, relacionamentos, responsabilidades e mapeamentos utilizados pela camada de persistência.

Este documento funciona como o catálogo oficial das entidades persistentes da plataforma.

---

# Escopo

Este documento contempla:

- Catálogo de Entidades
- Estrutura das Entidades
- Responsabilidades
- Relacionamentos
- Mapeamentos
- Restrições
- Índices
- Convenções
- Organização da Persistência

---

# Índice

- Visão Geral
- Organização das Entidades
- Catálogo
- Estrutura de Documentação
- Relacionamentos
- Mapeamentos
- Índices
- Convenções
- Atualização
- Documentação Relacionada

---

# Visão Geral

As entidades representam os objetos persistidos pela aplicação.

Cada entidade corresponde ao modelo utilizado pela camada de persistência e deve possuir documentação própria contendo sua estrutura completa.

A documentação das entidades não substitui a documentação de negócio.

---

# Organização das Entidades

As entidades devem ser agrupadas por domínio.

Exemplo:

```text
Segurança
Cadastros
Comercial
Financeiro
Estoque
Configuração
```

---

# Catálogo de Entidades

> O catálogo definitivo deverá ser construído durante o levantamento completo do banco de dados e dos projetos de infraestrutura.

| Entidade | Tabela | Módulo | Documento |
|-----------|---------|---------|-----------|
| Usuario | usuarios | Segurança | entities/usuario.md |
| Empresa | empresas | Cadastros | entities/empresa.md |
| Cliente | clientes | Comercial | entities/cliente.md |
| Produto | produtos | Comercial | entities/produto.md |
| Pedido | pedidos | Comercial | entities/pedido.md |
| Venda | vendas | Comercial | entities/venda.md |

*A lista acima representa apenas um exemplo de organização. Os nomes e entidades deverão refletir exatamente a implementação existente.*

---

# Estrutura de Documentação

Cada entidade deve possuir um documento seguindo o padrão abaixo.

## Nome

Nome da entidade.

---

## Objetivo

Responsabilidade da entidade.

---

## Tabela

Nome da tabela física.

---

## Chave Primária

Descrição da PK.

---

## Propriedades

| Campo | Tipo | Obrigatório | Observações |
|--------|------|-------------|-------------|

---

## Relacionamentos

| Tipo | Entidade | Cardinalidade |
|------|-----------|---------------|

Exemplo:

- 1:N
- N:N
- 1:1

---

## Índices

Descrição dos índices existentes.

---

## Restrições

- Unique
- Foreign Keys
- Defaults
- Checks

---

## Mapeamento

Classe responsável pelo mapeamento.

Exemplo:

```text
ProdutoMap

ou

ProdutoConfiguration
```

---

## Observações

Informações relevantes sobre a persistência.

---

# Relacionamentos

Os relacionamentos entre entidades devem ser documentados em um documento específico.

Exemplo:

```text
Cliente

1:N

Pedidos

1:N

ItensPedido

N:1

Produto
```

---

# Mapeamentos

Os mapeamentos utilizados pela camada de persistência devem documentar:

- nome da tabela;
- chave primária;
- relacionamentos;
- índices;
- constraints;
- tipos de dados;
- valores padrão.

A estratégia de mapeamento utilizada pela solução deverá refletir a implementação existente.

---

# Índices

Todo índice deve possuir documentação contendo:

- nome;
- finalidade;
- colunas;
- unicidade;
- impacto esperado.

Os detalhes encontram-se em:

```text
database/indexes.md
```

---

# Convenções

Toda entidade deve:

- representar apenas um conceito de domínio;
- possuir chave primária;
- possuir documentação própria;
- possuir mapeamento documentado;
- utilizar nomenclatura consistente.

Evitar:

- entidades com responsabilidades múltiplas;
- propriedades sem documentação;
- relacionamentos implícitos;
- duplicação de entidades.

---

# Atualização

Este documento deve ser atualizado sempre que ocorrer:

- criação de nova entidade;
- alteração estrutural;
- inclusão de relacionamento;
- alteração de índices;
- mudança de estratégia de persistência.

---

# Limitações Conhecidas

O levantamento técnico confirmou a utilização de entidades persistidas através do Entity Framework Core.

Entretanto, a documentação completa das entidades depende da análise detalhada:

- do modelo de dados;
- dos mapeamentos da camada de infraestrutura;
- dos projetos `agilium-manager-azure-business`, `agilium-manager-git-azure-infra`, `agilium-manager-azure-api` e `agilium-pdv-azure-api`.

A utilização de bancos NoSQL ou coleções MongoDB deverá ser documentada apenas se confirmada durante esses levantamentos.

---

# Documentação Relacionada

## Banco de Dados

- relationships.md
- indexes.md
- migrations.md
- mapping.md

## Arquitetura

- architecture/database.md
- architecture/layers.md

## Negócio

- business/modules.md
- business/business-rules.md
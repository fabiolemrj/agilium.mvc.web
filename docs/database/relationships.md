# Relacionamentos das Entidades

## Objetivo

Documentar os relacionamentos entre as entidades persistidas pelo Agilium Manager, descrevendo suas cardinalidades, dependências, regras de integridade e comportamentos definidos na camada de persistência.

Este documento representa a visão arquitetural dos relacionamentos do modelo de dados da aplicação.

---

# Escopo

Este documento contempla:

- Relacionamentos entre Entidades
- Cardinalidades
- Chaves Estrangeiras
- Navegação entre Entidades
- Integridade Referencial
- Comportamentos de Exclusão
- Convenções
- Diagramas

---

# Índice

- Visão Geral
- Modelo de Relacionamentos
- Diagrama de Entidades
- Catálogo de Relacionamentos
- Tipos de Relacionamento
- Integridade Referencial
- Comportamentos de Exclusão
- Convenções
- Boas Práticas
- Limitações Conhecidas
- Atualização
- Documentação Relacionada

---

# Visão Geral

Os relacionamentos representam a forma como as entidades persistidas interagem entre si.

Todo relacionamento deve ser explicitamente documentado para garantir:

- compreensão do domínio;
- consistência dos dados;
- rastreabilidade;
- manutenção simplificada;
- evolução controlada do modelo.

---

# Modelo de Relacionamentos

Os relacionamentos devem refletir o modelo de domínio da aplicação.

Exemplo conceitual:

```text
Empresa

├── Usuários
├── Clientes
├── Produtos
├── Pedidos

Pedido

├── Itens
└── Pagamentos

Cliente

└── Pedidos
```

O modelo definitivo deverá refletir exatamente as entidades existentes na solução.

---

# Diagrama de Entidades

O diagrama entidade-relacionamento (ER) deve ser mantido em documento próprio.

Local recomendado:

```text
database/diagrams/er-model.md
```

Sempre que houver alteração estrutural no modelo de dados, o diagrama deverá ser atualizado.

---

# Catálogo de Relacionamentos

> O catálogo definitivo será construído durante o levantamento da camada de persistência.

| Entidade Origem | Entidade Destino | Cardinalidade | Chave Estrangeira | Documento |
|-----------------|------------------|---------------|-------------------|-----------|
| Cliente | Pedido | 1:N | ClienteId | entities/pedido.md |
| Pedido | ItemPedido | 1:N | PedidoId | entities/item-pedido.md |
| Produto | ItemPedido | 1:N | ProdutoId | entities/produto.md |

*A tabela acima representa apenas um exemplo de organização.*

---

# Tipos de Relacionamento

Os relacionamentos podem assumir diferentes cardinalidades.

## Um para Um (1:1)

Cada registro possui exatamente um correspondente.

---

## Um para Muitos (1:N)

Uma entidade pode possuir diversos registros relacionados.

Exemplo:

```text
Cliente

↓

Pedidos
```

---

## Muitos para Muitos (N:N)

Relacionamento intermediado por entidade associativa quando aplicável.

Exemplo:

```text
Usuário

↓

PerfilUsuario

↓

Perfil
```

---

# Integridade Referencial

Todo relacionamento deve documentar:

- entidade de origem;
- entidade de destino;
- chave estrangeira;
- obrigatoriedade;
- cardinalidade;
- restrições.

A integridade referencial deve refletir a implementação da camada de persistência.

---

# Comportamentos de Exclusão

Os comportamentos associados aos relacionamentos devem ser documentados sempre que existirem.

Exemplos:

- Restrict
- Cascade
- Set Null
- No Action

A estratégia adotada para cada relacionamento deverá refletir a implementação existente.

---

# Convenções

Todo relacionamento deve:

- possuir documentação;
- possuir cardinalidade definida;
- possuir chave estrangeira identificada;
- possuir comportamento de exclusão documentado;
- manter consistência com o modelo de domínio.

---

# Boas Práticas

Sempre:

- documentar relacionamentos explicitamente;
- manter o diagrama atualizado;
- revisar impactos antes de alterar relacionamentos;
- manter consistência entre entidades e mapeamentos;
- evitar relacionamentos implícitos.

Evitar:

- relacionamentos sem documentação;
- dependências circulares;
- múltiplos caminhos de exclusão em cascata sem necessidade;
- divergência entre documentação e implementação.

---

# Limitações Conhecidas

O levantamento técnico confirmou:

- utilização de entidades persistidas;
- utilização de Entity Framework Core;
- existência de mapeamentos da camada de persistência.

Ainda deverão ser confirmados durante a análise dos projetos:

- `agilium-manager-git-azure-infra`;
- `agilium-manager-azure-business`;
- `agilium-manager-azure-api`;
- `agilium-pdv-azure-api`;

os seguintes aspectos:

- relacionamentos completos entre entidades;
- chaves estrangeiras existentes;
- comportamentos de exclusão;
- configuração da Fluent API;
- diagrama entidade-relacionamento definitivo.

---

# Atualização

Este documento deve ser atualizado sempre que ocorrer:

- criação de nova entidade;
- alteração de relacionamento;
- modificação de cardinalidade;
- alteração de comportamento de exclusão;
- evolução do modelo de dados.

---

# Documentação Relacionada

## Banco de Dados

- database/overview.md
- database/entities.md
- database/mappings.md
- database/indexes.md
- database/migrations.md

## Diagramas

- database/diagrams/er-model.md

## Arquitetura

- architecture/database.md
- architecture/layers.md
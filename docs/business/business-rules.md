# Regras de Negócio

## Objetivo

Documentar as regras de negócio que governam o comportamento do Agilium Manager, estabelecendo um catálogo centralizado das regras funcionais implementadas na plataforma.

Este documento define como as regras são organizadas, documentadas, classificadas e mantidas.

---

# Escopo

Este documento contempla:

- Arquitetura das Regras de Negócio
- Organização por Módulo
- Classificação das Regras
- Convenções
- Catálogo
- Processo de Atualização
- Documentação Relacionada

---

# Índice

- Visão Geral
- Organização das Regras
- Classificação
- Catálogo por Módulo
- Estrutura de Documentação
- Identificação das Regras
- Implementação
- Boas Práticas
- Limitações
- Documentação Relacionada

---

# Visão Geral

As regras de negócio representam as restrições funcionais que determinam o comportamento esperado do sistema.

Na arquitetura do Agilium Manager, essas regras são implementadas principalmente na camada de Application/Business, preservando Controllers e Repositories livres de lógica de negócio.

As validações de negócio são tratadas utilizando o Notification Pattern sempre que possível.

---

# Organização das Regras

As regras devem ser agrupadas por módulo funcional.

Exemplo:

```text
Produtos
Clientes
Usuários
Vendas
Financeiro
Caixa
Estoque
Pedidos
Configurações
```

Cada módulo possui documentação própria.

---

# Classificação

As regras podem ser classificadas conforme sua natureza.

| Tipo | Descrição |
|--------|-----------|
| Validação | Restringe dados de entrada |
| Processo | Controla um fluxo operacional |
| Permissão | Define acesso ao recurso |
| Integração | Controla comunicação externa |
| Fiscal | Atende legislação |
| Consistência | Garante integridade dos dados |

---

# Catálogo por Módulo

| Módulo | Documento |
|---------|-----------|
| Produtos | products.md |
| Clientes | customers.md |
| Usuários | users.md |
| Pedidos | orders.md |
| Vendas | sales.md |
| Caixa | cash-register.md |
| Estoque | inventory.md |
| Financeiro | financial.md |

Cada documento contém as regras específicas daquele módulo.

---

# Estrutura de Documentação

Toda regra deve possuir a seguinte estrutura.

## Identificação

```text
BR-001
```

---

## Nome

Título resumido da regra.

---

## Módulo

Módulo responsável.

---

## Objetivo

Qual problema a regra resolve.

---

## Descrição

Descrição completa da regra.

---

## Contexto

Em quais situações ela é aplicada.

---

## Pré-condições

O que deve ocorrer antes da execução.

---

## Fluxo

Quando a regra é avaliada.

---

## Resultado Esperado

Comportamento esperado.

---

## Implementação

Onde a regra está implementada.

Exemplo:

```text
ProdutoService

↓

BaseService

↓

Notification Pattern
```

---

## Tipo

- Validação
- Processo
- Fiscal
- Segurança
- Integração

---

## Severidade

- Error
- Warning
- Information

---

## Dependências

Outras regras relacionadas.

---

## Observações

Informações adicionais.

---

# Exemplo

## BR-001

**Nome**

Produto deve possuir descrição.

**Módulo**

Produtos

**Descrição**

Não é permitido cadastrar produtos sem descrição.

**Implementação**

ProdutoService

**Severidade**

Error

---

# Implementação

O levantamento técnico identificou que as regras de negócio devem permanecer centralizadas na camada de Services.

Fluxo recomendado:

```text
Controller

↓

Application Service

↓

Validações

↓

Notification Pattern

↓

Repository
```

Controllers não devem conter regras de negócio.

---

# Boas Práticas

Sempre:

- implementar regras na camada de Application;
- reutilizar regras existentes;
- utilizar Notification Pattern para validações esperadas;
- documentar novas regras;
- manter rastreabilidade entre código e documentação.

Evitar:

- regras em Controllers;
- regras em Repositories;
- duplicação de validações;
- mensagens inconsistentes.

---

# Limitações Conhecidas

O levantamento arquitetural confirmou a existência de uma camada de Services e do Notification Pattern como mecanismos centrais para aplicação das regras de negócio.

Entretanto, o catálogo completo das regras deverá ser construído durante o levantamento funcional dos módulos da solução.

---

# Atualização

Sempre que uma nova regra for criada:

1. Atualizar a documentação do módulo.
2. Atualizar o índice deste documento.
3. Revisar regras relacionadas.
4. Atualizar fluxos e diagramas quando necessário.

---

# Documentação Relacionada

- validations.md
- workflows.md
- modules.md
- architecture/layers.md
- architecture/patterns.md
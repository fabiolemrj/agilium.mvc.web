# Prompt: Refatoração

# Objetivo

Template para planejamento, execução e validação de refatorações no Agilium Manager.

Este prompt deve ser utilizado para reduzir débito técnico, melhorar a qualidade do código e simplificar implementações existentes, preservando integralmente o comportamento funcional e a arquitetura da solução.

---

# Quando utilizar

Utilize este prompt para:

- refatorar classes;
- refatorar métodos;
- reduzir complexidade;
- eliminar duplicações;
- melhorar legibilidade;
- reduzir acoplamento;
- reorganizar responsabilidades;
- preparar futuras evoluções.

---

# Prompt

```text
Realize a refatoração do seguinte escopo:

[ESCOPO]

Antes de qualquer alteração, realizar um levantamento completo da implementação atual.

---

## 1. Levantamento

Identificar:

- objetivo da funcionalidade;
- fluxo atual;
- dependências;
- regras de negócio;
- integrações;
- componentes envolvidos.

---

## 2. Arquitetura

Verificar aderência à arquitetura:

MVC

↓

Business

↓

Infrastructure

↓

Banco de Dados

Confirmar que a refatoração mantém essa organização.

---

## 3. Componentes Impactados

Identificar:

Controllers

MainController

Services

Repositories

Interfaces

ViewModels

Models

Entities

AutoMapper

Notification Pattern

FluentValidation

Middlewares

Dependency Injection

---

## 4. Motivação

Explicar claramente quais problemas serão resolvidos.

Exemplos:

- alta complexidade;
- duplicação;
- acoplamento;
- baixa coesão;
- baixa legibilidade;
- manutenção difícil;
- código morto;
- responsabilidades incorretas.

---

## 5. Estratégia

Definir a abordagem de refatoração.

Quando aplicável:

- extração de métodos;
- extração de classes;
- separação de responsabilidades;
- reorganização entre camadas;
- eliminação de duplicações;
- melhoria de nomenclatura;
- simplificação de fluxos;
- reutilização de componentes existentes.

---

## 6. Plano de Execução

Descrever passo a passo:

1.

2.

3.

4.

Após cada etapa validar a consistência da solução antes de prosseguir.

---

## 7. Preservação de Comportamento

Garantir que NÃO sejam alterados:

- contratos públicos;
- comportamento funcional;
- regras de negócio;
- APIs;
- ViewModels;
- integrações;
- respostas HTTP;
- estrutura de persistência (exceto quando fizer parte do escopo).

---

## 8. Validação

Verificar:

Notification Pattern

FluentValidation

AutoMapper

Repository Pattern

Unit of Work

Dependency Injection

Entity Framework Core

Dapper

Segurança

Tratamento de exceções

---

## 9. Impacto

Identificar impacto em:

Controllers

Services

Repositories

Banco de Dados

Views

ViewModels

Integrações

Documentação

---

## 10. Riscos

Identificar:

quebras de compatibilidade;

efeitos colaterais;

dependências ocultas;

regressões funcionais;

débitos técnicos remanescentes.

---

## 11. Testes

Verificar:

- funcionalidades afetadas;
- testes automatizados existentes;
- necessidade de novos testes;
- cenários que devem ser validados manualmente.

Caso não exista cobertura automatizada para a área refatorada, registrar essa limitação e indicar os cenários críticos que precisam ser testados.

---

## 12. Resultado

Apresentar:

Resumo Executivo

Arquivos Alterados

Classes Alteradas

Métodos Refatorados

Problemas Eliminados

Benefícios Obtidos

Riscos

Recomendações

Débitos Técnicos Restantes
```

---

# Parâmetros

| Parâmetro | Descrição | Exemplo |
|-----------|-----------|---------|
| `ESCOPO` | Classe, método, módulo ou funcionalidade | `ProdutoService`, `PedidoService.CriarPedido`, `Controllers/ProdutoController`, `Fluxo de Venda`, `Módulo Financeiro` |

---

# Resultado Esperado

A refatoração deve:

- preservar integralmente o comportamento funcional da aplicação;
- manter a arquitetura em camadas do Agilium Manager;
- reduzir complexidade, duplicação e acoplamento;
- melhorar a legibilidade e a manutenibilidade do código;
- respeitar os padrões arquiteturais e de desenvolvimento identificados na solução;
- produzir um relatório técnico contendo as alterações realizadas, impactos, riscos e recomendações.

Sempre que não houver evidências suficientes sobre determinado comportamento da funcionalidade, registrar explicitamente essa limitação antes de realizar alterações estruturais.
# Prompt: Levantamento / Discovery

# Objetivo

Template para realização de levantamentos técnicos no Agilium Manager.

Este prompt deve ser utilizado antes de qualquer implementação, correção, refatoração ou evolução da solução, permitindo compreender completamente a arquitetura, os componentes envolvidos e as regras existentes.

---

# Quando utilizar

Utilize este prompt para:

- entender um módulo;
- entender uma funcionalidade;
- analisar uma camada da solução;
- iniciar uma nova implementação;
- realizar migrações;
- planejar integrações;
- produzir documentação técnica.

---

# Prompt

```text
Realize um levantamento técnico completo da seguinte área do Agilium Manager:

Área:

[NOME_DA_AREA]

Camada:

[CAMADA]

Antes de propor qualquer alteração, compreender completamente o contexto da implementação existente.

---

## 1. Arquitetura

Identificar:

- posição da funcionalidade na arquitetura;
- relacionamento entre camadas;
- dependências;
- padrões arquiteturais utilizados.

Mapear o fluxo:

MVC

↓

Business

↓

Infrastructure

↓

Banco de Dados

---

## 2. Estrutura do Projeto

Identificar:

Controllers

Services

Repositories

Interfaces

ViewModels

Models

Entities

Configurations

Middlewares

Views

Componentes compartilhados

Arquivos relevantes

Organização das pastas

---

## 3. Componentes

Para cada componente identificar:

Nome

Responsabilidade

Dependências

Integrações

Pontos de reutilização

---

## 4. Fluxo da Funcionalidade

Documentar o fluxo completo:

Usuário

↓

View

↓

Controller

↓

Service

↓

Repository

↓

Banco de Dados

↓

Retorno

Informar todas as etapas envolvidas.

---

## 5. Persistência

Identificar:

Entity Framework Core

Dapper

Repositories

Unit of Work

Contextos

Entidades

Relacionamentos

Consultas relevantes

---

## 6. Regras de Negócio

Levantar:

Services

Notification Pattern

FluentValidation

Validações

Processamentos

Regras

---

## 7. Interface

Levantar:

Views

Layouts

Partial Views

View Components

ViewModels

AutoMapper

Data Annotations

---

## 8. Segurança

Verificar:

Identity

Authorize

ClaimsAuthorize

Autenticação

Autorização

Middlewares

---

## 9. Configuração

Levantar:

ResolveDependencyConfig

Startup

Program

Configurações

appsettings

Dependency Injection

---

## 10. Dependências

Mapear:

injeções

interfaces

serviços

repositórios

middlewares

bibliotecas

integrações

---

## 11. Banco de Dados

Identificar:

entidades

tabelas

relacionamentos

migrações

consultas

transações

---

## 12. Pontos de Atenção

Identificar:

código complexo

duplicações

acoplamentos

débitos técnicos

gargalos

possíveis bugs

limitações

riscos arquiteturais

---

## 13. Resultado

Gerar um relatório contendo:

Resumo Executivo

Arquitetura

Estrutura

Fluxo

Componentes

Dependências

Regras de Negócio

Persistência

Segurança

Pontos Fortes

Pontos de Atenção

Débitos Técnicos

Limitações

Recomendações

Próximos Passos
```

---

# Parâmetros

| Parâmetro | Descrição | Exemplo |
|-----------|-----------|---------|
| `NOME_DA_AREA` | Área, módulo ou funcionalidade | `Produtos`, `PDV`, `Financeiro`, `Licenciamento`, `PedidoService`, `Cadastro de Clientes` |
| `CAMADA` | Camada principal da análise | `MVC`, `Business`, `Infrastructure`, `API`, `Banco de Dados`, `Solution Completa` |

---

# Resultado Esperado

O levantamento deve produzir um relatório técnico estruturado que:

- descreva a arquitetura da área analisada;
- identifique os componentes envolvidos e suas responsabilidades;
- documente o fluxo completo da funcionalidade;
- mapeie dependências, integrações e persistência;
- identifique riscos, limitações e débitos técnicos;
- apresente recomendações para futuras implementações ou evoluções.

As conclusões devem ser baseadas exclusivamente no código-fonte e no levantamento técnico disponível, indicando explicitamente qualquer informação que dependa de análise adicional.
# ADR-0007 - Estratégia de Validação da Aplicação

| Campo | Valor |
|-------|-------|
| **Status** | Accepted |
| **Data** | 2026-07-29 |
| **Autor** | Equipe Agilium |
| **Versão** | 1.0 |

---

# Contexto

O Agilium Manager possui dezenas de módulos de negócio, APIs REST e aplicações MVC que compartilham diversas regras de validação.

As validações ocorrem em diferentes níveis da aplicação, incluindo:

- Requisições HTTP;
- DTOs;
- Casos de uso;
- Regras de domínio;
- Persistência;
- Banco de Dados.

Ao longo da evolução do sistema foram identificadas validações distribuídas entre Controllers, Services, Entities e Repositories, dificultando manutenção e reutilização.

Era necessário estabelecer uma estratégia única de validação para toda a plataforma.

---

# Problema

A ausência de uma estratégia padronizada ocasiona:

- Regras duplicadas;
- Validações inconsistentes;
- Código repetitivo;
- Dificuldade de manutenção;
- Erros difíceis de localizar;
- Mistura entre validações técnicas e regras de negócio.

Também foi identificado o uso inadequado de Exceptions para representar erros esperados de validação.

---

# Alternativas Consideradas

## Alternativa 1 — Validação apenas nas Controllers

### Vantagens

- Implementação simples.
- Fácil compreensão.

### Desvantagens

- Regras duplicadas.
- Não protege chamadas internas.
- Baixa reutilização.
- Controllers excessivamente grandes.

---

## Alternativa 2 — Validação distribuída

Cada camada realiza suas próprias validações.

### Vantagens

- Flexibilidade.

### Desvantagens

- Duplicação.
- Falta de padrão.
- Alto custo de manutenção.

---

## Alternativa 3 — Estratégia de Validação em Camadas (Escolhida)

### Vantagens

- Separação clara de responsabilidades.
- Reutilização.
- Código limpo.
- Melhor manutenção.
- Validações previsíveis.

### Desvantagens

- Requer disciplina arquitetural.
- Maior quantidade de classes de validação.

---

# Decisão

Foi adotada uma estratégia de validação baseada em camadas, onde cada nível da aplicação possui responsabilidades específicas.

Cada tipo de validação deverá ocorrer apenas na camada apropriada.

---

# Objetivos

A estratégia possui os seguintes objetivos:

- Padronizar validações.
- Evitar duplicação.
- Melhorar manutenção.
- Separar validações técnicas das regras de negócio.
- Facilitar testes.
- Melhorar a qualidade do código.

---

# Camadas de Validação

## 1. Validação de Entrada (Presentation)

Responsável por validar:

- Campos obrigatórios.
- Formatos.
- Tipos.
- Comprimentos.
- Datas inválidas.
- Estrutura da requisição.

Exemplos:

- CPF obrigatório.
- Data inválida.
- Quantidade negativa.
- Campo obrigatório.

Esta validação ocorre antes da execução da regra de negócio.

---

## 2. Validação da Aplicação (Application)

Responsável por validar:

- Consistência do caso de uso.
- Existência de entidades.
- Dependências externas.
- Pré-condições.

Exemplos:

- Cliente existe.
- Produto existe.
- Empresa ativa.
- Caixa aberto.

---

## 3. Validação de Domínio (Business)

Responsável por validar exclusivamente regras de negócio.

Exemplos:

- Produto não pode ser vendido sem estoque.
- Venda cancelada não pode ser alterada.
- Cliente bloqueado não pode comprar.
- Caixa fechado não pode registrar venda.

Essas regras pertencem ao domínio.

---

## 4. Validação de Persistência

Responsável por:

- Chaves duplicadas.
- Integridade referencial.
- Conflitos de concorrência.
- Restrições do banco.

---

# Fluxo

```text
Request

↓

Presentation Validation

↓

Application Validation

↓

Business Validation

↓

Repository

↓

Database Validation
```

---

# Responsabilidades

## Controller

Responsável apenas pela validação estrutural da requisição.

Não deve conter regras de negócio.

---

## Service

Responsável pelas validações do caso de uso.

---

## Domínio

Responsável exclusivamente pelas regras de negócio.

---

## Repository

Responsável apenas pelas validações relacionadas à persistência.

---

# Ferramentas de Validação

A estratégia oficial prevê:

- DataAnnotations para validações simples de entrada.
- FluentValidation para validações complexas de Requests e DTOs.
- Notification Pattern para retorno de erros de negócio.
- Entity Framework Core para validações de persistência.

---

# Notification Pattern

Todas as validações de negócio deverão registrar erros utilizando o Notification Pattern.

Não deverão lançar Exceptions para erros esperados.

Exemplo:

- Estoque insuficiente.
- Cliente bloqueado.
- Produto inativo.

---

# Exceptions

Exceptions devem representar apenas erros inesperados.

Exemplos:

- Timeout.
- Falha de conexão.
- Erro de banco.
- NullReferenceException.
- IOException.

Nunca utilizar Exceptions para regras de negócio.

---

# Ordem das Validações

As validações deverão seguir a seguinte sequência:

1. Estrutura da requisição.
2. Regras da aplicação.
3. Regras de domínio.
4. Persistência.
5. Banco de Dados.

Uma camada não deve repetir validações pertencentes a outra.

---

# Benefícios

- Código organizado.
- Menor duplicação.
- Melhor reutilização.
- Facilidade para testes.
- Melhor legibilidade.
- Separação de responsabilidades.
- Maior previsibilidade.

---

# Desvantagens

- Maior quantidade de classes.
- Necessidade de disciplina.
- Curva de aprendizado para novos desenvolvedores.

---

# Riscos

Caso esta estratégia não seja seguida:

- Validações duplicadas.
- Regras inconsistentes.
- Código difícil de manter.
- Uso excessivo de Exceptions.
- Falhas funcionais.

---

# Impacto

Esta decisão impacta:

- Controllers
- APIs
- MVC
- Services
- Business
- Repositories
- Banco de Dados
- Testes
- Documentação

---

# Plano de Implementação

1. Padronizar validações de entrada utilizando DataAnnotations e FluentValidation.
2. Centralizar regras de negócio na camada de domínio.
3. Utilizar Notification Pattern para erros funcionais.
4. Remover validações duplicadas.
5. Revisar Controllers e Services existentes.
6. Atualizar documentação técnica.

---

# Critérios de Aceitação

Uma implementação é considerada aderente quando:

- Controllers não possuem regras de negócio.
- Services validam apenas o caso de uso.
- O domínio concentra todas as regras de negócio.
- Notification Pattern é utilizado para erros esperados.
- Exceptions representam apenas falhas inesperadas.
- Não existem validações duplicadas entre camadas.

---

# ADRs Relacionados

- ADR-0001 — Arquitetura em Camadas
- ADR-0002 — Repository Pattern
- ADR-0003 — Notification Pattern
- ADR-0004 — Entity Framework Core
- ADR-0006 — Estratégia de Autorização
- ADR-0011 — Service Layer
- ADR-0014 — Tratamento Global de Exceções

---

# Referências

- Martin Fowler — *Patterns of Enterprise Application Architecture*
- Eric Evans — *Domain-Driven Design*
- Microsoft — *Model Validation in ASP.NET Core*
- FluentValidation Documentation
- OWASP — Input Validation Cheat Sheet

---

# Histórico

| Versão | Data | Descrição |
|---------|------|-----------|
| 1.0 | 2026-07-29 | Criação da ADR definindo a estratégia oficial de validação do Agilium Manager, estabelecendo responsabilidades por camada e padronizando o uso de DataAnnotations, FluentValidation, Notification Pattern e validações de domínio. |
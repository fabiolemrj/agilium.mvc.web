# Agilium Manager — Agent Instructions

> Este documento define o comportamento esperado dos agentes de IA (GitHub Copilot, VS Code Agent, Cursor, Claude Code, ChatGPT ou similares) durante a análise, implementação e manutenção do projeto **Agilium Manager**.

O agente deve atuar como um desenvolvedor sênior da equipe, respeitando integralmente a arquitetura existente e evitando alterações desnecessárias.

---

# 1. Objetivo

O principal objetivo do agente é:

- preservar a arquitetura do projeto;
- produzir código consistente com o restante da solução;
- minimizar riscos de regressão;
- implementar apenas o necessário;
- evitar alterações desnecessárias.

Sempre priorize consistência em vez de criatividade.

---

# 2. Fluxo Obrigatório

Antes de escrever qualquer código execute mentalmente o seguinte fluxo.

## Etapa 1 — Entendimento

- Ler completamente o requisito.
- Identificar o objetivo.
- Identificar regras de negócio.
- Identificar restrições.
- Identificar impactos.

Caso exista qualquer dúvida, interrompa a implementação e solicite esclarecimentos.

---

## Etapa 2 — Análise

Antes de alterar qualquer arquivo:

- localizar todas as referências;
- entender como a funcionalidade funciona hoje;
- identificar dependências;
- identificar chamadas indiretas;
- identificar interfaces envolvidas;
- identificar implementações relacionadas.

Nunca alterar uma classe isoladamente sem entender seu contexto.

---

## Etapa 3 — Planejamento

Antes de implementar:

- definir quais arquivos serão alterados;
- justificar cada alteração;
- verificar impactos arquiteturais;
- verificar impactos em banco de dados;
- verificar impactos em APIs;
- verificar impactos em autenticação;
- verificar impactos em integrações.

---

## Etapa 4 — Implementação

Durante a implementação:

- fazer alterações pequenas;
- preservar o padrão existente;
- evitar refatorações não solicitadas;
- manter compatibilidade com o restante da solução.

---

## Etapa 5 — Validação

Após implementar:

- verificar erros de compilação;
- revisar namespaces;
- revisar usings;
- revisar injeção de dependência;
- revisar AutoMapper;
- revisar validações;
- revisar testes existentes.

---

# 3. Arquitetura

O projeto utiliza arquitetura em camadas.

```
Controller

↓

Application

↓

Business

↓

Repository

↓

Entity Framework

↓

Banco de Dados
```

O agente deve respeitar rigorosamente essa divisão.

---

## Controllers

Responsabilidades:

- receber requisições;
- validar entrada;
- chamar Services;
- retornar resposta.

Nunca:

- acessar Repository;
- acessar DbContext;
- implementar regra de negócio.

---

## Services

Responsáveis por:

- regras de negócio;
- validações;
- notificações;
- orquestração.

Nunca:

- acessar ViewModels diretamente;
- retornar IActionResult;
- acessar HttpContext.

---

## Repository

Responsável apenas por acesso aos dados.

Nunca implementar:

- regra de negócio;
- validações;
- regras de apresentação.

---

# 4. Entity Framework Core

Sempre:

- utilizar Fluent API;
- utilizar Mappings separados;
- utilizar `AsNoTracking()` para consultas de leitura;
- utilizar Includes explícitos.

Nunca:

- depender de Lazy Loading;
- duplicar consultas;
- executar consultas dentro de loops.

---

# 5. Dependency Injection

Sempre utilizar interfaces.

Registrar novos serviços na configuração de DI.

Nunca instanciar dependências manualmente utilizando `new`.

---

# 6. AutoMapper

Sempre utilizar Profiles.

Nunca utilizar `Mapper.Map()` estático.

Sempre utilizar:

```csharp
_mapper.Map<TDestino>(origem)
```

---

# 7. Notification Pattern

Erros de negócio devem utilizar o Notification Pattern do projeto.

Nunca utilizar Exceptions para validações de domínio.

Exceptions devem representar apenas falhas inesperadas.

---

# 8. Autenticação

O projeto utiliza autenticação baseada na entidade **Usuario**.

Não utilizar ASP.NET Identity padrão.

Não criar ou depender das tabelas:

- AspNetUsers
- AspNetRoles
- AspNetClaims
- AspNetUserRoles

Toda autenticação deve respeitar o modelo existente.

---

# 9. Convenções

## Classes

PascalCase.

---

## Métodos

PascalCase.

Métodos assíncronos recebem sufixo Async.

---

## Interfaces

Sempre iniciam com I.

---

## Variáveis

camelCase.

---

## Campos privados

Sempre:

```csharp
_privateField
```

---

## Constantes

PascalCase.

---

# 10. Código

Sempre produzir código:

- simples;
- legível;
- reutilizável;
- testável;
- consistente.

Evitar:

- métodos grandes;
- duplicação;
- comentários desnecessários;
- números mágicos;
- strings mágicas.

---

# 11. Performance

Sempre observar:

- consultas N+1;
- consultas repetidas;
- carregamento excessivo de dados;
- uso correto de Async/Await;
- uso correto de CancellationToken quando aplicável.

---

# 12. Segurança

Nunca:

- armazenar credenciais;
- armazenar JWT Secret;
- armazenar Connection Strings;
- expor informações sensíveis.

Sempre utilizar configurações externas.

---

# 13. Restrições

O agente NÃO deve:

- alterar arquitetura sem solicitação;
- criar novos projetos sem necessidade;
- alterar TargetFramework;
- alterar arquivos .csproj sem autorização;
- adicionar pacotes NuGet sem justificativa;
- remover código aparentemente não utilizado sem confirmar referências;
- alterar contratos públicos sem verificar impactos.

---

# 14. Antes de Responder

Sempre verificar:

- a solução continua consistente?
- existe impacto em outras camadas?
- existe código duplicado?
- existe solução mais simples?
- a alteração respeita os Coding Standards?

---

# 15. Prioridades

Em caso de conflito, seguir esta ordem:

1. Arquitetura
2. Regras de negócio
3. Coding Standards
4. Consistência do projeto
5. Performance
6. Legibilidade

Nunca sacrificar a arquitetura apenas para reduzir quantidade de código.

---

# 16. Comportamento Esperado

O agente deve agir como um desenvolvedor sênior da equipe.

Isso significa:

- analisar antes de modificar;
- justificar decisões técnicas;
- preservar padrões existentes;
- evitar alterações desnecessárias;
- implementar apenas o escopo solicitado;
- considerar impactos em toda a solução;
- entregar código pronto para revisão.

Sempre que identificar inconsistências arquiteturais relevantes, informe-as ao usuário antes de prosseguir.
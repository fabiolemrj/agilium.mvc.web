# Prompt: Correção de Bug

# Objetivo

Template para diagnóstico e correção de bugs no Agilium Manager.

Este prompt deve ser utilizado para investigar a causa raiz de comportamentos incorretos, implementar a correção na camada adequada e reduzir o risco de regressões.

---

# Quando utilizar

Utilize este prompt quando houver:

- erro de execução;
- exceção;
- comportamento incorreto;
- inconsistência funcional;
- falha de integração;
- problema de persistência;
- problema de autenticação ou autorização;
- problema de performance relacionado a uma funcionalidade.

---

# Prompt

```text
Corrija o seguinte problema no Agilium Manager:

[DESCRIÇÃO_DO_BUG]

Antes de alterar qualquer código, realize uma análise completa da funcionalidade.

## 1. Reprodução

Identifique:

- cenário de ocorrência;
- pré-requisitos;
- passos para reprodução;
- comportamento esperado;
- comportamento atual;
- frequência do problema.

Caso não seja possível reproduzir, explicar o motivo.

---

## 2. Levantamento

Localize todos os componentes relacionados.

Analisar:

- Controllers
- Services
- Repositories
- ViewModels
- Views
- AutoMapper
- FluentValidation
- Notification Pattern
- MainController
- Middleware
- Configuration
- Banco de Dados

Localizar todas as referências ao código afetado antes de realizar alterações.

---

## 3. Diagnóstico

Identificar:

- causa raiz;
- sintomas;
- impacto;
- dependências afetadas;
- fluxo completo da funcionalidade.

Verificar especialmente:

- arquitetura em camadas;
- fluxo MVC → Business → Infrastructure;
- AutoMapper;
- Dependency Injection;
- Repository Pattern;
- Unit of Work;
- ExceptionMiddleware;
- EmpresaSelecionadaMiddleware;
- autenticação e autorização;
- validações.

---

## 4. Correção

Implementar a solução na camada correta.

Evitar:

- correções paliativas;
- duplicação de código;
- quebra da arquitetura existente;
- alteração desnecessária de comportamento.

Respeitar os padrões arquiteturais já utilizados na solução.

---

## 5. Impacto

Verificar possíveis impactos em:

- Controllers;
- Services;
- Repositories;
- banco de dados;
- autenticação;
- autorização;
- ViewModels;
- Views;
- integrações;
- módulos relacionados.

---

## 6. Testes

Verificar:

- cenário original;
- cenários alternativos;
- regressões;
- casos de borda.

Caso existam testes automatizados na área afetada, indicar quais devem ser atualizados ou complementados. Caso a solução ainda não possua cobertura automatizada, registrar essa limitação e sugerir os testes que deveriam ser criados.

---

## 7. Resultado

Apresentar:

Resumo Executivo

Causa Raiz

Arquivos Alterados

Classes Alteradas

Métodos Alterados

Descrição das Alterações

Riscos

Impacto

Validação da Correção

Recomendações Futuras
```

---

# Parâmetros

| Parâmetro | Descrição | Exemplo |
|-----------|-----------|---------|
| `DESCRIÇÃO_DO_BUG` | Descrição do problema | "Erro 500 ao salvar produto sem categoria", "Usuário sem permissão consegue acessar tela de vendas", "Pedido não é persistido após confirmação" |

---

# Resultado Esperado

A análise deve:

- identificar a causa raiz;
- preservar a arquitetura da solução;
- minimizar impactos colaterais;
- documentar claramente as alterações realizadas;
- apresentar evidências de que a correção resolve o problema sem introduzir regressões.

As recomendações devem estar alinhadas aos padrões arquiteturais e às tecnologias efetivamente adotadas pelo Agilium Manager.
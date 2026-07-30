# ADR-0020 - Estratégia de Testes (Testing Strategy)

| Campo | Valor |
|-------|-------|
| **Status** | Accepted |
| **Data** | 2026-07-29 |
| **Autor** | Equipe Agilium |
| **Versão** | 1.0 |

---

# Contexto

O Agilium Manager é uma plataforma composta por APIs, aplicações MVC, integrações, regras de negócio complexas e módulos críticos como:

- Autenticação
- Controle de Acesso
- Vendas
- Financeiro
- Estoque
- Licenciamento
- Caixa
- Integrações
- Configurações

Grande parte das funcionalidades implementa regras de negócio que evoluem continuamente.

A ausência de uma estratégia de testes aumenta significativamente o risco de regressões, falhas em produção e perda de qualidade.

Era necessário definir uma política oficial de testes para toda a plataforma.

---

# Problema

Sem uma estratégia de testes:

- Alterações podem quebrar funcionalidades existentes;
- Regras de negócio deixam de ser validadas;
- Refatorações tornam-se arriscadas;
- Bugs chegam à produção;
- Deploys possuem maior risco;
- Baixa confiabilidade da aplicação.

Era necessário estabelecer um processo padronizado de validação automática.

---

# Alternativas Consideradas

## Alternativa 1 — Apenas Testes Manuais

### Vantagens

- Simples.
- Nenhuma implementação adicional.

### Desvantagens

- Alto custo.
- Pouca cobertura.
- Processo lento.
- Baixa repetibilidade.

---

## Alternativa 2 — Apenas Testes de Integração

### Vantagens

- Boa validação do sistema.

### Desvantagens

- Execução lenta.
- Diagnóstico difícil.
- Pouco isolamento.

---

## Alternativa 3 — Pirâmide de Testes (Escolhida)

Combinar diferentes níveis de testes automatizados.

### Vantagens

- Alta cobertura.
- Execução rápida.
- Boa confiabilidade.
- Facilidade para refatoração.
- Redução de regressões.

### Desvantagens

- Maior investimento inicial.

---

# Decisão

Foi adotada a estratégia da **Pirâmide de Testes** como padrão oficial do Agilium Manager.

Os testes deverão priorizar as regras de negócio da aplicação e ser executados automaticamente durante o processo de integração contínua.

---

# Objetivos

Esta estratégia possui os seguintes objetivos:

- Garantir qualidade.
- Evitar regressões.
- Validar regras de negócio.
- Facilitar refatorações.
- Automatizar validações.
- Aumentar confiabilidade.

---

# Pirâmide de Testes

```text
             UI Tests
                ▲
                │
       Integration Tests
                ▲
                │
          Unit Tests
```

A maior parte da cobertura deverá estar concentrada em **Testes Unitários**.

---

# Tipos de Testes

## Testes Unitários

Responsáveis por validar:

- Regras de negócio;
- Services;
- Validadores;
- Objetos de domínio;
- Casos de uso.

Características:

- Rápidos;
- Isolados;
- Sem acesso ao banco;
- Sem acesso à rede.

---

## Testes de Integração

Responsáveis por validar:

- APIs;
- Banco de Dados;
- Entity Framework Core;
- Repositories;
- Middleware;
- Autenticação;
- Integrações internas.

Sempre que possível utilizar banco temporário ou ambiente isolado.

---

## Testes End-to-End (E2E)

Responsáveis por validar o fluxo completo do sistema.

Exemplos:

- Login;
- Cadastro;
- Venda;
- Emissão de pedidos;
- Fluxo financeiro.

Esses testes deverão representar cenários reais do usuário.

---

# Cobertura

Prioridades:

1. Regras de negócio.
2. Casos de uso.
3. Validações.
4. Autenticação.
5. Autorização.
6. Integrações.
7. Fluxos críticos.

Não é obrigatório atingir 100% de cobertura.

O objetivo é priorizar funcionalidades críticas.

---

# Ferramentas

Ferramentas recomendadas:

| Finalidade | Ferramenta |
|------------|------------|
| Testes Unitários | xUnit |
| Mock | Moq |
| Assertions | FluentAssertions |
| Cobertura | Coverlet |
| Integração | ASP.NET Core TestHost / WebApplicationFactory |
| Banco temporário | SQLite In-Memory |

---

# Organização

Estrutura recomendada:

```text
tests/

├── Agilium.Manager.UnitTests/

│   ├── Services/

│   ├── Domain/

│   ├── Validators/

│   └── ...

│

├── Agilium.Manager.IntegrationTests/

│   ├── Controllers/

│   ├── Repository/

│   └── ...

│

└── Agilium.Manager.E2ETests/
```

---

# Mocking

Dependências externas deverão ser simuladas utilizando mocks.

Exemplos:

- Repositories;
- APIs externas;
- Serviços SMTP;
- Cache;
- Integrações.

Não utilizar mocks para validar regras do domínio.

---

# Banco de Dados

Testes Unitários:

- Nunca acessar banco.

Testes de Integração:

- Utilizar banco temporário.
- Popular apenas dados necessários.

---

# Integração Contínua

Todo Pull Request deverá executar automaticamente:

1. Build.
2. Testes Unitários.
3. Testes de Integração.
4. Análise de cobertura.
5. Publicação de artefatos.

O merge somente deverá ocorrer quando os testes forem aprovados.

---

# Performance

Os testes deverão possuir execução rápida.

Objetivos:

- Testes Unitários: poucos segundos.
- Testes de Integração: poucos minutos.
- Pipeline completo: tempo adequado para feedback rápido.

---

# Nomenclatura

Os métodos deverão utilizar nomes descritivos.

Exemplo:

```text
Deve_Criar_Produto_Quando_DadosForemValidos

NaoDeve_Permitir_Login_Com_Senha_Invalida

Deve_Retornar_404_Quando_Produto_Nao_Existir
```

---

# Testes Obrigatórios

Toda funcionalidade crítica deverá possuir testes para:

- Cenário de sucesso;
- Cenários de erro;
- Casos limite;
- Validações;
- Regras de negócio.

---

# Não Testar

Evitar testes de:

- Getters e setters simples;
- DTOs;
- Mapeamentos triviais;
- Código sem regra de negócio.

Priorizar testes com valor funcional.

---

# Benefícios

- Redução de regressões.
- Maior confiabilidade.
- Refatorações seguras.
- Documentação executável.
- Maior qualidade do software.
- Feedback rápido para desenvolvedores.

---

# Desvantagens

- Tempo inicial de implementação.
- Necessidade de manutenção dos testes.
- Curva de aprendizado para novas ferramentas.

---

# Riscos

Caso esta estratégia não seja seguida:

- Bugs em produção.
- Regressões frequentes.
- Refatorações arriscadas.
- Maior custo de manutenção.
- Redução da qualidade do produto.

---

# Impacto

Esta decisão impacta:

- APIs
- MVC
- Services
- Domain
- Repository
- Entity Framework Core
- DevOps
- CI/CD
- Qualidade de Software

---

# Plano de Implementação

1. Criar solução de testes.
2. Configurar xUnit.
3. Configurar Moq e FluentAssertions.
4. Implementar testes unitários das regras de negócio.
5. Criar testes de integração das APIs.
6. Integrar execução ao pipeline CI/CD.
7. Monitorar cobertura e qualidade continuamente.

---

# Critérios de Aceitação

Uma implementação é considerada aderente quando:

- Existe um projeto específico para testes.
- Regras de negócio críticas possuem testes unitários.
- APIs críticas possuem testes de integração.
- Os testes são executados automaticamente no pipeline.
- Dependências externas são simuladas quando apropriado.
- O merge é bloqueado quando houver falha nos testes.

---

# ADRs Relacionados

- ADR-0001 — Arquitetura em Camadas
- ADR-0003 — Notification Pattern
- ADR-0004 — Entity Framework Core
- ADR-0007 — Estratégia de Validação
- ADR-0011 — Service Layer
- ADR-0014 — Tratamento Global de Exceções
- ADR-0019 — Estratégia de Versionamento e Migrações de Banco de Dados

---

# Referências

- xUnit Documentation
- Microsoft — Testing ASP.NET Core Applications
- Microsoft — Integration Testing in ASP.NET Core
- Martin Fowler — Test Pyramid
- Clean Architecture — Robert C. Martin
- FluentAssertions Documentation
- Moq Documentation

---

# Histórico

| Versão | Data | Descrição |
|---------|------|-----------|
| **1.0** | **2026-07-29** | Criação da ADR definindo a estratégia oficial de testes do Agilium Manager, adotando a Pirâmide de Testes como padrão arquitetural, estabelecendo diretrizes para testes unitários, integração e end-to-end, integração com CI/CD e priorização da cobertura das regras de negócio críticas. |
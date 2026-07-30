# ADR-0013 - Estratégia de Logging e Observabilidade

| Campo | Valor |
|-------|-------|
| **Status** | Accepted |
| **Data** | 2026-07-29 |
| **Autor** | Equipe Agilium |
| **Versão** | 1.0 |

---

# Contexto

O Agilium Manager é composto por diversos módulos (MVC, APIs, Jobs, Integrações e futuros Microsserviços) que executam operações críticas como:

- Autenticação;
- Processamento de vendas;
- Gestão financeira;
- Controle de estoque;
- Integrações com sistemas externos;
- Serviços de licenciamento;
- Processamentos em segundo plano.

À medida que a plataforma cresce, torna-se essencial possuir mecanismos padronizados para registrar eventos da aplicação, diagnosticar falhas, acompanhar o comportamento do sistema e facilitar auditorias.

Era necessário definir uma estratégia única para Logging e Observabilidade em toda a plataforma.

---

# Problema

Sem uma política de logging padronizada surgem diversos problemas:

- Dificuldade para identificar falhas;
- Baixa rastreabilidade;
- Logs inconsistentes;
- Informações insuficientes para suporte;
- Tempo elevado para diagnóstico (MTTR);
- Dificuldade para monitoramento em produção.

Também foi identificado o uso de mensagens pouco padronizadas e ausência de contexto nas exceções registradas.

---

# Alternativas Consideradas

## Alternativa 1 — Logging Manual

Utilizar apenas:

```csharp
Console.WriteLine(...)
```

### Vantagens

- Simples.
- Sem dependências.

### Desvantagens

- Não estruturado.
- Difícil pesquisa.
- Não escalável.
- Não apropriado para produção.

---

## Alternativa 2 — Apenas ILogger (Escolhida)

Utilizar o sistema oficial de Logging do ASP.NET Core.

### Vantagens

- Integrado ao framework.
- Extensível.
- Suporte a múltiplos Providers.
- Compatível com Serilog, Seq, Elastic, Azure Monitor, Application Insights, OpenTelemetry e outros.

### Desvantagens

- Requer configuração inicial.

---

## Alternativa 3 — Biblioteca Proprietária

### Vantagens

- Total controle.

### Desvantagens

- Alto custo de manutenção.
- Baixa interoperabilidade.
- Não recomendada.

---

# Decisão

Foi adotado o **Microsoft.Extensions.Logging (ILogger)** como mecanismo oficial de Logging da plataforma.

Toda classe da aplicação que necessite registrar informações deverá utilizar **ILogger<T>** injetado via Dependency Injection.

É proibido utilizar:

- Console.WriteLine();
- Debug.WriteLine();
- Arquivos de log próprios;
- Frameworks de logging não homologados.

---

# Objetivos

Esta estratégia possui os seguintes objetivos:

- Padronizar logs.
- Facilitar diagnóstico.
- Melhorar rastreabilidade.
- Permitir observabilidade da aplicação.
- Facilitar monitoramento.
- Reduzir tempo de resolução de incidentes.

---

# Arquitetura

```text
Application

↓

ILogger<T>

↓

Logging Provider

↓

Arquivo / Console / Seq / Elastic / Azure

↓

Monitoramento
```

---

# Injeção de Dependência

Todo componente deverá receber o logger via construtor.

Exemplo:

```csharp
public class VendaService : IVendaService
{
    private readonly ILogger<VendaService> _logger;

    public VendaService(ILogger<VendaService> logger)
    {
        _logger = logger;
    }
}
```

---

# Níveis de Log

## Trace

Utilizado apenas para diagnóstico extremamente detalhado.

Exemplo:

- Fluxo interno.
- Variáveis temporárias.

---

## Debug

Utilizado durante desenvolvimento.

Exemplo:

- Execução de métodos.
- Valores intermediários.

Não recomendado em produção.

---

## Information

Registrar eventos importantes da aplicação.

Exemplos:

- Login realizado.
- Pedido criado.
- Venda finalizada.
- Importação concluída.

---

## Warning

Registrar situações inesperadas, porém recuperáveis.

Exemplos:

- Tentativa de acesso negada.
- Recurso inexistente.
- Timeout recuperado.
- Token próximo do vencimento.

---

## Error

Registrar falhas que impediram a execução da operação.

Exemplos:

- Erro de integração.
- Erro de banco.
- Falha de autenticação.
- Exceções tratadas.

---

## Critical

Registrar falhas que comprometem o funcionamento da aplicação.

Exemplos:

- Banco indisponível.
- Aplicação impossibilitada de iniciar.
- Perda de conectividade crítica.

---

# Estrutura das Mensagens

As mensagens deverão conter contexto suficiente.

Exemplo:

```text
Usuário {UsuarioId} realizou login na empresa {EmpresaId}.
```

Evitar:

```text
Erro.
```

Preferir:

```text
Erro ao processar venda {VendaId} para empresa {EmpresaId}.
```

---

# Logging Estruturado

Sempre utilizar placeholders.

Exemplo:

```csharp
_logger.LogInformation(
    "Venda {VendaId} realizada para cliente {ClienteId}",
    venda.Id,
    venda.ClienteId);
```

Não concatenar strings.

Evitar:

```csharp
_logger.LogInformation(
    "Venda " + venda.Id);
```

---

# Tratamento de Exceções

Sempre registrar a exceção original.

Exemplo:

```csharp
_logger.LogError(
    exception,
    "Erro ao finalizar venda {VendaId}",
    venda.Id);
```

Nunca registrar apenas:

```text
Erro desconhecido.
```

---

# Dados Sensíveis

Nunca registrar:

- Senhas;
- Tokens JWT;
- Chaves de API;
- Cartões;
- CVV;
- Dados bancários;
- Dados pessoais sensíveis.

Sempre mascarar informações quando necessário.

---

# Correlação

Cada requisição deverá possuir um identificador único (CorrelationId ou TraceId).

Esse identificador deverá estar presente em todos os logs relacionados à requisição.

Fluxo:

```text
Request

↓

CorrelationId

↓

Controller

↓

Service

↓

Repository

↓

Resposta
```

---

# Observabilidade

A estratégia deverá permitir integração futura com:

- OpenTelemetry;
- Application Insights;
- Seq;
- Elastic Stack;
- Grafana;
- Prometheus;
- Azure Monitor.

---

# Monitoramento

Eventos importantes deverão possuir métricas.

Exemplos:

- Tempo de resposta;
- Quantidade de requisições;
- Taxa de erros;
- Utilização de recursos;
- Integrações realizadas.

---

# Organização

O Logging deverá ser transversal.

Nenhuma camada deverá implementar mecanismos próprios de log.

Toda a aplicação utilizará o mesmo padrão.

---

# Benefícios

- Logs padronizados.
- Diagnóstico simplificado.
- Melhor rastreabilidade.
- Observabilidade.
- Fácil integração com ferramentas externas.
- Maior qualidade operacional.

---

# Desvantagens

- Pequeno impacto de desempenho quando configurado em níveis muito detalhados.
- Necessidade de gerenciamento da retenção dos logs.

---

# Riscos

Caso esta estratégia não seja seguida:

- Diagnóstico difícil.
- Logs inconsistentes.
- Baixa rastreabilidade.
- Maior tempo de indisponibilidade.
- Informações insuficientes para suporte.

---

# Impacto

Esta decisão impacta:

- APIs
- MVC
- Services
- Repositories
- Integrações
- Jobs
- Middleware
- Infraestrutura
- DevOps
- Monitoramento

---

# Plano de Implementação

1. Padronizar utilização do `ILogger<T>`.
2. Configurar Providers de Logging.
3. Definir níveis de log por ambiente.
4. Implementar CorrelationId em todas as requisições.
5. Revisar mensagens de log existentes.
6. Remover `Console.WriteLine()` e similares.
7. Integrar com plataforma de observabilidade quando necessário.

---

# Critérios de Aceitação

Uma implementação é considerada aderente quando:

- Todas as classes utilizam `ILogger<T>` para registro de eventos.
- Não existem chamadas a `Console.WriteLine()` ou `Debug.WriteLine()` em código de produção.
- Os logs utilizam mensagens estruturadas com placeholders.
- Exceções são registradas juntamente com seu contexto.
- Informações sensíveis não são gravadas nos logs.
- Cada requisição possui um `CorrelationId` ou `TraceId` rastreável.
- A solução pode ser integrada facilmente a provedores externos de observabilidade.

---

# ADRs Relacionados

- ADR-0001 — Arquitetura em Camadas
- ADR-0005 — Estratégia de Autenticação
- ADR-0009 — Estratégia de Dependency Injection
- ADR-0011 — Service Layer
- ADR-0012 — Estratégia de Containerização e Deploy
- ADR-0014 — Tratamento Global de Exceções
- ADR-0017 — Auditoria

---

# Referências

- Microsoft — Logging in .NET
- Microsoft — ILogger<T> Documentation
- OpenTelemetry Specification
- Twelve-Factor App — Logs
- Serilog Documentation
- Elastic Common Schema (ECS)

---

# Histórico

| Versão | Data | Descrição |
|---------|------|-----------|
| **1.0** | 2026-07-29 | Criação da ADR definindo `Microsoft.Extensions.Logging (ILogger)` como mecanismo oficial de Logging do Agilium Manager, estabelecendo diretrizes para logging estruturado, níveis de severidade, tratamento de exceções, correlação de requisições e integração com plataformas de observabilidade. |
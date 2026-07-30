# ADR-0003 - Adoção do Notification Pattern

| Campo | Valor |
|-------|-------|
| **Status** | Accepted |
| **Data** | 2026-07-29 |
| **Autor** | Equipe Agilium |
| **Versão** | 1.0 |

---

# Contexto

O Agilium Manager possui diversos módulos de negócio responsáveis por operações complexas como vendas, estoque, financeiro, fiscal, licenciamento e integrações.

Durante a execução dessas operações, diversas validações de negócio precisam ser realizadas antes da persistência dos dados.

Historicamente, sistemas .NET costumam utilizar exceções para controlar validações de negócio, fazendo com que erros esperados sejam tratados como falhas de execução.

Essa abordagem dificulta a leitura do código, reduz a previsibilidade da API e gera tratamento excessivo de exceções.

Era necessário definir uma estratégia única para comunicação de erros de negócio.

---

# Problema

Utilizar Exceptions para representar regras de negócio gera diversos problemas:

- Código difícil de entender;
- Uso excessivo de try/catch;
- Baixa performance;
- Dificuldade para retornar múltiplos erros;
- Responses inconsistentes;
- Forte acoplamento entre validação e tratamento de exceções.

Era necessário um mecanismo que permitisse acumular erros de negócio sem interromper imediatamente a execução.

---

# Alternativas Consideradas

## Alternativa 1 — Exceptions para todas as validações

### Vantagens

- Implementação simples.
- Muito utilizada em projetos pequenos.

### Desvantagens

- Exceptions utilizadas como fluxo normal.
- Alto custo de processamento.
- Não permite retornar vários erros simultaneamente.
- Responses inconsistentes.

---

## Alternativa 2 — Retorno por boolean

### Vantagens

- Implementação simples.
- Baixo custo.

### Desvantagens

- Não informa detalhes do erro.
- Pouca escalabilidade.
- Dificulta manutenção.

---

## Alternativa 3 — Notification Pattern (Escolhida)

### Vantagens

- Acumula múltiplos erros.
- Separação entre validação e exceções.
- Responses padronizados.
- Melhor legibilidade.
- Facilita integração com APIs.

### Desvantagens

- Requer infraestrutura adicional.
- Necessita disciplina para utilização.

---

# Decisão

Foi adotado o **Notification Pattern** como padrão oficial para comunicação de erros de negócio.

As validações de domínio e de aplicação deverão registrar notificações ao invés de lançar exceções para situações previstas.

Exceptions ficam reservadas para erros inesperados de infraestrutura.

---

# Objetivos

O Notification Pattern deve permitir:

- Acumular múltiplos erros.
- Centralizar mensagens.
- Padronizar respostas da API.
- Separar regras de negócio de falhas técnicas.
- Melhorar experiência do usuário.

---

# Fluxo

```text
Request

↓

Controller

↓

Service

↓

Business

↓

Validação

↓

Notification

↓

Controller

↓

Response
```

---

# Responsabilidades

## Controller

Responsável por:

- Receber a requisição.
- Consultar se existem notificações.
- Retornar o Response adequado.

Não executa validações de negócio.

---

## Service

Responsável por:

- Executar casos de uso.
- Chamar validações.
- Registrar notificações.

---

## Business

Responsável por:

- Validar regras de domínio.
- Gerar notificações quando necessário.

---

## Notification

Responsável por armazenar:

- Código da regra.
- Mensagem.
- Campo relacionado.
- Severidade.

---

# Estrutura recomendada

```text
Business/

├── Notifications/

│   ├── Notification.cs

│   ├── NotificationContext.cs

│   ├── INotificationContext.cs

│   └── NotificationType.cs
```

---

# Exemplo

Ao tentar cadastrar um produto:

Validações:

- Código obrigatório
- Descrição obrigatória
- Unidade obrigatória
- Empresa obrigatória

Todas as mensagens devem ser retornadas juntas.

Exemplo:

```json
{
    "success": false,
    "errors": [
        {
            "field": "Codigo",
            "message": "Código é obrigatório."
        },
        {
            "field": "Descricao",
            "message": "Descrição é obrigatória."
        },
        {
            "field": "Empresa",
            "message": "Empresa inválida."
        }
    ]
}
```

---

# Quando utilizar

Utilizar Notification Pattern para:

- Validações de domínio.
- Regras de negócio.
- Erros de aplicação.
- Inconsistências de dados.
- Restrições funcionais.

---

# Quando NÃO utilizar

Não utilizar Notification Pattern para:

- Erros de infraestrutura.
- Falhas de conexão.
- Timeout.
- Erros de banco.
- Erros de rede.
- Exceções inesperadas.

Nestes casos devem ser utilizadas Exceptions.

---

# Exceptions

Exceptions deverão representar apenas falhas inesperadas.

Exemplos:

- NullReferenceException
- SqlException
- IOException
- TimeoutException
- Falhas externas

Nunca devem representar regras de negócio.

---

# Benefícios

- Código mais limpo.
- Melhor separação de responsabilidades.
- Responses consistentes.
- Acúmulo de erros.
- Melhor experiência para APIs.
- Facilidade para testes.
- Redução do uso de Exceptions.

---

# Desvantagens

- Maior quantidade de classes.
- Curva de aprendizado.
- Necessidade de disciplina da equipe.

---

# Riscos

Caso o padrão não seja seguido:

- Uso excessivo de Exceptions.
- Responses inconsistentes.
- Duplicação de mensagens.
- Regras espalhadas.
- Código difícil de manter.

---

# Impacto

Esta decisão impacta diretamente:

- Business
- Services
- Controllers
- API
- MVC
- Validações
- Responses
- Testes

---

# Plano de Implementação

1. Criar infraestrutura de Notification.
2. Criar NotificationContext.
3. Adaptar Services.
4. Adaptar Controllers.
5. Padronizar Responses.
6. Remover Exceptions utilizadas para regras de negócio.
7. Atualizar documentação.

---

# Critérios de Aceitação

Uma implementação é considerada aderente quando:

- Regras de negócio não lançam Exceptions.
- Todas as validações registram Notifications.
- Responses seguem padrão único.
- Controllers consultam NotificationContext antes do retorno.
- Exceptions são utilizadas apenas para erros inesperados.

---

# ADRs Relacionados

- ADR-0001 — Arquitetura em Camadas
- ADR-0002 — Repository Pattern
- ADR-0007 — Estratégia de Validação
- ADR-0011 — Service Layer
- ADR-0014 — Tratamento Global de Exceções
- ADR-0015 — Padronização das Respostas da API

---

# Referências

- Martin Fowler — *Notification Pattern*
- Eric Evans — *Domain-Driven Design*
- Microsoft — *Exception Handling Best Practices*
- Vaughn Vernon — *Implementing Domain-Driven Design*

---

# Histórico

| Versão | Data | Descrição |
|---------|------|-----------|
| 1.0 | 2026-07-29 | Criação da ADR definindo o Notification Pattern como estratégia oficial para comunicação de erros de negócio e validações do Agilium Manager. |
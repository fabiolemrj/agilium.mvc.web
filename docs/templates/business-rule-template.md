# Business Rule Template

# Objetivo

Template padrão para documentação de regras de negócio do Agilium Manager.

Cada regra deve representar um comportamento funcional efetivamente implementado na solução.

Não documentar regras presumidas ou desejadas.

---

# BR-XXXX — [Nome da Regra]

| Campo | Valor |
|--------|-------|
| **ID** | BR-XXXX |
| **Módulo** | |
| **Funcionalidade** | |
| **Status** | Ativa / Obsoleta |
| **Criticidade** | Crítica / Alta / Média / Baixa |

---

# Objetivo

Descrever o objetivo da regra de negócio.

---

# Descrição

Explicar claramente:

- o comportamento esperado;
- a finalidade da regra;
- o motivo de sua existência.

---

# Contexto

Informar:

- quando a regra é utilizada;
- em qual fluxo da aplicação;
- quais funcionalidades dependem dela.

---

# Gatilho

Descrever em quais situações a regra é executada.

Exemplos:

- inclusão;
- alteração;
- exclusão;
- autenticação;
- fechamento de venda;
- abertura de caixa;
- integração;
- processamento automático.

---

# Fluxo

Descrever onde a regra é executada.

Exemplo:

```
Controller

↓

Service

↓

Validação

↓

Repository

↓

Banco de Dados
```

---

# Implementação

Documentar onde a regra está implementada.

| Camada | Componente |
|---------|------------|
| Controller | |
| Service | |
| Repository | |
| FluentValidation | |
| Notification Pattern | |
| Banco de Dados | |

Quando uma camada não participar da regra, indicar explicitamente.

---

# Validação

Descrever:

- validações executadas;
- mensagens retornadas;
- notificações geradas;
- tratamento de erros.

---

# Entradas

Documentar:

- parâmetros utilizados;
- ViewModels;
- entidades envolvidas.

---

# Saídas

Documentar:

- retorno esperado;
- alterações realizadas;
- notificações;
- exceções (quando aplicável).

---

# Exemplos

## Cenário Válido

Descrever um exemplo de execução correta.

---

## Cenário Inválido

Descrever um exemplo que viola a regra.

Informar também:

- comportamento esperado;
- mensagem retornada;
- notificação gerada.

---

# Exceções

Documentar situações em que a regra não é aplicada.

Caso não existam exceções, registrar explicitamente.

---

# Dependências

Relacionar componentes envolvidos.

Exemplos:

- Services
- Repositories
- AutoMapper
- FluentValidation
- Notification Pattern
- Entity Framework Core
- Dapper
- APIs externas

---

# Impacto

Informar:

- módulos afetados;
- integrações afetadas;
- banco de dados;
- interfaces;
- APIs.

---

# Limitações Conhecidas

Registrar:

- regras ainda não confirmadas;
- comportamento parcialmente conhecido;
- pontos que dependem de análise adicional do código.

---

# Documentação Relacionada

Relacionar documentos relacionados.

Exemplos:

- Fluxo da Funcionalidade
- Arquitetura
- APIs
- Banco de Dados
- Módulo relacionado

---

# Histórico

| Versão | Data | Alteração |
|---------|------|-----------|
| 1.0 | | Criação |
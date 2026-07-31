# System Mechanism Discovery Template

# Objetivo

Template padrão para documentação de mecanismos e funcionalidades transversais de sistema descobertos através de análise de código (discovery). Este template é **agnóstico de projeto** e pode ser reutilizado em qualquer sistema para documentar padrões técnicos, recursos compartilhados e comportamentos sistêmicos encontrados via engenharia reversa de código.

Diferente do template de feature (que documenta funcionalidades de negócio), este template foca em **mecanismos internos** que atravessam múltiplos módulos: sistemas de ajuda, notificações, logging, caching, validação cross-cutting, etc.

---

# [NOME_DO_MECANISMO]

| Campo | Valor |
|-------|-------|
| **Status** | Descoberto / Documentado / Refatorado / Obsoleto |
| **Tipo** | Cross-Cutting / Infraestrutura / UI / Backend / Integração |
| **Projetos Afetados** | |
| **Data do Levantamento** | YYYY-MM-DD |
| **Responsável** | |

---

# Objetivo

Descrever **o que** este mecanismo faz e **qual problema** ele resolve no sistema.

Exemplo: "Sistema de tour guiado que orienta o usuário sobre os elementos de cada tela através de popovers passo-a-passo."

---

# Escopo

## Este documento cobre

- Arquitetura do mecanismo
- Componentes envolvidos
- Fluxo de execução
- Pontos de integração
- Problemas conhecidos
- Boas práticas

## Este documento NÃO cobre

- Regras de negócio específicas de cada módulo
- Funcionalidades que apenas consomem o mecanismo

---

# Índice

- [Visão Geral](#visão-geral)
- [Arquitetura](#arquitetura)
- [Camadas e Componentes](#camadas-e-componentes)
- [Fluxo de Execução](#fluxo-de-execução)
- [Distribuição no Código](#distribuição-no-código)
- [API / Contrato de Uso](#api--contrato-de-uso)
- [Problemas Conhecidos](#problemas-conhecidos)
- [Como Adicionar/Estender](#como-adicionarestender)
- [Checklist de Implementação](#checklist-de-implementação)
- [Referência Rápida](#referência-rápida)
- [Documentação Relacionada](#documentação-relacionada)

---

# Visão Geral

Resumo executivo do mecanismo. Deve responder em 3-5 parágrafos:

1. **O que é** — definição clara e objetiva
2. **Como funciona** — princípio de funcionamento
3. **Onde está** — localização no código/projeto
4. **Quem usa** — módulos ou funcionalidades que dependem dele
5. **Estatísticas** — quantidades relevantes (ex: número de ocorrências, arquivos afetados)

> **Exemplo:** O sistema possui 287 views com o botão `btnAjuda` e 69 manipuladores JavaScript, distribuídos em 2 aplicações. Cada tela define seu próprio roteiro de passos usando a biblioteca `dknotus-tour.js` v1.2.

---

# Arquitetura

## Diagrama de Camadas

Representar as camadas do mecanismo, da interface ao núcleo:

```
┌─────────────────────────────────────────┐
│  Camada 1: [NOME]                       │
│  └─ Onde o mecanismo é acionado         │
├─────────────────────────────────────────┤
│  Camada 2: [NOME]                       │
│  └─ Lógica de configuração/parametrização│
├─────────────────────────────────────────┤
│  Camada 3: [NOME]                       │
│  └─ Engine / biblioteca core            │
└─────────────────────────────────────────┘
```

## Dependências

| Componente | Versão | Tipo | Origem |
|------------|--------|------|--------|
| [nome] | [versão] | Biblioteca Interna / Terceiros | Local / CDN / NuGet |

---

# Camadas e Componentes

## Camada 1: [NOME_DA_CAMADA_1]

### Onde está

```
[CAMINHO_DOS_ARQUIVOS]
```

### Como se manifesta

Descrever como o mecanismo aparece nesta camada. Incluir exemplos de código.

```html
<!-- Exemplo de markup/uso -->
```

### Variações

Listar variações encontradas (ex: com/sem atributo, diferentes estilos).

## Camada 2: [NOME_DA_CAMADA_2]

### Onde está

```
[CAMINHO_DOS_ARQUIVOS]
```

### Padrão de implementação

```javascript
// Exemplo de código padrão
```

### Elementos comuns

Tabela de elementos/propriedades utilizados de forma recorrente:

| Elemento | Descrição | Onde aparece |
|----------|-----------|--------------|
| [nome] | [descrição] | [contextos] |

## Camada 3: [NOME_DA_CAMADA_3]

### Origem

- Nome:
- Versão:
- Licença:
- URL:
- Arquivos:

### API / Interface pública

Documentar os principais métodos, opções e parâmetros.

| Método/Propriedade | Tipo | Descrição |
|-------------------|------|-----------|
| [nome] | [tipo] | [descrição] |

### Opções de configuração

| Opção | Tipo | Padrão | Descrição |
|-------|------|--------|-----------|
| [nome] | [tipo] | [valor] | [descrição] |

### Comportamento interno

Descrever o ciclo de vida e o comportamento interno do componente core.

---

# Fluxo de Execução

Diagrama textual do fluxo completo:

```
[Gatilho]
    │
    ▼
[Etapa 1]
    │
    ▼
[Etapa 2]
    │
    ├─ [Sub-etapa 2a]
    ├─ [Sub-etapa 2b]
    │
    ▼
[Etapa 3]
    │
    ▼
[Resultado]
```

---

# Distribuição no Código

## Por Módulo/Projeto

| Módulo | Ocorrências | Arquivos |
|--------|-------------|----------|
| [módulo] | [número] | [lista] |

## Lista completa de arquivos relevantes

```
[caminho/arquivo1]
[caminho/arquivo2]
...
```

---

# API / Contrato de Uso

Se o mecanismo expõe uma API ou contrato para outros módulos consumirem:

```javascript
// Exemplo mínimo funcional
```

```csharp
// Exemplo de uso no backend (se aplicável)
```

---

# Problemas Conhecidos

| # | Problema | Impacto | Risco | Recomendação |
|---|----------|---------|------|--------------|
| 1 | [descrição] | [impacto] | Alto/Médio/Baixo | [recomendação] |

---

# Como Adicionar/Estender

Guia passo-a-passo para adicionar o mecanismo a um novo módulo ou contexto.

## Passo 1: [NOME_DO_PASSO]

```[linguagem]
// Código de exemplo
```

## Passo 2: [NOME_DO_PASSO]

...

---

# Checklist de Implementação

- [ ] [Item de verificação 1]
- [ ] [Item de verificação 2]
- [ ] [Item de verificação 3]
- [ ] [Item de verificação 4]
- [ ] [Item de verificação 5]

---

# Referência Rápida

Resumo dos principais comandos, métodos ou snippets para uso rápido por agentes de IA:

```[linguagem]
// Inicialização
// Configuração
// Uso básico
// Tratamento de erros
// Limpeza
```

---

# Documentação Relacionada

- [link] — [descrição]
- [link] — [descrição]

---

# Notas para Agentes de IA

Este documento segue o padrão de **System Mechanism Discovery**. Ao gerar um documento similar para outro mecanismo:

1. Substitua todos os placeholders `[NOME_DO_MECANISMO]`, `[CAMINHO]` etc.
2. Mantenha a estrutura de camadas (se o mecanismo tiver menos camadas, simplifique)
3. Priorize exemplos de código reais extraídos do projeto
4. Inclua estatísticas de ocorrências sempre que possível
5. Liste problemas conhecidos — isso é essencial para manutenção futura
6. A seção "Como Adicionar/Estender" deve ser um guia prático, não teórico
7. A "Referência Rápida" deve ser auto-contida — um agente deve conseguir implementar sem ler o resto do documento

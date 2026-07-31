# Prompt: System Mechanism Discovery

# Objetivo

Template de prompt para agentes de IA realizarem **levantamento e documentação** de mecanismos internos e funcionalidades transversais de qualquer sistema. Este prompt é agnóstico de projeto e pode ser parametrizado para diferentes contextos.

O resultado deve seguir o template `system-mechanism-discovery.md`.

---

# Quando utilizar

Utilize este prompt para:

- Levantar o funcionamento de um mecanismo transversal do sistema
- Entender como um recurso compartilhado funciona (ex: sistema de ajuda, notificações, logging)
- Documentar padrões técnicos implícitos encontrados via análise de código
- Produzir documentação de referência para agentes de IA
- Criar guias de implementação para mecanismos existentes

**Não utilize** para:
- Funcionalidades de negócio específicas de um módulo (use o prompt `levantamento.md` ou `new-feature.md`)
- Correções de bugs (use `bug-fix.md`)
- Refatorações (use `refactoring.md`)

---

# Prompt Parametrizável

```text
Realize um levantamento completo do seguinte mecanismo do sistema:

Mecanismo: [NOME_DO_MECANISMO] (ex: "botões de id 'btnAjuda'", "sistema de notificações toastr")

Escopo da busca: [PALAVRAS_CHAVE_PARA_BUSCA] (ex: "btnAjuda", "Toastr", "notification")

Tipos de arquivo a analisar: [EXTENSÕES] (ex: .cshtml, .js, .cs, .css)

Projetos a considerar: [PASTAS_DOS_PROJETOS] (ex: agilium-manager-azure-web/, agilum.mvc.web/)

---

## Instruções para o Agente

### 1. Discovery — Varredura Inicial

Realize buscas textuais pelas palavras-chave em todos os tipos de arquivo relevantes. Para cada ocorrência, classifique:

- **O QUE** é (HTML, JS, C#, CSS, configuração)
- **ONDE** está (caminho do arquivo)
- **COMO** se relaciona com as demais ocorrências

### 2. Classificação por Camada

Agrupe as ocorrências em camadas arquiteturais:

- **Camada de Apresentação** — Views (.cshtml), componentes visuais, atributos HTML
- **Camada de Comportamento** — JavaScript (.js), eventos, handlers, AJAX
- **Camada de Serviço** — Controllers, Services, APIs (.cs)
- **Camada de Dados** — Models, Entities, Repositories, migrations
- **Camada de Infraestrutura** — Configurações, middlewares, bibliotecas de terceiros

### 3. Análise do Fluxo de Execução

Trace o caminho completo de uma interação com o mecanismo:

1. Onde o mecanismo é **acionado** (gatilho)
2. Quais componentes são **ativados** em sequência
3. Como os dados **fluem** entre as camadas
4. Qual é o **resultado final** observável
5. Como o mecanismo é **finalizado** (cleanup)

### 4. Identificação de Padrões

Identifique:

- **Padrão comum** de implementação que se repete
- **Variações** encontradas (ex: com/sem atributo, diferentes configurações)
- **Anti-padrões** (ex: copy-paste sem personalização, ID duplicado)
- **Dependências** entre componentes

### 5. Documentação da API/Contrato

Se o mecanismo expõe uma API pública:

- Liste todos os métodos/propriedades públicas
- Documente parâmetros, tipos e valores padrão
- Forneça exemplos de uso mínimo funcional
- Documente callbacks e hooks de extensão

### 6. Levantamento Estatístico

Quantifique:

- Número total de ocorrências
- Distribuição por módulo/projeto
- Arquivos envolvidos (listar caminhos completos)
- Cobertura (ex: X% das views possuem o mecanismo)

### 7. Problemas e Riscos

Liste problemas conhecidos com:

| Campo | Descrição |
|-------|-----------|
| Problema | Descrição clara |
| Causa | Por que acontece |
| Impacto | O que é afetado |
| Risco | Alto / Médio / Baixo |
| Recomendação | Como mitigar ou corrigir |

### 8. Checklist de Extensão

Produza um checklist prático para um desenvolvedor/agente adicionar o mecanismo a um novo contexto:

- [ ] Passo 1: ...
- [ ] Passo 2: ...
- [ ] Passo 3: ...

### 9. Referência Rápida

Forneça um bloco de código auto-contido que sirva como "quick start" para o mecanismo:

```
// Tudo que um agente precisa para implementar o mecanismo
// em um novo contexto, sem ler o documento inteiro
```

---

## Formato de Saída

Produza o resultado no formato do template `docs/templates/system-mechanism-discovery.md`, que contém:

1. Metadados (status, tipo, projetos, data)
2. Objetivo
3. Escopo
4. Visão Geral
5. Arquitetura (camadas, dependências)
6. Camadas e Componentes (detalhamento por camada)
7. Fluxo de Execução
8. Distribuição no Código
9. API / Contrato de Uso
10. Problemas Conhecidos
11. Como Adicionar/Estender
12. Checklist de Implementação
13. Referência Rápida
14. Documentação Relacionada

---

## Validação

Antes de finalizar, verifique:

- [ ] Todas as camadas estão documentadas
- [ ] O fluxo de execução está completo (do gatilho ao cleanup)
- [ ] Estatísticas de ocorrências estão presentes
- [ ] Pelo menos 3 problemas conhecidos estão listados
- [ ] O checklist de extensão é prático (passos acionáveis)
- [ ] A referência rápida é auto-contida e funcional
- [ ] Caminhos de arquivos estão corretos (use paths absolutos ou relativos à raiz)
- [ ] Exemplos de código são reais (extraídos do código, não inventados)
```

---

# Exemplo de Uso

## Entrada

```text
Mecanismo: Sistema de ajuda guiada (botões id="btnAjuda")
Escopo da busca: btnAjuda, Tour.run, dknotus-tour
Tipos de arquivo: .cshtml, .js, .css, .min.js
Projetos: agilium-manager-azure-web/, agilum.mvc.web/
```

## Saída Esperada

Documento seguindo o template `system-mechanism-discovery.md` preenchido com:

- 3 camadas (HTML → JS → dknotus-tour.js)
- Fluxo: clique → `$('#btnAjuda').click()` → `Tour.run([...])` → renderização de popovers
- ~287 ocorrências em .cshtml, ~69 handlers .js
- 6 problemas conhecidos
- Checklist com 4 passos para adicionar ajuda a nova página
- Referência rápida com API completa do `Tour.run()`
- Distribuição por 8 módulos

---

# Parametrização para Outros Projetos

Para reutilizar este prompt em outro projeto, ajuste:

| Parâmetro | Exemplo | Como definir |
|-----------|---------|--------------|
| `NOME_DO_MECANISMO` | "Sistema de cache com Redis" | Nome descritivo do recurso |
| `PALAVRAS_CHAVE` | "Redis", "IDistributedCache", "CacheService" | Termos exatos encontrados no código |
| `EXTENSÕES` | .cs, .json, .config | Extensões relevantes ao stack do projeto |
| `PASTAS_DOS_PROJETOS` | src/Api/, src/Web/ | Pastas raiz dos projetos |
| `CAMADA_APRESENTACAO` | React, Angular, MVC, Blazor | Framework de UI do projeto |
| `CAMADA_COMPORTAMENTO` | TypeScript, JavaScript, WASM | Linguagem de frontend |
| `CAMADA_SERVICO` | .NET, Node, Java, Python | Stack de backend |
| `CAMADA_DADOS` | EF Core, Prisma, SQLAlchemy, MongoDB | ORM/banco |

---

# Dicas para Agentes de IA

1. **Comece pelas buscas** — use grep/search pelas palavras-chave antes de ler arquivos
2. **Agrupe por similaridade** — arquivos com mesmo padrão podem ser resumidos juntos
3. **Leia o núcleo primeiro** — a biblioteca/engine central contém a lógica mais importante
4. **Quantifique** — números de ocorrências ajudam a dimensionar o impacto
5. **Não assuma** — se um padrão existe em 50 arquivos mas não em 10, documente a exceção
6. **Pense no próximo agente** — a documentação será lida por outra IA; seja preciso e estruturado

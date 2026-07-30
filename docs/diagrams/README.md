# Diagramas

## Objetivo

Esta pasta contém os diagramas arquiteturais do **Agilium Manager**, representando visualmente a estrutura do sistema, fluxos de operação, comunicação entre componentes e arquitetura de implantação.

## Formatos

Os diagramas podem ser representados em:

- **Mermaid** (código renderizado em markdown)
- **PlantUML** (`.puml`)
- **Imagens** (`.png`, `.svg`) exportadas de ferramentas como draw.io, Lucidchart, Excalidraw
- **Referências** para arquivos externos

## Índice de Diagramas

| # | Arquivo | Tipo | Descrição |
|---|---------|------|-----------|
| 1 | [arquitetura-geral.md](./arquitetura-geral.md) | C4 / Blocos | Visão geral da arquitetura do sistema |
| 2 | [c4-model.md](./c4-model.md) | C4 Model | Modelo C4 (Context, Container, Component, Code) |
| 3 | [mvc.md](./mvc.md) | Camadas | Arquitetura MVC e separação em camadas |
| 4 | [request-pipeline.md](./request-pipeline.md) | Sequência | Pipeline de requisição HTTP |
| 5 | [autenticacao.md](./autenticacao.md) | Sequência | Fluxo de autenticação e autorização |
| 6 | [dependency-injection.md](./dependency-injection.md) | Componentes | Grafo de injeção de dependências |
| 7 | [persistencia.md](./persistencia.md) | Componentes | Camada de persistência (EF Core + Dapper + MongoDB) |
| 8 | [banco-de-dados.md](./banco-de-dados.md) | ER / Esquema | Modelo de banco de dados |
| 9 | [componentes.md](./componentes.md) | Componentes | Principais componentes do sistema |
| 10 | [integracoes.md](./integracoes.md) | Contexto | Integrações externas |
| 11 | [frontend.md](./frontend.md) | Componentes | Arquitetura do frontend (MVC + AdminLTE) |
| 12 | [backend.md](./backend.md) | Componentes | Arquitetura do backend (API + Services) |
| 13 | [fluxo-venda.md](./fluxo-venda.md) | Sequência | Fluxo completo de uma venda |
| 14 | [fluxo-login.md](./fluxo-login.md) | Sequência | Fluxo de login e autenticação |
| 15 | [fluxo-caixa.md](./fluxo-caixa.md) | Sequência | Fluxo de abertura e fechamento de caixa |
| 16 | [sequence.md](./sequence.md) | Sequência | Diagramas de sequência gerais |
| 17 | [deployment.md](./deployment.md) | Implantação | Arquitetura de deployment (Docker, Azure, Render) |
| 18 | [infraestrutura.md](./infraestrutura.md) | Infraestrutura | Infraestrutura de servidores e serviços |

## Convenções

- Usar **Mermaid** para diagramas versionáveis em texto
- Nomes de componentes em **português** (refletindo o domínio)
- Referenciar nomes reais de classes e projetos
- Manter diagramas atualizados com a implementação

## Ferramentas Recomendadas

- [Mermaid Live Editor](https://mermaid.live/)
- [PlantUML](https://plantuml.com/)
- [draw.io](https://app.diagrams.net/)
- [Excalidraw](https://excalidraw.com/)

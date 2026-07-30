# Processo de Build

# Objetivo

Documentar o processo de compilação da solução Agilium Manager, estabelecendo as diretrizes para restauração de dependências, compilação, geração de artefatos e validação antes da publicação.

Este documento define como os projetos da solução devem ser compilados em ambientes de desenvolvimento e integração contínua.

---

# Escopo

Este documento contempla:

- Processo de Build
- Pré-requisitos
- Configurações de Compilação
- Build Local
- Geração de Artefatos
- Validações
- Boas Práticas

---

# Índice

- Visão Geral
- Pré-requisitos
- Processo de Build
- Configurações de Build
- Build Local
- Artefatos Gerados
- Validações
- Boas Práticas
- Limitações Conhecidas
- Atualização
- Documentação Relacionada

---

# Visão Geral

O processo de build é responsável por transformar o código-fonte da solução em artefatos executáveis ou bibliotecas reutilizáveis.

Todo processo de compilação deve garantir:

- consistência da solução;
- restauração correta das dependências;
- compilação sem erros;
- geração dos artefatos esperados;
- rastreabilidade da versão compilada.

---

# Pré-requisitos

Antes de executar um build, recomenda-se verificar:

- SDK .NET compatível com a solução;
- dependências restauradas;
- ambiente configurado;
- acesso aos recursos necessários para compilação.

Os requisitos detalhados encontram-se em:

- development/environment.md
- development/getting-started.md

---

# Processo de Build

O fluxo conceitual de compilação é:

```text
Restore

↓

Compile

↓

Executar Testes (quando existentes)

↓

Gerar Artefatos

↓

Disponibilizar para Publicação
```

As etapas efetivamente executadas dependerão da estratégia adotada pela solução.

---

# Configurações de Build

As configurações normalmente utilizadas são:

## Debug

Destinada ao desenvolvimento local.

Características:

- símbolos de depuração;
- maior detalhamento para diagnóstico;
- otimizações reduzidas.

---

## Release

Destinada à publicação.

Características:

- otimizações habilitadas;
- geração de artefatos para distribuição;
- maior desempenho.

---

# Build Local

O procedimento de compilação deve seguir as etapas abaixo:

1. Restaurar dependências.
2. Compilar a solução.
3. Corrigir eventuais erros de compilação.
4. Executar os testes disponíveis (quando aplicável).
5. Validar os artefatos gerados.

Os comandos específicos deverão refletir a estrutura oficial da solução e ser documentados após validação.

---

# Artefatos Gerados

Cada projeto deverá documentar os artefatos produzidos.

Exemplo:

| Projeto | Tipo de Artefato | Destino |
|----------|------------------|----------|
| MVC | Aplicação Web | Publicação |
| API | Serviço REST | Publicação |
| Business | Biblioteca | Consumo interno |

---

# Validações

Antes de considerar um build concluído, recomenda-se validar:

- compilação sem erros;
- dependências restauradas;
- testes executados (quando existentes);
- geração correta dos artefatos;
- versão identificada adequadamente.

---

# Boas Práticas

Sempre:

- manter a solução compilando sem erros;
- utilizar configurações adequadas para cada ambiente;
- automatizar o processo sempre que possível;
- registrar falhas de compilação;
- manter o processo reproduzível.

Evitar:

- builds manuais diretamente em ambientes de produção;
- dependências não documentadas;
- diferenças entre build local e automatizado;
- alterações não rastreadas durante a compilação.

---

# Limitações Conhecidas

O levantamento técnico confirmou:

- utilização da plataforma .NET;
- organização da solução em múltiplos projetos.

Ainda deverão ser confirmados durante a análise dos projetos:

- `agilium-manager-azure-api`;
- `agilium-manager-azure-business`;
- `agilium-manager-git-azure-infra`;
- `agilium-pdv-azure-api`;

os seguintes aspectos:

- comandos oficiais de build;
- estratégia de publicação;
- utilização de Docker;
- pipeline de integração contínua;
- geração de artefatos;
- ferramentas de automação utilizadas.

---

# Atualização

Este documento deve ser revisado sempre que ocorrer:

- alteração da estrutura da solução;
- mudança na estratégia de compilação;
- inclusão de novos projetos;
- adoção de novas ferramentas de build;
- evolução do processo de entrega.

---

# Documentação Relacionada

## Desenvolvimento

- development/getting-started.md
- development/environment.md
- development/testing.md
- development/release-process.md

## Arquitetura

- architecture/overview.md
- architecture/deployment.md

## Infraestrutura

- infrastructure/ci-cd.md
- infrastructure/deployment.md
- infrastructure/docker.md
# Ambiente de Desenvolvimento

# Objetivo

Documentar os requisitos, configurações e boas práticas para preparação do ambiente de desenvolvimento do Agilium Manager.

Este documento define as diretrizes para que todos os desenvolvedores utilizem um ambiente consistente e reproduzível.

---

# Escopo

Este documento contempla:

- Configuração do Ambiente
- Ferramentas de Desenvolvimento
- Configuração da IDE
- Variáveis de Ambiente
- Gerenciamento de Configurações
- Dependências
- Boas Práticas

---

# Índice

- Visão Geral
- Requisitos do Ambiente
- Ferramentas
- Configuração da IDE
- Configuração da Aplicação
- Variáveis de Ambiente
- Gerenciamento de Configurações
- Organização do Ambiente
- Boas Práticas
- Limitações Conhecidas
- Atualização
- Documentação Relacionada

---

# Visão Geral

O ambiente de desenvolvimento deve fornecer todos os recursos necessários para que a solução possa ser compilada, executada, depurada e evoluída de maneira consistente.

A configuração do ambiente deve minimizar diferenças entre os desenvolvedores e facilitar a manutenção da aplicação.

---

# Requisitos do Ambiente

Antes de iniciar o desenvolvimento, recomenda-se verificar:

- SDK .NET compatível com a solução;
- Git instalado;
- IDE compatível;
- acesso ao repositório;
- acesso aos recursos necessários para desenvolvimento.

Os requisitos oficiais deverão ser mantidos atualizados conforme a evolução da plataforma.

---

# Ferramentas

O ambiente poderá utilizar diferentes ferramentas de apoio ao desenvolvimento, tais como:

- IDE compatível com .NET;
- cliente Git;
- ferramenta para testes de APIs;
- cliente de banco de dados;
- ferramentas de inspeção de logs;
- ferramentas de diagnóstico.

As ferramentas oficialmente adotadas deverão ser documentadas após validação da arquitetura e da infraestrutura.

---

# Configuração da IDE

A IDE utilizada deve permitir:

- compilação da solução;
- depuração;
- gerenciamento de projetos;
- execução de testes (quando disponíveis);
- navegação entre projetos;
- integração com controle de versão.

Configurações específicas da IDE deverão ser documentadas quando forem padronizadas pela equipe.

---

# Configuração da Aplicação

A configuração local poderá incluir:

- arquivos de configuração;
- parâmetros específicos do ambiente;
- credenciais de desenvolvimento;
- certificados;
- configurações de conexão;
- dependências externas.

Os procedimentos detalhados devem ser documentados conforme a implementação da solução.

---

# Variáveis de Ambiente

Sempre que possível, configurações específicas do ambiente devem ser externalizadas.

Exemplos de categorias:

- conexões com banco de dados;
- credenciais de serviços;
- parâmetros de execução;
- integrações externas;
- configurações específicas do ambiente.

A lista oficial de variáveis deverá ser documentada após validação da infraestrutura.

---

# Gerenciamento de Configurações

As configurações da aplicação devem:

- ser separadas por ambiente;
- evitar armazenamento de informações sensíveis no código-fonte;
- possuir documentação atualizada;
- manter consistência entre os ambientes.

A estratégia utilizada (arquivos de configuração, variáveis de ambiente, gerenciamento de segredos etc.) deverá refletir a implementação oficial da solução.

---

# Organização do Ambiente

Recomenda-se manter:

- ferramentas atualizadas;
- versões compatíveis entre os desenvolvedores;
- configuração padronizada;
- documentação sincronizada com o ambiente.

Mudanças significativas no ambiente devem ser registradas e comunicadas à equipe.

---

# Boas Práticas

Sempre:

- utilizar configurações específicas por ambiente;
- manter informações sensíveis fora do código-fonte;
- documentar dependências necessárias;
- revisar configurações antes de compartilhar alterações;
- manter o ambiente atualizado.

Evitar:

- credenciais fixas no código;
- diferenças não documentadas entre ambientes;
- dependências instaladas sem documentação;
- alterações locais que não possam ser reproduzidas por outros desenvolvedores.

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

- IDE oficialmente recomendada;
- ferramentas obrigatórias de desenvolvimento;
- utilização de User Secrets;
- lista oficial de variáveis de ambiente;
- estratégia de gerenciamento de segredos;
- ferramentas de banco de dados;
- ferramentas para testes de APIs;
- utilização de Docker no ambiente local.

---

# Atualização

Este documento deve ser revisado sempre que ocorrer:

- alteração das ferramentas utilizadas;
- atualização da plataforma .NET;
- mudança na estratégia de configuração;
- inclusão de novas dependências;
- alteração do processo de desenvolvimento.

---

# Documentação Relacionada

## Desenvolvimento

- development/getting-started.md
- development/build.md
- development/debugging.md
- development/testing.md

## Arquitetura

- architecture/overview.md
- architecture/layers.md

## Infraestrutura

- infrastructure/environments.md
- infrastructure/docker.md
- infrastructure/ci-cd.md

## Segurança

- security/authentication.md
- security/authorization.md
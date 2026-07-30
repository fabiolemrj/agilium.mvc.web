# Primeiros Passos

# Objetivo

Orientar novos desenvolvedores na preparação do ambiente de desenvolvimento do Agilium Manager, apresentando o fluxo inicial de instalação, configuração e execução da solução.

Este documento funciona como ponto de entrada para toda a documentação técnica do projeto.

---

# Escopo

Este documento contempla:

- Visão Geral
- Pré-requisitos
- Obtenção do Código-Fonte
- Configuração Inicial
- Compilação
- Execução Local
- Estrutura da Solução
- Próximos Passos

---

# Índice

- Visão Geral
- Pré-requisitos
- Obtenção do Código
- Configuração do Ambiente
- Compilação
- Execução Local
- Estrutura da Solução
- Documentação Recomendada
- Limitações Conhecidas
- Atualização
- Documentação Relacionada

---

# Visão Geral

O Agilium Manager é composto por múltiplos projetos organizados em camadas, seguindo uma arquitetura modular.

Antes de iniciar o desenvolvimento, recomenda-se configurar corretamente o ambiente e compreender a organização geral da solução.

---

# Pré-requisitos

Antes de compilar a solução, verifique a disponibilidade dos seguintes recursos:

| Recurso | Observação |
|----------|------------|
| SDK .NET compatível | Conforme a versão utilizada pela solução |
| IDE compatível | Visual Studio ou outra IDE compatível com .NET |
| Git | Controle de versão |
| Banco de dados | Conforme a infraestrutura da aplicação |

As versões oficiais deverão ser confirmadas na documentação de ambiente.

---

# Obtenção do Código

Clonar o repositório oficial da solução:

```bash
git clone <repositorio>

cd <diretorio-da-solucao>
```

Após obter o código-fonte, restaurar as dependências e realizar a compilação conforme documentado em `development/build.md`.

---

# Configuração do Ambiente

A configuração inicial poderá incluir:

- variáveis de ambiente;
- configurações de conexão;
- credenciais de desenvolvimento;
- certificados;
- dependências externas.

Os detalhes encontram-se em:

- development/environment.md

---

# Compilação

Após configurar o ambiente:

1. Restaurar as dependências.
2. Compilar a solução.
3. Corrigir eventuais dependências ausentes.
4. Validar que todos os projetos foram compilados.

Os comandos específicos encontram-se em:

- development/build.md

---

# Execução Local

A execução da aplicação dependerá da configuração da solução.

Antes da primeira execução, verificar:

- ambiente configurado;
- banco de dados disponível;
- dependências externas acessíveis;
- configurações da aplicação válidas.

Os procedimentos específicos deverão ser documentados conforme a implementação da solução.

---

# Estrutura da Solução

De forma conceitual, a solução é organizada em projetos especializados.

```text
Solution

├── MVC
├── API
├── Business
├── Domain
├── Infrastructure
├── Shared
└── Tests (quando existentes)
```

A estrutura definitiva deverá refletir a organização da solução.

---

# Documentação Recomendada

Após concluir a configuração inicial, recomenda-se a leitura dos seguintes documentos:

## Arquitetura

- architecture/overview.md
- architecture/layers.md
- architecture/solution-structure.md

## Desenvolvimento

- development/environment.md
- development/build.md
- development/debugging.md
- development/coding-standards.md

## Banco de Dados

- database/overview.md
- database/entities.md

## Segurança

- security/authentication.md
- security/authorization.md

---

# Limitações Conhecidas

O levantamento técnico confirmou:

- utilização da plataforma .NET;
- organização da solução em múltiplos projetos;
- arquitetura em camadas.

Ainda deverão ser confirmados durante a análise dos projetos:

- `agilium-manager-azure-api`;
- `agilium-manager-azure-business`;
- `agilium-manager-git-azure-infra`;
- `agilium-pdv-azure-api`;

os seguintes aspectos:

- versão oficial do SDK .NET;
- banco de dados utilizado;
- procedimento oficial de configuração do ambiente;
- comandos de execução;
- variáveis de ambiente obrigatórias;
- utilização de Docker;
- projetos de testes;
- estratégia de autenticação;
- portas utilizadas pela aplicação.

---

# Atualização

Este documento deve ser revisado sempre que ocorrer:

- alteração da estrutura da solução;
- mudança no processo de instalação;
- atualização da plataforma;
- inclusão de novos projetos;
- alteração dos requisitos de desenvolvimento.

---

# Documentação Relacionada

## Desenvolvimento

- development/environment.md
- development/build.md
- development/debugging.md

## Arquitetura

- architecture/overview.md
- architecture/layers.md
- architecture/solution-structure.md

## Banco de Dados

- database/overview.md

## Segurança

- security/authentication.md
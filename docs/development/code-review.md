# Revisão de Código

# Objetivo

Documentar o processo de revisão de código do Agilium Manager, estabelecendo critérios para garantir qualidade, consistência arquitetural, manutenibilidade e conformidade com os padrões definidos para a solução.

Este documento descreve as diretrizes utilizadas durante a revisão técnica das alterações antes de sua integração ao código principal.

---

# Escopo

Este documento contempla:

- Objetivos da Revisão
- Fluxo de Revisão
- Checklist Técnico
- Critérios de Aprovação
- Critérios de Rejeição
- Boas Práticas
- Governança

---

# Índice

- Visão Geral
- Objetivos
- Fluxo de Revisão
- Checklist
- Critérios de Aprovação
- Critérios de Rejeição
- Comentários de Revisão
- Boas Práticas
- Limitações Conhecidas
- Atualização
- Documentação Relacionada

---

# Visão Geral

A revisão de código é uma etapa essencial do processo de desenvolvimento.

Seu objetivo é garantir que alterações sejam incorporadas à solução mantendo:

- qualidade;
- legibilidade;
- consistência arquitetural;
- conformidade com padrões;
- baixo acoplamento;
- facilidade de manutenção.

A revisão deve concentrar-se na qualidade técnica da implementação, e não apenas na funcionalidade entregue.

---

# Objetivos

Toda revisão deve verificar:

- aderência à arquitetura;
- cumprimento das regras de negócio;
- qualidade do código;
- impacto sobre componentes existentes;
- riscos técnicos;
- documentação atualizada quando necessária.

---

# Fluxo de Revisão

Fluxo conceitual:

```text
Desenvolvimento

↓

Autoavaliação

↓

Solicitação de Revisão

↓

Revisão Técnica

↓

Correções

↓

Nova Revisão (quando necessário)

↓

Aprovação

↓

Integração
```

As etapas específicas dependerão da ferramenta de gerenciamento de código utilizada.

---

# Checklist

Durante a revisão recomenda-se verificar os seguintes aspectos.

## Arquitetura

- O código respeita a arquitetura em camadas?
- Há separação adequada de responsabilidades?
- Não foram introduzidas dependências indevidas entre camadas?

---

## Regras de Negócio

- As regras foram implementadas na camada correta?
- Não há lógica de negócio em Controllers ou Views?
- As validações seguem os padrões definidos?

---

## Qualidade do Código

- O código é legível?
- Os nomes são claros e consistentes?
- Existe duplicação desnecessária?
- O código segue os padrões estabelecidos?

---

## Persistência

- Consultas são eficientes?
- Alterações no modelo de dados estão documentadas?
- Mudanças em entidades e relacionamentos foram avaliadas?

---

## Segurança

- Dados sensíveis estão protegidos?
- Há validação adequada de entrada?
- O controle de acesso foi respeitado?

---

## Testes

Quando aplicável, verificar:

- existência de testes;
- atualização de testes existentes;
- impacto sobre funcionalidades já implementadas.

---

## Documentação

Verificar se houve necessidade de atualizar:

- documentação arquitetural;
- documentação funcional;
- ADRs;
- documentação de APIs;
- documentação de banco de dados.

---

# Critérios de Aprovação

Uma alteração poderá ser aprovada quando:

- estiver compilando corretamente;
- respeitar os padrões arquiteturais;
- não introduzir regressões conhecidas;
- possuir documentação atualizada quando necessário;
- atender aos requisitos definidos para a funcionalidade.

---

# Critérios de Rejeição

Uma revisão poderá solicitar ajustes quando identificar, por exemplo:

- violações arquiteturais;
- duplicação significativa de código;
- regras de negócio implementadas em camadas inadequadas;
- baixa legibilidade;
- riscos de segurança;
- documentação inconsistente;
- impacto não avaliado sobre componentes existentes.

---

# Comentários de Revisão

Sempre que possível, os comentários devem:

- explicar claramente o problema identificado;
- justificar a sugestão;
- indicar alternativas quando apropriado;
- manter linguagem objetiva e respeitosa.

Evitar comentários sem contexto ou apenas apontamentos genéricos.

---

# Boas Práticas

Sempre:

- revisar alterações com foco técnico;
- analisar o impacto da mudança na arquitetura;
- incentivar melhorias de qualidade;
- registrar decisões relevantes;
- manter a revisão colaborativa.

Evitar:

- aprovações sem análise;
- revisões focadas apenas em estilo;
- comentários sem justificativa;
- alterações adicionais não relacionadas ao objetivo da revisão.

---

# Limitações Conhecidas

O levantamento técnico confirmou:

- utilização de uma solução baseada em .NET;
- organização em múltiplos projetos;
- arquitetura em camadas.

Ainda deverão ser confirmados durante a análise dos projetos:

- `agilium-manager-azure-api`;
- `agilium-manager-azure-business`;
- `agilium-manager-git-azure-infra`;
- `agilium-pdv-azure-api`;

os seguintes aspectos:

- ferramenta oficial de revisão de código;
- fluxo de Pull Requests ou Merge Requests;
- quantidade mínima de revisores;
- política de aprovação;
- automação de verificações;
- checklist oficial da equipe.

---

# Atualização

Este documento deve ser revisado sempre que ocorrer:

- alteração do processo de revisão;
- adoção de novas ferramentas;
- atualização dos padrões arquiteturais;
- alteração dos critérios de qualidade.

---

# Documentação Relacionada

## Desenvolvimento

- development/testing.md
- development/build.md
- development/release-process.md
- development/coding-standards.md

## Arquitetura

- architecture/overview.md
- architecture/layers.md
- architecture/decisions/

## Banco de Dados

- database/overview.md

## Segurança

- security/authorization.md
# Architecture Template

# Objetivo

Template padrão para criação de documentos arquiteturais do Agilium Manager.

Este template deve ser utilizado para documentar componentes, módulos, padrões, fluxos e decisões arquiteturais da solução.

Toda informação documentada deve ser baseada exclusivamente na implementação existente e no levantamento técnico do projeto.

---

# [Título do Documento]

## Objetivo

Descreva, em uma ou duas frases, o propósito do documento e o componente arquitetural abordado.

---

## Escopo

Informar claramente:

### Este documento cobre

- ...

### Este documento NÃO cobre

- ...

---

## Índice

- [Visão Geral](#visão-geral)
- [Arquitetura](#arquitetura)
- [Componentes](#componentes)
- [Fluxo](#fluxo)
- [Responsabilidades](#responsabilidades)
- [Integrações](#integrações)
- [Boas Práticas](#boas-práticas)
- [Limitações Conhecidas](#limitações-conhecidas)
- [Atualização](#atualização)
- [Documentação Relacionada](#documentação-relacionada)

---

# Visão Geral

Apresentar uma visão geral do componente documentado.

Descrever:

- finalidade;
- contexto na solução;
- responsabilidades;
- relacionamento com outros módulos.

---

# Arquitetura

Descrever a arquitetura envolvida.

Quando aplicável informar:

- camada da solução;
- padrão arquitetural;
- componentes principais;
- dependências;
- tecnologias utilizadas.

Utilizar diagramas textuais sempre que agregarem clareza.

Exemplo:

```
Controller
        │
        ▼
Business
        │
        ▼
Repository
        │
        ▼
Banco de Dados
```

---

# Componentes

Listar os principais componentes envolvidos.

| Componente | Responsabilidade | Dependências |
|------------|------------------|--------------|
| | | |

---

# Fluxo

Descrever o fluxo completo da funcionalidade.

Exemplo:

```
Usuário

↓

Controller

↓

Service

↓

Repository

↓

Banco de Dados

↓

Resposta
```

---

# Responsabilidades

Documentar claramente as responsabilidades de cada componente envolvido.

Evitar descrever implementação linha a linha.

Priorizar responsabilidades arquiteturais.

---

# Integrações

Documentar integrações com:

- outros módulos;
- APIs;
- banco de dados;
- serviços;
- componentes compartilhados;
- bibliotecas relevantes.

---

# Convenções

Quando aplicável documentar:

- nomenclatura;
- organização dos arquivos;
- padrões utilizados;
- decisões arquiteturais relevantes.

---

# Boas Práticas

Registrar recomendações observadas na implementação.

Documentar apenas práticas efetivamente utilizadas pelo projeto.

Não incluir recomendações genéricas sem evidência.

---

# Limitações Conhecidas

Registrar explicitamente:

- funcionalidades ainda não analisadas;
- informações não confirmadas;
- decisões dependentes de investigação adicional;
- limitações identificadas durante o levantamento.

---

# Atualização

Informar:

- quando este documento deve ser atualizado;
- quais alterações arquiteturais exigem revisão desta documentação.

---

# Documentação Relacionada

Relacionar documentos complementares.

Exemplo:

- Arquitetura Geral
- MVC
- Repository Pattern
- Dependency Injection
- AutoMapper
- Notification Pattern
- Validation
- Banco de Dados

---

# Referências

Listar somente referências realmente utilizadas.

Podem incluir:

- documentação oficial;
- RFCs;
- documentação Microsoft;
- documentação MySQL;
- documentos internos do projeto.

Evitar referências não utilizadas pela implementação.
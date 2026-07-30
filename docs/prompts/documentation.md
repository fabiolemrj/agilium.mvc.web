# Prompt: Documentação

# Objetivo

Template para criação, atualização ou revisão da documentação técnica do Agilium Manager.

Este prompt deve ser utilizado para produzir documentação arquitetural consistente, baseada exclusivamente na implementação real da solução e no levantamento técnico existente.

---

# Quando utilizar

Utilize este prompt para documentar:

- funcionalidades;
- módulos;
- arquitetura;
- padrões;
- processos;
- integrações;
- componentes;
- infraestrutura;
- APIs;
- banco de dados;
- desenvolvimento.

---

# Prompt

```text
Crie ou atualize a documentação referente ao seguinte tópico:

[TOPICO]

Antes de escrever a documentação, analise o código e o levantamento técnico existente.

A documentação deve refletir exclusivamente a implementação real do projeto.

---

## 1. Localização

Determinar em qual diretório da documentação o conteúdo deve ser incluído.

Exemplos:

architecture/

api/

business/

database/

development/

ui/

deployment/

security/

operations/

---

## 2. Estrutura

O documento deve conter, quando aplicável:

# Objetivo

# Escopo

# Índice

# Visão Geral

# Arquitetura

# Componentes

# Fluxo

# Convenções

# Boas Práticas

# Limitações Conhecidas

# Atualização

# Documentação Relacionada

---

## 3. Conteúdo

Documentar somente informações confirmadas pelo código-fonte ou pelo levantamento técnico.

Sempre:

- explicar a arquitetura;
- explicar responsabilidades;
- explicar fluxo de funcionamento;
- explicar integração entre componentes;
- utilizar diagramas textuais quando agregarem clareza.

Nunca:

- inventar comportamentos;
- assumir tecnologias não confirmadas;
- documentar boas práticas como se fossem implementação existente.

Quando não houver informação suficiente, registrar explicitamente em:

# Limitações Conhecidas

indicando quais pontos dependem de análise adicional do código-fonte.

---

## 4. Organização

Sempre utilizar:

- linguagem técnica;
- texto objetivo;
- nomenclatura consistente com o projeto;
- títulos padronizados;
- organização hierárquica.

---

## 5. Relacionamentos

Adicionar referências para documentos relacionados.

Exemplo:

Arquitetura

Desenvolvimento

Banco de Dados

Interface

API

Negócio

Segurança

---

## 6. Atualização

Caso seja criada uma nova categoria de documentação:

- atualizar o índice principal;
- atualizar o README da documentação;
- manter a organização da estrutura de diretórios.

---

## 7. Resultado

O documento produzido deve:

- representar fielmente a arquitetura do Agilium Manager;
- servir como documentação técnica oficial;
- evitar redundâncias;
- identificar claramente limitações da documentação;
- facilitar futuras manutenções e evoluções.
```

---

# Parâmetros

| Parâmetro | Descrição | Exemplo |
|-----------|-----------|---------|
| `TOPICO` | Assunto da documentação | "Fluxo de exportação do Cardápio Digital", "Repository Pattern", "Arquitetura MVC", "Integração PDV × Cardápio Digital", "Notification Pattern" |

---

# Resultado Esperado

A documentação produzida deve:

- ser específica para o Agilium Manager;
- refletir exclusivamente informações comprovadas;
- seguir o padrão arquitetural adotado na documentação do projeto;
- indicar claramente limitações quando houver ausência de evidências;
- manter consistência com os demais documentos da pasta `docs/`.
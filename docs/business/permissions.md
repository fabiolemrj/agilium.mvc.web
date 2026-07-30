# Modelo de Permissões

## Objetivo

Documentar o modelo de autorização utilizado pelo Agilium Manager, descrevendo como permissões são organizadas, atribuídas, verificadas e aplicadas ao longo da plataforma.

Este documento complementa a documentação de autenticação e autorização da arquitetura.

---

# Escopo

Este documento contempla:

- Modelo de Permissões
- Perfis
- Claims
- Controle de Acesso
- Contexto da Empresa
- Matriz de Permissões
- Fluxo de Autorização
- Boas Práticas

---

# Índice

- Visão Geral
- Arquitetura de Permissões
- Modelo de Autorização
- Perfis
- Permissões
- Claims
- Fluxo de Validação
- Pontos de Aplicação
- Matriz de Permissões
- Convenções
- Atualização
- Documentação Relacionada

---

# Visão Geral

O Agilium Manager utiliza um mecanismo de autorização baseado em permissões, integrado ao processo de autenticação da plataforma.

Após a autenticação do usuário, cada requisição passa pelo processo de validação das permissões antes da execução da funcionalidade solicitada.

---

# Arquitetura de Permissões

```text
Usuário

↓

Autenticação

↓

Identity

↓

Claims

↓

ClaimsAuthorizeAttribute

↓

ICaService.UsuarioTemPermissao()

↓

Controller

↓

Application Service
```

---

# Modelo de Autorização

A autorização é composta pelos seguintes elementos:

- Usuário
- Perfil (quando aplicável)
- Claims
- Permissões
- Empresa Selecionada
- Funcionalidades

As permissões determinam quais operações um usuário pode executar dentro do contexto da empresa selecionada.

---

# Perfis

> A definição oficial dos perfis deverá ser confirmada durante o levantamento funcional da solução.

Exemplo de estrutura:

| Perfil | Objetivo |
|---------|----------|
| Administrador | Acesso completo ao sistema |
| Gerente | Gestão operacional |
| Operador | Operação diária |
| Consulta | Acesso somente leitura |

Os nomes e responsabilidades acima são ilustrativos e não representam necessariamente a implementação atual.

---

# Permissões

Cada funcionalidade protegida deve possuir uma permissão identificável.

Exemplo:

```text
Produtos.Visualizar
Produtos.Cadastrar
Produtos.Alterar
Produtos.Excluir

Clientes.Visualizar
Clientes.Alterar

Vendas.Realizar
Vendas.Cancelar

Caixa.Abrir
Caixa.Fechar
```

A nomenclatura definitiva deverá refletir a implementação existente.

---

# Claims

As permissões são representadas por Claims utilizadas durante o processo de autorização.

Exemplos de informações associadas ao usuário:

- Identificador
- Nome
- Empresa
- Permissões
- Perfil

A estrutura completa das Claims deverá ser validada na implementação.

---

# Fluxo de Validação

```text
Login

↓

Identity

↓

Cookie

↓

Claims

↓

ClaimsAuthorizeAttribute

↓

ICaService.UsuarioTemPermissao()

↓

Execução da ação
```

---

# Pontos de Aplicação

A autorização pode ser aplicada em diferentes níveis.

## Controllers

Proteção de endpoints.

---

## Actions

Proteção de operações específicas.

---

## Views

Exibição condicional de menus e funcionalidades.

---

## Services

Validação complementar para operações críticas.

---

## APIs

Proteção dos endpoints expostos.

---

# Matriz de Permissões

A matriz definitiva deverá ser construída durante o levantamento funcional.

Exemplo conceitual:

| Permissão | Administrador | Gerente | Operador | Consulta |
|------------|:-------------:|:--------:|:---------:|:---------:|
| Produtos.Visualizar | ✔ | ✔ | ✔ | ✔ |
| Produtos.Cadastrar | ✔ | ✔ | ✔ | ✖ |
| Produtos.Alterar | ✔ | ✔ | ✔ | ✖ |
| Produtos.Excluir | ✔ | ✔ | ✖ | ✖ |
| Vendas.Realizar | ✔ | ✔ | ✔ | ✖ |
| Caixa.Fechar | ✔ | ✔ | ✖ | ✖ |

---

# Convenções

As permissões devem:

- possuir nomes padronizados;
- representar uma única responsabilidade;
- ser reutilizadas sempre que possível;
- ser documentadas antes da implementação.

Evitar:

- permissões duplicadas;
- nomenclaturas inconsistentes;
- verificações espalhadas sem padronização.

---

# Boas Práticas

Sempre:

- proteger operações críticas;
- validar permissões também na camada de aplicação quando necessário;
- manter Controllers responsáveis apenas pela autorização de entrada;
- centralizar a lógica de autorização.

---

# Limitações Conhecidas

O levantamento técnico confirmou a utilização de componentes como:

- ASP.NET Core Identity;
- ClaimsAuthorizeAttribute;
- Claims;
- Cookie Authentication;
- EmpresaSelecionadaMiddleware;
- ICaService.UsuarioTemPermissao().

Entretanto, a estrutura completa de perfis, permissões e seus relacionamentos deverá ser consolidada após a análise dos projetos `agilium-manager-azure-api` e `agilium-pdv-azure-api`.

---

# Atualização

Este documento deve ser atualizado sempre que:

- novas permissões forem criadas;
- perfis forem alterados;
- novas funcionalidades forem protegidas;
- o mecanismo de autorização sofrer alterações.

---

# Documentação Relacionada

- architecture/security.md
- architecture/authorization.md
- api/authentication.md
- business/modules/security.md
- business/business-rules.md
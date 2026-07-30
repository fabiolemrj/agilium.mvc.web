# Glossário de Negócio

## Objetivo

Documentar a terminologia oficial utilizada pelo ecossistema Agilium Manager, estabelecendo uma linguagem comum entre desenvolvedores, analistas, arquitetos, equipes de negócio e sistemas integrados.

Este documento funciona como o vocabulário oficial da plataforma.

---

# Escopo

Este documento contempla:

- Conceitos de Negócio
- Entidades de Domínio
- Termos Técnicos
- Acrônimos
- Convenções
- Linguagem Ubíqua (Ubiquitous Language)

---

# Índice

- Visão Geral
- Como Utilizar o Glossário
- Conceitos de Negócio
- Entidades do Domínio
- Processos
- Acrônimos
- Convenções
- Atualização
- Documentação Relacionada

---

# Visão Geral

Todos os documentos da plataforma devem utilizar os termos definidos neste glossário.

Quando um novo conceito surgir durante o desenvolvimento, ele deverá ser registrado aqui antes de ser utilizado na documentação oficial.

---

# Como Utilizar

Cada termo deve possuir:

- Nome
- Categoria
- Definição
- Sinônimos (quando existirem)
- Termos Relacionados
- Documentação relacionada

---

# Conceitos de Negócio

## Empresa

**Categoria**

Entidade

**Definição**

Organização cadastrada na plataforma responsável pela operação dos módulos do sistema.

**Relacionados**

- Usuário
- Filial
- Permissões

---

## Usuário

**Categoria**

Entidade

**Definição**

Pessoa autorizada a acessar o sistema mediante autenticação.

---

## Cliente

**Categoria**

Entidade

**Definição**

Pessoa física ou jurídica atendida pela empresa.

---

## Produto

**Categoria**

Entidade

**Definição**

Item comercializado ou controlado pela plataforma.

---

## Pedido

**Categoria**

Processo

**Definição**

Solicitação de venda realizada por um cliente.

---

## Venda

**Categoria**

Processo

**Definição**

Operação comercial concluída envolvendo cliente, produtos e pagamentos.

---

## Caixa

**Categoria**

Financeiro

**Definição**

Sessão operacional responsável pelo registro financeiro das vendas.

---

## PDV

**Categoria**

Módulo

**Definição**

Ponto de Venda utilizado para registrar vendas presenciais.

---

## Permissão

**Categoria**

Segurança

**Definição**

Autorização concedida para execução de determinada funcionalidade.

---

## Claim

**Categoria**

Segurança

**Definição**

Informação de autorização utilizada para controlar acesso aos recursos.

---

## Perfil

**Categoria**

Segurança

**Definição**

Conjunto de permissões atribuídas a um usuário.

---

## Notification

**Categoria**

Arquitetura

**Definição**

Mecanismo utilizado para registrar erros de negócio sem utilização de exceções em fluxos esperados.

---

# Entidades do Domínio

Cada entidade deverá possuir um documento próprio.

| Entidade | Documento |
|----------|-----------|
| Empresa | entities/company.md |
| Usuário | entities/user.md |
| Cliente | entities/customer.md |
| Produto | entities/product.md |
| Pedido | entities/order.md |
| Venda | entities/sale.md |

---

# Processos

Os principais processos da plataforma incluem:

- Cadastro
- Venda
- Pedido
- Pagamento
- Fechamento de Caixa
- Controle de Estoque

Cada processo possui documentação específica.

---

# Acrônimos

| Acrônimo | Significado |
|-----------|-------------|
| API | Application Programming Interface |
| DTO | Data Transfer Object |
| EF Core | Entity Framework Core |
| MVC | Model-View-Controller |
| ORM | Object Relational Mapper |
| PDV | Ponto de Venda |
| DI | Dependency Injection |
| JWT | JSON Web Token |
| CRUD | Create, Read, Update, Delete |
| UoW | Unit of Work |

Outros acrônimos deverão ser adicionados conforme forem identificados.

---

# Convenções

Sempre utilizar:

- o mesmo termo em toda a documentação;
- nomes idênticos aos utilizados no domínio;
- definições únicas para cada conceito.

Evitar:

- sinônimos para o mesmo conceito;
- traduções diferentes do mesmo termo;
- abreviações não documentadas.

---

# Atualização

Sempre que:

- um novo módulo for criado;
- surgir um novo conceito de negócio;
- uma entidade for adicionada;
- uma integração introduzir novos termos;

este glossário deverá ser atualizado.

---

# Documentação Relacionada

- overview.md
- modules.md
- business-rules.md
- workflows.md
- architecture/overview.md
# Controllers

## Objetivo

Este documento descreve a arquitetura da camada de **Controllers** utilizada pelo projeto **Agilium Manager**.

Os Controllers representam a camada de entrada da API, sendo responsáveis por receber as requisições HTTP, validar os parâmetros básicos, delegar o processamento para a camada Business e retornar as respostas ao cliente.

Os Controllers **não implementam regras de negócio**, atuando apenas como intermediários entre o cliente e os serviços da aplicação.

---

# Visão Geral

A camada Controllers é responsável por:

- Receber requisições HTTP
- Validar parâmetros básicos
- Validar autenticação
- Validar autorização
- Encaminhar requisições para a camada Business
- Retornar respostas HTTP
- Padronizar códigos de retorno
- Tratar exceções da API

---

# Arquitetura

```
Cliente HTTP

↓

Controller

↓

Business (Services)

↓

Repository

↓

Banco de Dados
```

Toda regra operacional permanece na camada Business.

---

# Responsabilidades

Cada Controller deve:

- Receber requisições
- Validar ModelState
- Receber DTOs/ViewModels
- Chamar apenas Services
- Retornar ActionResult
- Utilizar códigos HTTP adequados
- Não acessar banco diretamente

---

# Organização

Os Controllers permanecem organizados por domínio.

Exemplo:

```
Controllers

├── EmpresaController
├── UsuarioController
├── ClienteController
├── ProdutoController
├── VendaController
├── PedidoController
├── CaixaController
├── TurnoController
├── FinanceiroController
├── FiscalController
└── EstoqueController
```

Cada Controller deve representar um recurso da API.

---

# Fluxo de Requisição

```
HTTP Request

↓

Controller

↓

Validação

↓

Business

↓

Repository

↓

Banco

↓

Business

↓

Controller

↓

HTTP Response
```

---

# Entrada de Dados

Os Controllers devem receber apenas objetos próprios para comunicação com a API.

Exemplos:

- DTO
- ViewModel
- Request Model

Evitar receber entidades do domínio diretamente.

---

# Saída de Dados

Os Controllers devem retornar:

- DTO
- ViewModel
- PagedResult
- ActionResult\<T\>

Nunca retornar entidades completas quando houver DTO específico.

---

# Validação

Os Controllers devem validar apenas:

- parâmetros obrigatórios
- formato dos dados
- ModelState
- autenticação
- autorização

Validações de negócio permanecem na camada Business.

---

# Tratamento de Exceções

As exceções devem ser tratadas de forma centralizada.

Fluxo:

```
Controller

↓

Business

↓

Exception

↓

Middleware/Filtro

↓

Resposta HTTP
```

Evitar blocos `try/catch` repetitivos em todos os Controllers.

---

# Autenticação

Os Controllers devem respeitar as políticas de autenticação configuradas.

Exemplos:

- JWT
- Bearer Token
- Policies
- Claims

Todo endpoint protegido deve exigir autenticação.

---

# Autorização

Antes de executar operações críticas, validar permissões do usuário.

Exemplos:

- Consultar
- Inserir
- Alterar
- Excluir
- Cancelar

A autorização deve utilizar as políticas definidas pelo sistema.

---

# Métodos HTTP

Os Controllers devem seguir os verbos HTTP apropriados.

| Método | Finalidade |
|---------|------------|
| GET | Consulta |
| POST | Inclusão |
| PUT | Atualização |
| PATCH | Atualização parcial |
| DELETE | Exclusão lógica ou física (quando permitido) |

---

# Rotas

As rotas devem ser consistentes e seguir um padrão.

Exemplo:

```
api/v1/produtos

api/v1/clientes

api/v1/vendas
```

Evitar rotas que representem ações quando um recurso puder ser utilizado.

---

# Status HTTP

Utilizar códigos apropriados.

Exemplos:

- 200 OK
- 201 Created
- 204 No Content
- 400 Bad Request
- 401 Unauthorized
- 403 Forbidden
- 404 Not Found
- 409 Conflict
- 500 Internal Server Error

---

# Integração com Services

Toda lógica deve ser delegada aos Services.

```
Controller

↓

ProdutoService
```

Nunca acessar Repository diretamente.

---

# Integração com Repository

Fluxo correto:

```
Controller

↓

Service

↓

Repository
```

Fluxo incorreto:

```
Controller

↓

Repository
```

---

# Documentação da API

Todos os endpoints devem ser documentados.

Exemplos:

- Swagger
- OpenAPI

Informações recomendadas:

- descrição
- parâmetros
- respostas
- exemplos
- códigos HTTP

---

# Versionamento

Sempre que suportado, utilizar versionamento da API.

Exemplo:

```
/api/v1/

↓

/api/v2/
```

Evita quebra de compatibilidade entre consumidores.

---

# Paginação

Consultas que retornam grandes volumes de dados devem utilizar paginação.

Fluxo:

```
Filtros

↓

Service

↓

Repository

↓

PagedResult
```

---

# Segurança

Nunca:

- acessar banco diretamente
- implementar SQL
- validar regra de negócio
- armazenar estado da aplicação
- expor detalhes internos das exceções

Sempre validar autenticação e autorização antes de operações protegidas.

---

# Convenções

Os Controllers devem seguir as convenções do projeto:

- Nome terminado em `Controller`
- Um Controller por recurso
- Herança de `ControllerBase` (ou classe base equivalente)
- Métodos assíncronos sempre que possível
- Uso consistente de injeção de dependência
- Respostas padronizadas

---

# Impactos de Alterações

Alterações nos Controllers podem impactar:

- Clientes da API
- Swagger
- Front-end
- Aplicativos móveis
- Integrações externas
- Testes automatizados
- Versionamento

Toda alteração deve preservar a compatibilidade sempre que possível.

---

# Boas Práticas

- Não implementar regras de negócio.
- Não acessar Repository diretamente.
- Validar apenas entrada de dados.
- Utilizar DTOs específicos.
- Retornar códigos HTTP corretos.
- Manter métodos pequenos e objetivos.
- Utilizar injeção de dependência.
- Centralizar tratamento de exceções.
- Documentar todos os endpoints.

---

# Checklist

Antes de alterar um Controller:

☐ Endpoint documentado

☐ DTO revisado

☐ ModelState validado

☐ Autenticação validada

☐ Autorização validada

☐ Service atualizado

☐ Repository não acessado diretamente

☐ Resposta HTTP correta

☐ Swagger atualizado

☐ Impactos avaliados

---

# Conclusão

A camada de **Controllers** do **Agilium Manager** representa o ponto de entrada das requisições HTTP, sendo responsável por receber solicitações, validar dados básicos, aplicar autenticação e autorização e delegar o processamento para a camada Business.

Ao manter os Controllers enxutos, sem regras de negócio e desacoplados da camada de persistência, a aplicação preserva uma arquitetura limpa, facilita a manutenção do código e garante maior reutilização dos serviços de negócio.

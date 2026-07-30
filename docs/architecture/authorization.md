# Autorização

## Objetivo

Documentar o modelo de autorização utilizado no ecossistema Agilium Manager, descrevendo como permissões, papéis, claims e políticas controlam o acesso aos recursos da plataforma.

Este documento serve como referência para desenvolvedores e arquitetos na implementação de novos recursos e na manutenção das regras de segurança.

---

# Escopo

Este documento contempla:

- Modelo de Autorização
- Roles
- Claims
- Policies
- Custom Authorization
- Permissões
- Controle de Acesso
- Segurança
- Boas Práticas

---

# Índice

- Visão Geral
- Arquitetura
- Fluxo de Autorização
- Roles
- Claims
- Policies
- Custom Authorization
- Controller Authorization
- Service Authorization
- Permissões
- Empresa Selecionada
- Segurança
- Boas Práticas
- Limitações
- Documentação Relacionada

---

# Visão Geral

Após a autenticação do usuário, todas as requisições passam pelo processo de autorização.

A autorização determina se o usuário autenticado possui permissão para executar determinada operação.

A plataforma utiliza uma combinação de:

- ASP.NET Core Authorization
- Claims
- Policies
- Atributos customizados
- Validação de permissões
- Regras de negócio

---

# Arquitetura

```text
Usuário

      │

Autenticação

      │

Claims

      │

Policies

      │

Custom Authorization

      │

Controller

      │

Service

      │

Recurso
```

---

# Fluxo de Autorização

```text
Requisição

      │

Authenticate

      │

Authorize

      │

ClaimsAuthorizeAttribute

      │

ICaService.UsuarioTemPermissao()

      │

Controller

      │

Service
```

Sempre que o usuário não possuir permissão, a execução deve ser interrompida antes da lógica de negócio.

---

# Roles

As Roles representam agrupamentos de permissões atribuídas aos usuários.

Exemplos:

| Role | Descrição |
|------|-----------|
| Administrador | Acesso total |
| Gerente | Gestão operacional |
| Operador | Operações do sistema |
| Consulta | Apenas leitura |

> Os papéis efetivamente utilizados deverão ser documentados após análise do banco de dados e da configuração do Identity.

---

# Claims

As Claims representam permissões específicas concedidas ao usuário autenticado.

Exemplos:

| Claim | Finalidade |
|--------|------------|
| Produto.Visualizar | Consultar produtos |
| Produto.Incluir | Cadastrar produtos |
| Produto.Alterar | Atualizar produtos |
| Produto.Excluir | Remover produtos |

Sempre que possível, as permissões devem ser baseadas em Claims, evitando verificações diretas por Role.

---

# Policies

As Policies agrupam uma ou mais regras de autorização.

Exemplo:

```csharp
services.AddAuthorization(options =>
{
    options.AddPolicy("Produtos", policy =>
        policy.RequireClaim("Produto.Visualizar"));
});
```

As policies registradas devem ser documentadas conforme a implementação real.

---

# Custom Authorization

A plataforma pode utilizar atributos personalizados para encapsular regras de autorização.

Exemplo conceitual:

```csharp
[ClaimsAuthorize("Produto", "Visualizar")]
public IActionResult Index()
{
}
```

Responsabilidades do atributo:

- validar Claims;
- verificar permissões;
- impedir acesso não autorizado;
- padronizar verificações entre Controllers.

A implementação deve refletir o atributo utilizado pelo projeto (por exemplo, `ClaimsAuthorizeAttribute` ou outro equivalente).

---

# Autorização em Controllers

Os Controllers podem utilizar:

```csharp
[Authorize]
```

ou

```csharp
[AllowAnonymous]
```

e, quando necessário, atributos customizados para controle granular de acesso.

Sempre que possível:

- proteger o Controller inteiro;
- flexibilizar apenas ações públicas.

---

# Autorização em Services

Embora a validação principal ocorra antes da execução da Action, regras críticas de negócio também podem ser reforçadas na camada de Services.

Exemplos:

- validação de empresa;
- validação de proprietário do recurso;
- validação de estado da operação.

---

# Permissões

As permissões devem ser organizadas por módulo funcional.

Exemplo:

```text
Produtos
    Visualizar
    Incluir
    Alterar
    Excluir

Clientes
    Visualizar
    Incluir
    Alterar
    Excluir

Vendas
    Abrir
    Cancelar
    Finalizar
```

A lista oficial deve ser mantida em `docs/business/permissions.md`.

---

# Empresa Selecionada

O levantamento identificou um componente responsável por validar a empresa selecionada durante o processamento da requisição.

Essa validação garante que o usuário acesse apenas recursos pertencentes ao contexto autorizado.

A implementação deve ser detalhada em um documento específico da arquitetura.

---

# Segurança

Boas práticas recomendadas:

- negar acesso por padrão;
- validar permissões antes da execução da Action;
- utilizar Claims em vez de verificações manuais espalhadas pelo código;
- centralizar regras de autorização;
- registrar tentativas de acesso negado quando apropriado.

---

# Boas Práticas

Sempre:

- utilizar `[Authorize]` nos Controllers;
- utilizar atributos customizados para permissões específicas;
- documentar novas Claims;
- manter Policies centralizadas;
- revisar permissões sempre que novos módulos forem criados.

Evitar:

- lógica de autorização duplicada;
- verificações de permissões diretamente nos Controllers;
- uso de valores literais espalhados pelo código.

---

# Limitações Conhecidas

O levantamento técnico confirmou a utilização de:

- ASP.NET Core Authorization;
- Claims;
- `ClaimsAuthorizeAttribute`;
- `ICaService.UsuarioTemPermissao()`;
- `EmpresaSelecionadaMiddleware`.

A lista completa de:

- Roles;
- Claims;
- Policies;
- atributos customizados;
- regras de autorização por módulo;

deverá ser confirmada durante a análise dos projetos:

- agilium-manager-azure-api;
- agilium-pdv-azure-api.

---

# Atualização

Sempre que uma nova permissão ou política for criada:

- atualizar este documento;
- revisar `permissions.md`;
- revisar Controllers afetados;
- atualizar exemplos;
- revisar documentação de autenticação.

---

# Documentação Relacionada

- authentication.md
- ../api/authentication.md
- ../api/authorization.md
- ../business/permissions.md
- ../architecture/security.md
- ../architecture/request-pipeline.md
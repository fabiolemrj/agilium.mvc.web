# Swagger / OpenAPI

## Objetivo

Documentar a configuração, utilização e padronização do Swagger (OpenAPI) utilizado pelas APIs do ecossistema Agilium Manager.

Este documento descreve como a documentação é gerada, como deve ser configurada e quais padrões devem ser seguidos para manter a documentação consistente entre todos os projetos.

---

# Escopo

Este documento contempla:

- Swagger
- OpenAPI
- Swashbuckle
- Versionamento
- API Explorer
- JWT Bearer
- XML Documentation
- Operation Filters
- Document Filters
- Segurança
- URLs
- Customizações

---

# Índice

- Visão Geral
- Arquitetura
- Componentes
- Configuração
- Versionamento
- Segurança
- XML Documentation
- Filtros
- Interface Swagger UI
- URLs
- Boas Práticas
- Limitações
- Documentação Relacionada

---

# Visão Geral

O Swagger é utilizado para gerar automaticamente a documentação das APIs REST.

Principais objetivos:

- documentação automática;
- testes interativos;
- exploração dos endpoints;
- geração de clientes;
- apoio ao desenvolvimento;
- integração com ferramentas externas.

Toda alteração em Controllers deve refletir automaticamente na documentação OpenAPI.

---

# Arquitetura

```text
Controllers

      │

ApiExplorer

      │

SwaggerGen

      │

OpenAPI

      │

Swagger UI

      │

Consumidor
```

---

# Componentes

A infraestrutura do Swagger normalmente é composta por:

- Swashbuckle.AspNetCore
- SwaggerGen
- SwaggerUI
- ApiExplorer
- XML Documentation
- Versioned API Explorer

A utilização exata deve ser confirmada durante a análise dos projetos.

---

# Configuração

A configuração central do Swagger deve ficar concentrada em um único componente.

Exemplo:

```
Configuration/
    SwaggerConfig.cs
```

Responsabilidades:

- registrar SwaggerGen;
- registrar documentos OpenAPI;
- configurar autenticação;
- configurar versionamento;
- registrar filtros;
- configurar XML Documentation.

---

# OpenAPI

Cada versão da API deve gerar um documento OpenAPI independente.

Exemplo:

```
swagger/v1/swagger.json

swagger/v2/swagger.json
```

---

# Segurança

Caso a API utilize JWT, o Swagger deve permitir autenticação através do botão **Authorize**.

Fluxo esperado:

```text
Usuário

     │

Authorize

     │

Bearer Token

     │

Swagger UI

     │

Endpoints protegidos
```

Configuração recomendada:

- Security Definition
- Security Requirement
- Bearer Authentication

Exemplo de cabeçalho:

```http
Authorization: Bearer {token}
```

Caso o projeto utilize outro mecanismo de autenticação (Cookies, Identity ou outro), este documento deverá refletir a implementação real.

---

# Integração com Versionamento

Quando houver múltiplas versões da API, cada uma deve possuir documentação própria.

Exemplo:

```
v1

v2

v3
```

A integração normalmente é realizada utilizando:

- ApiVersion
- ApiExplorer
- VersionedApiExplorer

A configuração efetiva deverá ser validada durante a análise da solução.

---

# XML Documentation

Sempre que possível, utilizar comentários XML para enriquecer a documentação.

Exemplo:

```csharp
/// <summary>
/// Obtém um produto.
/// </summary>
```

Os comentários devem documentar:

- Controllers
- Actions
- DTOs
- Parâmetros
- Respostas

---

# Operation Filters

Os Operation Filters permitem personalizar operações específicas.

Exemplos:

- inclusão automática de headers;
- documentação de autenticação;
- documentação de respostas;
- parâmetros comuns.

---

# Document Filters

Os Document Filters permitem alterar o documento OpenAPI completo.

Exemplos:

- ocultar endpoints;
- reorganizar grupos;
- adicionar informações globais;
- incluir metadados.

---

# Interface Swagger UI

A interface permite:

- explorar endpoints;
- testar requisições;
- visualizar modelos;
- visualizar respostas;
- autenticar usuários;
- navegar entre versões da API.

---

# URLs

Os endereços variam conforme o ambiente.

| Ambiente | URL |
|-----------|-----|
| Desenvolvimento | https://localhost:{porta}/swagger |
| Homologação | Definir conforme ambiente |
| Produção | Definir conforme ambiente |

Quando houver múltiplas versões:

```
/swagger/v1/swagger.json

/swagger/v2/swagger.json
```

---

# Organização

A documentação deve ser agrupada por:

- versão;
- controller;
- tag;
- recurso.

---

# Boas Práticas

Sempre:

- documentar todos os Controllers;
- documentar todas as Actions;
- utilizar XML Documentation;
- informar códigos HTTP;
- documentar parâmetros;
- documentar DTOs;
- documentar autenticação;
- documentar respostas;
- manter Swagger sincronizado com o código.

Evitar:

- endpoints sem documentação;
- descrições genéricas;
- parâmetros sem descrição;
- respostas incompletas.

---

# Atualização

Sempre que um endpoint for criado ou alterado:

- revisar Swagger;
- validar OpenAPI;
- revisar XML Documentation;
- atualizar exemplos;
- revisar autenticação;
- revisar versionamento.

---

# Limitações Conhecidas

O levantamento técnico identificou a utilização de Swagger na solução, porém a configuração detalhada dos projetos **agilium-manager-azure-api** e **agilium-pdv-azure-api** ainda deve ser confirmada.

Itens que precisam ser validados:

- SwaggerConfig.cs;
- configuração do SwaggerGen;
- configuração do SwaggerUI;
- autenticação (JWT, Cookie ou outro mecanismo);
- integração com versionamento;
- Operation Filters;
- Document Filters;
- XML Documentation.

---

# Documentação Relacionada

- overview.md
- endpoints.md
- authentication.md
- authorization.md
- versioning.md
- conventions.md
- examples.md
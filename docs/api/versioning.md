# Versionamento da API

## Objetivo

Documentar a estratégia de versionamento adotada pelas APIs do ecossistema Agilium Manager, estabelecendo padrões para criação, evolução e descontinuação de versões, garantindo compatibilidade entre clientes e reduzindo impactos durante a evolução da plataforma.

---

# Escopo

Este documento contempla:

- Estratégia de Versionamento
- Versionamento por URL
- Configuração dos Controllers
- Configuração do ApiVersioning
- Integração com ApiExplorer
- Integração com Swagger
- Compatibilidade
- Depreciação
- Evolução de Endpoints
- Boas Práticas

---

# Índice

- Visão Geral
- Estratégia
- Estrutura das URLs
- Configuração dos Controllers
- Configuração da Aplicação
- ApiExplorer
- Swagger
- Compatibilidade
- Evolução das APIs
- Depreciação
- Boas Práticas
- Limitações
- Documentação Relacionada

---

# Visão Geral

O versionamento permite que novas funcionalidades sejam introduzidas sem interromper consumidores existentes.

Cada versão representa um contrato público entre a API e seus consumidores.

Alterações incompatíveis devem resultar em uma nova versão da API.

---

# Estratégia

A plataforma utiliza versionamento baseado na URL da requisição.

Exemplo:

```
/api/v1/
/api/v2/
/api/v3/
```

Essa abordagem facilita:

- descoberta das versões;
- documentação independente;
- coexistência entre versões;
- migração gradual dos consumidores.

A implementação deverá ser confirmada durante a análise da solução.

---

# Estrutura das URLs

Padrão esperado:

```
/api/v{version}/[controller]
```

Exemplos:

```
GET /api/v1/produtos

POST /api/v1/clientes

GET /api/v2/produtos
```

Os recursos devem manter a mesma nomenclatura entre versões sempre que possível.

---

# Configuração dos Controllers

Cada Controller deve declarar explicitamente a(s) versão(ões) suportada(s).

Exemplo:

```csharp
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class ProdutoController : ControllerBase
{
}
```

Quando um Controller atender múltiplas versões, todas devem ser declaradas.

Exemplo:

```csharp
[ApiVersion("1.0")]
[ApiVersion("2.0")]
```

---

# Configuração da Aplicação

O versionamento normalmente é configurado através do pacote:

```
Microsoft.AspNetCore.Mvc.Versioning
```

Itens normalmente configurados:

- AddApiVersioning()
- DefaultApiVersion
- AssumeDefaultVersionWhenUnspecified
- ReportApiVersions
- ApiVersionReader

A configuração efetiva deverá ser documentada após análise do projeto.

---

# ApiExplorer

O ApiExplorer é responsável por expor os metadados das versões para ferramentas como o Swagger.

Normalmente é configurado utilizando:

```
Microsoft.AspNetCore.Mvc.Versioning.ApiExplorer
```

Responsabilidades:

- descobrir versões;
- organizar endpoints;
- gerar documentação independente;
- integrar com Swagger.

---

# Integração com Swagger

Cada versão deve gerar um documento OpenAPI independente.

Exemplo:

```
/swagger/v1/swagger.json

/swagger/v2/swagger.json
```

Na interface Swagger UI, cada versão deve estar disponível para seleção.

---

# Compatibilidade

As seguintes alterações são consideradas compatíveis e normalmente não exigem nova versão:

- inclusão de novos endpoints;
- inclusão de parâmetros opcionais;
- inclusão de propriedades opcionais em respostas;
- melhorias internas sem alteração do contrato.

As seguintes alterações exigem nova versão:

- remoção de endpoints;
- alteração de rotas;
- alteração de parâmetros obrigatórios;
- alteração do formato da resposta;
- alteração de comportamento incompatível.

---

# Evolução das APIs

Sempre que possível:

1. manter compatibilidade;
2. introduzir novos recursos na versão atual;
3. criar nova versão apenas quando necessário;
4. manter versões anteriores durante o período de transição.

---

# Depreciação

Quando uma versão deixar de ser recomendada, ela deverá ser marcada como obsoleta.

Fluxo recomendado:

```
Versão Ativa

↓

Versão Marcada como Deprecated

↓

Comunicação aos Consumidores

↓

Prazo de Migração

↓

Remoção
```

A política oficial deve definir:

- tempo mínimo de suporte;
- prazo para migração;
- data prevista para descontinuação;
- canais de comunicação.

---

# Boas Práticas

Sempre:

- utilizar versionamento explícito;
- manter documentação separada por versão;
- atualizar o Swagger;
- documentar alterações incompatíveis;
- preservar compatibilidade sempre que possível;
- comunicar alterações aos consumidores.

Evitar:

- alterar contratos existentes;
- reutilizar versões para mudanças incompatíveis;
- remover endpoints sem período de transição.

---

# Atualização

Sempre que uma nova versão for criada:

- atualizar este documento;
- atualizar o Swagger;
- revisar a documentação dos endpoints;
- revisar exemplos;
- revisar autenticação e autorização;
- atualizar notas de migração.

---

# Limitações Conhecidas

O levantamento técnico atual não confirmou a configuração exata do versionamento nos projetos:

- agilium-manager-azure-api
- agilium-pdv-azure-api

Devem ser verificados:

- AddApiVersioning();
- AddVersionedApiExplorer();
- ApiVersionReader;
- atributos [ApiVersion];
- configuração do Swagger por versão.

---

# Documentação Relacionada

- overview.md
- endpoints.md
- swagger.md
- conventions.md
- authentication.md
- examples.md
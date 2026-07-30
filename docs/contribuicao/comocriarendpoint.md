# Como Criar um Endpoint (API)

## Objetivo

Guia passo a passo para criar um novo **endpoint REST** nas APIs do Agilium Manager (`agilium-manager-azure-api` ou `agilium-pdv-azure-api`).

---

## Pré-requisitos

- Service e Repository já implementados na camada Business
- DTO/ViewModel de request e response criados

---

## Passo a Passo

### 1. Localização

- **API Principal:** `agilium-manager-azure-api/Controllers/`
- **API PDV:** `agilium-pdv-azure-api/Controllers/`

### 2. Estrutura Base

```csharp
using agilium.api.business.Interfaces.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace agilium_manager_azure_api.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    public class NomeController : MainController
    {
        private readonly INomeService _nomeService;

        public NomeController(INomeService nomeService, /* ... */)
            : base(/* ... */)
        {
            _nomeService = nomeService;
        }

        /// <summary>
        /// Lista todos os registros.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NomeDto>>> Get()
        {
            var lista = await _nomeService.ObterTodas();
            var dtos = _mapper.Map<List<NomeDto>>(lista);
            return Ok(dtos);
        }

        /// <summary>
        /// Obtém por ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<NomeDto>> Get(long id)
        {
            var entidade = await _nomeService.ObterPorId(id);
            if (entidade == null) return NotFound();
            return Ok(_mapper.Map<NomeDto>(entidade));
        }

        /// <summary>
        /// Cria um novo registro.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<NomeDto>> Post([FromBody] NomeRequestDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var entidade = _mapper.Map<Nome>(dto);
            await _nomeService.Adicionar(entidade);

            if (!OperacaoValida())
                return BadRequest(ObterNotificacoes());

            await _nomeService.Salvar();
            var resultDto = _mapper.Map<NomeDto>(entidade);
            return CreatedAtAction(nameof(Get), new { id = resultDto.Id }, resultDto);
        }

        /// <summary>
        /// Atualiza um registro.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, [FromBody] NomeRequestDto dto)
        {
            if (id != dto.Id) return BadRequest("ID inconsistente");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var entidade = _mapper.Map<Nome>(dto);
            await _nomeService.Atualizar(entidade);

            if (!OperacaoValida())
                return BadRequest(ObterNotificacoes());

            await _nomeService.Salvar();
            return NoContent();
        }

        /// <summary>
        /// Remove um registro.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var entidade = await _nomeService.ObterPorId(id);
            if (entidade == null) return NotFound();

            await _nomeService.Remover(id);
            if (!OperacaoValida())
                return BadRequest(ObterNotificacoes());

            await _nomeService.Salvar();
            return NoContent();
        }
    }
}
```

---

## Convenções da API

| Convenção | Exemplo |
|-----------|---------|
| Versionamento | `[ApiVersion("1.0")]` + `/api/v{version}/[controller]` |
| Rota base | `api/v{version:apiVersion}/nomes` |
| GET (lista) | `GET /api/v1/nomes` → `200 OK` |
| GET (por id) | `GET /api/v1/nomes/{id}` → `200 OK` / `404 Not Found` |
| POST | `POST /api/v1/nomes` → `201 Created` |
| PUT | `PUT /api/v1/nomes/{id}` → `204 No Content` |
| DELETE | `DELETE /api/v1/nomes/{id}` → `204 No Content` |
| Erro de validação | `400 Bad Request` + array de mensagens |
| Erro de negócio | `400 Bad Request` via `ObterNotificacoes()` |
| Autorização | `[Authorize]` + `[ClaimsAuthorize]` quando necessário |

---

## Códigos HTTP

| Código | Quando Usar |
|--------|-------------|
| 200 | GET bem-sucedido |
| 201 | POST criado com sucesso |
| 204 | PUT/DELETE bem-sucedido (sem corpo) |
| 400 | Validação falhou (ModelState ou Notificações) |
| 401 | Não autenticado |
| 403 | Autenticado mas sem permissão |
| 404 | Recurso não encontrado |
| 500 | Erro interno (capturado pelo ExceptionMiddleware) |

---

## Documentação Swagger

Após criar o endpoint:

1. O Swagger detecta automaticamente o controller
2. Verifique se os comentários XML (`/// <summary>`) aparecem na UI
3. Acesse `/swagger` para testar

---

## Checklist do Endpoint

☐ Controller herda de `MainController`

☐ `[ApiVersion("1.0")]` definido

☐ Rota segue padrão `api/v{version}/[controller]`

☐ `[Authorize]` presente (exceto endpoints públicos)

☐ Retorno tipado `ActionResult<T>`

☐ `ModelState.IsValid` validado no POST/PUT

☐ `OperacaoValida()` verificado após chamar Service

☐ Códigos HTTP apropriados (200, 201, 204, 400, 404)

☐ DTOs separados para request e response

☐ Documentado com `/// <summary>` para Swagger

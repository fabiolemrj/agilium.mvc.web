# Padrão AutoMapper

## Objetivo

Documentar o padrão de uso do **AutoMapper** no projeto Agilium Manager, incluindo configuração de profiles, convenções de mapeamento, injeção e boas práticas.

---

## Visão Geral

O AutoMapper é utilizado para converter objetos entre as camadas:

```
Model (Domínio)  ←→  ViewModel (Apresentação)
Model (Domínio)  ←→  DTO (API)
```

A configuração central está em `agilum.mvc.web/Configuration/AutomapperConfig.cs`.

---

## Configuração

### Registro no Startup

```csharp
// Startup.cs — ConfigureServices
services.AddAutoMapper(typeof(Startup));
```

O AutoMapper escaneia o assembly em busca de classes que herdam de `Profile`.

### Classe de Perfil

```csharp
// Configuration/AutomapperConfig.cs
public class AutomapperConfig : Profile
{
    public AutomapperConfig()
    {
        ShouldMapProperty = p => true;

        // Model → ViewModel
        CreateMap<Produto, ProdutoViewModel>()
            .ForMember(dest => dest.NomeCategoria,
                       opt => opt.MapFrom(src => src.Categoria.Nome));

        // ViewModel → Model
        CreateMap<ProdutoViewModel, Produto>();

        // Bidirecional
        CreateMap<Cliente, ClienteViewModel>().ReverseMap();
    }
}
```

---

## Convenções

| Regra | Descrição |
|-------|-----------|
| `ShouldMapProperty = p => true` | Mapeia todas as propriedades, inclusive privadas |
| `.ReverseMap()` | Cria mapeamento bidirecional quando a estrutura é simétrica |
| `.ForMember()` | Customiza mapeamentos específicos (nomes diferentes, cálculos) |
| `.Ignore()` | Ignora propriedades que não devem ser mapeadas |
| Sufixo `ViewModel` | ViewModels usam sufixo descritivo |

---

## Padrões de Uso

### Injeção nos Controllers

```csharp
public class ProdutoController : MainController
{
    public ProdutoController(..., IMapper mapper) : base(..., mapper, ...) { }

    public async Task<ActionResult> Index()
    {
        var produtos = await _produtoService.ObterTodos();
        var viewModels = _mapper.Map<List<ProdutoViewModel>>(produtos);
        return View(viewModels);
    }
}
```

### Mapeamento de Listas

```csharp
var viewModels = _mapper.Map<List<ProdutoViewModel>>(models);
```

### Mapeamento de PagedResult

```csharp
CreateMap<PagedResult<Produto>, PagedResult<ProdutoViewModel>>().ReverseMap();
```

---

## Tipos de Mapeamento no Projeto

| Origem | Destino | Direção |
|--------|---------|---------|
| Model → ViewModel | Controller → View | Leitura |
| ViewModel → Model | View → Controller (POST) | Escrita |
| Model → DTO | API Response | Leitura |
| DTO → Model | API Request | Escrita |
| PagedResult\<T\> → PagedResult\<V\> | Paginação | Ambos |

---

## Boas Práticas

- Centralizar todos os profiles em `AutomapperConfig.cs`
- Usar `.ReverseMap()` apenas quando Model e ViewModel forem simétricos
- Nomes de propriedades iguais são mapeados automaticamente — só usar `.ForMember()` quando necessário
- Não mapear entidades com dados sensíveis (senhas, chaves) para ViewModels públicos
- Usar `.Ignore()` para propriedades que devem ser preenchidas manualmente

---

## Pontos de Atenção

- `AutomapperConfig.cs` é **monolítico** — contém mapeamentos de todos os domínios (~25+ mapeamentos)
- Para projetos grandes, considerar separar profiles por domínio (`ProdutoProfile`, `ClienteProfile`, etc.)
- AutoMapper 8.1.1 — API estável, mas versão mais recente tem melhorias de performance

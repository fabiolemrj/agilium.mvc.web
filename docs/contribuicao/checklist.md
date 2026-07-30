# Checklist de Desenvolvimento

## Objetivo

Checklist completo para validação antes, durante e após o desenvolvimento de novas funcionalidades no Agilium Manager, garantindo adesão aos padrões arquiteturais do projeto.

---

## Antes de Começar

☐ Li e entendi o requisito completamente

☐ Identifiquei a(s) camada(s) afetada(s): MVC / API / Business / Infra

☐ Verifiquei se há testes existentes relacionados

☐ Consultei a documentação do domínio em `docs/dominio/`

☐ Revisei os padrões em `docs/padroes/`

---

## Durante o Desenvolvimento

### Nova Feature

☐ Criei Model na camada Business

☐ Criei Enum (se necessário) em `agilium-manager-azure-business/Enums/`

☐ Criei Interface do Repository em `Interfaces/IRepository/`

☐ Implementei Repository herdando `Repository<T>`

☐ Criei Interface do Service em `Interfaces/IService/`

☐ Implementei Service herdando `BaseService`

☐ Criei FluentValidation em `Models/Validations/`

☐ Registrei DI em `ResolveDependencyConfig.cs`

☐ Criei ViewModel em `ViewModels/{Dominio}/`

☐ Adicionei mapeamento AutoMapper em `AutomapperConfig.cs`

☐ Criei Controller herdando `MainController`

☐ Adicionei `[Authorize]` no Controller

☐ Adicionei `[ClaimsAuthorizeAttribute(idTag)]` nas ações

☐ Criei Views (Index + CreateEdit)

☐ Adicionei `_ValidationScriptsPartial` na seção Scripts

☐ Criei testes unitários para o Service

### Correção de Bug

☐ Reproduzi o bug localmente

☐ Identifiquei a causa raiz

☐ Corrigi na camada correta (não apenas o sintoma)

☐ Adicionei teste que reproduz o bug (regressão)

---

## Antes de Commitar

☐ `dotnet build` compila sem erros

☐ Testes existentes ainda passam

☐ Controller herda de `MainController`

☐ Controller tem `[Authorize]`

☐ Service herda de `BaseService`

☐ Service usa `ExecutarValidacao()` antes de persistir

☐ Service usa `AdicionarSemSalvar` / `AtualizarSemSalvar`

☐ Repository herda de `Repository<T>`

☐ ViewModel usa Data Annotations

☐ AutoMapper configurado em `AutomapperConfig.cs`

☐ DI registrado em `ResolveDependencyConfig.cs`

☐ Métodos async com sufixo `Async`

☐ Sem `.Result` / `.Wait()` bloqueante

☐ Sem `Console.WriteLine` solto

☐ Sem connection strings hardcoded

☐ Sem lógica de negócio no Controller

☐ Sem acesso a Repository pelo Controller

☐ Form POST com `@Html.AntiForgeryToken()`

---

## Auto-Review

| Item | ✔️ |
|------|-----|
| Nomes descritivos e consistentes (PascalCase) | |
| Métodos pequenos e com responsabilidade única | |
| Injeção de dependência via construtor | |
| Async/await de ponta a ponta | |
| Validação: FluentValidation + Notification Pattern | |
| Consultas somente leitura com `AsNoTracking()` / `Buscar()` | |
| Tratamento de erros adequado | |
| Sem código duplicado | |
| Sem complexidade desnecessária | |
| Testes cobrem cenários principais | |

---

## Documentação

☐ Atualizado `docs/dominio/{dominio}.md` se novas regras

☐ Atualizado `docs/fluxos/` se novo fluxo

☐ Atualizado `docs/api/endpoints.md` se novo endpoint

☐ Atualizado `docs/diagrams/` se mudança arquitetural

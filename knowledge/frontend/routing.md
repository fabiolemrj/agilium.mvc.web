# Rotas e Navegação

## Objetivo

Documentar o sistema de rotas do Agilium Manager MVC, incluindo padrões de URL, convenções de nomenclatura e mecanismos de navegação.

---

# Visão Geral

As rotas são configuradas em `Startup.Configure()` via `UseEndpoints`. O sistema utiliza 4 padrões de rota: Razor Pages (Identity), Controllers (API), Areas e rota default MVC. Os controllers usam atributos `[Route]` para prefixos personalizados.

---

# Organização

### Rotas Configuradas (Startup.cs)

```csharp
endpoints.MapRazorPages();                                          // Identity
endpoints.MapControllers();                                         // API
endpoints.MapControllerRoute("areas", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
endpoints.MapAreaControllerRoute("Back", "Back", "back/{controller=Home}/{action=Index}/{id?}");
endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
```

### Padrões de Rota por Controller

| Controller | Rota Base | Exemplo |
|------------|-----------|---------|
| `HomeController` | `/` | `/Home/Index` |
| `CompraController` | `/compra` | `/compra/lista`, `/compra/novo` |
| `ProdutoController` | `/produto` | `/produto/novo`, `/produto/editar` |
| `VendaController` | `/venda` | `/venda/index` |
| `EmpresaController` | `/empresa` | `/Empresa/SelecionarEmpresa` |
| `Identity` | `/Identity/Account/` | `/Identity/Account/Login` |

---

# Principais Conceitos

- **Attribute Routing**: Controllers definem prefixo via `[Route("compra")]`
- **Conventional Routing**: Rota default `{controller}/{action}/{id?}`
- **Areas**: Identity usa Razor Pages em `Areas/Identity/`
- **Area "Back"**: Rota alternativa `back/{controller}/{action}/{id?}`
- **Redirecionamentos**: `RedirectToAction()`, `RedirectToRoute()`, `Redirect()`

---

# Navegação

- **Sidebar**: `_ASideMenu.cshtml` — menu lateral com links para controllers
- **Navbar**: `_LoginPartial.cshtml` — usuário logado, empresa selecionada, logout
- **Breadcrumbs**: Não implementados nativamente
- **Modal global**: `#myModal` para carregar partials via AJAX

---

# Fluxos Relacionados

- `docs/fluxos/fluxo-autenticacao.md` — Redirecionamento pós-login

---

# Componentes Relacionados

- `Startup.cs` — Configuração de rotas
- `_ASideMenu.cshtml` — Menu de navegação
- `_LoginPartial.cshtml` — Header com navegação

---

# APIs Relacionadas

- N/A

---

# Boas Práticas

- Usar `[Route]` para prefixos descritivos por controller
- Usar `RedirectToAction()` para redirecionamentos internos
- Evitar URLs hardcoded — usar `asp-action`, `asp-controller`, `asp-route-*`

---

# ADRs Relacionadas

Consultar:

`knowledge/decisions.md`

---

# Documentação Relacionada

- `docs/frontend/mvc.md` — Configuração de rotas
- `docs/frontend/layouts.md` — Menu de navegação

---

# Documentação Oficial

`docs/frontend/`

---

# Fluxo Recomendado para Agentes de IA

1. Verificar `Startup.cs` — `UseEndpoints()` para rotas
2. Verificar `[Route]` attributes em cada controller
3. Verificar `_ASideMenu.cshtml` para links de navegação
4. Verificar `_LoginPartial.cshtml` para header

---

# Resumo

Rotas MVC com attribute routing e conventional routing. A navegação principal é via sidebar AdminLTE (`_ASideMenu.cshtml`). Identity usa Razor Pages em área separada.

# Navegação

## Objetivo

Documentar a estrutura de navegação do Agilium Manager: menus, breadcrumbs e experiência do usuário na interface.

---

# Visão Geral

A navegação principal é feita pela **sidebar AdminLTE** (`_ASideMenu.cshtml`), complementada pela **navbar superior** (`_LoginPartial.cshtml`). Não há breadcrumbs implementados nativamente. O modal global `#myModal` é usado para operações rápidas sem sair da página.

---

# Organização

### Sidebar (`_ASideMenu.cshtml`)

Menu lateral com ícones Font Awesome, organizado por módulos:

```
📊 Dashboard        → Home/Index
📦 Produtos         → Produto/Index
🛒 Compras          → Compra/IndexCompra
💰 Vendas           → Venda/Index
💵 Caixa            → Caixa/Index
👥 Clientes         → Cliente/Index
🏭 Fornecedores     → Fornecedor/Index
👤 Funcionários     → Funcionario/Index
🏢 Empresas         → Empresa/Index
📦 Estoque          → Estoque/Index
🔄 Inventário       → Inventario/Index
📋 Pedidos          → Pedido/Index
💳 Financeiro       → Conta/Index
📊 Plano de Contas  → PlanoConta/Index
🏷️ Categorias Fin.  → CategoriaFinanceira/Index
💲 Formas de Pagto. → FormaPagamento/Index
💱 Moedas           → Moeda/Index
🎫 Vales            → Vale/Index
🔧 Configurações    → Config/Index
📝 Logs             → Log/Index
```

### Navbar Superior

- Nome do usuário logado
- Empresa selecionada
- Link para selecionar outra empresa
- Logout

---

# Principais Conceitos

- **Sidebar AdminLTE**: `sidebar-mini`, `sidebar-light-navy`, colapsável
- **Ícones**: Font Awesome 5 (`fas fa-*`)
- **Links**: `asp-action` e `asp-controller` Tag Helpers
- **Modal global**: `#myModal` carrega partials sem navegação completa
- **Botão Voltar**: `history.back()` via jQuery

---

# Fluxos Relacionados

- `docs/fluxos/fluxo-autenticacao.md` — Pós-login → Home

---

# Componentes Relacionados

- `_ASideMenu.cshtml` — Menu lateral
- `_LoginPartial.cshtml` — Header
- `#myModal` — Modal global AJAX

---

# APIs Relacionadas

- N/A

---

# Boas Práticas

- Links usar `asp-action` e `asp-controller` (nunca hardcoded)
- Menu refletir permissões do usuário (esconder itens não autorizados)
- Sidebar colapsável para telas menores
- Modal para operações rápidas (evitar navegação completa)

---

# ADRs Relacionadas

Consultar:

`knowledge/decisions.md`

---

# Documentação Relacionada

- `docs/frontend/layouts.md` — Layout AdminLTE
- `knowledge/frontend/routing.md` — Rotas

---

# Documentação Oficial

`docs/frontend/`

---

# Fluxo Recomendado para Agentes de IA

1. Verificar `_ASideMenu.cshtml` para estrutura do menu
2. Verificar `_LoginPartial.cshtml` para header
3. Ao adicionar novas páginas, incluir link no menu
4. Respeitar permissões ao renderizar itens do menu

---

# Resumo

Navegação via sidebar AdminLTE com ícones Font Awesome, navbar superior com usuário/empresa/logout, e modal global AJAX para operações rápidas sem navegação completa.

# Performance

## Objetivo

Documentar as estratégias de otimização de performance do frontend do Agilium Manager: carregamento de assets, cache e boas práticas.

---

# Visão Geral

A performance do frontend é otimizada através de: carregamento de bibliotecas via CDN (jQuery, Bootstrap), arquivos estáticos locais minificados (AdminLTE), consultas server-side com Dapper para dados pesados e paginação em listagens.

---

# Organização

### Estratégias de Carregamento

| Recurso | Estratégia | Motivo |
|---------|-----------|--------|
| jQuery 3.6.0 | CDN | Cache cross-site, menor latência |
| Bootstrap 4.5.3/4.6.2 | CDN | Cache cross-site |
| AdminLTE | Local (`dist/`) | Controle de versão |
| Plugins | Local (`dist/plugins/`) | Controle de versão |
| site.css/site.js | Local com `asp-append-version` | Cache busting |

### Otimizações Server-Side

- **AsNoTracking()**: Consultas EF Core somente leitura
- **Dapper**: Consultas complexas e relatórios
- **Paginação**: `PagedResult<T>` com page size configurável
- **Partial Views**: Carregamento sob demanda via AJAX

---

# Principais Conceitos

- **CDN**: jQuery e Bootstrap do CDN para cache cross-site
- **Minificação**: AdminLTE e plugins já vêm minificados
- **Cache busting**: `asp-append-version="true"` em scripts locais
- **Lazy loading**: Modais e partials carregados via AJAX sob demanda
- **Overlay de loading**: Feedback visual durante operações assíncronas
- **DataTables server-side**: Paginação e busca processadas no servidor

---

# Fluxos Relacionados

- N/A (performance é transversal)

---

# Componentes Relacionados

- `_main.cshtml` — Carregamento de assets
- `PaginacaoViewComponent` — Paginação
- DataTables — Tabelas com paginação server-side

---

# APIs Relacionadas

- N/A

---

# Boas Práticas

- Usar CDN para bibliotecas grandes (jQuery, Bootstrap)
- Minificar CSS/JS customizados em produção
- Paginação em todas as listagens
- `AsNoTracking()` em consultas somente leitura
- Dapper para relatórios e consultas complexas
- Evitar carregar todos os plugins em todas as páginas

---

# ADRs Relacionadas

Consultar:

`knowledge/decisions.md`

---

# Documentação Relacionada

- `docs/frontend/framework.md` — Estrutura de assets
- `docs/padroes/dapper.md` — Dapper para performance
- `docs/padroes/efcore.md` — EF Core otimizações

---

# Documentação Oficial

`docs/frontend/`

---

# Fluxo Recomendado para Agentes de IA

1. Verificar `_main.cshtml` para ordem de carregamento de scripts
2. Usar `asp-append-version` para cache busting em assets locais
3. Implementar paginação em novas listagens
4. Preferir Dapper para consultas com 3+ joins

---

# Resumo

CDN para jQuery/Bootstrap, AdminLTE local minificado, cache busting com `asp-append-version`, Dapper para consultas pesadas, paginação em listagens, carregamento lazy de modais.

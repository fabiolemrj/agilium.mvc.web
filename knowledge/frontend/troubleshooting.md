# Troubleshooting do Frontend

## Objetivo

Documentar problemas comuns do frontend do Agilium Manager e suas soluções rápidas.

---

# Visão Geral

Problemas comuns no frontend MVC incluem: validação que não funciona, modais AJAX com erro, DataTables não inicializando, Select2 não aplicando, CSS quebrado após atualização e erros de rota.

---

# Problemas Comuns

### Validação Client-Side Não Funciona

**Sintoma**: Formulário não valida no cliente, só no servidor.

**Causas prováveis**:
- `_ValidationScriptsPartial.cshtml` não incluído na View
- Ordem dos scripts incorreta (jquery → jquery.validate → jquery.validate.unobtrusive)
- Data Annotations ausentes no ViewModel

**Solução**: Verificar `@section Scripts { <partial name="_ValidationScriptsPartial" /> }`

---

### Modal AJAX Não Carrega

**Sintoma**: Modal abre vazio ou com erro.

**Causas prováveis**:
- URL da action incorreta
- Action não retorna `PartialView()`
- Erro no servidor (verificar console do navegador)

**Solução**: Verificar network tab para resposta do servidor. Action deve retornar `PartialView()`.

---

### DataTables Não Inicializa

**Sintoma**: Tabela aparece sem paginação/busca.

**Causas prováveis**:
- Script DataTables não carregado
- Inicialização antes do DOM pronto
- Seletor jQuery incorreto

**Solução**:
```javascript
$(document).ready(function () {
    $('#minhaTabela').DataTable({ ... });
});
```

---

### Select2 Não Aplica

**Sintoma**: Dropdown nativo em vez de Select2.

**Causas prováveis**:
- Script Select2 não carregado
- Inicialização antes do DOM pronto
- Conflito com Bootstrap 4 (usar theme `select2-bootstrap4`)

**Solução**:
```javascript
$('.select2').select2({ theme: 'bootstrap4' });
```

---

### CSS Quebrado Após Atualização

**Sintoma**: Layout desalinhado, cores erradas.

**Causas prováveis**:
- Cache do navegador
- Ordem de carregamento de CSS alterada
- Versão do AdminLTE ou Bootstrap conflitante

**Solução**: Limpar cache (Ctrl+F5), verificar ordem no `_main.cshtml`.

---

### Sessão Expirada

**Sintoma**: Redirecionado para login durante uso.

**Causa**: Cookie expirou (3h sem sliding por inatividade).

**Solução**: Renovar login. Aumentar `ExpireTimeSpan` em `IdentityConfig.cs` se necessário.

---

### Empresa Não Selecionada

**Sintoma**: `EmpresaSelecionadaMiddleware` bloqueia acesso.

**Causa**: Session perdida (reinicialização do servidor, cookie expirado).

**Solução**: Redirecionar para seleção de empresa, limpar e refazer login.

---

# Fluxos Relacionados

- `docs/fluxos/fluxo-autenticacao.md` — Problemas de autenticação

---

# APIs Relacionadas

- N/A

---

# Boas Práticas

- Sempre verificar console do navegador (F12) para erros JS
- Verificar network tab para respostas do servidor
- Limpar cache ao testar mudanças de CSS/JS
- Usar `Try-Catch` no `$.ajax` para tratamento de erros

---

# ADRs Relacionadas

Consultar:

`knowledge/decisions.md`

---

# Documentação Relacionada

- `knowledge/frontend/validation.md` — Validação
- `knowledge/frontend/ui-components.md` — Componentes
- `knowledge/frontend/authentication.md` — Autenticação

---

# Documentação Oficial

`docs/frontend/`

---

# Fluxo Recomendado para Agentes de IA

1. Verificar console do navegador para erros JS
2. Verificar ordem de carregamento de scripts no `_main.cshtml`
3. Verificar se `_ValidationScriptsPartial` está incluído
4. Verificar se DataTables/Select2 são inicializados no `$(document).ready`
5. Para erros de servidor, verificar `INotificador` e `ModelState`

---

# Resumo

Problemas mais comuns: validação client-side ausente, modais AJAX com erro, DataTables/Select2 não inicializando e sessão expirada. A maioria se resolve verificando ordem de scripts e console do navegador.

# Gerenciamento de Estado

## Objetivo

Documentar como o estado é gerenciado no frontend do Agilium Manager, considerando o modelo server-side MVC.

---

# Visão Geral

Por ser uma aplicação **server-side MVC**, o estado é primariamente gerenciado no servidor. O cliente mantém estado mínimo através de: Session (empresa selecionada), TempData (mensagens entre redirects), Cookies (autenticação) e variáveis JavaScript para estado temporário da interface.

---

# Organização

### Estado no Servidor

| Mecanismo | Escopo | Uso |
|-----------|--------|-----|
| **Session** | Usuário/sessão | Empresa selecionada (`EmpresaUsuarioViewModel`) |
| **TempData** | Entre redirects | Mensagens de sucesso/erro, notificações |
| **ViewData/ViewBag** | Requisição atual | Título da página, dados auxiliares |
| **Cookie Auth** | 3h sliding | Identidade do usuário autenticado |

### Estado no Cliente

| Mecanismo | Uso |
|-----------|-----|
| **jQuery .data()** | Estado temporário em elementos DOM |
| **Variáveis JS** | Flags, seleções, estado de modais |
| **localStorage** | Uso limitado (tour guiado `dknotus-tour`) |

---

# Principais Conceitos

- **Session**: `EmpresaUsuarioViewModel` armazenado após seleção de empresa
- **TempData**: `TempData["Mensagem"]`, `TempData["TipoMensagem"]`, `TempData["Titulo"]`
- **INotificador (Scoped)**: Acumula erros de negócio durante a requisição
- **ModelState**: Estado de validação do formulário
- **Overlay de loading**: `#overlay` controlado via jQuery durante AJAX

---

# Fluxos Relacionados

- `docs/fluxos/fluxo-autenticacao.md` — Estado de autenticação (Cookie)
- `docs/fluxos/fluxo-configuracao.md` — Empresa na Session

---

# Componentes Relacionados

- `EmpresaSelecionadaMiddleware` — Garante empresa na Session
- `MainController` — Acesso a TempData, Session, Notificador

---

# APIs Relacionadas

- N/A

---

# Boas Práticas

- Não armazenar dados de negócio no cliente (usar Session/Model)
- Limpar estado de modais ao fechar
- Usar TempData apenas para mensagens entre redirects
- Evitar ViewBag para dados principais — usar ViewModel

---

# ADRs Relacionadas

Consultar:

`knowledge/decisions.md`

---

# Documentação Relacionada

- `docs/padroes/notification.md` — Notification Pattern (estado de erros)
- `knowledge/frontend/authentication.md` — Cookie de autenticação

---

# Documentação Oficial

`docs/frontend/`

---

# Fluxo Recomendado para Agentes de IA

1. Verificar `MainController` — acesso a Session, TempData, Notificador
2. Verificar `EmpresaSelecionadaMiddleware` — garantia de empresa na Session
3. Verificar `INotificador` (Scoped) — acumula erros por requisição
4. Para estado no cliente, usar jQuery `.data()` ou variáveis locais

---

# Resumo

Estado primariamente server-side: Session (empresa), TempData (mensagens), Cookie (auth), INotificador (erros). Cliente com estado mínimo: jQuery .data() e variáveis JS.

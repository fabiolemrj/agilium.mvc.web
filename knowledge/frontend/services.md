# Serviços Frontend

## Objetivo

Documentar a organização dos serviços na camada de apresentação do Agilium Manager, comunicação com o backend e abstrações utilizadas.

---

# Visão Geral

Como aplicação server-side MVC, os "serviços frontend" são os **serviços injetados nos Controllers** via DI. A comunicação é direta (in-process), não via HTTP/REST. Serviços específicos da camada MVC estão em `agilum.mvc.web/Services/`.

---

# Organização

```
agilum.mvc.web/Services/
├── AuthService.cs           # Autenticação customizada (IAuthService)
├── ServiceEmail.cs          # Envio de e-mails (IEmailSender)
└── CryptoService.cs         # Criptografia

agilium-manager-azure-business/Services/
├── CompraService.cs         # Lógica de negócio (injetado nos controllers)
├── VendaService.cs
├── ProdutoService.cs
└── ...                      # 40+ serviços de negócio
```

---

# Principais Conceitos

### Injeção nos Controllers

```csharp
public class CompraController : MainController
{
    public CompraController(
        ICompraService compraService,
        IEmpresaService empresaService,
        IMapper mapper,
        INotificador notificador,
        ...) : base(...)
    { }
}
```

### Serviços MVC-Específicos

| Serviço | Interface | Função |
|---------|-----------|--------|
| `AuthService` | `IAuthService` | Autenticação, refresh token |
| `ServiceEmail` | `IEmailSender` | Envio de e-mails via SMTP |
| `CryptoService` | — | Criptografia de dados |

### Comunicação AJAX

Para modais e carregamento parcial, jQuery AJAX chama actions MVC:

```javascript
$.ajax({
    url: '/compra/importar',
    type: 'POST',
    data: formData,
    success: function (result) {
        $('#myModalContent').html(result);
        $('#myModal').modal('show');
    }
});
```

---

# Fluxos Relacionados

- `docs/fluxos/` — Cada fluxo de negócio

---

# Componentes Relacionados

- `MainController` — Base com serviços injetados
- `ResolveDependencyConfig.cs` — Registro DI

---

# APIs Relacionadas

- `agilium-manager-azure-api/` — API REST (alternativa ao MVC)

---

# Boas Práticas

- Controllers dependem de interfaces, não de implementações
- Serviços MVC não devem conter lógica de negócio
- AJAX usa endpoints MVC (não API REST diretamente)
- Tratar erros de AJAX com `fail()` callback e Toastr

---

# ADRs Relacionadas

Consultar:

`knowledge/decisions.md`

---

# Documentação Relacionada

- `docs/padroes/services.md` — Padrão de serviços
- `docs/padroes/notification.md` — Notification Pattern

---

# Documentação Oficial

`docs/frontend/`

---

# Fluxo Recomendado para Agentes de IA

1. Verificar `ResolveDependencyConfig.cs` para registro de serviços
2. Verificar `MainController` para serviços base injetados
3. Verificar `agilum.mvc.web/Services/` para serviços MVC-específicos
4. Para lógica de negócio, consultar `agilium-manager-azure-business/Services/`

---

# Resumo

Serviços são injetados via DI nos Controllers. Serviços MVC-específicos em `agilum.mvc.web/Services/` (Auth, Email, Crypto). A comunicação é in-process; AJAX chama actions MVC para carregamento parcial.

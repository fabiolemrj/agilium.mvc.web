# Fluxo de Configuração

## Objetivo

Documentar os fluxos de configuração do Agilium Manager: parâmetros do sistema, configuração de e-mail, licenciamento, multi-empresa e integrações.

---

## Fluxo: Seleção de Empresa (Multi-Empresa)

```
[Após Login]
      │
      ▼
Redirecionado para seleção de empresa
      │
      ▼
EmpresaController.ObterListasEmpresasPorUsuario()
      │
      ├── Buscar empresas vinculadas ao usuário (EmpresaAuth)
      │
      ▼
[View] Lista de empresas disponíveis
      │
      ▼
[POST] /Empresa/SelecionarEmpresa
      │
      ├── Armazenar EmpresaUsuarioViewModel na Session
      │     ├── IDEMPRESA
      │     ├── NomeEmpresa
      │     └── IDUSUARIO
      │
      ▼
Redirect /Home/Index
      │
      ▼
EmpresaSelecionadaMiddleware
      └── Toda requisição subsequente exige empresa na sessão
```

---

## Fluxo: Configuração de E-mail

```
[Config] /config/index (chave/valor)

O e-mail é configurado via ConfigService (banco de dados),
usando o padrão CHAVE/VALOR por empresa:

Chaves de e-mail (EClassificacaoConfiguracao.Email):
  ├── MAIL_EMAIL (usuário)
  ├── MAIL_SMTP (servidor SMTP)
  ├── MAIL_POP (servidor POP)
  ├── MAIL_PORTA_POP (porta POP)
  ├── MAIL_PORTA_SMTP (porta SMTP)
  ├── MAIL_REMETENTE (from)
  └── MAIL_SENHA (senha)
      │
      ▼
ConfigService.ObterConfiguracoes(idEmpresa)
      │
      └── Filtra por EClassificacaoConfiguracao.Email
      │
      ▼
ServiceEmail.ObterConfigEmail(idEmpresa)
      │
      ├── Consulta ConfigService para obter valores
      ├── Popula EmailSettings:
      │     ├── PrimaryDomain (servidor SMTP)
      │     ├── PrimaryPort (porta)
      │     ├── UsernameEmail (usuário)
      │     ├── UsernamePassword (senha)
      │     ├── FromEmail (remetente)
      │     ├── ToEmail (destinatário padrão)
      │     └── CcEmail (cópia)
      │
      ▼
ServiceEmail.SendEmailAsync(email, subject, htmlMessage, idEmpresa)
      │
      └── Envia e-mail via SMTP (MailKit/System.Net.Mail)
```

---

## Fluxo: Licenciamento

```
[Acesso ao Sistema]
      │
      ▼
HomeController.Licenca()
      │
      ▼
Verificar licença da empresa:
      │
      ├── LicencaService.ObterPorIdEmpresa(idEmpresa)
      │     │
      │     ├── Licença existe?
      │     │     ├── Não → Bloquear / Solicitar ativação
      │     │     └── Sim ↓
      │     │
      │     ├── Licença válida (não expirada)?
      │     │     ├── Não → Bloquear / Solicitar renovação
      │     │     └── Sim ↓
      │     │
      │     └── Chaves de ativação válidas?
      │           ├── Descriptografar K1...K7
      │           └── Validar assinatura
      │
      ▼
Sistema liberado para uso
```

---

## Fluxo: Configurações por Empresa

```
[Config] Parâmetros da Empresa
      │
      ▼
ConfigService.ObterConfiguracoes(idEmpresa)
      │
      ├── Configurações Fiscais
      │     ├── Regime Tributário
      │     ├── Inscrição Estadual
      │     ├── CNAE
      │     └── Certificado Digital
      │
      ├── Configurações do PDV
      │     ├── Impressoras
      │     ├── Número de vias do comprovante
      │     └── Mensagem do rodapé
      │
      ├── Configurações Financeiras
      │     ├── Conta bancária padrão
      │     ├── Taxas e juros
      │     └── Dias de vencimento padrão
      │
      └── Configurações de Integração
            ├── Cardapio Digital (ConnectionString + ApiBaseUrl)
            ├── Site Mercado (marketplace)
            ├── WhatsApp
            └── E-mail
```

---

## Fluxo: Integração com Cardápio Digital

```
[Config] CardapioDigital
      │
      ▼
appsettings.json:
  "CardapioDigital": {
    "ConnectionString": "Server=...;Database=cardapio_digital;...",
    "ApiBaseUrl": "http://localhost:5555"
  }
      │
      ▼
IntegracaoCardapioService
  ├── SincronizarProduto(produto)
  │     └── Envia produto para API do cardápio
  │
  ├── SincronizarPreco(produto, preco)
  │     └── Atualiza preço no cardápio
  │
  └── SincronizarFoto(produto, foto)
        └── Envia foto para o cardápio
```

---

## Fluxo: Versão do Sistema

```
[Inicialização]
      │
      ▼
appsettings.json:
  "ConnectionStrings": {
    "versaobd-major": "8",
    "versaobd-minor": "0",
    "versaobd-build": "19",
    "versao-major": "1",
    "versao-minor": "0",
    "versao-build": "0"
  }
      │
      ▼
Startup.ConfigureServices()
      └── Valida compatibilidade da versão do banco
```

---

## Fluxo: Render (Cloud Deployment)

```
[Inicialização] Program.cs
      │
      ▼
Detectar ambiente Render:
  var isRender = Environment.GetEnvironmentVariable("RENDER") != null;
      │
      ├── Se Render:
      │     ├── NÃO usar UseHttpsRedirection (Render gerencia HTTPS)
      │     ├── NÃO usar UseHsts
      │     └── Porta = Environment.GetEnvironmentVariable("PORT") ?? "5000"
      │
      └── Se não Render:
            ├── Usar UseHttpsRedirection
            └── Usar UseHsts (em produção)
```

---

## Entidades de Configuração

| Entidade | Papel |
|----------|-------|
| `Config` | Parâmetros de configuração |
| `ConfigImagem` | Logotipos e imagens |
| `Empresa` | Dados cadastrais e configurações |
| `Licenca` | Licenciamento por empresa |
| `VersaoSistema` | Controle de versão |
| `EmpresaAuth` | Vínculo usuário-empresa |

---

## Serviços Envolvidos

- `ConfigService`
- `EmpresaService`
- `LicencaService`
- `ServiceEmail`
- `PassCryptoService`
- `IntegracaoCardapioService`
- `SiteMercadoService`

---

## Regras de Negócio

- Usuário deve selecionar empresa **após login**
- Empresa inativa **não permite** operações
- Licença expirada **bloqueia** o sistema
- Configurações são **por empresa** (não globais)
- Connection strings são injetadas via **variáveis de ambiente** em produção
- Render requer configurações especiais de HTTPS

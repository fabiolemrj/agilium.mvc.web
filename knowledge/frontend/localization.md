# Localização

## Objetivo

Documentar a configuração de localização, formatação de dados regionais e internacionalização do Agilium Manager.

---

# Visão Geral

O Agilium Manager é uma aplicação voltada ao mercado brasileiro, com localização fixa em **pt-BR**. A cultura é configurada em `Startup.Configure()` com:
- Decimal: vírgula (`,`) 
- Agrupamento de milhar: ponto (`.`)
- Data curta: `dd/MM/yyyy`
- Data longa: `dd/MM/yyyy hh:mm:ss tt`

---

# Organização

### Configuração em Startup.cs

```csharp
var cultura = new CultureInfo("pt-BR");
cultura.NumberFormat.NumberDecimalSeparator = ",";
cultura.NumberFormat.NumberGroupSeparator = ".";

var dateformat = new DateTimeFormatInfo
{
    ShortDatePattern = "dd/MM/yyyy",
    LongDatePattern = "dd/MM/yyyy hh:mm:ss tt"
};
cultura.DateTimeFormat = dateformat;

app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("pt-BR"),
    SupportedCultures = new[] { cultura },
    SupportedUICultures = new[] { cultura }
});
```

---

# Principais Conceitos

- **pt-BR fixo**: Não há suporte a múltiplos idiomas
- **Formato numérico**: `1.234,56` (vírgula decimal, ponto de milhar)
- **Formato de data**: `29/07/2026` (dd/MM/yyyy)
- **Fuso horário**: Horário local do servidor
- **Idioma da interface**: Português brasileiro em todas as Views
- **Plugins localizados**: DataTables, Select2, DateRangePicker com locale pt-BR

---

# Fluxos Relacionados

- N/A (localização é transversal)

---

# Componentes Relacionados

- `Startup.cs` — Configuração de cultura
- `_main.cshtml` — `<html lang="pt-br">`
- Plugins com locale pt-BR
- `MoneyInputTagHelper` — Formatação monetária

---

# APIs Relacionadas

- N/A

---

# Boas Práticas

- Sempre usar `pt-BR` para formatação de dados
- Plugins devem ser configurados com locale `pt-BR`
- Datas no formato `dd/MM/yyyy`
- Valores monetários no formato `R$ 1.234,56`
- Mensagens de erro em português

---

# ADRs Relacionadas

Consultar:

`knowledge/decisions.md`

---

# Documentação Relacionada

- `docs/frontend/framework.md` — Plugins e locale
- `docs/frontend/mvc.md` — Configuração no pipeline

---

# Documentação Oficial

`docs/frontend/`

---

# Fluxo Recomendado para Agentes de IA

1. Verificar `Startup.cs` — Cultura pt-BR
2. Verificar `_main.cshtml` — `lang="pt-br"`
3. Configurar plugins com locale pt-BR
4. Usar `MoneyInputTagHelper` para campos monetários

---

# Resumo

Localização fixa pt-BR: vírgula decimal, data dd/MM/yyyy, interface em português. Configurado via `CultureInfo` no Startup e `UseRequestLocalization`.

# Testes de Frontend

## Objetivo

Documentar a estratégia de testes do frontend do Agilium Manager: testes unitários, de integração e End-to-End.

---

# Visão Geral

O projeto `agilum.mvc.web.tests` contém testes unitários para a camada MVC. Por ser uma aplicação server-side, os testes focam em: mapeamentos AutoMapper, lógica de controllers e validações. Não há testes End-to-End com Selenium/Playwright configurados.

---

# Organização

```
agilum.mvc.web.tests/
├── agilum.mvc.web.tests.csproj
├── ProdutoExportarPedidoMappingTests.cs
├── bin/
└── obj/
```

---

# Principais Conceitos

- **Testes unitários**: xUnit (a confirmar) para serviços e mapeamentos
- **AutoMapper**: Testes de mapeamento Model → ViewModel
- **FluentValidation**: Validadores testados individualmente
- **Controllers**: Testes de ações com serviços mockados

---

# Fluxos Relacionados

- N/A

---

# Componentes Relacionados

- `agilum.mvc.web.tests/` — Projeto de testes

---

# APIs Relacionadas

- N/A

---

# Boas Práticas

- Testar mapeamentos AutoMapper (Model → ViewModel)
- Testar validadores FluentValidation com casos válidos e inválidos
- Mockar serviços e repositórios em testes de controller
- Testar redirecionamentos e retornos de View
- Verificar `OperacaoValida()` após chamadas de serviço

---

# ADRs Relacionadas

Consultar:

`knowledge/decisions.md`

---

# Documentação Relacionada

- `docs/padroes/validacoes.md` — Validadores testáveis
- `docs/padroes/automapper.md` — Mapeamentos testáveis

---

# Documentação Oficial

`docs/frontend/`

---

# Fluxo Recomendado para Agentes de IA

1. Verificar `agilum.mvc.web.tests/` para testes existentes
2. Adicionar testes para novos mapeamentos AutoMapper
3. Testar novos validadores FluentValidation
4. Mockar `INotificador`, `IMapper`, serviços nos testes de controller

---

# Resumo

Testes unitários no projeto `agilum.mvc.web.tests/` focados em AutoMapper, FluentValidation e Controllers. Sem testes E2E configurados.

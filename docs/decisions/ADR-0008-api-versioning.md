# ADR-0008 - Estratégia de Versionamento de APIs

| Campo | Valor |
|-------|-------|
| **Status** | Accepted |
| **Data** | 2026-07-29 |
| **Autor** | Equipe Agilium |
| **Versão** | 1.0 |

---

# Contexto

O Agilium Manager disponibiliza APIs REST consumidas por diversos sistemas da plataforma, incluindo:

- Agilium Manager (MVC)
- Agilium PDV
- Agilium Mobile
- Cardápio Digital
- Sistemas de Licenciamento
- Integrações de terceiros
- Futuras aplicações

Esses sistemas possuem ciclos de atualização independentes.

Ao longo da evolução da plataforma, novos recursos serão adicionados, contratos poderão evoluir e alguns endpoints poderão sofrer alterações incompatíveis com versões anteriores.

Era necessário estabelecer uma estratégia oficial para evolução das APIs sem interromper o funcionamento dos clientes existentes.

---

# Problema

Alterações incompatíveis em uma API podem causar:

- Quebra de aplicações em produção;
- Necessidade de atualização simultânea de todos os clientes;
- Alto risco em implantações;
- Dificuldade para manter compatibilidade;
- Código legado difícil de remover.

Era necessário definir uma política clara para evolução dos contratos da API.

---

# Alternativas Consideradas

## Alternativa 1 — Sem Versionamento

### Vantagens

- Implementação simples.
- Apenas uma versão da API.

### Desvantagens

- Qualquer alteração pode quebrar clientes.
- Alto risco em produção.
- Evolução limitada.

---

## Alternativa 2 — Versionamento por Header

Exemplo:

```
Api-Version: 2
```

### Vantagens

- URL permanece limpa.
- Compatível com REST.

### Desvantagens

- Pouco intuitivo.
- Mais difícil para testes manuais.
- Maior complexidade em ferramentas externas.

---

## Alternativa 3 — Versionamento por QueryString

Exemplo:

```
/api/produtos?version=2
```

### Vantagens

- Fácil implementação.

### Desvantagens

- Pouco utilizado.
- Não representa corretamente o recurso.
- Pode gerar ambiguidades.

---

## Alternativa 4 — Versionamento por URL (Escolhida)

Exemplo:

```
/api/v1/produtos
/api/v2/produtos
```

### Vantagens

- Simples.
- Explícito.
- Fácil documentação.
- Excelente suporte pelo Swagger.
- Fácil utilização por clientes.
- Ampla adoção pela comunidade.

### Desvantagens

- URLs maiores.
- Necessidade de manter múltiplas versões quando necessário.

---

# Decisão

Foi adotado o **versionamento por URL** como estratégia oficial para todas as APIs do Agilium Manager.

Toda API pública deverá possuir sua versão explicitamente definida na rota.

Exemplo:

```text
/api/v1/produtos
/api/v1/clientes
/api/v1/vendas
```

Quando houver alterações incompatíveis com versões anteriores, deverá ser criada uma nova versão da API.

---

# Objetivos

A estratégia possui os seguintes objetivos:

- Garantir compatibilidade entre versões.
- Permitir evolução segura da API.
- Reduzir impacto em clientes existentes.
- Facilitar manutenção.
- Possibilitar descontinuação planejada de versões antigas.

---

# Convenção de Rotas

Formato oficial:

```text
/api/v{versão}/{recurso}
```

Exemplos:

```text
/api/v1/clientes

/api/v1/produtos

/api/v1/usuarios

/api/v1/vendas

/api/v2/vendas
```

---

# Quando criar uma nova versão

Uma nova versão deverá ser criada quando ocorrer:

- Alteração incompatível no contrato.
- Remoção de propriedades.
- Alteração de tipos.
- Mudança obrigatória de parâmetros.
- Mudança de comportamento incompatível.
- Alteração de autenticação.
- Alteração significativa do modelo de resposta.

---

# Quando NÃO criar nova versão

Não é necessário criar nova versão quando houver:

- Inclusão de novos endpoints.
- Inclusão de propriedades opcionais.
- Correções internas.
- Melhorias de desempenho.
- Correções de bugs.
- Alterações sem impacto no contrato público.

---

# Compatibilidade

As versões anteriores deverão permanecer disponíveis durante o período de suporte definido pela equipe.

Sempre que possível:

- Não remover endpoints existentes.
- Não alterar contratos públicos.
- Não alterar comportamento esperado pelos clientes.

---

# Depreciação

Quando uma versão deixar de ser recomendada:

- Ela deverá ser marcada como **Deprecated**.
- A documentação deverá informar a versão substituta.
- O prazo de remoção deverá ser comunicado aos consumidores.

Exemplo:

```
v1 → Deprecated

↓

v2 → Current
```

---

# Organização dos Controllers

Estrutura recomendada:

```text
Controllers/

├── V1/

│   ├── ProdutoController.cs

│   ├── ClienteController.cs

│   └── VendaController.cs

│

├── V2/

│   ├── ProdutoController.cs

│   └── VendaController.cs
```

---

# Swagger

Cada versão deverá possuir documentação própria.

Exemplo:

```
Swagger

↓

v1

↓

v2

↓

v3
```

Cada versão deverá documentar apenas os endpoints correspondentes.

---

# Versionamento do Modelo

Sempre que houver alteração incompatível nos DTOs:

Criar novos modelos.

Exemplo:

```text
ProdutoResponseV1

ProdutoResponseV2

VendaRequestV2
```

Evitar reutilizar modelos incompatíveis entre versões.

---

# Versionamento de Banco

Mudanças no banco de dados não exigem necessariamente nova versão da API.

A API deverá preservar seu contrato público independentemente da evolução interna da persistência.

---

# Benefícios

- Evolução segura.
- Compatibilidade entre clientes.
- Melhor organização.
- Facilidade de documentação.
- Facilidade para testes.
- Baixo impacto em produção.

---

# Desvantagens

- Necessidade de manter múltiplas versões.
- Maior quantidade de Controllers e DTOs.
- Manutenção adicional durante o período de coexistência.

---

# Riscos

Caso esta estratégia não seja seguida:

- Quebra de clientes.
- Atualizações obrigatórias.
- Alto risco em deploys.
- Perda de compatibilidade.
- Dificuldade para evoluir a API.

---

# Impacto

Esta decisão impacta:

- APIs REST
- Controllers
- DTOs
- Swagger
- Clientes Mobile
- PDV
- Cardápio Digital
- Integrações
- Documentação

---

# Plano de Implementação

1. Padronizar todas as rotas utilizando `/api/v{versão}`.
2. Configurar suporte a versionamento no ASP.NET Core.
3. Organizar Controllers por versão.
4. Criar documentação Swagger por versão.
5. Versionar DTOs quando necessário.
6. Definir política de depreciação.
7. Atualizar documentação técnica.

---

# Critérios de Aceitação

Uma implementação é considerada aderente quando:

- Todas as APIs públicas possuem versão na URL.
- Alterações incompatíveis geram nova versão.
- Swagger documenta cada versão separadamente.
- Controllers são organizados por versão.
- DTOs incompatíveis são versionados.
- Versões antigas permanecem disponíveis durante o período de suporte.

---

# ADRs Relacionados

- ADR-0001 — Arquitetura em Camadas
- ADR-0002 — Repository Pattern
- ADR-0005 — Estratégia de Autenticação
- ADR-0006 — Estratégia de Autorização
- ADR-0015 — Padronização das Respostas da API
- ADR-0019 — Estratégia de Migrations

---

# Referências

- Microsoft — ASP.NET Core API Versioning
- Microsoft — REST API Guidelines
- Microsoft REST API Design Guidelines
- Richardson Maturity Model
- RESTful Web APIs — Leonard Richardson & Mike Amundsen

---

# Histórico

| Versão | Data | Descrição |
|---------|------|-----------|
| 1.0 | 2026-07-29 | Criação da ADR definindo o versionamento por URL (`/api/v{versão}`) como estratégia oficial para evolução das APIs do Agilium Manager, garantindo compatibilidade entre clientes e evolução segura dos contratos públicos. |
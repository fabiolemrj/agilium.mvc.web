# 📋 Levantamento de Contexto — Integração Agilium Manager ↔ CardapioDigital

> **Data:** 2026-07-14  
> **Projeto:** Agilium Manager MVC (agilum.mvc.web)  
> **Sistema externo:** CardapioDigital.API (.NET 8)  
> **Objetivo:** Documentar o contexto de integração bidirecional entre os sistemas

---

## 1. Visão Geral dos Sistemas

| Aspecto | Agilium Manager | CardapioDigital |
| --- | --- | --- |
| **Propósito** | ERP completo (PDV, estoque, fiscal, financeiro) | Cardápio digital + pedidos online |
| **.NET** | Core 3.1 (⚠️ EOL) | 8.0 |
| **Banco** | MySQL 8 (`agiliumadm`) | MySQL 8 (`cardapio_digital`) |
| **ORM** | EF Core 3.1 + Dapper | EF Core 8 (Pomelo) |
| **Arquitetura** | MVC + camadas separadas | Clean Architecture (Domain → App → Infra → API) |
| **Autenticação** | ASP.NET Identity + Claims (tags numéricas) | JWT Bearer (admin) + 2FA WhatsApp (cliente) |
| **Frontend** | Razor Views + AdminLTE | API REST + frontend separado (nginx) |

---

## 2. Contexto de Integração

```text
┌──────────────────────────┐                         ┌──────────────────────────┐
│     AGILIUM MANAGER       │                         │    CARDAPIODIGITAL        │
│     (ERP / PDV)           │                         │    (Cardápio Online)      │
│                           │                         │                          │
│  ┌─────────────────┐     │   ① EXPORTAR PRODUTOS    │  ┌─────────────────┐     │
│  │  produto         │     │◄────────────────────────│  │  produto         │     │
│  │  STEXPORTARPEDIDO│     │   GET /api/Produtos      │  │  (cardápio)     │     │
│  │  (Sim/Não)       │     │   GET /api/Produtos/     │  │                 │     │
│  └─────────────────┘     │      /categorias          │  └─────────────────┘     │
│                           │                         │                          │
│  ┌─────────────────┐     │   ② IMPORTAR PEDIDOS     │  ┌─────────────────┐     │
│  │  pedido          │     │─────────────────────────▶│  │  pedido          │     │
│  │  venda           │     │   POST /api/Pedido       │  │  (delivery)     │     │
│  └─────────────────┘     │                         │  └─────────────────┘     │
│                           │                         │                          │
│  ┌─────────────────┐     │   ③ CONSULTAR STATUS      │  ┌─────────────────┐     │
│  │  status_pedido   │     │◄────────────────────────│  │  status          │     │
│  └─────────────────┘     │   GET /api/Pedido/{id}    │  └─────────────────┘     │
│                           │                         │                          │
└──────────────────────────┘                         └──────────────────────────┘
```

### Fluxo ① — Exportar Produtos (CardapioDigital → Agilium)

**Campo já implementado:** `produto.STEXPORTARPEDIDO` (enum `ESimNao`)

1. Agilium consulta periodicamente `GET /api/Produtos` e `GET /api/Produtos/categorias`
2. Produtos com `STEXPORTARPEDIDO = Sim` são enviados/atualizados no CardapioDigital
3. Mapeamento de IDs mantido em tabela de equivalência local

### Fluxo ② — Importar Pedidos (Agilium → CardapioDigital)

1. Pedido criado no PDV Agilium
2. Se o pedido for do tipo "delivery online", enviar via `POST /api/Pedido`
3. CardapioDigital processa e retorna `PedidoResponse` com ID e status

### Fluxo ③ — Consultar Status (CardapioDigital → Agilium)

1. Polling periódico via `GET /api/Pedido/{id}`
2. Atualizar status local conforme máquina de estados do CardapioDigital

---

## 3. Mapeamento de Entidades

### 3.1 Produto

| Agilium (`produto`) | CardapioDigital (`produto`) | Observação |
| --- | --- | --- |
| `IDPRODUTO` (PK) | `id_produto_agilium` BIGINT | **Chave de match** — PK do Agilium |
| `CDPRODUTO` | `cd_produto_pdv` VARCHAR(50) | Código interno — **guia visual** |
| `NMPRODUTO` | `nome` | Nome do produto |
| `DSVOLUME` / descrição | `descricao` | Descrição detalhada |
| `NUPRECO` | `preco` | Preço de venda |
| — | `preco_promocional` | Preço promocional (se houver) |
| — | `promocao_ativa` | Flag de promoção ativa |
| Imagem (wwwroot) | `imagem_url` | URL da imagem |
| `IDGRUPO` → GrupoProduto | `categoria_id` | Categoria do produto |
| `STPRODUTO` (EAtivo) | `ativo` | Se está disponível |
| — | `destaque` | Flag de destaque |
| **`STEXPORTARPEDIDO`** ✅ | — | **Filtro: só exportar se Sim** |

### 3.2 Categoria → Grupo

| Agilium (`grupoproduto`) | CardapioDigital (`categoria`) |
| --- | --- |
| `IDGRUPO` (PK) | `id` (PK) |
| `CDGRUPO` | — |
| `Nome` | `nome` |
| — | `descricao` |
| `StAtivo` | `ativo` |

### 3.3 Pedido

| Agilium (`venda` / `pedido`) | CardapioDigital (`pedido`) |
| --- | --- |
| ID venda | `id` (retornado na resposta) |
| Cliente (telefone) | `cliente_telefone` |
| Cliente (nome) | `cliente_nome` |
| Endereço completo | `endereco` (objeto aninhado) |
| Subtotal | `subtotal` |
| Taxa de entrega | `taxa_entrega` |
| Total | `total` |
| Forma de pagamento | `forma_pagamento` |
| Troco | `troco_para` |
| Observação | `observacao` |
| Retirada na loja? | `retirar_loja` |
| Status | `status` ("Recebido" / "Em Preparo" / ...) |

### 3.4 Item do Pedido

| Agilium (`venda_item`) | CardapioDigital (`item_pedido`) |
| --- | --- |
| `IDPRODUTO` → mapear ID | `produto_id` |
| Quantidade | `quantidade` |
| Preço unitário | `preco_unitario` |
| Subtotal | `subtotal` |

---

## 4. CardapioDigital — Endpoints Relevantes para Integração

### 4.1 Endpoints Públicos (sem autenticação)

| Método | Endpoint | Uso no Agilium |
| --- | --- | --- |
| `GET` | `/api/Produtos` | **Exportar catálogo** de produtos ativos |
| `GET` | `/api/Produtos/{id}` | Buscar produto específico por ID |
| `GET` | `/api/Produtos/categorias` | **Exportar categorias** (grupos) |
| `POST` | `/api/Pedido` | **Importar pedido** do PDV |
| `GET` | `/api/Pedido/{id}` | **Consultar status** de pedido |
| `GET` | `/api/Pedido/telefone/{tel}` | Consultar pedidos de um cliente |
| `POST` | `/api/frete/calcular` | Calcular frete antes de enviar pedido |

### 4.2 Endpoints Administrativos (JWT 🔒)

| Método | Endpoint | Uso no Agilium |
| --- | --- | --- |
| `POST` | `/api/auth/login` | Obter token JWT (admin) |
| `GET` | `/api/admin/pedidos` | Listar pedidos com filtros/paginação |
| `PATCH` | `/api/admin/pedidos/{id}/status` | Atualizar status de pedido |
| `POST` | `/api/admin/produtos` | Criar produto no cardápio |
| `PUT` | `/api/admin/produtos/{id}` | Atualizar produto no cardápio |
| `POST` | `/api/upload/imagem` | Upload de imagem de produto |

---

## 5. CardapioDigital — Estrutura do Projeto

```text
CardapioDigital.sln
├── CardapioDigital.Domain/          ← Entidades + Interfaces de repositório
│   └── Entities/
│       ├── Categoria.cs
│       ├── Produto.cs
│       ├── Cliente.cs
│       ├── Endereco.cs
│       ├── Pedido.cs
│       ├── ItemPedido.cs
│       └── ConfigEntrega.cs
├── CardapioDigital.Application/     ← DTOs, Services, Validators
│   ├── DTOs/
│   ├── Interfaces/
│   ├── Services/
│   └── Validators/
├── CardapioDigital.Infrastructure/  ← EF Core, Repositories, Migrations
│   ├── Data/AppDbContext.cs
│   ├── Repositories/
│   └── Migrations/
└── CardapioDigital.API/             ← Controllers + Auth
    ├── Controllers/
    └── Services/AuthService.cs
```

---

## 6. CardapioDigital — Banco de Dados

### Banco único: `cardapio_digital` (MySQL 8)

| Tabela | Descrição |
| --- | --- |
| `produto` | Cardápio (nome, preço, imagem, categoria, destaque, ativo) |
| `categoria` | Categorias do cardápio |
| `pedido` | Pedidos (cliente, endereço, status, total, forma pgto) |
| `item_pedido` | Itens do pedido |
| `cliente` | Clientes (telefone único, 2FA) |
| `endereco` | Endereços de entrega |
| `config_entrega` | Configuração de entrega (taxa fixa/distância) |
| `faixa_cep_entrega` | Faixas de CEP por taxa |
| `usuarios` | Identity (admins) |
| `roles` | Roles (Admin) |

### Tabelas planejadas (ainda não implementadas)

- `integracao_produto_equivalencia` — Mapeamento bidirecional de IDs
- `integracao_log` — Auditoria de integração

---

## 7. Status da Máquina de Pedidos (CardapioDigital)

```text
Recebido ──▶ Em Preparo ──▶ Saiu para Entrega ──▶ Entregue
    │
    └──▶ Cancelado
```

| Status | Significado |
| --- | --- |
| `Recebido` | Pedido criado, aguardando confirmação |
| `Em Preparo` | Cozinha iniciou o preparo |
| `Saiu para Entrega` | Entregador a caminho |
| `Entregue` | Pedido finalizado com sucesso |
| `Cancelado` | Pedido cancelado |

---

## 8. O Que Já Foi Feito no Agilium

### 8.1 Campo `STEXPORTARPEDIDO`

✅ Adicionado à tabela `produto`:
- Tipo: `ESimNao` (Sim/Não)
- Exibido na tela de cadastro/edição de produto como "Exportar para Pedido?"
- Mapeado no EF Core, AutoMapper, ViewModels, Views

### 8.2 Serviço de Exportação (IntegracaoCardapioService)

✅ Implementado seguindo os padrões de `documentacao-tecnica-agilium-mvc-web.md`:

| Camada | Arquivo | Descrição |
| --- | --- | --- |
| **Business** | `business/Interfaces/IIntegracaoCardapioService.cs` | Interface + DTOs |
| **Infra** | `infra/Services/IntegracaoCardapioService.cs` | Implementação Dapper (banco externo) |
| **Web** | `web/Controllers/ProdutoController.cs` | Action `ExportarParaCardapio()` |
| **Web** | `web/Views/Produto/Index.cshtml` | Botão "Exportar Cardápio" |
| **Web** | `web/Configuration/ResolveDependencyConfig.cs` | Registro DI |
| **Web** | `web/appsettings.json` | `CardapioDigital.ConnectionString` |

- **Chave de match:** `IDPRODUTO` (PK Agilium) ↔ `id_produto_agilium` (CardapioDigital)
- **Guia visual:** `CDPRODUTO` → `cd_produto_pdv`
- **Conexão:** Dapper direto ao banco `cardapio_digital`
- **Sincronização:** Grupos → Categorias automático, colunas criadas sob demanda

### Arquivos alterados (campo STEXPORTARPEDIDO)

| # | Arquivo | Alteração |
| --- | --- | --- |
| 1 | `agilium-manager-azure-business/Models/Produto.cs` | Propriedade `STEXPORTARPEDIDO` |
| 2 | `agilium-manager-git-azure-infra/Mappings/ProdutoMapping.cs` | Coluna `STEXPORTARPEDIDO INT` |
| 3 | `agilum.mvc.web/ViewModels/Produtos/ProdutoViewModel.cs` | `ExportarPedido` |
| 4 | `agilum.mvc.web/Configuration/AutomapperConfig.cs` | Mapeamento AutoMapper |
| 5 | `agilum.mvc.web/Views/Produto/CreateEditProduto.cshtml` | Dropdown Sim/Não |
| 6 | `agilium-manager-azure-api/ViewModels/.../ProdutoViewModel.cs` | `ExportarPedido` |
| 7 | `agilium-manager-azure-api/Configuration/AutomapperConfig.cs` | Mapeamento |
| 8 | `agilium-manager-azure-web/ViewModels/.../ProdutoViewModel.cs` | `ExportarPedido` |
| 9 | `agilium-manager-azure-web/Views/Produto/CreateEditProduto.cshtml` | Dropdown |
| 10 | `agilium-pdv-azure-api/ViewModels/.../ProdutoViewModel.cs` | `ExportarPedido` |
| 11 | `agilium-pdv-azure-api/Configuration/AutomapperConfig.cs` | Mapeamento |

---

## 9. Próximos Passos na Integração

### 9.1 Imediato — Banco de Dados

```sql
-- Agilium: aplicar migration do novo campo
ALTER TABLE produto ADD COLUMN STEXPORTARPEDIDO INT NULL;

-- CardapioDigital: criar tabela de equivalência (quando implementada)
-- Ver LEVANTAMENTO-COMPLETO.md seção 21.2
```

### 9.2 Curto Prazo — Serviço de Exportação

- [x] Criar `IIntegracaoCardapioService` ✅
- [x] Filtrar apenas produtos com `STEXPORTARPEDIDO = Sim` ✅
- [x] Sincronizar categorias via Dapper ✅
- [ ] Criar tabela `integracao_produto_equivalencia`
- [ ] Implementar consumo via API REST (além do Dapper direto)

### 9.3 Curto Prazo — Serviço de Importação de Pedidos

- [ ] Criar endpoint/service para enviar pedidos via `POST /api/Pedido`
- [ ] Mapear `venda` → `CriarPedidoRequest`
- [ ] Validar CEP via `POST /api/frete/calcular` antes de enviar
- [ ] Armazenar `id_pedido_cardapio` na venda local

### 9.4 Médio Prazo

- [ ] Polling de status: `GET /api/Pedido/{id}` a cada 30s
- [ ] Webhook reverso: CardapioDigital notifica Agilium sobre mudanças
- [ ] Log de integração (tabela `integracao_log`)

### 9.5 Longo Prazo

- [ ] Autenticação API Key para integração server-to-server
- [ ] Importação em lote de produtos
- [ ] Sincronização bidirecional de preços e disponibilidade

---

## 10. Resumo Técnico para Desenvolvimento

### Configuração no Agilium (appsettings.json)

```json
{
  "CardapioDigital": {
    "BaseUrl": "http://localhost:5555/api",
    "AdminEmail": "admin@email.com",
    "AdminSenha": "********",
    "TimeoutSegundos": 30,
    "IntervaloSyncProdutosMinutos": 15,
    "IntervaloPollingStatusSegundos": 30
  }
}
```

### Fluxo de Sincronização de Produtos (pseudocode)

```csharp
public async Task SincronizarProdutosAsync()
{
    // 1. Obter token admin
    var token = await _authService.LoginAsync(email, senha);
    
    // 2. Obter categorias do CardapioDigital
    var categorias = await _httpClient.GetAsync("/api/Produtos/categorias");
    
    // 3. Obter produtos do CardapioDigital
    var produtos = await _httpClient.GetAsync("/api/Produtos");
    
    // 4. Para cada produto local com STEXPORTARPEDIDO = Sim
    var produtosParaExportar = await _produtoService
        .ObterProdutosParaExportar(idEmpresa);
    
    foreach (var produto in produtosParaExportar)
    {
        // Mapear Produto → CriarProdutoRequest
        // Verificar se já existe (tabela de equivalência)
        // Se sim: PUT /api/admin/produtos/{id}
        // Se não: POST /api/admin/produtos
    }
}
```

---

> **Documento gerado em 2026-07-14 como levantamento de contexto para integração Agilium Manager ↔ CardapioDigital.**

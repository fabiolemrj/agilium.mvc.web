# 📋 Plano de Implementação — Exportar Produtos para CardapioDigital

> **Data:** 2026-07-14  
> **Status:** ✅ Implementado  
> **Projeto:** Agilium Manager MVC (agilum.mvc.web)  
> **Sistema destino:** CardapioDigital.API (banco `cardapio_digital`)

---

## 1. Objetivo

Permitir que o operador do Agilium Manager exporte produtos selecionados para o **CardapioDigital** (sistema de pedidos online), com sincronização automática de grupos (categorias) e comparação entre produtos novos e existentes.

A ação é disparada por um **botão na tela de listagem de produtos** e processa apenas os produtos com o campo `STEXPORTARPEDIDO = Sim`.

---

## 2. Arquitetura da Solução

```text
┌──────────────────────────────────────────────────────────────────┐
│                    agilum.mvc.web (Agilium Manager)               │
│                                                                  │
│  ┌─────────────────┐     ┌──────────────────────────────────┐   │
│  │ ProdutoController│────▶│ IntegracaoCardapioService         │   │
│  │ ExportarPara     │     │ (agilium.api.business/Services)  │   │
│  │ Cardapio()      │     │                                  │   │
│  └─────────────────┘     │  ┌────────────────────────────┐  │   │
│                          │  │ 1. Lê produtos Agilium      │  │   │
│  ┌─────────────────┐     │  │    STEXPORTARPEDIDO = Sim   │  │   │
│  │ Views/Produto/   │     │  └────────────────────────────┘  │   │
│  │ Index.cshtml     │     │  ┌────────────────────────────┐  │   │
│  │ [Exportar Cardápio]│   │  │ 2. Conecta ao              │  │   │
│  └─────────────────┘     │  │    cardapio_digital (Dapper) │  │   │
│                          │  └────────────────────────────┘  │   │
│                          │  ┌────────────────────────────┐  │   │
│                          │  │ 3. Garante colunas         │  │   │
│                          │  │    id_produto_agilium,     │  │   │
│                          │  │    cd_produto_pdv          │  │   │
│                          │  └────────────────────────────┘  │   │
│                          │  ┌────────────────────────────┐  │   │
│                          │  │ 4. Sincroniza categorias    │  │   │
│                          │  │    (grupo → categoria)      │  │   │
│                          │  └────────────────────────────┘  │   │
│                          │  ┌────────────────────────────┐  │   │
│                          │  │ 5. Para cada produto:       │  │   │
│                          │  │   Match por IDPRODUTO ↔    │  │   │
│                          │  │   id_produto_agilium        │  │   │
│                          │  │   ├─ Existe → UPDATE        │  │   │
│                          │  │   └─ Novo   → INSERT        │  │   │
│                          │  └────────────────────────────┘  │   │
│                          └──────────────────────────────────┘   │
│                                      │                          │
│                         Dapper (MySqlConnector)                  │
│                                      │                          │
└──────────────────────────────────────┼──────────────────────────┘
                                       │
                                       ▼
                          ┌─────────────────────────┐
                          │   cardapio_digital       │
                          │   (MySQL 8.0)            │
                          │                         │
                          │  ┌───────────────────┐  │
                          │  │ produto            │  │
                          │  │ + cd_produto_pdv   │  │  ← Coluna criada automaticamente
                          │  └───────────────────┘  │
                          │  ┌───────────────────┐  │
                          │  │ categoria          │  │
                          │  └───────────────────┘  │
                          └─────────────────────────┘
```

---

## 3. Arquivos da Implementação

### 3.1 Criados

| # | Arquivo | Camada | Descrição |
| --- | --- | --- | --- |
| 1 | `agilium-manager-azure-business/Interfaces/IIntegracaoCardapioService.cs` | Business | Interface + DTOs (`ResultadoExportacao`, `CardapioProdutoDto`) |
| 2 | `agilium-manager-azure-business/Services/IntegracaoCardapioService.cs` | Business | Implementação do serviço de integração |

### 3.2 Modificados

| # | Arquivo | Alteração |
| --- | --- | --- |
| 3 | `agilum.mvc.web/Configuration/ResolveDependencyConfig.cs` | Registro DI: `IIntegracaoCardapioService → IntegracaoCardapioService` |
| 4 | `agilum.mvc.web/Controllers/ProdutoController.cs` | + campo `_integracaoCardapioService` no construtor + action `ExportarParaCardapio()` |
| 5 | `agilum.mvc.web/Views/Produto/Index.cshtml` | Botão "Exportar Cardápio" na toolbar |
| 6 | `agilum.mvc.web/appsettings.json` | Nova seção `CardapioDigital.ConnectionString` |

---

## 4. Algoritmo Detalhado

### 4.1 Fluxo Principal: `ExportarProdutosAsync(long idEmpresa)`

```text
Entrada: ID da empresa selecionada

1. VALIDAR CONFIGURAÇÃO
   ├── Lê "CardapioDigital:ConnectionString" do appsettings.json
   └── Se vazio → retorna erro "Connection string não configurada"

2. BUSCAR PRODUTOS AGILIUM
   ├── Filtro: idEmpresa = X AND STEXPORTARPEDIDO = Sim AND STPRODUTO = Ativo
   └── Se nenhum → retorna "Nenhum produto marcado para exportação"

3. CONECTAR AO CARDAPIODIGITAL
   ├── new MySqlConnection(cardapioConnectionString)
   └── connection.Open()

4. GARANTIR COLUNAS DE INTEGRAÇÃO
   ├── Verifica information_schema.COLUMNS
   ├── Se não existe id_produto_agilium → ALTER TABLE ADD COLUMN id_produto_agilium BIGINT NULL
   └── Se não existe cd_produto_pdv → ALTER TABLE ADD COLUMN cd_produto_pdv VARCHAR(50) NULL

5. SINCRONIZAR CATEGORIAS
   ├── Extrai nomes de grupos únicos dos produtos Agilium
   ├── Busca categorias existentes no CardapioDigital
   └── Insere categorias novas (nome = nome do grupo)

6. BUSCAR PRODUTOS EXISTENTES NO CARDAPIODIGITAL
   ├── SELECT id, nome, id_produto_agilium, cd_produto_pdv, categoria_id FROM produto
   └── Cria dicionário: id_produto_agilium → CardapioProdutoDto

7. PARA CADA PRODUTO AGILIUM:
   ├── match = lookupCardapio[produto.Id]  (PK do Agilium)
   ├── Se encontrou:
   │   └── UPDATE produto SET nome, descricao, preco, categoria_id, ativo, cd_produto_pdv, data_atualizacao
   │       WHERE id = match.Id
   │       → resultado.Atualizados++
   └── Se NÃO encontrou:
       └── INSERT INTO produto (nome, descricao, preco, categoria_id, id_produto_agilium, cd_produto_pdv, ...)
           → resultado.Inseridos++

8. RETORNAR ResultadoExportacao
   ├── Sucesso: bool
   ├── TotalProdutosAgilium: int
   ├── Inseridos: int
   ├── Atualizados: int
   ├── ComErro: int
   ├── Erros: List<string>
   └── Mensagem: string (resumo formatado)
```

### 4.2 Chave de Correspondência

| Agilium | CardapioDigital | Tipo |
| --- | --- | --- |
| `produto.IDPRODUTO` (PK, bigint) | `produto.id_produto_agilium` (bigint) | **Chave primária — usada para match** |
| `produto.CDPRODUTO` (varchar 6) | `produto.cd_produto_pdv` (varchar 50) | Código do produto — **guia visual** para o usuário |

> A PK do Agilium (`IDPRODUTO`) deve existir no banco destino como `id_produto_agilium`.  
> O `CDPRODUTO` é armazenado apenas como referência para consulta do operador.

### 4.3 Mapeamento de Dados

| Agilium (`produto`) | CardapioDigital (`produto`) | Observação |
| --- | --- | --- |
| `IDPRODUTO` (PK) | `id_produto_agilium` | **Chave de correspondência** — PK do Agilium |
| `CDPRODUTO` | `cd_produto_pdv` | Código do produto — guia visual |
| `NMPRODUTO` | `nome` | Nome do produto |
| `DSVOLUME` | `descricao` | Descrição/volume |
| `NUPRECO` | `preco` | Preço de venda |
| `GrupoProduto.Nome` | `categoria.nome` | Grupo → Categoria (sincronizado automaticamente) |
| `GrupoProduto → categoria.id` | `categoria_id` | FK resolvida por nome |
| `STPRODUTO (Ativo)` | `ativo = 1` | Sempre ativo na exportação |
| — | `destaque = 0` | Padrão: não destacado |
| — | `preco_promocional` | Não mapeado (opcional futuro) |
| — | `promocao_ativa` | Não mapeado (opcional futuro) |
| — | `imagem_url` | Não mapeado (opcional futuro) |

---

## 5. Interface com o Usuário

### 5.1 Tela de Listagem de Produtos

```text
┌──────────────────────────────────────────────────────────────────┐
│  [+] Novo    [?] Ajuda    [🔄] IBPT    [☁️] Exportar Cardápio    │
├──────────────────────────────────────────────────────────────────┤
│  🔍 Pesquisar por descrição...                                   │
├──────────────────────────────────────────────────────────────────┤
│  Código │ Nome        │ Preço  │ Situação │ ...                  │
│  ───────┼─────────────┼────────┼──────────┼──────────────────────│
│  000001 │ X-Burguer   │ 22,90  │ Ativo    │ [Editar] [Excluir]   │
│  000002 │ Refrigerante│ 8,00   │ Ativo    │ [Editar] [Excluir]   │
└──────────────────────────────────────────────────────────────────┘
```

### 5.2 Fluxo de Uso

1. Operador marca produtos individuais com **"Exportar para Pedido? = Sim"** na tela de edição
2. Na listagem, clica em **☁️ Exportar Cardápio**
3. Confirma a ação no diálogo
4. Sistema processa e redireciona para a listagem com a mensagem de resultado:
   - ✅ Verde: "Exportação concluída! 5 inserido(s), 3 atualizado(s) de 8 produto(s)."
   - ⚠️ Amarelo: "Exportação parcial: 4 inserido(s), 2 atualizado(s), 2 erro(s)..."
   - 🔴 Vermelho: erros de conexão/configuração

---

## 6. Configuração

### 6.1 Connection String (appsettings.json)

```json
{
  "CardapioDigital": {
    "ConnectionString": "Server=localhost;Database=cardapio_digital;Uid=root;Pwd=123456;port=3306"
  }
}
```

### 6.2 Pré-requisitos no CardapioDigital

| Requisito | Status |
| --- | --- |
| Banco `cardapio_digital` acessível | ✅ Obrigatório |
| Tabela `produto` existe | ✅ Obrigatório |
| Tabela `categoria` existe | ✅ Obrigatório |
| Coluna `id_produto_agilium` | 🔧 Criada automaticamente na 1ª execução |
| Coluna `cd_produto_pdv` | 🔧 Criada automaticamente na 1ª execução |
| Usuário MySQL com permissão INSERT/UPDATE/ALTER | ✅ Obrigatório |

---

## 7. Tratamento de Erros

| Cenário | Comportamento |
| --- | --- |
| Connection string não configurada | Mensagem de erro, redireciona para Index |
| Banco `cardapio_digital` inacessível | Erro capturado, exibido na tela |
| Nenhum produto com `STEXPORTARPEDIDO = Sim` | Mensagem informativa, nada é feito |
| Erro em um produto específico | Registrado em `Erros[]`, continua processando os demais |
| Coluna `cd_produto_pdv` já existe | Ignora `ALTER TABLE`, prossegue normalmente |
| Coluna `id_produto_agilium` já existe | Ignora `ALTER TABLE`, prossegue normalmente |
| Categoria (grupo) nova | Criada automaticamente |

---

## 8. Observações de Design

1. **Conexão separada**: O serviço usa `MySqlConnection` diretamente (Dapper) para o banco `cardapio_digital`, sem reutilizar o `DbContext` do Agilium. Isso garante isolamento total entre os bancos.

2. **Somente CardapioDigital é alterado**: Nenhum dado do Agilium é modificado durante a exportação. O fluxo é unidirecional (Agilium → CardapioDigital).

3. **Idempotente**: Execuções repetidas não duplicam produtos. O match por `IDPRODUTO` (PK do Agilium ↔ `id_produto_agilium`) garante que o mesmo produto seja apenas atualizado.

4. **Resiliência parcial**: Se um produto falhar (ex: erro de constraint), os demais continuam sendo processados. Os erros são coletados e exibidos no final.

5. **Colunas de integração**: Criadas sob demanda: `id_produto_agilium` (BIGINT, chave de match) e `cd_produto_pdv` (VARCHAR(50), guia visual). A criação é segura (verifica existência antes de criar).

---

## 9. Próximas Evoluções Possíveis

| # | Melhoria | Descrição |
| --- | --- | --- |
| 1 | Mapear `imagem_url` | Fazer upload da imagem do produto Agilium para o CardapioDigital |
| 2 | Mapear `preco_promocional` | Sincronizar preços promocionais |
| 3 | Exportação agendada | Job background para sincronização periódica |
| 4 | Log detalhado | Salvar resultado no banco (tabela `integracao_log`) |
| 5 | Sincronização reversa | Importar pedidos do CardapioDigital → Agilium |
| 6 | Tabela de equivalência | Tabela dedicada `integracao_produto_equivalencia` em vez da coluna inline |

---

> **Documento gerado em 2026-07-14. A implementação completa está nos arquivos listados na seção 3.**

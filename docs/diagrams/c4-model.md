# Diagrama: C4 Model

## Objetivo

Representar a arquitetura do Agilium Manager utilizando o **modelo C4** (Context, Container, Component, Code), que fornece quatro níveis de abstração para documentar sistemas de software.

---

## Nível 1: Contexto (System Context)

```mermaid
graph TB
    User["Usuário Admin<br/>Operador do sistema"]

    subgraph "Agilium Manager"
        System["Sistema de Gestão Empresarial<br/>PDV, Estoque, Financeiro, Fiscal"]
    end

    Cardapio["Cardápio Digital<br/>Sistema externo"]
    Email["Servidor de E-mail<br/>SMTP"]
    NFe["NF-e<br/>Importação XML"]

    User -->|"Gerencia operações"| System
    System -->|"Exporta produtos"| Cardapio
    System -->|"Envia notificações"| Email
    System -->|"Importa XML"| NFe
```

---

## Nível 2: Containers

```mermaid
graph TD
    User["Usuário<br/>Browser"]

    subgraph "Agilium Manager"
        MVC["Web Application<br/>agilum.mvc.web<br/>.NET Core 3.1 + Razor"]
        API["REST API<br/>agilium-manager-azure-api<br/>.NET Core 3.1"]
        PDV["PDV API<br/>agilium-pdv-azure-api<br/>.NET Core 3.1"]

        subgraph "Data"
            MySQL[("MySQL 8.0<br/>Dados relacionais")]
            MongoDB[("MongoDB<br/>Documentos")]
        end
    end

    User -->|"HTTPS"| MVC
    User -->|"HTTPS"| API
    
    MVC -->|"EF Core + Dapper"| MySQL
    API -->|"EF Core + Dapper"| MySQL
    PDV -->|"EF Core + Dapper"| MySQL
    
    API -->|"MongoDB Driver"| MongoDB
```

---

## Nível 3: Componentes (MVC Web)

```mermaid
graph TD
    subgraph "agilum.mvc.web"
        direction TB
        
        subgraph "Controllers"
            Main["MainController<br/>(base)"]
            Prod["ProdutoController"]
            Compra["CompraController"]
            Venda["VendaController"]
            Caixa["CaixaController"]
            Dots["... 23 outros"]
        end

        subgraph "Views"
            Layout["_main.cshtml<br/>AdminLTE 3.x"]
            ProdView["Produto/"]
            CompraView["Compra/"]
        end

        subgraph "Configuration"
            Identity["IdentityConfig"]
            AutoMapperCfg["AutomapperConfig"]
            DI["ResolveDependencyConfig"]
            MVCConfig["MvcConfig"]
        end

        subgraph "Business Layer"
            Svc["Services<br/>agilium-manager-azure-business"]
        end

        subgraph "Infrastructure"
            Infra["Repository<br/>agilium-manager-git-azure-infra"]
        end
    end

    Main --> Prod
    Main --> Compra
    Main --> Venda
    Main --> Caixa

    Prod --> ProdView
    Compra --> CompraView

    Prod --> Svc
    Compra --> Svc
    Venda --> Svc

    Svc --> Infra

    Identity --> Main
    AutoMapperCfg --> Main
    DI --> Main
    MVCConfig --> Main
```

---

## Nível 4: Código (Exemplo: CompraController)

```mermaid
classDiagram
    class MainController {
        <<abstract>>
        #INotificador _notificador
        #IConfiguration _configuration
        #IMapper _mapper
        #IUser AppUser
        #IUtilDapperRepository _utilDapperRepository
        #ILogService _logService
        #ILicencaService _licencaService
        #IAuthService _authService
        +OperacaoValida() bool
        +GerarId() long
        +NotificarErro(string)
        +ObterNotificacoes() string[]
        +ObterObjetoEmpresaSelecionada() EmpresaUsuarioViewModel
    }

    class CompraController {
        -ICompraService _compraService
        -IEmpresaService _empresaService
        -IFornecedorService _fornecedorService
        -ITabelaAuxiliarFiscalService _tabela
        -ITurnoService _turnoService
        -IProdutoService _produtoService
        -IEstoqueService _estoqueService
        -IUnidadeService _unidadeService
        -IUsuarioService _usuarioService
        +IndexCompra() Task~IActionResult~
        +Create() Task~IActionResult~
        +Edit(long) Task~IActionResult~
        +Cancelar(long) Task~IActionResult~
        +Efetivar(long) Task~IActionResult~
        +ImportarXML() Task~IActionResult~
    }

    MainController <|-- CompraController
```

---

## Para Preencher

> **TODO:** Adicionar diagramas C4 dos níveis 3 e 4 para as APIs e para o módulo de Vendas.

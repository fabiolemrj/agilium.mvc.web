# Diagrama: Fluxo de Caixa

## Objetivo

Representar o fluxo de abertura, operação e fechamento do caixa no PDV do Agilium Manager.

---

## Fluxo Completo

```mermaid
sequenceDiagram
    actor Operador
    participant PDV
    participant Controller as CaixaController
    participant CaixaSvc as CaixaService
    participant TurnoSvc as TurnoService
    participant Repo as Repository
    participant DB

    Note over Operador,DB: Pré-condição: Turno aberto

    %% ABERTURA
    Operador->>PDV: Abrir Caixa
    PDV->>Controller: Abrir(saldoInicial, moedas)
    Controller->>TurnoSvc: ObterTurnoAberto(idEmpresa)
    TurnoSvc->>Repo: Buscar()
    Repo->>DB: SELECT
    DB-->>Repo: Turno
    Repo-->>TurnoSvc: Turno
    TurnoSvc-->>Controller: Turno aberto

    alt Turno não encontrado
        Controller-->>PDV: Notificar("Não há turno aberto")
    end

    Controller->>CaixaSvc: AbrirCaixa(turno, saldoInicial)
    CaixaSvc->>Repo: AdicionarSemSalvar(Caixa)
    Note over CaixaSvc: Situação: Aberto<br/>DataAbertura: now

    loop Cada Moeda
        CaixaSvc->>Repo: AdicionarSemSalvar(CaixaMoeda)
    end

    CaixaSvc->>Repo: SaveChanges()
    Repo->>DB: INSERT

    Controller-->>PDV: Caixa aberto

    %% OPERAÇÃO
    Note over Operador,DB: Durante a operação...

    opt Venda Realizada
        VendaSvc->>CaixaSvc: RegistrarMovimentoVenda(valor)
        CaixaSvc->>Repo: AdicionarSemSalvar(CaixaMovimento)
        Note over CaixaMovimento: Tipo: Entrada (Venda)
        CaixaSvc->>Repo: Atualizar CaixaMoeda (+)
    end

    opt Sangria
        Operador->>PDV: Registrar Sangria
        PDV->>Controller: Sangria(valor, motivo)
        Controller->>CaixaSvc: RegistrarSangria()
        CaixaSvc->>Repo: AdicionarSemSalvar(CaixaMovimento)
        Note over CaixaMovimento: Tipo: Saída (Sangria)
        CaixaSvc->>Repo: Atualizar CaixaMoeda (-)
    end

    opt Suprimento
        Operador->>PDV: Registrar Suprimento
        PDV->>Controller: Suprimento(valor, motivo)
        Controller->>CaixaSvc: RegistrarSuprimento()
        CaixaSvc->>Repo: AdicionarSemSalvar(CaixaMovimento)
        Note over CaixaMovimento: Tipo: Entrada (Suprimento)
        CaixaSvc->>Repo: Atualizar CaixaMoeda (+)
    end

    %% FECHAMENTO
    Operador->>PDV: Fechar Caixa
    PDV->>Controller: Fechar(id)

    Controller->>CaixaSvc: FecharCaixa(id)
    CaixaSvc->>Repo: ObterPorId(Caixa)
    
    CaixaSvc->>CaixaSvc: Calcular saldo:
    Note over CaixaSvc: SaldoInicial<br/>+ Total Vendas<br/>+ Total Suprimentos<br/>- Total Sangrias<br/>= Saldo Calculado

    CaixaSvc->>CaixaSvc: Comparar com saldo declarado

    alt Confere
        CaixaSvc->>Repo: Atualizar(Caixa)
        Note over Caixa: Situação: Fechado<br/>DataFechamento: now
        CaixaSvc->>Repo: SaveChanges()
        Controller-->>PDV: Caixa fechado com sucesso
    else Não confere
        CaixaSvc->>CaixaSvc: Registrar divergência
        CaixaSvc->>Repo: Atualizar(Caixa)
        CaixaSvc->>Repo: SaveChanges()
        Controller-->>PDV: Caixa fechado com divergência
    end
```

---

## Estados do Caixa

```mermaid
stateDiagram-v2
    [*] --> Aberto: Abrir Caixa
    Aberto --> Aberto: Venda (Entrada)
    Aberto --> Aberto: Sangria (Saída)
    Aberto --> Aberto: Suprimento (Entrada)
    Aberto --> Conferencia: Iniciar Fechamento
    Conferencia --> Fechado: Saldo Confere
    Conferencia --> FechadoDiv: Saldo Não Confere
    Fechado --> [*]
    FechadoDiv --> [*]
```

---

## Para Preencher

> **TODO:** Adicionar fluxo de reabertura de caixa e conciliação financeira.

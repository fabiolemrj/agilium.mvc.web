# ADR-0002 - Adoção do Repository Pattern

| Campo | Valor |
|-------|-------|
| **Status** | Accepted |
| **Data** | 2026-07-29 |
| **Autor** | Equipe Agilium |
| **Versão** | 1.0 |

---

# Contexto

O Agilium Manager possui uma arquitetura baseada em múltiplos módulos de negócio, responsáveis pelo gerenciamento de clientes, empresas, usuários, produtos, estoque, financeiro, pedidos, licenciamento e demais funcionalidades.

Todas essas funcionalidades realizam operações de persistência de dados utilizando principalmente Entity Framework Core e, em cenários específicos de alto desempenho, Dapper.

Era necessário definir um padrão único para o acesso aos dados, evitando que Controllers, Services ou regras de negócio acessassem diretamente o `DbContext`.

---

# Problema

Permitir que qualquer camada acesse diretamente o banco de dados pode gerar diversos problemas:

- Forte acoplamento ao Entity Framework Core;
- Regras de persistência espalhadas pela solução;
- Código duplicado;
- Dificuldade para testes unitários;
- Dificuldade para substituir a tecnologia de persistência futuramente;
- Consultas SQL misturadas com regras de negócio.

Era necessário criar uma camada responsável exclusivamente pelo acesso aos dados.

---

# Alternativas Consideradas

## Alternativa 1 — Utilizar DbContext diretamente nas Services

### Vantagens

- Menor quantidade de classes.
- Implementação rápida.

### Desvantagens

- Alto acoplamento ao EF Core.
- Regras de persistência espalhadas.
- Baixa reutilização.
- Difícil manutenção.
- Dificulta testes unitários.

---

## Alternativa 2 — Utilizar apenas Dapper

### Vantagens

- Excelente desempenho.
- Total controle sobre SQL.

### Desvantagens

- Grande quantidade de SQL manual.
- Maior custo de manutenção.
- Maior chance de duplicação de consultas.
- Menor produtividade para operações CRUD.

---

## Alternativa 3 — Repository Pattern (Escolhida)

### Vantagens

- Isolamento da persistência.
- Baixo acoplamento.
- Melhor organização.
- Facilidade para testes.
- Reutilização de consultas.
- Independência parcial da tecnologia de persistência.

### Desvantagens

- Aumento da quantidade de classes.
- Necessidade de manter interfaces sincronizadas com implementações.

---

# Decisão

Foi adotado o **Repository Pattern** como padrão oficial para acesso aos dados da solução.

Nenhuma camada da aplicação poderá acessar diretamente o `DbContext`, exceto os próprios Repositories.

Toda operação de persistência deverá ocorrer através de um Repository.

Fluxo padrão:

```text
Controller

↓

Service

↓

Repository

↓

DbContext

↓

Database
```

---

# Responsabilidades

## Controller

Responsável apenas por:

- Receber requisições;
- Validar entrada;
- Chamar Services.

Não acessa banco.

---

## Service

Responsável por:

- Aplicar regras de negócio;
- Coordenar operações;
- Controlar transações.

Não executa SQL.

---

## Repository

Responsável por:

- Inserir registros;
- Atualizar registros;
- Excluir registros;
- Consultar dados;
- Executar consultas especializadas;
- Persistir alterações.

Não contém regras de negócio.

---

## DbContext

Responsável exclusivamente por:

- Controle de entidades;
- Tracking;
- SaveChanges();
- Configuração do EF Core.

---

# Estrutura recomendada

```text
Repository/

├── Interfaces/

│   ├── IProdutoRepository.cs

│   ├── IClienteRepository.cs

│   └── ...

│

├── ProdutoRepository.cs

├── ClienteRepository.cs

└── ...
```

---

# Interface padrão

Cada Repository deve possuir uma interface.

Exemplo:

```csharp
public interface IProdutoRepository
{
    Task<Produto?> ObterPorId(Guid id);

    Task<IEnumerable<Produto>> Listar();

    Task Adicionar(Produto produto);

    Task Atualizar(Produto produto);

    Task Remover(Guid id);
}
```

---

# Implementação

A implementação deverá utilizar:

- Entity Framework Core para operações padrão;
- Dapper para consultas específicas de alta performance, quando justificadas.

---

# Quando utilizar Dapper

Dapper poderá ser utilizado para:

- Relatórios;
- Dashboards;
- Consultas complexas;
- Grandes volumes de leitura;
- Consultas somente leitura.

Sempre encapsulado dentro do Repository.

---

# Quando NÃO utilizar

Não utilizar Repository para:

- Regras de negócio;
- Validações;
- Conversões entre DTOs;
- Tratamento de autenticação;
- Controle de permissões.

Essas responsabilidades pertencem às camadas superiores.

---

# Convenções

Todos os Repositories devem:

- Possuir interface.
- Ser registrados na Injeção de Dependência.
- Não depender de Controllers.
- Não chamar Services.
- Não acessar Views.
- Não conter lógica de apresentação.

---

# Benefícios

- Separação clara de responsabilidades.
- Código reutilizável.
- Menor acoplamento.
- Melhor organização.
- Facilidade para testes.
- Facilidade para evolução da persistência.
- Centralização das consultas.

---

# Desvantagens

- Maior número de classes.
- Necessidade de manutenção das interfaces.
- Camada adicional na arquitetura.

---

# Riscos

Caso o padrão não seja seguido:

- Controllers poderão acessar banco diretamente.
- Duplicação de SQL.
- Regras de persistência espalhadas.
- Forte acoplamento ao EF Core.
- Dificuldade para manutenção.

---

# Impacto

Esta decisão impacta diretamente:

- Services
- Business
- API
- MVC
- Persistência
- Banco de Dados
- Testes Unitários

---

# Plano de Implementação

1. Criar interface para cada Repository.
2. Implementar Repository utilizando EF Core.
3. Registrar Repository na Injeção de Dependência.
4. Refatorar acessos diretos ao DbContext.
5. Utilizar Dapper apenas quando houver ganho comprovado de desempenho.
6. Atualizar documentação técnica.

---

# Critérios de Aceitação

Uma implementação está aderente a esta ADR quando:

- Nenhum Controller acessa o DbContext.
- Nenhuma Service executa SQL diretamente.
- Toda persistência ocorre através de Repository.
- Cada Repository possui interface própria.
- Regras de negócio permanecem fora do Repository.
- Consultas de alta performance utilizam Dapper somente quando justificadas.

---

# ADRs Relacionados

- ADR-0001 — Arquitetura em Camadas
- ADR-0003 — Notification Pattern
- ADR-0004 — Entity Framework Core
- ADR-0009 — Dependency Injection
- ADR-0010 — Dapper para Consultas
- ADR-0011 — Service Layer

---

# Referências

- Martin Fowler — *Patterns of Enterprise Application Architecture*
- Microsoft — *Repository Pattern with Entity Framework Core*
- Eric Evans — *Domain-Driven Design*

---

# Histórico

| Versão | Data | Descrição |
|---------|------|-----------|
| 1.0 | 2026-07-29 | Criação da ADR definindo o Repository Pattern como padrão oficial para acesso aos dados do Agilium Manager. |
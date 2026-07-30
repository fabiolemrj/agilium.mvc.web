# Módulo Empresas

## Objetivo

O módulo **Empresas** representa a organização principal do sistema e é responsável por centralizar as configurações, parâmetros e informações cadastrais utilizadas pelos demais módulos da aplicação.

Praticamente todos os processos do sistema estão vinculados a uma empresa.

A Empresa é considerada a raiz de grande parte do domínio do negócio.

---

# Responsabilidades

O módulo é responsável por:

- Cadastro de empresas
- Alteração de dados cadastrais
- Configuração operacional
- Configuração fiscal
- Configuração financeira
- Configuração do PDV
- Configuração de integrações
- Controle de empresas ativas
- Disponibilização das informações para os demais módulos

---

# Fluxo Geral

```
Cadastrar Empresa

↓

Validar Dados

↓

Salvar

↓

Configurar Empresa

↓

Disponibilizar para Operação

↓

Utilização pelos demais módulos
```

---

# Papel da Empresa no Sistema

A Empresa é a entidade principal utilizada para segmentar os dados da aplicação.

Todos os módulos operacionais dependem direta ou indiretamente de uma empresa.

```
Empresa

├── Usuários
├── Funcionários
├── Clientes
├── Produtos
├── Estoque
├── Turnos
├── Caixas
├── Vendas
├── Pedidos
├── Financeiro
├── Fiscal
└── Integrações
```

---

# Dependências

O cadastro da empresa depende de:

- Endereço
- Cidade
- Estado
- País
- Responsável
- Configurações fiscais
- Configurações financeiras

---

# Principais Informações

O cadastro da empresa pode conter:

- Razão Social
- Nome Fantasia
- CNPJ
- Inscrição Estadual
- Inscrição Municipal
- Regime Tributário
- CNAE
- Telefones
- Email
- Site
- Endereço
- CEP
- Cidade
- Estado
- País
- Situação
- Data de Cadastro

---

# Fluxo de Cadastro

```
Nova Empresa

↓

Informar Dados

↓

Validar CNPJ

↓

Validar Endereço

↓

Salvar

↓

Empresa Disponível
```

---

# Fluxo de Configuração

Após o cadastro, normalmente são configurados:

- Dados fiscais
- Formas de pagamento
- Moedas
- Parâmetros do PDV
- Integrações
- Usuários
- Permissões
- Produtos
- Estoque

---

# Regras de Negócio

## Cadastro

Toda empresa deve possuir:

- Razão Social
- Nome Fantasia (quando aplicável)
- CNPJ válido
- Endereço válido
- Situação definida

---

## Identificação

O CNPJ deve ser único no sistema, respeitando as regras de negócio.

---

## Operação

Somente empresas ativas podem:

- realizar vendas
- abrir turnos
- abrir caixas
- emitir documentos fiscais
- integrar com sistemas externos

---

## Inativação

A inativação de uma empresa deve impedir novas operações comerciais, preservando o histórico já existente.

---

# Configurações da Empresa

Cada empresa pode possuir configurações próprias.

Exemplos:

- Regime tributário
- Configurações fiscais
- Configurações financeiras
- Impressoras
- PDVs
- Formas de pagamento
- Integrações
- Parâmetros do sistema

---

# Integração com Usuários

Os usuários pertencem a uma empresa.

```
Empresa

↓

Usuário

↓

Permissões
```

As permissões são aplicadas considerando a empresa à qual o usuário está vinculado.

---

# Integração com Produtos

Os produtos são cadastrados e disponibilizados para uma empresa.

```
Empresa

↓

Produto

↓

Venda
```

---

# Integração com Clientes

Os clientes podem estar associados a uma empresa.

```
Empresa

↓

Cliente

↓

Pedido

↓

Venda
```

---

# Integração com Turnos e Caixa

A operação do PDV inicia a partir da empresa.

```
Empresa

↓

Turno

↓

Caixa

↓

Venda
```

---

# Integração com Estoque

Toda movimentação de estoque ocorre no contexto de uma empresa.

---

# Integração com Financeiro

As movimentações financeiras pertencem a uma empresa.

Exemplos:

- Receitas
- Despesas
- Contas
- Fluxo de Caixa

---

# Integração com Fiscal

A empresa fornece as informações utilizadas na emissão de documentos fiscais.

Exemplos:

- CNPJ
- Inscrição Estadual
- Regime Tributário
- Endereço Fiscal

---

# Integrações Externas

A empresa pode possuir configurações específicas para integração com:

- Cardápio Digital
- Marketplace
- WhatsApp
- Email
- APIs externas

Cada integração deve respeitar as configurações da empresa.

---

# Situações da Empresa

Exemplos:

```
Ativa

↓

Inativa

↓

Bloqueada
```

A situação determina a disponibilidade para utilização do sistema.

---

# Permissões

As operações relacionadas à empresa devem respeitar as permissões do usuário autenticado.

Exemplos:

- Consultar Empresa
- Cadastrar Empresa
- Alterar Empresa
- Configurar Empresa
- Inativar Empresa

---

# Principais Entidades Relacionadas

- Empresa
- Usuario
- Funcionario
- Cliente
- Produto
- Turno
- Caixa
- Venda
- Pedido
- Estoque
- FormaPagamento
- ConfiguracaoFiscal

---

# Serviços Envolvidos

Exemplos:

- EmpresaService
- UsuarioService
- ProdutoService
- ClienteService
- TurnoService
- CaixaService

Toda regra de negócio deve permanecer na camada Business.

---

# Controllers Relacionados

Exemplos:

- EmpresaController
- ConfiguracaoController

Os Controllers apenas recebem as requisições, validam os dados de entrada e delegam o processamento aos Services.

---

# Impactos de Alterações

Alterações no módulo Empresa podem impactar praticamente toda a aplicação.

Principais módulos afetados:

- Usuários
- Produtos
- Clientes
- Estoque
- Vendas
- Pedidos
- Financeiro
- Fiscal
- Turnos
- Caixas
- Relatórios
- Integrações

Antes de alterar qualquer regra da empresa, é obrigatório avaliar os impactos em todos os módulos dependentes.

---

# Boas Práticas

- Validar o CNPJ antes da gravação.
- Evitar exclusão física de empresas com histórico operacional.
- Centralizar regras no `EmpresaService`.
- Manter configurações específicas por empresa.
- Não implementar regras de negócio em Controllers.

---

# Checklist

Antes de implementar alterações:

☐ Razão Social informada

☐ CNPJ validado

☐ Endereço consistente

☐ Empresa ativa quando necessário

☐ Configurações fiscais preservadas

☐ Configurações financeiras preservadas

☐ Integração com Turnos preservada

☐ Integração com Caixas preservada

☐ Integração com Vendas preservada

☐ Integrações externas avaliadas

☐ Impactos analisados

---

# Conclusão

O módulo **Empresas** é a base organizacional do **Agilium Manager**, servindo como ponto central para a configuração e segmentação dos dados da aplicação.

Como praticamente todos os módulos dependem da entidade Empresa, qualquer alteração em suas regras ou estrutura deve ser cuidadosamente analisada para garantir a integridade do sistema, a consistência das operações e a compatibilidade com os demais componentes da plataforma.

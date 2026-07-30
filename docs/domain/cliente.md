# Módulo Clientes

## Objetivo

O módulo **Clientes** gerencia o cadastro de clientes (Pessoa Física e Jurídica), seus contatos, endereços e preços diferenciados por cliente.

---

# Responsabilidades

- Cadastro de clientes PF (Pessoa Física)
- Cadastro de clientes PJ (Pessoa Jurídica)
- Cadastro de contatos
- Cadastro de endereços
- Preços diferenciados por cliente (ClientePreco)
- Vínculo com vendas e pedidos

---

# Principais Entidades

| Entidade | Descrição |
|----------|-----------|
| Cliente | Registro base (tipo PF/PJ) |
| ClientePF | Dados de Pessoa Física |
| ClientePJ | Dados de Pessoa Jurídica |
| ClienteContato | Contatos do cliente |
| ClientePreco | Preços diferenciados por cliente e produto |
| Contato | Contato genérico |
| Endereco | Endereço do cliente |

---

# Dependências

- Empresa
- Produto (para ClientePreco)

---

# Regras de Negócio

## Cadastro

- Nome/Razão Social obrigatório
- CPF/CNPJ obrigatório conforme tipo
- Endereço principal obrigatório
- Pelo menos um contato

## Preços Diferenciados

- Cliente pode ter preço especial por produto
- Preço diferenciado substitui preço de venda padrão na venda

---

# Serviços Envolvidos

- ClienteService
- ContatoService
- EnderecoService

---

# Controllers Relacionados

- ClienteController (`agilum.mvc.web/Controllers/ClienteController.cs`)
- EnderecoController (`agilum.mvc.web/Controllers/EnderecoController.cs`)

---

# Checklist

☐ Tipo (PF/PJ) definido

☐ CPF/CNPJ válido

☐ Endereço cadastrado

☐ Contato principal informado

☐ Preços diferenciados configurados (se aplicável)

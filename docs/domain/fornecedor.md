# Módulo Fornecedores

## Objetivo

O módulo **Fornecedores** gerencia o cadastro de fornecedores de produtos e serviços, seus contatos e endereços, sendo referenciado principalmente pelo módulo de Compras.

---

# Responsabilidades

- Cadastro de fornecedores
- Cadastro de contatos do fornecedor
- Cadastro de endereços
- Vínculo com compras

---

# Principais Entidades

| Entidade | Descrição |
|----------|-----------|
| Fornecedor | Registro principal |
| FornecedorContato | Contatos do fornecedor |
| Contato | Contato genérico |
| Endereco | Endereço |

---

# Dependências

- Empresa

---

# Regras de Negócio

- Razão Social / Nome obrigatório
- CNPJ/CPF obrigatório
- Pelo menos um contato

---

# Serviços Envolvidos

- FornecedorService
- ContatoService
- EnderecoService

---

# Controllers Relacionados

- FornecedorController (`agilum.mvc.web/Controllers/FornecedorController.cs`)

---

# Checklist

☐ Nome/Razão Social informado

☐ CNPJ/CPF válido

☐ Contato cadastrado

☐ Endereço informado

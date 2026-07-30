# Módulo Licenças

## Objetivo

O módulo **Licenças** controla o licenciamento do sistema por empresa, gerenciando a ativação, validade e renovação de licenças de uso do software.

---

# Responsabilidades

- Registro de licenças por empresa
- Controle de chaves de ativação
- Validação de expiração
- Criptografia de chaves

---

# Principais Entidades

| Entidade | Descrição |
|----------|-----------|
| Licenca | Registro de licença da empresa |

---

# Dependências

- Empresa

---

# Serviços Envolvidos

- LicencaService
- PassCryptoService (criptografia)

---

# Controllers Relacionados

- LicencaController
- HomeController (rota `/licenca`)

---

# Checklist

☐ Empresa vinculada

☐ Chaves criptografadas

☐ Data de expiração controlada

☐ Validação no acesso ao sistema

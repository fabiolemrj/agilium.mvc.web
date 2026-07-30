# Módulo Logs

## Objetivo

O módulo **Logs** registra eventos do sistema para auditoria, depuração e rastreabilidade de operações realizadas pelos usuários.

---

# Responsabilidades

- Registro de logs de sistema (LogSistema)
- Registro de erros (LogErro)
- Consulta de logs

---

# Principais Entidades

| Entidade | Descrição |
|----------|-----------|
| LogSistema | Log de operações do sistema |
| LogErro | Log de erros/exceções |

---

# Dependências

- Usuario
- Empresa

---

# Serviços Envolvidos

- LogService

---

# Controllers Relacionados

- LogController

---

# Boas Práticas

- Registrar operações críticas (criação, alteração, exclusão)
- Incluir identificador do usuário e empresa
- Incluir dados serializados do objeto (Deserializar)
- Registrar data/hora de cada evento

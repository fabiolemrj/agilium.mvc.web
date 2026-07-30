# Agilium Manager — Development Checklist

> Utilize este checklist antes, durante e após qualquer implementação.
> Nenhuma feature, correção ou refatoração deve ser considerada concluída sem passar por estas verificações.

---

# 📋 Antes de Começar

## Entendimento

- [ ] Li e compreendi completamente o requisito
- [ ] Esclareci todas as dúvidas antes de iniciar
- [ ] Identifiquei impactos funcionais
- [ ] Identifiquei impactos técnicos
- [ ] Verifiquei possíveis impactos em funcionalidades existentes

---

## Arquitetura

- [ ] Identifiquei a camada correta para implementação
  - [ ] MVC
  - [ ] API
  - [ ] Application
  - [ ] Business
  - [ ] Repository
  - [ ] Infra
  - [ ] Data
- [ ] Consultei os padrões arquiteturais do projeto
- [ ] Revisei implementações semelhantes
- [ ] Consultei o `coding-standards.md`
- [ ] Consultei o `instructions.md`

---

## Banco de Dados

- [ ] Verifiquei se a alteração exige mudança no banco
- [ ] Verifiquei entidades relacionadas
- [ ] Verifiquei possíveis impactos em migrations

---

# 🏗 Durante o Desenvolvimento

## Código

- [ ] Mantive responsabilidade única por classe
- [ ] Mantive responsabilidade única por método
- [ ] Evitei duplicação de código
- [ ] Evitei métodos muito grandes
- [ ] Evitei comentários desnecessários
- [ ] Segui os padrões de nomenclatura

---

## Nova Feature

- [ ] Criei ViewModel ou DTO quando necessário
- [ ] Criei ou atualizei Interfaces
- [ ] Implementei Service
- [ ] Implementei Repository (quando necessário)
- [ ] Atualizei Dependency Injection
- [ ] Atualizei AutoMapper
- [ ] Atualizei FluentValidation
- [ ] Atualizei Notifications
- [ ] Atualizei Controllers
- [ ] Atualizei documentação da API (quando aplicável)

---

## Correção de Bug

- [ ] Reproduzi o problema
- [ ] Identifiquei a causa raiz
- [ ] Corrigi a causa e não apenas o sintoma
- [ ] Verifiquei efeitos colaterais
- [ ] Adicionei teste de regressão (quando aplicável)

---

## Entity Framework Core

- [ ] Consultas utilizam `AsNoTracking()` quando apropriado
- [ ] Includes são explícitos
- [ ] Não existe Lazy Loading implícito
- [ ] Consultas evitam problema N+1
- [ ] Não existe SQL duplicado
- [ ] Não há consultas desnecessárias

---

## Banco de Dados

Caso exista alteração estrutural:

- [ ] Migration criada
- [ ] Nome descritivo
- [ ] `Up()` revisado
- [ ] `Down()` revisado
- [ ] Migration testada localmente

---

## Controllers

- [ ] Controller não possui regra de negócio
- [ ] Apenas orquestra chamadas
- [ ] ModelState validado
- [ ] Respostas padronizadas
- [ ] ActionResult tipado

---

## Services

- [ ] Contêm toda regra de negócio
- [ ] Utilizam Notification Pattern
- [ ] Não acessam HttpContext
- [ ] Não retornam IActionResult

---

## Repository

- [ ] Apenas acesso aos dados
- [ ] Não contém regra de negócio
- [ ] Não retorna ViewModels
- [ ] Não contém lógica de apresentação

---

## Segurança

- [ ] Não existem credenciais no código
- [ ] Não existem senhas hardcoded
- [ ] Não existem JWT Secrets hardcoded
- [ ] Configurações via Environment Variables ou appsettings

---

## Autenticação

- [ ] Utiliza autenticação baseada na entidade `Usuario`
- [ ] Não utiliza tabelas padrão do ASP.NET Identity
- [ ] Permissões respeitam as regras do domínio
- [ ] Endpoints protegidos utilizam `[Authorize]` quando necessário

---

## Logging

- [ ] Erros importantes registrados
- [ ] Logs possuem contexto suficiente
- [ ] Não existem logs de debug esquecidos

---

# 🧪 Testes

## Unitários

- [ ] Novos testes adicionados (quando aplicável)
- [ ] Testes existentes continuam passando

---

## Integração

- [ ] Endpoints testados
- [ ] Fluxo completo validado
- [ ] Casos de erro testados
- [ ] Casos de sucesso testados

---

## Manual

- [ ] Fluxo principal validado
- [ ] Fluxos alternativos testados
- [ ] Validações funcionando
- [ ] Mensagens de erro corretas

---

# 🔍 Auto Review

## Clean Code

- [ ] Métodos pequenos
- [ ] Classes pequenas
- [ ] Sem código morto
- [ ] Sem código duplicado
- [ ] Sem complexidade desnecessária
- [ ] Sem comentários obsoletos

---

## Arquitetura

- [ ] Controller não acessa Repository
- [ ] Controller não acessa DbContext
- [ ] Services concentram regras
- [ ] Repository apenas acessa banco
- [ ] Dependências seguem arquitetura

---

## Performance

- [ ] Consultas eficientes
- [ ] Sem N+1
- [ ] Sem consultas repetidas
- [ ] Objetos desnecessários evitados
- [ ] Uso adequado de Async/Await

---

# ✅ Antes do Commit

## Código

- [ ] `dotnet build` executado
- [ ] Sem erros de compilação
- [ ] Sem warnings relevantes
- [ ] Imports organizados
- [ ] Namespaces corretos

---

## Limpeza

- [ ] Sem `Console.WriteLine`
- [ ] Sem `Debug.WriteLine`
- [ ] Sem TODO esquecidos
- [ ] Sem código comentado
- [ ] Sem arquivos temporários

---

## Qualidade

- [ ] Coding Standards atendidos
- [ ] Checklist revisado
- [ ] Código revisado manualmente
- [ ] Commits pequenos e descritivos

---

# 🚀 Antes do Deploy

## Configuração

- [ ] appsettings revisados
- [ ] Variáveis de ambiente configuradas
- [ ] Connection Strings corretas
- [ ] Docker atualizado (quando necessário)

---

## Banco

- [ ] Migrations aplicadas
- [ ] Backup realizado (quando necessário)

---

## Publicação

- [ ] Build Release executado
- [ ] Testes finais realizados
- [ ] Logs configurados
- [ ] Ambiente validado

---

# ✔ Checklist Final

Nenhuma entrega deve ser considerada concluída enquanto existir qualquer item pendente deste checklist.
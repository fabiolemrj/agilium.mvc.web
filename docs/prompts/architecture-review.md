# Prompt: Revisão Arquitetural

# Objetivo

Template para realização de revisões arquiteturais no Agilium Manager.

Este prompt deve ser utilizado para avaliar aderência à arquitetura da solução, identificar violações de padrões estabelecidos, oportunidades de melhoria e potenciais débitos técnicos.

---

# Quando utilizar

Utilize este prompt para revisar:

- um módulo específico;
- um Controller;
- um Service;
- um Repository;
- uma funcionalidade;
- um projeto completo;
- toda a Solution.

---

# Prompt

```text
Realize uma revisão arquitetural do [ESCOPO] do Agilium Manager considerando o levantamento técnico da solução.

Analise obrigatoriamente os seguintes aspectos:

1. Arquitetura em Camadas
   - verificar se o fluxo respeita:
     MVC
     ↓
     Business
     ↓
     Infrastructure

2. Controllers
   - verificar herança do MainController
   - utilização de ViewModels
   - responsabilidade adequada
   - Actions excessivamente complexas

3. Services
   - concentração das regras de negócio
   - utilização correta do Notification Pattern
   - utilização de FluentValidation

4. Repository Pattern
   - utilização de interfaces
   - separação entre Business e Infrastructure
   - uso adequado de Entity Framework Core e Dapper
   - integração com Unit of Work

5. Dependency Injection
   - registros em ResolveDependencyConfig.cs
   - utilização de Constructor Injection
   - ausência de instanciação manual de dependências

6. AutoMapper
   - utilização para conversão entre Models e ViewModels
   - ausência de conversões manuais desnecessárias

7. Validação
   - utilização de Data Annotations
   - utilização de FluentValidation
   - utilização do Notification Pattern

8. Middleware
   - utilização adequada do ExceptionMiddleware
   - utilização do EmpresaSelecionadaMiddleware quando aplicável

9. Segurança
   - utilização de Authorize
   - ClaimsAuthorize
   - autenticação
   - autorização
   - exposição de informações sensíveis

10. Persistência
    - separação entre regras de negócio e acesso aos dados
    - utilização adequada de Repositories
    - consultas complexas utilizando a tecnologia apropriada

11. Organização do código
    - aderência aos padrões arquiteturais do projeto
    - nomenclatura
    - organização das pastas
    - reutilização de componentes

12. Débito técnico
    - código duplicado
    - acoplamento excessivo
    - violações de responsabilidade única
    - oportunidades de simplificação

Para cada item identificado informar:

• Arquivo

• Classe

• Método

• Linha (quando possível)

• Severidade
  - Baixa
  - Média
  - Alta
  - Crítica

• Descrição do problema

• Justificativa técnica

• Sugestão de correção

Ao final apresentar:

- Resumo Executivo
- Pontos Fortes
- Oportunidades de Melhoria
- Riscos Arquiteturais
- Prioridade das Correções
- Plano de Refatoração recomendado
```

---

# Parâmetros

| Parâmetro | Descrição | Exemplo |
|-----------|-----------|---------|
| `ESCOPO` | Área da revisão | "Produtos", "Financeiro", "PedidoService", "Toda a Solution", "Controllers do módulo Fiscal" |

---

# Resultado Esperado

A revisão deve produzir um relatório técnico contendo:

- aderência à arquitetura do Agilium Manager;
- violações identificadas;
- riscos arquiteturais;
- débitos técnicos;
- recomendações de melhoria;
- prioridades de correção;
- impacto esperado das mudanças.

O relatório deve considerar exclusivamente os padrões arquiteturais adotados pela solução e evitar recomendações baseadas em tecnologias ou convenções que não façam parte do projeto.
# Prompt: Revisão de Código

# Objetivo

Template para realização de revisões de código no Agilium Manager.

Este prompt deve ser utilizado para avaliar a qualidade técnica das alterações, sua aderência à arquitetura da solução e os possíveis impactos funcionais e não funcionais antes da integração do código.

---

# Quando utilizar

Utilize este prompt para revisar:

- Pull Requests;
- Branches;
- Commits;
- Arquivos específicos;
- Funcionalidades completas;
- Refatorações.

---

# Prompt

```text
Realize uma revisão técnica das alterações em:

[ARQUIVOS_OU_BRANCH]

A análise deve considerar a arquitetura e os padrões adotados pelo Agilium Manager.

## 1. Arquitetura

Verificar:

- aderência à arquitetura MVC → Business → Infrastructure;
- separação de responsabilidades;
- dependências entre camadas;
- acoplamento excessivo;
- respeito aos padrões arquiteturais existentes.

---

## 2. Controllers

Verificar:

- herança do MainController;
- utilização de ViewModels;
- Actions excessivamente complexas;
- responsabilidade da camada MVC.

---

## 3. Services

Verificar:

- concentração das regras de negócio;
- utilização do Notification Pattern;
- utilização de FluentValidation;
- ausência de regras de persistência.

---

## 4. Repository Pattern

Verificar:

- utilização correta das interfaces;
- separação entre Business e Infrastructure;
- utilização adequada de Entity Framework Core e Dapper;
- integração com Unit of Work;
- consultas potencialmente ineficientes ou duplicadas.

---

## 5. Dependency Injection

Verificar:

- registro de novas dependências em ResolveDependencyConfig.cs;
- utilização de Constructor Injection;
- ausência de instanciação manual (`new`) para serviços da aplicação.

---

## 6. AutoMapper

Verificar:

- utilização adequada de IMapper;
- conversão entre Models e ViewModels;
- ausência de mapeamentos manuais repetitivos quando já houver suporte do AutoMapper.

---

## 7. Validação

Verificar:

- utilização de Data Annotations;
- utilização de FluentValidation;
- utilização do Notification Pattern;
- validação consistente dos dados de entrada.

---

## 8. Segurança

Verificar:

- autenticação;
- autorização (`Authorize` e `ClaimsAuthorize`, quando aplicável);
- exposição de informações sensíveis;
- tratamento seguro de entradas do usuário.

---

## 9. Middleware

Verificar:

- integração adequada com o ExceptionMiddleware;
- utilização do EmpresaSelecionadaMiddleware quando aplicável.

---

## 10. Performance

Identificar oportunidades de melhoria relacionadas a:

- consultas ao banco de dados;
- carregamento de dados;
- uso adequado das tecnologias de persistência (Entity Framework Core e Dapper);
- processamento desnecessário ou duplicado.

---

## 11. Código

Avaliar:

- legibilidade;
- organização;
- reutilização;
- nomenclatura;
- duplicação;
- complexidade;
- aderência aos padrões definidos para o projeto.

---

## 12. Testes

Verificar se:

- as alterações afetam funcionalidades existentes;
- existem testes automatizados relacionados à área alterada;
- há necessidade de criar ou atualizar testes.

Caso a solução não possua cobertura automatizada para a funcionalidade revisada, registrar essa limitação e indicar os cenários que deveriam ser testados.

---

Para cada problema identificado informar:

• Arquivo

• Classe

• Método

• Linha (quando possível)

• Severidade

- Crítica
- Alta
- Média
- Baixa

• Descrição

• Justificativa técnica

• Sugestão de correção

Ao final apresentar:

- Resumo Executivo
- Pontos Positivos
- Pontos de Atenção
- Débitos Técnicos
- Riscos
- Recomendações
- Prioridade das Correções
```

---

# Parâmetros

| Parâmetro | Descrição | Exemplo |
|-----------|-----------|---------|
| `ARQUIVOS_OU_BRANCH` | Escopo da revisão | `feature/nova-tela-produto`, `PedidoService.cs`, `Controllers/ProdutoController.cs`, `release/2.5.0` |

---

# Resultado Esperado

A revisão deve produzir um relatório técnico contendo:

- conformidade com a arquitetura do Agilium Manager;
- problemas identificados;
- classificação por severidade;
- riscos técnicos;
- recomendações de melhoria;
- impactos potenciais das alterações;
- prioridades para correção.

As recomendações devem estar alinhadas exclusivamente aos padrões arquiteturais e às tecnologias efetivamente utilizadas pela solução.
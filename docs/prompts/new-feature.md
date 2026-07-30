# Prompt: New Feature

## Objective
Template prompt for implementing new features in Agilium Manager.

## Usage
Use this prompt when asked to implement a new feature or functionality.

---

## Prompt Template

```
Implemente a feature [DESCRICAO_DA_FEATURE] no projeto Agilium Manager:

1. **Análise**: em quais camadas a feature será implementada? (MVC, API, Business, Infra)
2. **Plano**: enumere os passos necessários em ordem
3. **Model/ViewModel**: defina ou atualize os modelos necessários
4. **Service**: implemente a lógica de negócio na camada Business
5. **Repository**: se necessário, atualize ou crie repositórios na camada Infra
6. **Controller**: crie as ações no controller apropriado
7. **View/API**: implemente as views (MVC) ou endpoints (API)
8. **DI**: registre novas dependências
9. **Testes**: adicione testes para a nova funcionalidade

Siga os coding standards do projeto.
Use padrões existentes (Notification, AutoMapper, async/await).
```

---

## Parameters

| Parameter | Description | Example |
|-----------|-------------|---------|
| `DESCRICAO_DA_FEATURE` | Feature description | "Tela de histórico de preços do produto" |

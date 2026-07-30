# Agilium Manager — Technical Documentation

## Overview

Welcome to the Agilium Manager technical documentation. This repository contains the complete technical reference for the Agilium Manager platform, covering architecture, business domain, APIs, database, development guides, design patterns, and decision records.

## Folder Organization

| Folder | Purpose |
|--------|---------|
| [`architecture/`](./architecture/) | System architecture, solution structure, layers, dependencies, auth, deployment |
| [`business/`](./business/) | Business domain, glossary, modules, workflows, business rules, validations |
| [`api/`](./api/) | REST API documentation, endpoints, authentication, versioning, conventions |
| [`database/`](./database/) | Database schema, entities, relationships, migrations, indexes, performance |
| [`development/`](./development/) | Dev setup, environment, build, debugging, testing, code review, releases |
| [`patterns/`](./patterns/) | Design patterns: Repository, DI, Notification, AutoMapper, Validation, Logging, Async |
| [`frontend/`](./frontend/) | MVC architecture, Razor views, layouts, JavaScript, CSS, UI components |
| [`prompts/`](./prompts/) | AI prompt templates for common development tasks |
| [`decisions/`](./decisions/) | Architecture Decision Records (ADRs) and template |
| [`images/`](./images/) | Images and screenshots used in documentation |
| [`diagrams/`](./diagrams/) | Architecture, sequence, and entity-relationship diagrams |
| [`templates/`](./templates/) | Document templates for features, endpoints, modules, business rules, architecture |

## How to Use This Documentation

### For New Developers
Start with:
1. [`architecture/overview.md`](./architecture/overview.md) — understand the system
2. [`development/getting-started.md`](./development/getting-started.md) — set up your environment
3. [`business/glossary.md`](./business/glossary.md) — learn the domain language

### For Feature Development
1. Check [`business/modules.md`](./business/modules.md) for module context
2. Use [`prompts/new-feature.md`](./prompts/new-feature.md) as a development guide
3. Follow [`templates/feature-template.md`](./templates/feature-template.md) for specification

### For API Development
1. Review [`api/conventions.md`](./api/conventions.md) for API standards
2. Use [`prompts/endpoint.md`](./prompts/endpoint.md) for endpoint creation
3. Document in [`api/endpoints.md`](./api/endpoints.md)

### For Code Review
1. Follow [`development/code-review.md`](./development/code-review.md)
2. Use [`prompts/code-review.md`](./prompts/code-review.md) as a review prompt
3. Reference [`.github/agents/coding-standards.md`](../.github/agents/coding-standards.md)

## Adding New Documents

### Naming Conventions
- Use **lowercase** with **hyphens** for file names: `business-rules.md`
- Use **descriptive names** that reflect the content
- Group related documents in the appropriate folder

### Document Structure
Every document must have:
1. **Title** (`# Title`)
2. **Objective** — what this document explains
3. **Scope** — what is covered and what is not
4. **Index** — table of contents with anchor links
5. **Content sections** — use `> **TODO:**` for future work

### Templates
Use the templates in [`templates/`](./templates/) for standardized documentation:
- [`feature-template.md`](./templates/feature-template.md) — new features
- [`endpoint-template.md`](./templates/endpoint-template.md) — API endpoints
- [`module-template.md`](./templates/module-template.md) — business modules
- [`business-rule-template.md`](./templates/business-rule-template.md) — business rules
- [`architecture-template.md`](./templates/architecture-template.md) — architecture docs

### Cross-Referencing
- Use **relative links** between documents: `[Link text](../other-folder/doc.md)`
- Each document should have a **Related Documents** section at the bottom
- Update the index in relevant parent documents when adding new files

## Conventions

| Convention | Rule |
|------------|------|
| Language | English for folders and file names |
| Format | Markdown (`.md`) |
| Headings | ATX-style (`#`), one H1 per file |
| Links | Relative paths, no absolute paths |
| TODOs | `> **TODO:** Description of what's needed` |
| Tables | Use GitHub-flavored markdown tables |
| Code blocks | Fenced with language tag: ` ```csharp ` |

## Related Resources

- [Agent Instructions](.github/agents/instructions.md)
- [Coding Standards](.github/agents/coding-standards.md)
- [Development Checklist](.github/agents/checklist.md)
- [Agent Persona](.github/agents/persona.md)

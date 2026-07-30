# Módulo Fiscal

## Objetivo

O módulo **Fiscal** fornece as tabelas de referência tributária utilizadas em todo o sistema para classificação fiscal de produtos, emissão de documentos fiscais e cálculos de impostos em compras e vendas.

---

# Responsabilidades

- Cadastro de CFOP (Código Fiscal de Operações)
- Cadastro de CST (Código de Situação Tributária)
- Cadastro de CSOSN (Simples Nacional)
- Cadastro de CEST (Código Especificador da Substituição Tributária)
- Cadastro de NCM (Nomenclatura Comum do Mercosul)
- Cadastro de IBPT (Imposto sobre produto)
- Vínculo CEST x NCM (CestNcm)
- Cadastro de CEPs (Cep)

---

# Principais Entidades

| Entidade | Descrição |
|----------|-----------|
| Cfop | Código Fiscal de Operações |
| Cst | Código de Situação Tributária |
| Csosn | Código Simples Nacional |
| CestNcm | Vínculo CEST x NCM |
| Ncm | Classificação fiscal do produto |
| Ibpt | Alíquotas IBPT por NCM |
| Cep | Cadastro de CEPs |

---

# Dependências

- Produto (referencia NCM, CEST, CFOP, CST)
- Compra (referencia CFOP, valores fiscais)
- Venda (referencia dados fiscais)
- Empresa (regime tributário)

---

# Regras de Negócio

- CFOP define natureza da operação (entrada/saída, dentro/fora do estado)
- CST/CSOSN define tributação do produto
- NCM é obrigatório para produtos
- CEST é obrigatório para produtos com substituição tributária

---

# Serviços Envolvidos

- TabelaAuxiliarFiscalService
- ProdutoService (consome tabelas fiscais)

---

# Boas Práticas

- Manter tabelas fiscais sempre atualizadas
- Validar consistência entre CFOP e tipo de operação
- Não permitir duplicidade de NCM

---

# Checklist

☐ Tabelas fiscais atualizadas

☐ CFOP compatível com operação

☐ NCM correto para cada produto

☐ CEST vinculado ao NCM quando aplicável

---

# Conclusão

O módulo **Fiscal** é a base de conformidade tributária do sistema. Qualquer erro nas tabelas fiscais pode gerar documentos fiscais incorretos e problemas com o fisco.

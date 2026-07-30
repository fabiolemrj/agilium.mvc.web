# Troubleshooting de Fluxos

## Objetivo

Documentar problemas frequentes nos fluxos de negócio do Agilium Manager e suas causas mais comuns.

---

## Venda Não Conclui

**Sintoma**: `RealizarVenda()` retorna false ou notificações.

**Causas prováveis**:
1. **Caixa fechado** → `ObterCaixaAberto()` retorna null
2. **Funcionário não vinculado** → `ObterIdFuncionarioPorUsuarioEmpresa()` retorna 0
3. **Estoque insuficiente** → validação de saldo falha
4. **Pagamento inválido** → `VLPAGO <= 0` ou `VendaMoeda` vazio
5. **Produto inativo** → produto não está ativo no cadastro

---

## Compra Não Efetiva

**Sintoma**: `EfetivarCompra()` notifica erro.

**Causas prováveis**:
1. **Itens sem produto** → "Não foram encontrados itens associados a produtos"
2. **Conta de estoque não configurada** → `CONTA_IDCONTAESTOQUE` vazio
3. **Controle contábil não configurado** → `CONTA_REALIZARCONTROLE` não é "1"
4. **Compra já efetivada/cancelada** → `STCOMPRA != Aberta`

---

## Caixa Não Abre

**Sintoma**: `AbrirCaixa()` falha.

**Causas prováveis**:
1. **Usuário sem funcionário vinculado** → não encontra `IDFUNC`
2. **PDV não configurado** → `IDPDV` inválido
3. **Já existe caixa aberto** → `ObterCaixaAbertoPorEmpresa()` retorna existente

---

## Sessão Expirada / Empresa Não Selecionada

**Sintoma**: Redirecionado para login ou seleção de empresa.

**Causas prováveis**:
1. **Cookie expirado** → 3h sem atividade (sliding)
2. **Servidor reiniciado** → Session perdida
3. **Empresa não selecionada** → `EmpresaSelecionadaMiddleware` bloqueia

---

## Licença Inválida

**Sintoma**: Acesso bloqueado, mensagem de licença.

**Causas prováveis**:
1. **Licença expirada** → data de validade ultrapassada
2. **Chaves de ativação inválidas** → K1...K7 não conferem
3. **Empresa sem licença** → `LicencaService.ObterPorIdEmpresa()` retorna null

---

## Validação Client-Side Não Funciona

**Sintoma**: Formulário só valida no servidor.

**Causas prováveis**:
1. `_ValidationScriptsPartial` não incluído na View
2. Ordem dos scripts errada (jquery → validate → unobtrusive)
3. Data Annotations ausentes no ViewModel

---

## XML NF-e Não Importa

**Sintoma**: `ImportarCompraDeXmlNfe()` falha.

**Causas prováveis**:
1. **XML malformado** → desserialização falha
2. **Schema NF-e incorreto** → versão não suportada
3. **Compra não encontrada** → `idCompra` inválido

---

## Documentação Relacionada

- `docs/fluxos/` — Documentação oficial de cada fluxo
- `knowledge/frontend/troubleshooting.md` — Problemas de frontend

---

## Fluxo Recomendado para Agentes de IA

1. Identificar o sintoma exato
2. Verificar pré-condições do fluxo (caixa aberto? empresa selecionada?)
3. Verificar notificações (`INotificador.TemNotificacao()`)
4. Consultar a documentação oficial do fluxo em `docs/fluxos/`
5. Verificar logs de erro do servidor

---

## Resumo

Problemas mais comuns: venda não conclui (caixa fechado, estoque insuficiente), compra não efetiva (itens sem produto), sessão expirada, licença inválida. A maioria se resolve verificando pré-condições e notificações.

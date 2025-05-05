$('#btnSalvar').click(function () {
    on();
    $('#btnSendForm').click();
    off();
});

$(function () {
    $('#btnAjuda').click(function () {

        Tour.run([
            {
                element: $('#btnNovoCadastro'),
                content: '<strong><div align="center" class="text-info">Botão adicionar</div></strong><p><div align="center">Incluir novo registro.</div></p>',
                position: 'top'
            },
            {
                element: $('#breadcrumb'),
                content: '<strong><div align="center" class="text-info">Breadcrumb</div></strong><p><div align="center">Area de Breadcrumb para navegação.</div></p>',
                position: 'top'
            },
            {
                element: $('#areaFiltro'),
                content: '<strong><div align="center" class="text-info">Filtro</div></strong><p><div align="center">Area de filtro para lista.</div></p>',
                position: 'left'
            },
            {
                element: $('#search-btn'),
                content: '<strong><div align="center" class="text-info">Botão de filtro</div></strong><p><div align="center">Faz a pesquisa de especialidade de acordo com o filtro.</div></p>',
                position: 'bottom'
            },
            {
                element: $('#divGridResultado'),
                content: '<strong><div align="center" class="text-info">Resultado</div></strong><p><div align="center">Retorna os dados da consulta.</div></p>',
                position: 'top'
            },
            {
                element: $('#btnReturn'),
                content: '<strong><div align="center" class="text-info">Botão Voltar</div></strong><p><div align="center">Voltar para pagina anterior.</div></p>',
                position: 'top'
            },
            {
                element: $('#btnSalvar'),
                content: '<strong><div align="center" class="text-info">Botão Salvar</div></strong><p><div align="center">Confirmar gravação dos dados.</div></p>',
                position: 'top'
            },
            {
                element: $('#labelEmpresa'),
                content: '<strong><div align="center" class="text-info">Empresa</div></strong><p><div align="center">campo para seleção da empresa relacionado a conta a receber.</div></p>',
                position: 'top'
            },
            {
                element: $('#labelCliente'),
                content: '<strong><div align="center" class="text-info">Cliente</div></strong><p><div align="center">campo para seleção da empresa relacionado a conta a receber.</div></p>',
                position: 'top'
            },
         
            {
                element: $('#labeCategFinanc'),
                content: '<strong><div align="center" class="text-info">Categoria Financeira</div></strong><p><div align="center">Campo para seleção da categoria financeira.</div></p>',
            },
            {
                element: $('#labelConta'),
                content: '<strong><div align="center" class="text-info">Conta</div></strong><p><div align="center">campo para seleção da conta (plano de contas) relacionado a conta a receber.</div></p>',
                position: 'top'
            },
            {
                element: $('#labelDesc'),
                content: '<strong><div align="center" class="text-info">Descrição</div></strong><p><div align="center">Campo Descrição da conta a receber.</div></p>',
            },
            {
                element: $('#labelParcelaIni'),
                content: '<strong><div align="center" class="text-info">Parcela</div></strong><p><div align="center">Campo referente a parcela da conta a receber.</div></p>',
            },
            {
                element: $('#labelDtVenc'),
                content: '<strong><div align="center" class="text-info">Data Vencimento</div></strong><p><div align="center">Campo referente a data de vencimento da conta a receber.</div></p>',
            },
            {
                element: $('#labelSituacao'),
                content: '<strong><div align="center" class="text-info">Situação</div></strong><p><div align="center">campo situação (prevista/consolidada).</div></p>',
                position: 'top'
            },
            {
                element: $('#labelTipoConta'),
                content: '<strong><div align="center" class="text-info">Tipo de conta</div></strong><p><div align="center">campo para seleção da natureza da conta (Debito/Credito).</div></p>',
            },
            {
                element: $('#labelValorConta'),
                content: '<strong><div align="center" class="text-info">Valor</div></strong><p><div align="center">campo para informar o valor da conta.</div></p>',
            },
            {
                element: $('#labelValorDesc'),
                content: '<strong><div align="center" class="text-info">Desconto</div></strong><p><div align="center">campo para informar o valor de desconto que será subtraido do valor da conta.</div></p>',
            },
            {
                element: $('#labelValorAcrescimo'),
                content: '<strong><div align="center" class="text-info">Acréscimo</div></strong><p><div align="center">campo para informar o valor de acréscimo que será somando ao valor da conta.</div></p>',
            },
            {
                element: $('#labelDataNF'),
                content: '<strong><div align="center" class="text-info">Data NF</div></strong><p><div align="center">campo para a data da nota fiscal de pagamento da conta.</div></p>',
            },
            {
                element: $('#labelNumNF'),
                content: '<strong><div align="center" class="text-info">Nº NF</div></strong><p><div align="center">campo para o numero da nota fiscal de pagamento da conta.</div></p>',
            },
            {
                element: $('#labelDataPag'),
                content: '<strong><div align="center" class="text-info">Data Pagamento</div></strong><p><div align="center">campo para data de pagamento da conta.</div></p>',
            },
            {
                element: $('#labelObs'),
                content: '<strong><div align="center" class="text-info">Observação</div></strong><p><div align="center">campo para descrição da observação relativos a conta.</div></p>',
            },
        ]);
    });
});
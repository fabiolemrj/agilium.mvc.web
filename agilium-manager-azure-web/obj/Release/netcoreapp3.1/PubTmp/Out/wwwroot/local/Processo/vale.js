$('#btnSalvar').click(function () {
    on();
    $('#btnSendForm').click();
    off();
});


$(function () {
    $('.money').mask('##0,00', { reverse: true });
    $('.js-example-basic-single').select2();
    // $('.percent').mask('##0,00', { reverse: true });
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
                element: $('#Codigo'),
                content: '<strong><div align="center" class="text-info">Codigo</div></strong><p><div align="center">Codigo do vale</div></p>',
                position: 'top'
            },
            {
                element: $('#Situacao'),
                content: '<strong><div align="center" class="text-info">Situação</div></strong><p><div align="center">Situação da vale(cancelado/ativo/utilizado).</div></p>',
                position: 'top'
            },
            {
                element: $('#labelEmpresa'),
                content: '<strong><div align="center" class="text-info">Empresa</div></strong><p><div align="center">Seleção da empresa em que o vale pode ser usado.</div></p>',
                position: 'top'
            },     

            {
                element: $('#labelCliente'),
                content: '<strong><div align="center" class="text-info">Cliente</div></strong><p><div align="center">Cliente a que foi atribuido o vale.</div></p>',
            },
            {
                element: $('#labelTipo'),
                content: '<strong><div align="center" class="text-info">Tipo</div></strong><p><div align="center">Tipo do vale (troca/presente/promoção).</div></p>',
            },
            {
                element: $('#labelValor'),
                content: '<strong><div align="center" class="text-info">Valor</div></strong><p><div align="center">Valor do vale.</div></p>',
            },

        ]);
    });
});
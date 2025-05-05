$('#btnSalvar').click(function () {
    on();
    $('#btnSendForm').click();
    off();
});

$(function () {
    $('.money').mask('##0,00', { reverse: true });
    $('.js-example-basic-single').select2();
    $(".select2").select2({  theme: "classic"});
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
                element: $('#labelEmpresa'),
                content: '<strong><div align="center" class="text-info">Empresa</div></strong><p><div align="center">seleção da empresa relativa a perda/sobra.</div></p>',
                position: 'top'
            },
            {
                element: $('#labelCodigo'),
                content: '<strong><div align="center" class="text-info">Codigo</div></strong><p><div align="center">Codigo da perda/sobra</div></p>',
                position: 'top'
            },
            {
                element: $('#labelTipo'),
                content: '<strong><div align="center" class="text-info">Tipo</div></strong><p><div align="center">seleção do tipo de perda/sobra (Quebra/Devolucao/Vencido/AcertoSaldo/FalhaOpercional/Outros).</div></p>',
                position: 'top'
            },
            {
                element: $('#labelMovimento'),
                content: '<strong><div align="center" class="text-info">Movimento</div></strong><p><div align="center">seleção do tipo de movimento (perda/sobra).</div></p>',
            },
            {
                element: $('#labelProduto'),
                content: '<strong><div align="center" class="text-info">Produto</div></strong><p><div align="center">Seleção do produto relacionado a perda/sobra.</div></p>',
            },
            {
                element: $('#labelQuant'),
                content: '<strong><div align="center" class="text-info">Quantidade</div></strong><p><div align="center">Valor de quantidade de perda/sobra.</div></p>',
            },
            {
                element: $('#labelObs'),
                content: '<strong><div align="center" class="text-info">Observação</div></strong><p><div align="center">Campo para incluir observação da perda/sobra.</div></p>',
            },

        ]);
    });
});
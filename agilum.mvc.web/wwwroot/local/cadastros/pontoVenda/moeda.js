$('#btnSalvar').click(function () {
    on();

    $('#btnSendForm').click();
    off();
});

$(document).ready(function () {
    $('.my-colorpicker1').colorpicker();

    $('.my-colorpicker1 .fa-palette').css('color', event.color.toString());
});

$(document).ready(function () {
    $('.money').mask('##0,000', { reverse: true });
})


$('#btnAjuda').click(function () {

    Tour.run([
        {
            element: $('#btnNovoCadastro'),
            content: '<strong><div align="center" class="text-info">Botão adicionar</div></strong><p><div align="center">Incluir novo registro.</div></p>',
            position: 'top'
        },
        {
            element: $('#search-btn'),
            content: '<strong><div align="center" class="text-info">Botão de filtro</div></strong><p><div align="center">Faz a pesquisa de especialidade de acordo com o filtro.</div></p>',
            position: 'bottom'
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
            element: $('#divGridResultado'),
            content: '<strong><div align="center" class="text-info">Resultado</div></strong><p><div align="center">Retorna os dados da consulta.</div></p>',
            position: 'top'
        },
        {
            element: $('#labelCodigo'),
            content: '<strong><div align="center" class="text-info">Codigo</div></strong><p><div align="center">Campo de codigo de identificação da moeda.</div></p>',
            position: 'top'
        },
        {
            element: $('#labelEmpresa'),
            content: '<strong><div align="center" class="text-info">Empresa</div></strong><p><div align="center">Campo de seleção da empresa.</div></p>',
            position: 'top'
        },
        {
            element: $('#labeltpMoeda'),
            content: '<strong><div align="center" class="text-info">Tipo Moeda</div></strong><p><div align="center">Campo para classificação do tipo de Moeda.</div></p>',
            position: 'top'
        },
        {
            element: $('#labelDesc'),
            content: '<strong><div align="center" class="text-info">Descrição</div></strong><p><div align="center">Campo para Descrição da moeda.</div></p>',
            position: 'top'
        },
        {
            element: $('#labelPorcTaxa'),
            content: '<strong><div align="center" class="text-info">% por Taxa</div></strong><p><div align="center">Campo de Porcentagem por taxa.</div></p>',
            position: 'top'
        },
        {
            element: $('#labelTroco'),
            content: '<strong><div align="center" class="text-info">Situação Troco</div></strong><p><div align="center">Campo para situação do troco (permite opu não).</div></p>',
            position: 'top'
        },
        {
            element: $('#labeltpFiscal'),
            content: '<strong><div align="center" class="text-info">TipoDocumento Fiscal</div></strong><p><div align="center">Campo para informar o tipo de documento fiscal.</div></p>',
            position: 'top'
        },
        {
            element: $('#labelTecla_Atalho'),
            content: '<strong><div align="center" class="text-info">Tecla de Atalho</div></strong><p><div align="center">Campo para Tecla de atalho.</div></p>',
            position: 'top'
        },
        {
            element: $('#labelCorBotao'),
            content: '<strong><div align="center" class="text-info">Cor Botão</div></strong><p><div align="center">campo Cor do botão.</div></p>',
            position: 'top'
        },
        {
            element: $('#labelCorFonte'),
            content: '<strong><div align="center" class="text-info">Cor Label</div></strong><p><div align="center">campo do Label.</div></p>',
            position: 'top'
        },
        {
            element: $('#labelativo'),
            content: '<strong><div align="center" class="text-info">Situação</div></strong><p><div align="center">campo situação (ativo/invativo).</div></p>',
            position: 'top'
        },
    ]);
});

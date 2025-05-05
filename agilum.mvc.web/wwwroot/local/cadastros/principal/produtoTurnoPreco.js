
$('#btnSalvar').click(function () {
    on();

    $('#btnSendForm').click();
    off();
});


$(function () {
    $('.money').mask('##0,00', { reverse: true });
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
                content: '<strong><div align="center" class="text-info">Produto</div></strong><p><div align="center">Nome do Produto relacionado ao preço.</div></p>',
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
                element: $('#labelTurno'),
                content: '<strong><div align="center" class="text-info">Numero Turno</div></strong><p><div align="center">Campo para indicação do numero do turno que o preço se refere.</div></p>',
                position: 'top'
            },
            {
                element: $('#labelTipoDiferença'),
                content: '<strong><div align="center" class="text-info">Diferença</div></strong><p><div align="center">campo para indicar o sentido do calculo do preço a ser aplicado no turno (Desconto/Acrescimo).</div></p>',
                position: 'top'
            },
            {
                element: $('#labelTipoValor'),
                content: '<strong><div align="center" class="text-info">Tipo Valor</div></strong><p><div align="center">campo para indicar se o valor a ser aplicado será absoluto ou percentual (Valor/Percentual).</div></p>',
                position: 'top'
            },
            {
                element: $('#labelValor'),
                content: '<strong><div align="center" class="text-info">Valor</div></strong><p><div align="center">Valor a ser aplicado ao preço do produto.</div></p>',
                position: 'top'
            },
        ]);
    });
});
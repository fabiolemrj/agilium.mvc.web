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
                content: '<strong><div align="center" class="text-info">Empresa</div></strong><p><div align="center">campo para seleção da empresa relacionado ao produto.</div></p>',
                position: 'top'
            },
            {
                element: $('#labelProduto'),
                content: '<strong><div align="center" class="text-info">Produto PDV</div></strong><p><div align="center">Campo de selção do produto do PDV.</div></p>',
                position: 'top'
            },     
            {
                element: $('#labelDesc'),
                content: '<strong><div align="center" class="text-info">Descrição SM</div></strong><p><div align="center">Campo Descrição do produto no Site Mercado.</div></p>',
                position: 'top'
            },
            {
                element: $('#labelPromo'),
                content: '<strong><div align="center" class="text-info">Valor Promoção</div></strong><p><div align="center">Campo do preço de promoção do produto.</div></p>',
                position: 'top'
            },
            {
                element: $('#labelQuantAtacado'),
                content: '<strong><div align="center" class="text-info">Quantidade Atacado</div></strong><p><div align="center">Campo relativo a quantidade de promoção do produto.</div></p>',
                position: 'top'
            },
            {
                element: $('#labelVlAtacado'),
                content: '<strong><div align="center" class="text-info">Valor Atacado</div></strong><p><div align="center">Campo relativo ao preço de atacado do produto.</div></p>',
                position: 'top'
            },

            {
                element: $('#labelSituacao'),
                content: '<strong><div align="center" class="text-info">Situação</div></strong><p><div align="center">Campo para definir o tipo de situação do produto.</div></p>',
                position: 'top'
            },
            {
                element: $('#labelValidade'),
                content: '<strong><div align="center" class="text-info">Validade</div></strong><p><div align="center">Campo para informar se a validade do produto está proxima.</div></p>',
                position: 'top'
            },
            {
                element: $('#card-prod'),
                content: '<strong><div align="center" class="text-info">Informações Produto</div></strong><p><div align="center">Campo para detalhar informações do produto PDV selecionado.</div></p>',
                position: 'top'
            },
            {
                element: $('#labelMoeda'),
                content: '<strong><div align="center" class="text-info">Moeda PDV</div></strong><p><div align="center">Campo seleção de moeda do PDV para associação.</div></p>',
                position: 'top'
            },
            {
                element: $('#labelMoedaSM'),
                content: '<strong><div align="center" class="text-info">Moeda SM</div></strong><p><div align="center">Campo seleção de moeda do SM para associação.</div></p>',
                position: 'top'
            },
        ]);
    });
});
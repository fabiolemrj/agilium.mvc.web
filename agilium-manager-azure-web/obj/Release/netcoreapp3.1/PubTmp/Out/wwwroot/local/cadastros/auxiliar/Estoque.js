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
                content: '<strong><div align="center" class="text-info">Empresa</div></strong><p><div align="center">Campo para seleção da empresa.</div></p>',
                position: 'top'
            },            

            {
                element: $('#labeldescr'),
                content: '<strong><div align="center" class="text-info">Descrição</div></strong><p><div align="center">Campo Descrição.</div></p>',
                position: 'top'
            },
            {
                element: $('#labeltipo'),
                content: '<strong><div align="center" class="text-info">Tipo</div></strong><p><div align="center">Campo para seleção do Tipo de estoque (Almoxarifado/combustivel) .</div></p>',
                position: 'top'
            },
            {
                element: $('#labelCapacidade'),
                content: '<strong><div align="center" class="text-info">Capacidade</div></strong><p><div align="center">Campo informa capacidade armazenamento.</div></p>',
                position: 'top'
            },
            {
                element: $('#labelativo'),
                content: '<strong><div align="center" class="text-info">Situação</div></strong><p><div align="center">campo situação (ativo/invativo).</div></p>',
                position: 'top'
            },
        ]);
    });
});
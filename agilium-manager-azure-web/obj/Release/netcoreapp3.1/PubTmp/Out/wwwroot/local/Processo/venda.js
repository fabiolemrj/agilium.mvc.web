$('#btnSalvar').click(function () {
    on();
    $('#btnSendForm').click();
    off();
});

$(document).ready(function () {
    SetModal();
});



$(function () {
    $('.money').mask('##0,00', { reverse: true });
    $('.datetime').mask('99/99/9999', { placeholder: "dd/MM/yyyy", selectOnFocus: true });
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
                element: $('#lbempresa'),
                content: '<strong><div align="center" class="text-info">Empresa</div></strong><p><div align="center">Descrição da empresa.</div></p>',
                position: 'top'
            },
            {
                element: $('#lbturno'),
                content: '<strong><div align="center" class="text-info">Nº Turno</div></strong><p><div align="center">Numero do Turno</div></p>',
                position: 'top'
            },
            {
                element: $('#lbdtturno'),
                content: '<strong><div align="center" class="text-info">Data Turno</div></strong><p><div align="center">Data do Turno.</div></p>',
                position: 'top'
            },
            {
                element: $('#lbusuarioAbert'),
                content: '<strong><div align="center" class="text-info">Usuario abertura</div></strong><p><div align="center">Usuario responsavel pela abertura do turno.</div></p>',
            },
            {
                element: $('#lbdtabert'),
                content: '<strong><div align="center" class="text-info">Data abertura</div></strong><p><div align="center">Data de abertura do turno.</div></p>',
            },
            {
                element: $('#labelObs'),
                content: '<strong><div align="center" class="text-info">Observação</div></strong><p><div align="center">Campo para incluir observação sobre o fechamento do turno.</div></p>',
            },

        ]);
    });
});
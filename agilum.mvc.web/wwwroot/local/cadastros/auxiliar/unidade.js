$('#btnSalvar').click(function () {
    on();
    $('#btnSendForm').click();
    off();
});

$(function () {
    $('#btnAjuda').click(function () {

        Tour.run([
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
                element: $('#breadcrumb'),
                content: '<strong><div align="center" class="text-info">Breadcrumb</div></strong><p><div align="center">Area de Breadcrumb para navegação.</div></p>',
                position: 'top'
            },
            {
                element: $('#labelCodigo'),
                content: '<strong><div align="center" class="text-info">Codigo</div></strong><p><div align="center">Campo Codigo da unidade.</div></p>',
                position: 'top'
            },
            {
                element: $('#labeldescr'),
                content: '<strong><div align="center" class="text-info">Descrição</div></strong><p><div align="center">Campo Descrição da Unidade.</div></p>',
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
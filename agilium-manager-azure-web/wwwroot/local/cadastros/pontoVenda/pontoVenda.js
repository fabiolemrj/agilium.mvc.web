$('#btnSalvar').click(function () {
    on();

    $('#btnSendForm').click();
    off();
});


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
            element: $('#labelEstoque'),
            content: '<strong><div align="center" class="text-info">Estoque</div></strong><p><div align="center">Campo do estoque que o PDV está relacionado.</div></p>',
            position: 'top'
        },
        {
            element: $('#labelDesc'),
            content: '<strong><div align="center" class="text-info">Descrição</div></strong><p><div align="center">Campo para Descrição do PDV.</div></p>',
            position: 'top'
        },
        {
            element: $('#labelNomeMaquina'),
            content: '<strong><div align="center" class="text-info">Noma Maquina</div></strong><p><div align="center">Campo de exibidr o nome da maquina (PC) que está relacionada ao PDV.</div></p>',
            position: 'top'
        },
        {
            element: $('#labelCaminhoCertDig'),
            content: '<strong><div align="center" class="text-info">Caminho Certificado Digital</div></strong><p><div align="center">Campo para informar caminho do certificado digital.</div></p>',
            position: 'top'
        },
        {
            element: $('#labelSenhaCertDig'),
            content: '<strong><div align="center" class="text-info">Senha Certificado Digital</div></strong><p><div align="center">Campo para informar a senha do certificado digital.</div></p>',
            position: 'top'
        },
        {
            element: $('#labelativo'),
            content: '<strong><div align="center" class="text-info">Situação</div></strong><p><div align="center">campo situação (ativo/invativo).</div></p>',
            position: 'top'
        },
    ]);
});

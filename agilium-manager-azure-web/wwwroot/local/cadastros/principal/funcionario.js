$('#btnSalvar').click(function () {
    on();

    $('#btnSendForm').click();
    off();
});


function BuscarCep() {
    const _cep = $('.cep').val();
    //ModalMensagem("success",_cep);
    $.ajax({
        type: 'get',
        url: '/Endereco/BuscarCep?cep=' + _cep,
        success: function (resultado) {
            if (resultado.erro || resultado.id_logradouro == 0 || resultado.endereco == null) {
                toastr.error("Cep não localizado")
                return;
            }
            const objetoCep = JSON.parse(JSON.stringify(resultado));

            $('.Logradouro').val(objetoCep.endereco);
            $('.Bairro').val(objetoCep.bairro);
            $('.Cidade').val(objetoCep.cidade);
            $('.Uf').val(objetoCep.uf);
        },
        error: function (result) {
            toastr.error(result)
        }
    });
}

$('#btnAjuda').click(function () {

    Tour.run([
        // === Tela de Listagem (Index) ===
        {
            element: $('#btnNovoCadastro'),
            content: '<strong><div align="center" class="text-info">Novo Funcionário</div></strong><p><div align="center">Cadastrar novo funcionário.</div></p>',
            position: 'top'
        },
        {
            element: $('#areaFiltro'),
            content: '<strong><div align="center" class="text-info">Filtro</div></strong><p><div align="center">Área de filtro para pesquisa de funcionários.</div></p>',
            position: 'left'
        },
        {
            element: $('#search-btn'),
            content: '<strong><div align="center" class="text-info">Pesquisar</div></strong><p><div align="center">Pesquisar funcionário pelo nome.</div></p>',
            position: 'bottom'
        },
        {
            element: $('#divGridResultado'),
            content: '<strong><div align="center" class="text-info">Resultado</div></strong><p><div align="center">Lista de funcionários cadastrados.</div></p>',
            position: 'top'
        },

        // === Tela de Cadastro/Edição (Create/Edit) ===
        {
            element: $('#btnVoltar'),
            content: '<strong><div align="center" class="text-info">Botão Voltar</div></strong><p><div align="center">Retornar para a página anterior.</div></p>',
            position: 'top'
        },
        {
            element: $('#btnSalvar'),
            content: '<strong><div align="center" class="text-info">Botão Salvar</div></strong><p><div align="center">Confirmar gravação dos dados do funcionário.</div></p>',
            position: 'top'
        },
        {
            element: $('#breadcrumb'),
            content: '<strong><div align="center" class="text-info">Breadcrumb</div></strong><p><div align="center">Área de navegação (breadcrumb).</div></p>',
            position: 'top'
        },
        {
            element: $('#labelUsuario'),
            content: '<strong><div align="center" class="text-info">Usuário (Controle de Acesso)</div></strong><p><div align="center">Vínculo do funcionário com um usuário do sistema para controle de acesso.</div></p>',
            position: 'top'
        },
        {
            element: $('#labelEmpresa'),
            content: '<strong><div align="center" class="text-info">Empresa</div></strong><p><div align="center">Empresa à qual o funcionário está vinculado.</div></p>',
            position: 'top'
        },
        {
            element: $('#labelCodigo'),
            content: '<strong><div align="center" class="text-info">Código</div></strong><p><div align="center">Código de identificação do funcionário.</div></p>',
            position: 'top'
        },
        {
            element: $('#labelTipoFunc'),
            content: '<strong><div align="center" class="text-info">Tipo de Funcionário</div></strong><p><div align="center">Classificação do tipo de funcionário (Vendedor, Operador de Caixa, etc.).</div></p>',
            position: 'top'
        },
        {
            element: $('#labelativo'),
            content: '<strong><div align="center" class="text-info">Situação</div></strong><p><div align="center">Situação do funcionário (Ativo/Inativo).</div></p>',
            position: 'top'
        },
        {
            element: $('#labelNome'),
            content: '<strong><div align="center" class="text-info">Nome</div></strong><p><div align="center">Nome completo do funcionário.</div></p>',
            position: 'top'
        },
        {
            element: $('#labelCpf'),
            content: '<strong><div align="center" class="text-info">CPF</div></strong><p><div align="center">Número do CPF do funcionário.</div></p>',
            position: 'top'
        },
        {
            element: $('#labelDocumento'),
            content: '<strong><div align="center" class="text-info">Documento</div></strong><p><div align="center">Número de documento complementar (RG, CNH, etc.).</div></p>',
            position: 'top'
        },
        {
            element: $('#labelRfid'),
            content: '<strong><div align="center" class="text-info">RFID</div></strong><p><div align="center">Código RFID do funcionário para identificação no PDV.</div></p>',
            position: 'top'
        },
        {
            element: $('#labelTurno'),
            content: '<strong><div align="center" class="text-info">Turno</div></strong><p><div align="center">Turno de trabalho do funcionário.</div></p>',
            position: 'top'
        },
        {
            element: $('#labelNoturno'),
            content: '<strong><div align="center" class="text-info">Noturno</div></strong><p><div align="center">Indica se o funcionário trabalha em horário noturno.</div></p>',
            position: 'top'
        },
        {
            element: $('#labelDtAdmissao'),
            content: '<strong><div align="center" class="text-info">Data de Admissão</div></strong><p><div align="center">Data de admissão do funcionário.</div></p>',
            position: 'top'
        },
        {
            element: $('#labelDtDemissao'),
            content: '<strong><div align="center" class="text-info">Data de Demissão</div></strong><p><div align="center">Data de demissão do funcionário (se aplicável).</div></p>',
            position: 'top'
        },

        // === Endereço (compartilhado via partial _endereco.cshtml) ===
        {
            element: $('#labelCep'),
            content: '<strong><div align="center" class="text-info">CEP</div></strong><p><div align="center">CEP do endereço do funcionário. Ao preencher, os demais campos são preenchidos automaticamente.</div></p>',
            position: 'top'
        },
        {
            element: $('#labelEndereco'),
            content: '<strong><div align="center" class="text-info">Endereço</div></strong><p><div align="center">Logradouro do endereço do funcionário.</div></p>',
            position: 'top'
        },
        {
            element: $('#labelNumero'),
            content: '<strong><div align="center" class="text-info">Número</div></strong><p><div align="center">Número do endereço.</div></p>',
            position: 'top'
        },
        {
            element: $('#labelComplemento'),
            content: '<strong><div align="center" class="text-info">Complemento</div></strong><p><div align="center">Complemento do endereço.</div></p>',
            position: 'top'
        },
        {
            element: $('#labelBairro'),
            content: '<strong><div align="center" class="text-info">Bairro</div></strong><p><div align="center">Bairro do endereço.</div></p>',
            position: 'top'
        },
        {
            element: $('#labelCidade'),
            content: '<strong><div align="center" class="text-info">Cidade</div></strong><p><div align="center">Cidade do endereço.</div></p>',
            position: 'top'
        },
        {
            element: $('#labelUf'),
            content: '<strong><div align="center" class="text-info">UF</div></strong><p><div align="center">Unidade da Federação (Estado).</div></p>',
            position: 'top'
        },
        {
            element: $('#labelPontoRef'),
            content: '<strong><div align="center" class="text-info">Ponto de Referência</div></strong><p><div align="center">Ponto de referência do endereço.</div></p>',
            position: 'top'
        },
    ]);
});

$('.money').mask('##0,00', { reverse: true });
$(".cnpj").mask("99.999.999/9999-99");
$(".cep").mask("99.999-999");

$('#btnSalvar').click(function () {
    on();
    $('#btnSendForm').click();
    off();
});

function BuscarCep() {
    const _cep = '/endereco/buscar-cep?cep=' + $('.cep').val();
    $.ajax({
        type: 'get',
        url: _cep,
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

$(document).on('click', '.delete', function (e) {
    e.preventDefault();

    const idContato = $(this).data('idcontato');
    const idEmpresa = $(this).data('idempresa');
    const contato = `${$(this).data('tpcontato')} - ${$(this).data('descr1')}`;

    Swal.fire({
        title: 'Confirmar exclusão',
        text: `Deseja realmente apagar o contato ${contato}?`,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Sim, apagar',
        cancelButtonText: 'Cancelar'
    }).then((confirmacao) => {

        if (!confirmacao.isConfirmed) return;

        $.ajax({
            url: '/empresa/contato/apagar',
            type: 'get',
            data: {
                idContato: idContato,
                idEmpresa: idEmpresa,
                __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
            },
            success: function (response) {

                if (!response.success) {
                    Swal.fire('Erro', response.message, 'error');
                    return;
                }

                Swal.fire('Sucesso', response.message, 'success');

                if (response.url) {
                    $('#ContatoTarget').load(response.url);
                }
            },
            error: function () {
                Swal.fire('Erro', 'Erro inesperado ao remover o contato.', 'error');
            }
        });
    });
});


$(function () {
    $('#btnAjuda').click(function () {

        Tour.run([
            // === BARRA DE BOTÕES ===
            {
                element: $('#btnVoltar'),
                content: '<strong><div align="center" class="text-info">Botão Voltar</div></strong><p><div align="center">Voltar para a lista de empresas.</div></p>',
                position: 'top'
            },
            {
                element: $('#btnSalvar'),
                content: '<strong><div align="center" class="text-info">Botão Salvar</div></strong><p><div align="center">Confirmar gravação dos dados da empresa.</div></p>',
                position: 'top'
            },
            {
                element: $('#breadcrumb'),
                content: '<strong><div align="center" class="text-info">Breadcrumb</div></strong><p><div align="center">Área de navegação. Indica Home > Empresas > Editar.</div></p>',
                position: 'top'
            },

            // === DADOS DA EMPRESA ===
            {
                element: $('#labelCodigo'),
                content: '<strong><div align="center" class="text-info">Código</div></strong><p><div align="center">Código da empresa (somente leitura).</div></p>',
                position: 'top'
            },
            {
                element: $('#labeltpempresa'),
                content: '<strong><div align="center" class="text-info">Tipo Empresa</div></strong><p><div align="center">Selecione o tipo de empresa (Matriz, Filial, etc.).</div></p>',
                position: 'top'
            },
            {
                element: $('#labelRazSoc'),
                content: '<strong><div align="center" class="text-info">Razão Social</div></strong><p><div align="center">Nome da razão social da empresa.</div></p>',
                position: 'top'
            },
            {
                element: $('#labelNomFant'),
                content: '<strong><div align="center" class="text-info">Nome Fantasia</div></strong><p><div align="center">Nome fantasia da empresa.</div></p>',
                position: 'top'
            },

            // === DOCUMENTAÇÃO ===
            {
                element: $('#labelCnpj'),
                content: '<strong><div align="center" class="text-info">CNPJ</div></strong><p><div align="center">Número do CNPJ da empresa.</div></p>',
                position: 'top'
            },
            {
                element: $('#labelInscrEstad'),
                content: '<strong><div align="center" class="text-info">Inscrição Estadual</div></strong><p><div align="center">Número da Inscrição Estadual.</div></p>',
                position: 'top'
            },
            {
                element: $('#labelInscrEstadVinc'),
                content: '<strong><div align="center" class="text-info">Inscrição Estadual Vinculada</div></strong><p><div align="center">Número da Inscrição Estadual vinculada, se houver.</div></p>',
                position: 'top'
            },
            {
                element: $('#labelInscrMunicip'),
                content: '<strong><div align="center" class="text-info">Inscrição Municipal</div></strong><p><div align="center">Número da Inscrição Municipal.</div></p>',
                position: 'top'
            },

            // === SITE MERCADO ===
            {
                element: $('#labelClienteIdMerc'),
                content: '<strong><div align="center" class="text-info">Cliente ID — Site Mercado</div></strong><p><div align="center">ID do cliente para integração com o Site Mercado.</div></p>',
                position: 'top'
            },
            {
                element: $('#labelClienteSecret_SM'),
                content: '<strong><div align="center" class="text-info">Client Secret — Site Mercado</div></strong><p><div align="center">Senha/Secret para integração com o Site Mercado.</div></p>',
                position: 'top'
            },
            {
                element: $('#labelLojaSM'),
                content: '<strong><div align="center" class="text-info">Loja — Site Mercado</div></strong><p><div align="center">Identificador da loja no Site Mercado.</div></p>',
                position: 'top'
            },

            // === CNAE / CSC ===
            {
                element: $('#labelCnae'),
                content: '<strong><div align="center" class="text-info">CNAE</div></strong><p><div align="center">Código Nacional de Atividade Econômica (CNAE).</div></p>',
                position: 'top'
            },
            {
                element: $('#labelIdCsc'),
                content: '<strong><div align="center" class="text-info">ID CSC</div></strong><p><div align="center">Identificador do Código de Segurança do Contribuinte.</div></p>',
                position: 'top'
            },
            {
                element: $('#labelCSC'),
                content: '<strong><div align="center" class="text-info">CSC</div></strong><p><div align="center">Código de Segurança do Contribuinte (CSC).</div></p>',
                position: 'top'
            },

            // === TRIBUTAÇÃO ===
            {
                element: $('#labelJuntaCom'),
                content: '<strong><div align="center" class="text-info">Junta Comercial</div></strong><p><div align="center">Número de registro na Junta Comercial.</div></p>',
                position: 'top'
            },
            {
                element: $('#labelCrt'),
                content: '<strong><div align="center" class="text-info">CRT — Regime Tributário</div></strong><p><div align="center">Selecione o Código do Regime Tributário da empresa.</div></p>',
                position: 'top'
            },

            // === DISTRIBUIÇÃO ===
            {
                element: $('#labelDistrib'),
                content: '<strong><div align="center" class="text-info">Distribuidora</div></strong><p><div align="center">Nome da distribuidora associada à empresa.</div></p>',
                position: 'top'
            },
            {
                element: $('#labelLucroPres'),
                content: '<strong><div align="center" class="text-info">Lucro Presumido</div></strong><p><div align="center">Indica se a empresa opta pelo regime de Lucro Presumido.</div></p>',
                position: 'top'
            },
            {
                element: $('#labelMicroEmp'),
                content: '<strong><div align="center" class="text-info">Microempresa</div></strong><p><div align="center">Indica se a empresa é enquadrada como Microempresa (ME).</div></p>',
                position: 'top'
            },

            // === ENDEREÇO (partial _endereco.cshtml) ===
            {
                element: $('#labelCep'),
                content: '<strong><div align="center" class="text-info">CEP</div></strong><p><div align="center">Digite o CEP e o sistema preencherá o endereço automaticamente.</div></p>',
                position: 'top'
            },
            {
                element: $('#labelEndereco'),
                content: '<strong><div align="center" class="text-info">Endereço</div></strong><p><div align="center">Logradouro do endereço (preenchido automaticamente ao informar o CEP).</div></p>',
                position: 'top'
            },
            {
                element: $('#labelNumero'),
                content: '<strong><div align="center" class="text-info">Número</div></strong><p><div align="center">Número do endereço.</div></p>',
                position: 'top'
            },
            {
                element: $('#labelComplemento'),
                content: '<strong><div align="center" class="text-info">Complemento</div></strong><p><div align="center">Complemento do endereço (apartamento, sala, bloco, etc.).</div></p>',
                position: 'top'
            },
            {
                element: $('#labelBairro'),
                content: '<strong><div align="center" class="text-info">Bairro</div></strong><p><div align="center">Bairro do endereço (preenchido automaticamente ao informar o CEP).</div></p>',
                position: 'top'
            },
            {
                element: $('#labelCidade'),
                content: '<strong><div align="center" class="text-info">Cidade</div></strong><p><div align="center">Cidade do endereço (preenchida automaticamente ao informar o CEP).</div></p>',
                position: 'top'
            },
            {
                element: $('#labelUf'),
                content: '<strong><div align="center" class="text-info">UF</div></strong><p><div align="center">Unidade Federativa (Estado) do endereço.</div></p>',
                position: 'top'
            },
            {
                element: $('#labelPontoRef'),
                content: '<strong><div align="center" class="text-info">Ponto de Referência</div></strong><p><div align="center">Ponto de referência para facilitar a localização do endereço.</div></p>',
                position: 'top'
            },

            // === CONTATOS (partial _contatoLista.cshtml) ===
            {
                element: $('#ContatoTarget'),
                content: '<strong><div align="center" class="text-info">Contatos da Empresa</div></strong><p><div align="center">Lista de contatos (telefone, e-mail, etc.) associados à empresa. Utilize o botão "Adicionar Contato" para incluir novos registros.</div></p>',
                position: 'top'
            },
        ]);
    });
});

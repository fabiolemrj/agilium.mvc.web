$(document).ready(function () {
    const _tipoPessoa = $('#tpPessoaInput').val();
    ExibirDivTipoPessoa(_tipoPessoa);
});

$('.money').mask('##0,00', { reverse: true });
$(".cnpj").mask("99.999.999/9999-99");
$(".cep").mask("99.999-999");
$('.cpf').mask('000.000.000-00', { reverse: true });

$('#btnSalvar').click(function () {
    on();

    $('#btnSendForm').click();
    off();
});


$('#btnAjuda').click(function () {

    Tour.run([
        // === Tela de Listagem (Index) ===
        {
            element: $('#btnNovoCadastro'),
            content: '<strong><div align="center" class="text-info">Novo Cliente</div></strong><p><div align="center">Cadastrar novo cliente.</div></p>',
            position: 'top'
        },
        {
            element: $('#breadcrumb'),
            content: '<strong><div align="center" class="text-info">Breadcrumb</div></strong><p><div align="center">Área de navegação (breadcrumb).</div></p>',
            position: 'top'
        },
        {
            element: $('#areaFiltro'),
            content: '<strong><div align="center" class="text-info">Filtro</div></strong><p><div align="center">Área de filtro para pesquisa de clientes.</div></p>',
            position: 'left'
        },
        {
            element: $('#search-btn'),
            content: '<strong><div align="center" class="text-info">Pesquisar</div></strong><p><div align="center">Pesquisar cliente pelo nome.</div></p>',
            position: 'bottom'
        },
        {
            element: $('#divGridResultado'),
            content: '<strong><div align="center" class="text-info">Resultado</div></strong><p><div align="center">Lista de clientes cadastrados.</div></p>',
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
            content: '<strong><div align="center" class="text-info">Botão Salvar</div></strong><p><div align="center">Confirmar gravação dos dados do cliente.</div></p>',
            position: 'top'
        },
        {
            element: $('#breadcrumb'),
            content: '<strong><div align="center" class="text-info">Breadcrumb</div></strong><p><div align="center">Área de navegação (breadcrumb).</div></p>',
            position: 'top'
        },
        {
            element: $('#labelCodigo'),
            content: '<strong><div align="center" class="text-info">Código</div></strong><p><div align="center">Código de identificação do cliente.</div></p>',
            position: 'top'
        },
        {
            element: $('#labeltpPessoa'),
            content: '<strong><div align="center" class="text-info">Tipo de Pessoa</div></strong><p><div align="center">Classificação do cliente: Pessoa Física ou Jurídica.</div></p>',
            position: 'top'
        },
        {
            element: $('#labelSituacao'),
            content: '<strong><div align="center" class="text-info">Situação</div></strong><p><div align="center">Situação do cliente (Ativo/Inativo).</div></p>',
            position: 'top'
        },
        {
            element: $('#labelRazSoc'),
            content: '<strong><div align="center" class="text-info">Nome / Razão Social</div></strong><p><div align="center">Nome do cliente ou Razão Social (Pessoa Jurídica).</div></p>',
            position: 'top'
        },
        {
            element: $('#labelPublicaEmail'),
            content: '<strong><div align="center" class="text-info">Publicidade E-mail</div></strong><p><div align="center">Permite o envio de publicidade por e-mail para o cliente.</div></p>',
            position: 'top'
        },
        {
            element: $('#labelPublicaSMS'),
            content: '<strong><div align="center" class="text-info">Publicidade SMS</div></strong><p><div align="center">Permite o envio de publicidade por SMS para o cliente.</div></p>',
            position: 'top'
        },

        // === Endereço ===
        {
            element: $('#divTabEndereco'),
            content: '<strong><div align="center" class="text-info">Endereço</div></strong><p><div align="center">Aba de endereço do cliente. Utilize as abas para navegar entre endereço padrão, cobrança, faturamento e entrega.</div></p>',
            position: 'top'
        },
        {
            element: $('#labelCepEndereco'),
            content: '<strong><div align="center" class="text-info">CEP</div></strong><p><div align="center">CEP do endereço do cliente.</div></p>',
            position: 'top'
        },
        {
            element: $('#labelLogradouroEndereco'),
            content: '<strong><div align="center" class="text-info">Logradouro</div></strong><p><div align="center">Logradouro do endereço do cliente.</div></p>',
            position: 'top'
        },
        {
            element: $('#labelNumeroEndereco'),
            content: '<strong><div align="center" class="text-info">Número</div></strong><p><div align="center">Número do endereço.</div></p>',
            position: 'top'
        },
        {
            element: $('#labelComplementoEndereco'),
            content: '<strong><div align="center" class="text-info">Complemento</div></strong><p><div align="center">Complemento do endereço.</div></p>',
            position: 'top'
        },
        {
            element: $('#labelBairroEndereco'),
            content: '<strong><div align="center" class="text-info">Bairro</div></strong><p><div align="center">Bairro do endereço.</div></p>',
            position: 'top'
        },
        {
            element: $('#labelCidadeEndereco'),
            content: '<strong><div align="center" class="text-info">Cidade</div></strong><p><div align="center">Cidade do endereço.</div></p>',
            position: 'top'
        },
        {
            element: $('#labelUfEndereco'),
            content: '<strong><div align="center" class="text-info">UF</div></strong><p><div align="center">Unidade da Federação (Estado).</div></p>',
            position: 'top'
        },
        {
            element: $('#labelPontoRefEndereco'),
            content: '<strong><div align="center" class="text-info">Ponto de Referência</div></strong><p><div align="center">Ponto de referência do endereço.</div></p>',
            position: 'top'
        },
        {
            element: $('#ContatoTarget'),
            content: '<strong><div align="center" class="text-info">Contatos</div></strong><p><div align="center">Lista de contatos do cliente (disponível na edição).</div></p>',
            position: 'top'
        },
    ]);
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

$(".cep.cobranca").on("blur", function () {
    const _tipoPessoa = $(this).val();
    BuscarCep(_tipoPessoa, '.Logradouro.cobranca', '.Bairro.cobranca', '.Cidade.cobranca', '.Uf.cobranca');
});

$(".cep.entrega").on("blur", function () {
    const _tipoPessoa = $(this).val();
    BuscarCep(_tipoPessoa, '.Logradouro.entrega', '.Bairro.entrega', '.Cidade.entrega', '.Uf.entrega');
});

$(".cep.faturamento").on("blur", function () {
    const _tipoPessoa = $(this).val();
    BuscarCep(_tipoPessoa, '.Logradouro.faturamento', '.Bairro.faturamento', '.Cidade.faturamento', '.Uf.faturamento');
});

$(".cep.padrao").on("blur", function () {
    const _tipoPessoa = $(this).val();
    BuscarCep(_tipoPessoa, '.Logradouro.padrao', '.Bairro.padrao', '.Cidade.padrao', '.Uf.padrao');
});

function BuscarCep(_cep, logradouro, bairro, cidade, uf) {
        
    $.ajax({
        type: 'get',
        url: '/Endereco/BuscarCep?cep=' + _cep,
        success: function (resultado) {
            if (resultado.erro || resultado.id_logradouro == 0 || resultado.endereco == null) {
                toastr.error("Cep não localizado")
                return;
            }
            const objetoCep = JSON.parse(JSON.stringify(resultado));
            if (objetoCep) {
                $(logradouro).val(objetoCep.endereco);
                $(bairro).val(objetoCep.bairro);
                $(cidade).val(objetoCep.cidade);
                $(uf).val(objetoCep.uf);
            }
        },
        error: function (result) {
            toastr.error(result)
        }
    });
}

function ExibirDivTipoPessoa(_tipoPessoa) {

    if (_tipoPessoa == "0") {

        $('#divPJ').hide();
        $('#divPF').show();
         $('#ClientePessoaJuridica.RazaoSocial').val('');
         $('#ClientePessoaJuridica.Cnpj').val('');
         $('#ClientePessoaJuridica.Cnpj').val('');
    } else {
        $('#divPJ').show();
        $('#divPF').hide();
        $('#ClientePessoaFisica.CPF').val('');
        $('#ClientePessoaFisica.NumeroDocumento').val('');
        $('#ClientePessoaFisica.DataNascimento').val('');
    }
}
$("#tpPessoaInput").on("change", function () {
    const _tipoPessoa = $('#tpPessoaInput').val();
    ExibirDivTipoPessoa(_tipoPessoa);
});


$('.delete').click(function (event) {

    event.preventDefault();

    const idContato = $(this).attr("data-idContato");
    const idCliente = $(this).attr("data-idCliente");
    const contato = $(this).attr("data-tpcontato") + " - " + $(this).attr("data-descr1");

    Swal.fire({
        title: 'Deseja realmente apagar o contato selecionado?',
        text: `${contato}`,
        icon: 'danger',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        cancelButtonText: 'Sair',
        confirmButtonText: 'OK'
    }).then((result) => {
        if (result.isConfirmed) {

            $.ajax({
                type: 'get',
                url: `/Cliente/DeleteContato?idContato=${idContato}&idCliente=${idCliente}`,
                success: function (resultado) {
                    if (resultado.erro) {
                        toastr.error(resultado.erro)
                        Swal.fire({
                            icon: 'error',
                            title: 'Oops...',
                            text: resultado.erro
                        });
                        if (res.url) {
                            $('#ContatoTarget').load(res.url);
                        }
                        return;
                    }
                },
                error: function (result) {
                    toastr.error(result)
                }
            }).then((result) => {
                let icone = "success";
                let titulo = "Sucesso"
                let msg = `Contato ${contato} Removido!`;
                if (result.erro) {
                    icone = "error"
                    msg = erro;
                    titulo = 'Oops...'
                }

                Swal.fire({
                    icon: icone,
                    title: titulo,
                    text: msg
                }).then((res) => {
                    if (result.url) {
                        $('#ContatoTarget').load(result.url);
                    }
                })
            });
        }
    })
});
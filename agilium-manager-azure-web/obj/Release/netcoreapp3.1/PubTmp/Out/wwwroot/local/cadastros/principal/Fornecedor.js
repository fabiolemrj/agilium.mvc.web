
$('.money').mask('##0,00', { reverse: true });
$(".cnpj").mask("99.999.999/9999-99");
$(".cep").mask("99.999-999");
$('.cpf').mask('000.000.000-00', { reverse: true });

//function SelecionMascara() {
//    const _tipoPessoa = $('#TipoPessoa').val();
//    if (_tipoPessoa == 'F')
//        $('#CpfCnpj').removeClass('cnpj').addClass('cpf');
//}

$(document).ready(function () {
    $('#TipoPessoa').val('1');
    OcultarCPfCnpj();
 
});

$("#TipoPessoa").on("change", function () {
    OcultarCPfCnpj();
  
});

function OcultarCPfCnpj() {
    //const _tipoPessoa = $('#TipoPessoa').val();
    //if (_tipoPessoa == '0') {       
    //    $('.cnpj').hide();
    //    $('.cpf').show();
    //    $('.cnpj').val('');
    //}
    //else {
    //    $('.cpf').hide();
    //    $('.cnpj').show();
    //    $('.cpf').val('');
    //}
}


$('.delete').click(function (event) {

    alert('delete');
    event.preventDefault();

    const idContato = $(this).attr("data-idContato");
    const idFornecedor = $(this).attr("data-idFornecedor");
    const contato = $(this).attr("data-tpcontato") + " - " + $(this).attr("data-descr1");

    Swal.fire({
        title: 'Deseja realmente apagar o contato selecionado?',
        text: `${contato}?`,
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
                url: `/Fornecedor/DeleteContato?idContato=${idContato}&idFornecedor=${idFornecedor}`,
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
                content: '<strong><div align="center" class="text-info">Codigo</div></strong><p><div align="center">Campo de codigo de identificação das empresas.</div></p>',
                position: 'top'
            },

            {
                element: $('#labeltpPessoa'),
                content: '<strong><div align="center" class="text-info">Tipo Pessoa</div></strong><p><div align="center">Campo para classificação do tipo de pessoa (Fisica ou Juridica).</div></p>',
                position: 'top'
            },
            {
                element: $('#labelRazSoc'),
                content: '<strong><div align="center" class="text-info">Razão Social</div></strong><p><div align="center">Campo para Razão Social ou Nome do fornecedor.</div></p>',
                position: 'top'
            },
            {
                element: $('#labelFantasia'),
                content: '<strong><div align="center" class="text-info">Nome Fantasia</div></strong><p><div align="center">Campo de Nome fantasia no caso de pessoa juridica.</div></p>',
                position: 'top'
            },
            {
                element: $('#labelCnpj'),
                content: '<strong><div align="center" class="text-info">CPF/CNPJ</div></strong><p><div align="center">Campo para informar numero do documento, CPF ou CNPJ de acordo com o tipo de pessoa.</div></p>',
                position: 'top'
            },
            {
                element: $('#labelInscrEstad'),
                content: '<strong><div align="center" class="text-info">Inscrição Estadual</div></strong><p><div align="center">Campo para informar numero inscrição estadual no caso de pessoas juridica.</div></p>',
                position: 'top'
            },
            {
                element: $('#labeltpFiscal'),
                content: '<strong><div align="center" class="text-info">Tipo Fiscal</div></strong><p><div align="center">Campo para classificação de Tipo Fiscal (Simples/Distribuição/industria).</div></p>',
                position: 'top'
            },
            {
                element: $('#labelativo'),
                content: '<strong><div align="center" class="text-info">Situação</div></strong><p><div align="center">campo situação (ativo/invativo).</div></p>',
                position: 'top'
            },
            {
                element: $('#labelCep'),
                content: '<strong><div align="center" class="text-info">CEP</div></strong><p><div align="center">campo de CEP do endereço.</div></p>',
                position: 'top'
            },
            {
                element: $('#labelEndereco'),
                content: '<strong><div align="center" class="text-info">Endereço</div></strong><p><div align="center">Campo descrição do Endereço.</div></p>',
                position: 'top'
            },
            {
                element: $('#labelNumero'),
                content: '<strong><div align="center" class="text-info">Numero</div></strong><p><div align="center">campo Numero do endereço.</div></p>',
                position: 'top'
            },
            {
                element: $('#labelComplemento'),
                content: '<strong><div align="center" class="text-info">Complemento</div></strong><p><div align="center">campo de complemento do endereço.</div></p>',
                position: 'top'
            },

            {
                element: $('#labelBairro'),
                content: '<strong><div align="center" class="text-info">Bairro</div></strong><p><div align="center">campo de Bairro do endereço.</div></p>',
                position: 'top'
            },
            {
                element: $('#labelCidade'),
                content: '<strong><div align="center" class="text-info">Cidade</div></strong><p><div align="center">campo de Cidade do endereço.</div></p>',
                position: 'top'
            },           
            {
                element: $('#labelUf'),
                content: '<strong><div align="center" class="text-info">UF</div></strong><p><div align="center">campo de Unidade da federação (Estados).</div></p>',
                position: 'top'
            },
            {
                element: $('#labelPontoRef'),
                content: '<strong><div align="center" class="text-info">Ponto de Referencia</div></strong><p><div align="center">campo de Referencia do endereço.</div></p>',
                position: 'top'
            },            
       
            {
                element: $('#divGridResultadoContato'),
                content: '<strong><div align="center" class="text-info">Contatos</div></strong><p><div align="center">Lista de contatos do forncededor.</div></p>',
                position: 'top'
            },
            {
                element: $('#btnaddContato'),
                content: '<strong><div align="center" class="text-info">Incluir novo contato</div></strong><p><div align="center">Botão para incluir novo contato do fornecedor.</div></p>',
                position: 'top'
            }, 
            {
                element: $('#btnedtContato'),
                content: '<strong><div align="center" class="text-info">Editar contato</div></strong><p><div align="center">Botão para editar contato selecionado do fornecedor.</div></p>',
                position: 'top'
            }, 
            {
                element: $('#btndeleteContato'),
                content: '<strong><div align="center" class="text-info">Apagar contato</div></strong><p><div align="center">Botão para apagar contato selecionado do fornecedor.</div></p>',
                position: 'top'
            }, 
        ]);
    });

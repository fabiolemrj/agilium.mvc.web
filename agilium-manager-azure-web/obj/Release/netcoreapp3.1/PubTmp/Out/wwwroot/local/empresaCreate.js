
$('.money').mask('##0,00', { reverse: true });
$(".cnpj").mask("99.999.999/9999-99");
$(".cep").mask("99.999-999");

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

function AdicionarContato(tpcontato, descr1, descr2) {
    alert('adicionar contato');
    //$.ajax({
    //    type: 'get',
    //    url: `/Empresa/AdicionarContato?tpcontato=${tpcontato}&descr1=${descr1}&descr2=${descr2}`,
    //    success: function (resultado) {
    //        if (resultado.erro) {
    //            toastr.error("erro")
    //            return;
    //        }
    //        const objeto = JSON.parse(JSON.stringify(resultado));
    //    },
    //    error: function (result) {
    //        toastr.error(result)
    //    }
    //});

}


$('.delete').click(function (event) {

    event.preventDefault();

    const idContato = $(this).attr("data-idContato");
    const idEmpresa = $(this).attr("data-idEmpresa");
    const contato = $(this).attr("data-tpcontato") +" - " +  $(this).attr("data-descr1");

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
                url: `/Empresa/DeleteContato?idContato=${idContato}&idEmpresa=${idEmpresa}`,
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
                content: '<strong><div align="center" class="text-info">Codigo</div></strong><p><div align="center">Campo Codigo da Empresa.</div></p>',
                position: 'top'
            },
            {
                element: $('#labeltpempresa'),
                content: '<strong><div align="center" class="text-info">Tipo Empresa</div></strong><p><div align="center">Campo Tipo de Empresa.</div></p>',
                position: 'top'
            },
            {
                element: $('#labelRazSoc'),
                content: '<strong><div align="center" class="text-info">Razão Social</div></strong><p><div align="center">campo Razão Social.</div></p>',
                position: 'top'
            },
            {
                element: $('#labelNomFant'),
                content: '<strong><div align="center" class="text-info">Nome Fantasia</div></strong><p><div align="center">campo Nome fantasia.</div></p>',
                position: 'top'
            },           
            {
                element: $('#labelCnpj'),
                content: '<strong><div align="center" class="text-info">CNPJ</div></strong><p><div align="center">campo CNPJ da empresa.</div></p>',
                position: 'top'
            },
            {
                element: $('#labelInscrEstad'),
                content: '<strong><div align="center" class="text-info">Inscrição Estadual</div></strong><p><div align="center">campo Inscrição Estadual.</div></p>',
                position: 'top'
            },
            {
                element: $('#labelInscrEstadVinc'),
                content: '<strong><div align="center" class="text-info">Inscrição Estadual Vinculada</div></strong><p><div align="center">campo de inscrição estadual.</div></p>',
                position: 'top'
            },
            {
                element: $('#labelInscrMunicip'),
                content: '<strong><div align="center" class="text-info">Inscrição Municipal</div></strong><p><div align="center">campo Inscrição Municipal.</div></p>',
                position: 'top'
            },
            {
                element: $('#labelClienteIdMerc'),
                content: '<strong><div align="center" class="text-info">Cliente ID SM</div></strong><p><div align="center">campo ID do cliente para acesso ao serviço Site Mercado (SM).</div></p>',
                position: 'top'
            },
            {
                element: $('#labelClienteSecret_SM'),
                content: '<strong><div align="center" class="text-info">Secret SM</div></strong><p><div align="center">campo senha para acesso ao serviço Site Mercado.</div></p>',
                position: 'top'
            },
            {
                element: $('#labelLojaSM'),
                content: '<strong><div align="center" class="text-info">Loja SM</div></strong><p><div align="center">campo do serviço da Loja do site mercado.</div></p>',
                position: 'top'
            },
            {
                element: $('#labelCnae'),
                content: '<strong><div align="center" class="text-info">CNAE</div></strong><p><div align="center">campo CNAE.</div></p>',
                position: 'top'
            },
            {
                element: $('#labelIdCsc'),
                content: '<strong><div align="center" class="text-info">ID CSC</div></strong><p><div align="center">campo Codigo de segurança do contribuinte (CSC).</div></p>',
                position: 'top'
            },
            {
                element: $('#labelCSC'),
                content: '<strong><div align="center" class="text-info">CSC</div></strong><p><div align="center">campo Codigo de segurança do contribuinte.</div></p>',
                position: 'top'
            },
            {
                element: $('#labelJuntaCom'),
                content: '<strong><div align="center" class="text-info">Junta Comercial</div></strong><p><div align="center">campo para codigo na junta comercial.</div></p>',
                position: 'top'
            },
            {
                element: $('#labelNuCaparm'),
                content: '<strong><div align="center" class="text-info">NUCAPARM</div></strong><p><div align="center">campo NUCAPRM.</div></p>',
                position: 'top'
            },
            {
                element: $('#labelCrt'),
                content: '<strong><div align="center" class="text-info">CRT</div></strong><p><div align="center">campo Codigo do Regime Tributario.</div></p>',
                position: 'top'
            },
            {
                element: $('#labelDistrib'),
                content: '<strong><div align="center" class="text-info">Distribuidora</div></strong><p><div align="center">campo nome da distribuidora.</div></p>',
                position: 'top'
            },
            {
                element: $('#labelLucroPres'),
                content: '<strong><div align="center" class="text-info">Lucro Presumido</div></strong><p><div align="center">campo para confirmar que a empresa é Lucro Presumido.</div></p>',
                position: 'top'
            },
            {
                element: $('#labelMicroEmp'),
                content: '<strong><div align="center" class="text-info">MicroEmpresa</div></strong><p><div align="center">campo para confirmar que a empresa é Micro Empresa.</div></p>',
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
                element: $('#labelBairro'),
                content: '<strong><div align="center" class="text-info">Bairro</div></strong><p><div align="center">campo de Bairro do endereço.</div></p>',
                position: 'top'
            },
            {
                element: $('#labelUf'),
                content: '<strong><div align="center" class="text-info">UF</div></strong><p><div align="center">campo de Unidade da federação (Estados).</div></p>',
                position: 'top'
            },
            {
                element: $('#labeltpend'),
                content: '<strong><div align="center" class="text-info">Tipo Endereço</div></strong><p><div align="center">campo de Tipo de endereço.</div></p>',
                position: 'top'
            },
            {
                element: $('#labelPontoRef'),
                content: '<strong><div align="center" class="text-info">Ponto de Referencia</div></strong><p><div align="center">campo de Ponto de Referencia.</div></p>',
                position: 'top'
            },
        ]);
    });
});
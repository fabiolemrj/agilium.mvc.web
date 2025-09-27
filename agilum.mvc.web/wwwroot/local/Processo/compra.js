$('#btnSalvar').click(function () {
    on();
    $('#btnSendForm').click();
    off();
});

$(document).ready(function () {
    SetModalLocal();
});

function Salvar()
{
    on();
    $('#btnSendForm').click();
    off();
}

$(function () {
    // $('.money').mask('#.##9,99', { reverse: true });
    $('.money').mask('000.000.000.000.000,00', { reverse: true, placeholder: "0,00" });
    $('.datetime').mask('99/99/9999', { placeholder: "dd/MM/yyyy", selectOnFocus: true });
});


$('.item').click(function (event) {

    event.preventDefault();
    
    const id = $(this).attr("data-idcompra");
    const _url = '/compra/IndexItem?id=' + id;
    //const _url = '/mvc/compra/IndexItem?id=' + id;
    on();
    $.ajax({
        type: 'get',
        url: _url,
        success: function (resultado) {
            $("#item").html(resultado);
            off();
        },
        error: function (result) {
            toastr.error(result);
            off();
        }
    });
});

function AbrirArquivo() {
    $('#arquivoNFe').click();
}

function ImportarArquivo(e) {

    var formData = new FormData($('#frmXMLImportada').get(0));

    // e.preventDefault();
    on();
    $.ajax({
        type: "post",
        url: '/compra/ImportarXML',
        mimeType: "multipart/form-data",
        contentType: false,
        processData: false,
        data: formData,
        success: function (data) {    
           
            if (data) {
                $('#resultado').html(data);
                if ($('#sucesso').val().toLowerCase() === "true")
                    toastr.success("Arquivo NFe importado com sucesso");
                
            } else {
                $('#arquivoNFe').val('');
            }
            off();
          
        },
        error: function (result) {
            var msg = result;
            $('#arquivoNFe').val('');
            toastr.error(msg);
            off();
        }
    });
};

function AbrirArquivoNfeClick(idcompra) {
    $('#abrirArquivoNfe').click();

}

function BuscarProduto()
{
    const idProduto = $("#IDPRODUTO").val();
    on();
    const _url = '/produto/ObterProduto?id=' + idProduto;
    //const _url = '/mvc/produto/ObterProduto?id=' + idProduto;
    $.ajax({
        type: 'get',
        url: _url,
        success: function (resultado) {
            var objeto = JSON.parse(JSON.stringify(resultado));
            $("#Relacao").val(objeto.relacaoCompraVenda);
            $("#ValorNovoPrecoVenda").val(objeto.preco); 
            
            off();
        },
        error: function (result) {
            toastr.error(result);
            off();
        }
    });
}
/*
function SetModalLocal() {

    $(document).ready(function () {
        $(function () {
         //   $.ajaxSetup({ cache: false });

            $("a[data-modal-local]").on("click",
                function (e) {
                    $('#myModalContent').load(this.href,
                        function () {
                            $('#myModal').on('shown.bs.modal', function () {
                                // aqui você pode inicializar máscaras e outros plugins
                            });

                            $('#myModal').modal({
                                keyboard: true
                            },
                                'show');
                            bindFormLocal(this);
                        });
                    return false;
                });
        });
    });
}

function bindFormLocal(dialog) {
    $('form', dialog).submit(function () {
        $.ajax({
            url: this.action,
            type: this.method,
            data: $(this).serialize(),
            success: function (result) {
               
                if (result.success) {   
                    $('#myModal').modal('hide');
                    if (result.url) {
                        window.location.href = result.url; // Carrega o resultado HTML para a div demarcada

                        ModalMensagem("success","Operação realizada com sucesso")
                    }

                } else {
                    $('#myModalContent').html(result);
                    bindFormLocal(dialog);
                }
            }
        });

        SetModalLocal();
        return false;
    });
}
*/

// Função para aplicar máscara de moeda
function aplicarMascaraMoeda() {
    if ($.fn.maskMoney) {
        $(".moeda").maskMoney({
            prefix: "R$ ",
            allowNegative: false,
            thousands: ".",
            decimal: ",",
            affixesStay: true
        });
    }
}

// Função principal para configurar links que abrem modais
function SetModalLocal() {
    // delega eventos para links com data-modal-local
    $(document).on("click", "a[data-modal-local]", function (e) {
        e.preventDefault();

        var url = this.href;

        // carrega o conteúdo do modal
        $('#myModalContent').load(url, function () {
            // aplica máscara nos campos carregados
            aplicarMascaraMoeda();

            // exibe o modal
            $('#myModal').modal({
                keyboard: true,
                show: true
            });

            // vincula o submit do formulário dentro do modal
            bindFormLocal($('#myModalContent'));
        });
    });
}

// Função para vincular o submit do formulário no modal
function bindFormLocal(dialog) {
    $('form', dialog).off('submit').on('submit', function (e) {
        e.preventDefault();

        var $form = $(this);

        $.ajax({
            url: $form.attr('action'),
            type: $form.attr('method'),
            data: $form.serialize(),
            success: function (result) {
                if (result.success) {
                    $('#myModal').modal('hide');

                    if (result.url) {
                        window.location.href = result.url;
                        ModalMensagem("success", "Operação realizada com sucesso");
                    }
                } else {
                    $('#myModalContent').html(result);
                    // reaplica bind e máscara para novo conteúdo
                    bindFormLocal($('#myModalContent'));
                    aplicarMascaraMoeda();
                }
            }
        });
    });
}

// Inicializa a função
$(document).ready(function () {
    SetModalLocal();
});


function formatarMoeda(meuInput) {
    const input = document.getElementById(meuInput);
    let valor = input.value;

    // Remove caracteres não numéricos
    valor = valor.replace(/[^0-9]/g, '');

    // Verifica se o valor é um número válido
    if (isNaN(valor) || valor === "") {
        input.value = ""; // Limpa o campo se não for um número válido
        return;
    }

    // Converte para número e formata
    const numero = Number(valor) / 100;
    input.value = numero.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' });
}

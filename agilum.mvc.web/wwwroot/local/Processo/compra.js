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
    //const id = 1;
    const id = $(this).attr("data-idcompra");
    on();
    $.ajax({
        type: 'get',
        url: '/compra/IndexItem?id='+id,
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
    $.ajax({
        type: 'get',
        url: '/produto/ObterProduto?id=' + idProduto,
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

function SetModalLocal() {

    $(document).ready(function () {
        $(function () {
         //   $.ajaxSetup({ cache: false });

            $("a[data-modal-local]").on("click",
                function (e) {
                    $('#myModalContent').load(this.href,
                        function () {
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

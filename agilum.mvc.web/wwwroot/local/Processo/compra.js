$('#btnSalvar').click(function () {
    on();
    $('#btnSendForm').click();
    off();
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
// Função para aplicar máscara de moeda
function aplicarMascaraMoeda() {
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

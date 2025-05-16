$('#btnSalvar').click(function () {
    on();
    $('#btnSendForm').click();
    off();
});


function SelecionarEmpresa() {
    $('#cbEmpresaUsuario').change(function () {
        const id = $(this).val();
        $('a').attr('asp-route-idEmpresa', $(this).val());
        $('#idEmpresaSelec').val(id);
        ObterIdEmpresaSelecionada();
    });
}

$(document).ready(function () {

    //Renderiza a mensagem que retorna da controller
    if ("@ViewBag.TipoMensagem" != '') {
        ModalMensagem("@ViewBag.TipoMensagem", "@Html.Raw(ViewBag.Mensagem)");
    }
    else if ("@TempData["TipoMensagem"]" != '')
{
    ModalMensagem("@TempData["TipoMensagem"]", "@TempData["Mensagem"]");
}

ObterFoto();

// ObterIdEmpresaSelecionada();

ObterEmpresaUsuario();
SelecionarEmpresa();
         });

function ObterFoto() {
    const id = "@appUser.ObterUserId()";
    //alert(id);
    $.get('/Usuario/ExibirImagemUsuarioJson?idUserAspNet=' + id, null, function (response) {
        var objeto = JSON.parse(JSON.stringify(response));
        if (objeto.error) {
            alert(objeto.error)
            return;
        }

        if (objeto.imagem != "" && objeto.imagem != undefined && objeto.imagem != null)
            $("#fotoUsuario").attr("src", objeto.imagem);

    }).fail(function (err) {
        toastr.error('ERRO: ' + err);
    });
}

function ObterIdEmpresaSelecionada() {
    const idEmpresaSelecionada = $('#cbEmpresaUsuario').val();

    $.get('/Usuario/SelecionarEmpresa?idEmpresaSelecionada=' + idEmpresaSelecionada, null, function (response) {
        var objeto = JSON.parse(JSON.stringify(response));
        if (objeto.error) {
            alert(objeto.error)
            return;
        }

        if (objeto.idSelecionado) {

            return objeto.idSelecionado
        }

        return "-1";
    });
}

function ObterEmpresaUsuario() {
    const id = "@appUser.ObterUserId()";


    $.get('/Usuario/ObterEmpresasUsuarioJson?idUserAspNet=' + id, null, function (response) {
        var objeto = JSON.parse(JSON.stringify(response));
        if (objeto.error) {
            alert(objeto.error)
            return;
        }

        var $select = $('#cbEmpresaUsuario');

        objeto.lista.map(item => {
            //if (item.idempresa == objeto.idEmpresaSelecionada)
            //    alert(objeto.idEmpresaSelecionada);

            $select.append('<option value=' + item.idempresa + '>' + item.nomeEmpresa + '</option>');
        });

        if (objeto.idEmpresaSelecionada && objeto.idEmpresaSelecionada !== '' && $select.val() !== objeto.idEmpresaSelecionada)
            $select.val(objeto.idEmpresaSelecionada);

    }).fail(function (err) {
        toastr.error('ERRO: ' + err);
    });
}

function on() {
    document.getElementById("overlay").style.display = "block";
}

function off() {
    document.getElementById("overlay").style.display = "none";
}

function ModalMensagem(tipoModal, mensagemModal) {
    var _tipoModal;

    toastr.options = {
        "closeButton": true,
        "debug": false,
        "newestOnTop": false,
        "progressBar": true,
        "positionClass": "toast-top-right",
        "preventDuplicates": false,
        "showDuration": "1000",
        "hideDuration": "1000",
        "timeOut": "10000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    }

    switch (tipoModal) {

        case "warning":
            toastr.warning(mensagemModal)
            break;
        case "danger":
            toastr.error(mensagemModal)
            break;
        case "info":
            toastr.info(mensagemModal)
            break;
        case "success":
            toastr.success(mensagemModal)
            break;

        default:
            toastr.success(mensagemModal)
            break;
    }
}

//$('#modal-dialog').on('show', function () {
//    var link = $(this).data('link'),
//        confirmBtn = $(this).find('.confirm');
//})


$('#btnYes').click(function () {

    // handle form processing here

    alert('submit form');
    $('form').submit();

});

$(".indisponivel").click(function () {
    ModalMensagem("danger", "Recurso Indisponível para versão Web");
});

$(document).ready(function () {
    SetModal();

    $("input[data-bootstrap-switch]").each(function () {
        $(this).bootstrapSwitch('state', $(this).prop('checked'));
    });

    $('#formEmpresaAuth').submit(function (e) {
        e.preventDefault();
        $('#myModal').modal('hide');
        return false;
    });
});

$(function () {
    $('#btnAjuda').click(function () {
        Tour.run([
            {
                element: $('#btnVoltar'),
                content: '<strong><div align="center" class="text-info">Botão de Voltar</div></strong><p><div align="center">Retorna a pagina anterior.</div></p>',
                position: 'top'
            },
            {
                element: $('#btnNovo'),
                content: '<strong><div align="center" class="text-info">Botão de Adicionar</div></strong><p><div align="center">Adiciona novo registro de usuario.</div></p>',
                position: 'top'
            },
            {
                element: $('.barra-de-posicao-atual'),
                content: '<strong><div align="center" class="text-info">Bread Crumb</div></strong><p><div align="center">Caminho de telas percorridos até a tela atual.</div></p>',
                position: 'left'
            },

            {
                element: $('#labelFiltro'),
                content: '<strong><div align="center" class="text-info">Cliente</div></strong><p><div align="center">Filtro para pesquisa de usuarios por nome.</div></p>',
                position: 'top'
            },
            {
                element: $('#search-btn'),
                content: '<strong><div align="center" class="text-info">Botão de Busca</div></strong><p><div align="center">Botão de pesquisa de clientes.</div></p>',
                position: 'right'
            },
            {
                element: $('#divGridResultado'),
                content: '<strong><div align="center" class="text-info">Resultado</div></strong><p><div align="center">Retorna lista de usuarios.</div></p>',
                position: 'top'
            },
            {
                element: $('#listaNome'),
                content: '<strong><div align="center" class="text-info">Nome do usuario</div></strong><p><div align="center">Coluna com campo Nome do usuario.</div></p>',
                position: 'top'
            },
            {
                element: $('#listaEmail'),
                content: '<strong><div align="center" class="text-info">Email</div></strong><p><div align="center">Coluna com campo Email usuario.</div></p>',
                position: 'top'
            },
            {
                element: $('#listaBtAtivo'),
                content: '<strong><div align="center" class="text-info">Mudar situação</div></strong><p><div align="center">Coluna com botão para mudar a situação do usuário (inativar/ativar).</div></p>',
                position: 'top'
            },
            {
                element: $('#listaAcoes'),
                content: '<strong><div align="center" class="text-info">Ações</div></strong><p><div align="center">Coluna com botão de ações relacionados ao usuário (Editar, gerenciar claims e afins).</div></p>',
                position: 'top'
            },
        ]);
    });
});

function NovoUsuarioWeb(idUsuario) {
    on();
  
    let _cep = '/usuario/criar-novo-usuario?idUsuario=' + idUsuario;

    toastr.options = {
        "closeButton": true,
        "debug": false,
        "newestOnTop": false,
        "progressBar": true,
        "positionClass": "toast-top-center",
        "preventDuplicates": false,
        "showDuration": "1000",
        "hideDuration": "1000",
        "timeOut": "50000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    }

    $.ajax({
        type: 'get',
        url: _cep,
        success: function (resultado) {
            if (!resultado.sucesso) {
                toastr.error(resultado.erro,"ERRO");
                off();
                return;
            }

            toastr.success(resultado.erro,"SUCESSO")
            off();
        },
        error: function (result) {
            toastr.error(result,"ERRO");
            off();
        }
    });
}

function ReenviarEmail(idUsuario) {
    on();

    let _cep = '/usuario/reenviar-email?idUsuario=' + idUsuario;

   

    toastr.options = {
        "closeButton": true,
        "debug": false,
        "newestOnTop": false,
        "progressBar": true,
        "positionClass": "toast-top-center",
        "preventDuplicates": false,
        "showDuration": "1000",
        "hideDuration": "1000",
        "timeOut": "50000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    }

    $.ajax({
        type: 'get',
        url: _cep,
        success: function (resultado) {
            if (!resultado.sucesso) {
                toastr.error(resultado.erro, "ERRO");
                off();
                return;
            }

            toastr.success(resultado.erro, "SUCESSO")
            off();
        },
        error: function (result) {
            toastr.error(result, "ERRO");
            off();
        }
    });
}

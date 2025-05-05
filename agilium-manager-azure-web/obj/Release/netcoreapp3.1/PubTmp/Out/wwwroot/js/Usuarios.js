

async function Remover(id, ativo) {
    console.log('2');
    const filtroPesq = $("#q").val();
    const ps = $("#PageSize").val();
    const page = $("#PageIndex").val();

    console.log('3');
    var willdelete = await Swal.fire({
        title: 'Alterar Situação Usuário',
        text: "Tem certeza que deseja alterar a situação do usuário?",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Sim'
    });
    console.log('4');
    if (willdelete.isConfirmed) {
        on();
        const _url = '/Usuario/MudarSituacaoUsuario?id=' + (id) + "&ativo=" + ativo;
        console.log('5');
        jQuery.ajax({
            url: _url,
            success: function (data) {
                console.log('6');
                var objeto = JSON.parse(JSON.stringify(data));
                if (objeto.error) {
                    var msg = "Erro ao tentar mudar situação do usuário";
                    if (objeto.msg != undefined && objeto.msg != '' && objeto.msg != null)
                        msg = objeto.msg;

                    Swal.fire({
                        title: 'Erro',
                        text: msg,
                        icon: 'error'
                    });
                    return false;
                }

                var msg = "Situação do usuario alterada!";
                if (objeto.msg != undefined && objeto.msg != '' && objeto.msg != null)
                    msg = objeto.msg;
                console.log('7');
                var url = `/usuarios?page=${page}&ps=${ps}&q=${filtroPesq}`;
                window.location = url;
                $("#q").val(filtroPesq);

                toastr.success(msg);
            }
        });


        off();
    } else {
        console.log('1');
        return;
    }

}
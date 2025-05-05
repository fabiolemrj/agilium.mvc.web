$('#btnSalvar').click(function () {
    on();
    $('#btnSendForm').click();
    off();
});


$(function () {
    $('#btnAjuda').click(function () {

        Tour.run([
            {
                element: $('#btnNovoCadastro'),
                content: '<strong><div align="center" class="text-info">Botão adicionar</div></strong><p><div align="center">Incluir novo registro.</div></p>',
                position: 'top'
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
                element: $('#search-btn'),
                content: '<strong><div align="center" class="text-info">Botão de filtro</div></strong><p><div align="center">Faz a pesquisa de especialidade de acordo com o filtro.</div></p>',
                position: 'bottom'
            },
            {
                element: $('#divGridResultado'),
                content: '<strong><div align="center" class="text-info">Resultado</div></strong><p><div align="center">Retorna os dados da consulta.</div></p>',
                position: 'top'
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
                element: $('#labelEmpresa'),
                content: '<strong><div align="center" class="text-info">Empresa</div></strong><p><div align="center">Seleção da empresa do inventario.</div></p>',
                position: 'top'
            },
            {
                element: $('#labelestoque'),
                content: '<strong><div align="center" class="text-info">Estoque</div></strong><p><div align="center">Seleção do estoque que está vinculado a inventário.</div></p>',
                position: 'top'
            },
            {
                element: $('#labelCodigo'),
                content: '<strong><div align="center" class="text-info">Codigo</div></strong><p><div align="center">Codigo de identificação do inventario</div></p>',
                position: 'top'
            },
            {
                element: $('#labelData'),
                content: '<strong><div align="center" class="text-info">Data</div></strong><p><div align="center">Data de realização do inventario</div></p>',
                position: 'top'
            },
            {
                element: $('#labelDescricao'),
                content: '<strong><div align="center" class="text-info">Descrição</div></strong><p><div align="center">Descrição de identificação do inventario</div></p>',
                position: 'top'
            },
            {
                element: $('#labelsituacao'),
                content: '<strong><div align="center" class="text-info">Situação</div></strong><p><div align="center">Situação do inventario (Cancelada, Aberta, Execução e Concluída).</div></p>',
                position: 'top'
            },
            {
                element: $('#labelTipoAnalise'),
                content: '<strong><div align="center" class="text-info">Tipo Analise</div></strong><p><div align="center">campo Tipo de analise do inventario (Manuel e APP).</div></p>',
                position: 'top'
            },
            {
                element: $('#labelObs'),
                content: '<strong><div align="center" class="text-info">Observação</div></strong><p><div align="center">Observações do inventario.</div></p>',
            },
         

        ]);
    });
});



$(document).ready(function () {
    SetModal();
   
});

async function CadastroProdutoEstoque() {

    var cadastroTodosProdutosEstoque = await Swal.fire({
        title: 'Cadastro automatico de produtos por estoque',
        text: "Tem certeza que deseja incluir todos os produtos do estoque?",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Sim',
    });

    if (cadastroTodosProdutosEstoque.isConfirmed) {
        const id = $('#idInventario').val();
        on();
        $.ajax({
            type: "get",
            url: '/inventario/CadastroAutomaticoProdutos?id='+id,
            success: function (result) {
                
                if (result.url) {
                    window.location.href = result.url; // Carrega o resultado HTML para a div demarcada

                    ModalMensagem("success", "Operação realizada com sucesso")
                }
                off();
            },
            error: function (res) {
                alert(res);
                off();
            },
            done: function (data) {
                off();
            }

        });
    } 
}

async function Concluir(id) {
    if (id == '0')
    {
        return;
    }
    var concluirInventario = await Swal.fire({
        title: 'Concluir',
        text: "Tem certeza que deseja Concluir o Inventário?",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Sim',
    });

    if (concluirInventario.isConfirmed) {
        on();
        $.ajax({
            type: "get",
            url: '/inventario/concluir?id=' + id,
            success: function (result) {

                if (result.url) {
                    window.location.href = result.url; // Carrega o resultado HTML para a div demarcada

                    if (result.success) {
                        ModalMensagem("success", "Operação realizada com sucesso");
                    } else {
                        ModalMensagem("danger", result.msg);
                    }
                    
                }
                off();
            },
            error: function (res) {
                alert(res);
                off();
            },
            done: function (data) {
                off();
            }

        });
    }
}

async function Inventariar(id) {

    var cadastroTodosProdutosEstoque = await Swal.fire({
        title: 'Inventariar',
        text: "Tem certeza que deseja mudar a situação do inventario para 'Em Execução'?",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Sim',
    });

    if (cadastroTodosProdutosEstoque.isConfirmed) {

       
        
        if (id == 0)
            id = $('#idInventario').val();

        on();
        $.ajax({
            type: "get",
            url: '/inventario/Inventariar?id=' + id,
            success: function (result) {

                if (result.url) {
                    window.location.href = result.url; // Carrega o resultado HTML para a div demarcada

                    ModalMensagem("success", "Operação realizada com sucesso")
                }
                off();
            },
            error: function (res) {
                alert(res);
                off();
            },
            done: function (data) {
                off();
            }

        });
    }

}

//$('#Adicionar').click(function () {
//    $('#frm').attr('action', '/inventario/IncluirItemTemp');
// });


//$('#Gravar').click(function () {
//    $('#frm').attr('action', '/inventario/concluir');
// });


function preencherObjeto() {
    let resultado = "";
    var header = [];
    var rows = [];
    $("#tabela tr th").each(function (i, th) {
        header.push($(th).html());
    });

    $("#tabela tr:has(td)").each(function (i, tr) {
        var row = {};
        $(tr).find('td').each(function (j, td) {
            const txt = $(td).text();
            const _html = $(td).html();
            row[header[j]] = txt;
//                $(td).html();
        });
        rows.push(row);
    });

    console.log(JSON.stringify(rows))
}
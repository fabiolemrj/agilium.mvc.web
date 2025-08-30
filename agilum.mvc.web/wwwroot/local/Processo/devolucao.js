$('#btnSalvar').click(function () {
    on();
    $('#btnSendForm').click();
    off();
});

$(document).ready(function () {
    SetModal();
    InicializarComponente();

    $('#Selecaovenda').change(function () {
        //Use $option (with the "$") to see that the variable is a jQuery object
        //alert($(this).val());
        const iddev = $('#Id').val();
        SelecionarItensVendaPorVenda($(this).val(), iddev);
    });

});

function InicializarComponente() {
    const dataVenda = $('#DataConsulta').val();
    
    if (dataVenda) {
        Selecionarvenda().then(() => {
            const iddev = $('#Id').val();
            const Selecaovenda = $('#Selecaovenda').val();

        });
               
    }
}

async function SelecionarItensVendaPorVenda(id,iddev) {
    //alert(id);
    on();
    $.ajax({
        type: 'get',
        dataType: 'json',
        url: "/devolucao/ObterItemVendaPorId?idvenda=" + id + "&iddev=" + iddev,
        success: function (result) {
            var objeto = JSON.parse(JSON.stringify(result));
            if (objeto.error) {
                toastr.error('Error');
            }
            $("#IDVENDA").val(id);
            MontarResultado(objeto.model);
            off();
        },
        error: function (result) {
            off();
            toastr.error(result);
        }
    });
}

function MontarResultado(data) {
    // Usando template literals para construir o HTML da tabela de forma mais limpa
    let resultado = `
        <table class='table table-hover' id='divGridResultado'>
            <thead class='table thead-dark'>
                <tr>
                    <th></th>
                    <th>Item</th>
                    <th>Produto</th>
                    <th>Qtd Vendida</th>
                    <th>Vlr Vendido</th>
                    <th>Qtd Devolução</th>
                    <th>Vlr Devolução</th>
                </tr>
            </thead>
            <tbody>
                ${$.map(data, (item, idx) => {
        const options = { style: 'currency', currency: 'BRL', minimumFractionDigits: 2, maximumFractionDigits: 3 };
        const formatNumber = new Intl.NumberFormat('pt-BR', options);

        const valorTotal = formatNumber.format(item.ValorTotal);
        const valorDevolucao = formatNumber.format(item.ValorDevolucao);

        // A construção de cada linha também usa template literals
        return `
                        <tr>
                            <td>
                                <div class='form-check'>
                                    <input 
                                          type='checkbox'
                                        id='DevolucaoItens_${idx}__selecionado' 
                                        name='DevolucaoItens[${idx}].selecionado' 
                                      
                                        ${item.selecionado ? 'checked' : ''} 
                                    />
                                    <input type='hidden' value='${item.idItemVenda}' id='DevolucaoItens_${idx}__idItemVenda' name='DevolucaoItens[${idx}].idItemVenda' />
                                    <input type='hidden' value='${item.idProduto}' id='DevolucaoItens_${idx}__idProduto' name='DevolucaoItens[${idx}].idProduto' />
                                    <input type='hidden' value='${item.idDevolucaoItem}' id='DevolucaoItens_${idx}__idDevolucaoItem' name='DevolucaoItens[${idx}].idDevolucaoItem' />
                                    <input type='hidden' value='${item.ValorTotal}' id='DevolucaoItens_${idx}__ValorTotal' name='DevolucaoItens[${idx}].ValorTotal' />
                                    <input type='hidden' value='${item.QuantidadeDevolucao}' id='DevolucaoItens_${idx}__QuantidadeDevolucao' name='DevolucaoItens[${idx}].QuantidadeDevolucao' />
                                    <input type='hidden' value='${item.QuantidadeVendida}' id='DevolucaoItens_${idx}__QuantidadeVendida' name='DevolucaoItens[${idx}].QuantidadeVendida' />
                                    <input type='hidden' value='${item.idDevolucao}' id='DevolucaoItens_${idx}__idDevolucao' name='DevolucaoItens[${idx}].idDevolucao' />
                                </div>
                            </td>
                            <td>${item.SeqVenda}</td>
                            <td>${item.ProdutoNome}</td>
                            <td>${item.QuantidadeVendida}</td>
                            <td>${valorTotal}</td>
                            <td>${item.QuantidadeDevolucao}</td>
                            <td>${valorDevolucao}</td>
                        </tr>
                    `;
    }).join('')}
            </tbody>
        </table>
    `;

    // Atualiza o HTML e anexa o evento
    $("#listaVendas").html(resultado);

    // Anexa o evento de 'change' aos checkboxes da tabela
    $('#divGridResultado input[type="checkbox"]').on('change', function () {
        // A propriedade checked já reflete o estado atual, então não é necessário
        // definir novamente
        const isChecked = $(this).prop('checked');

        if (isChecked) {
            console.log('Checkbox ' + this.id + ' foi marcado!');
        } else {
            console.log('Checkbox ' + this.id + ' foi desmarcado!');
        }
    });
}

async function Selecionarvenda() {

    var _date = $('#DataConsulta').val();

    const [year, month, day] = _date.split('-');
       
    let dataSplit = _date.split('-');
    let dateConverted;

    if (dataSplit[2].split(" ").length > 1) {

        var hora = dataSplit[2].split(" ")[1].split(':');
        dataSplit[2] = dataSplit[2].split(" ")[0];
        dateConverted = new Date(dataSplit[0], dataSplit[1] - 1, dataSplit[2], hora[0], hora[1]);

    } else {
        dateConverted = new Date(dataSplit[0], dataSplit[1] - 1, dataSplit[2]);
    }

    let getYear = dateConverted.toLocaleString("default", { year: "numeric" });
    let getMonth = dateConverted.toLocaleString("default", { month: "2-digit" });
    let getDay = dateConverted.toLocaleString("default", { day: "2-digit" });
    let data = getYear + "-" + getMonth + "-" + getDay;

    on();
    $.ajax({
        type: 'get',
        dataType: 'json',
        url: "/devolucao/ObterVendaPorData?data="+data ,
        success: function (result) {
            var objeto = JSON.parse(JSON.stringify(result));
            if (objeto.error) {
                toastr.error('Error');
            }
            let s = "";
            $("#Selecaovenda").children().remove();
            $("#Selecaovenda").empty();
            $("#Selecaovenda").append(new Option("--", "", true));
            $.map(objeto.viewDevolucao.VendasItens, function (item, idx) {
                $("#Selecaovenda").append(new Option(item.VendaNome, item.idVenda));
               
            });
            //alert(s);
            off();
        },
        error: function (result) {
            off();
            toastr.error(result)
        }
    });
}


$(function () {
    $('.money').mask('##0,00', { reverse: true });
    $('.datetime').mask('99/99/9999', { placeholder: "dd/MM/yyyy", selectOnFocus: true });

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
                content: '<strong><div align="center" class="text-info">Empresa</div></strong><p><div align="center">Seleção da empresa.</div></p>',
                position: 'top'
            },
            {
                element: $('#labelCodigo'),
                content: '<strong><div align="center" class="text-info">Codigo</div></strong><p><div align="center">Codigo da Devolução</div></p>',
                position: 'top'
            },
            {
                element: $('#labelmotdev'),
                content: '<strong><div align="center" class="text-info">Motivo devolução</div></strong><p><div align="center">Motivo da Devolução</div></p>',
                position: 'top'
            }, 
            {
                element: $('#labelCliente'),
                content: '<strong><div align="center" class="text-info">Cliente</div></strong><p><div align="center">Cliente que está relacionada a devolução.</div></p>',
                position: 'top'
            },
            {
                element: $('#labelObs'),
                content: '<strong><div align="center" class="text-info">Observação</div></strong><p><div align="center">Observações da devolução.</div></p>',
            },
            {
                element: $('#labelativo'),
                content: '<strong><div align="center" class="text-info">Situação</div></strong><p><div align="center">campo situação (ativo/invativo).</div></p>',
                position: 'top'
            },
            {
                element: $('#lbitensDev'),
                content: '<strong><div align="center" class="text-info">Itens devolvidos</div></strong><p><div align="center">Area relacionada aos itens de venda que serão devolvidos.</div></p>',
                position: 'top'
            },
            {
                element: $('#labelData'),
                content: '<strong><div align="center" class="text-info">Data para seleção de vendas</div></strong><p><div align="center">campo da data  qeu ocorram as vendas cujos itens serão devolvidos.</div></p>',
                position: 'top'
            },
            {
                element: $('#search-venda-btn'),
                content: '<strong><div align="center" class="text-info">Botão de filtro venda</div></strong><p><div align="center">Faz a pesquisa da venda.</div></p>',
                position: 'bottom'
            },
            
            {
                element: $('#labelIdVenda'),
                content: '<strong><div align="center" class="text-info">Venda</div></strong><p><div align="center">campo para seleção da venda cujo os itens serão devolvidos.</div></p>',
                position: 'top'
            },
            {
                element: $('#listaVendas'),
                content: '<strong><div align="center" class="text-info">Itens devolvidos</div></strong><p><div align="center">Lista de itens que serão devolvidos.</div></p>',
                position: 'top'
            },
            
        ]);
    });
});
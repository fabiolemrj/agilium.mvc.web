$(function () {
    bsCustomFileInput.init();
});


$('#btnSalvar').click(function () {
    on();

    $('#btnSendForm').click();
    off();
});

$(document).ready(function () {
    $('#Foto').change(function (e) {

        const tamanhoByte = parseInt(e.target.files[0].size);
        if (tamanhoByte > 1048576) {
            Swal.fire({
                icon: 'error',
                title: 'ERRO',
                text: 'O limite de tamanho de imagens são de até 1MB'
            });
            return;
        }
        const tamanhoMB = tamanhoByte / (1024 * 1024);
        const fileName = e.target.files[0].name + "<br/>" + tamanhoMB.toFixed(2) + "MB";
        $("#fotoCarregada").html(fileName);
        const fotoConvertida = getBase64(e.target.files[0]).then(result =>
            $("#ImagemConvertida").attr("src", result)
        );
        // fotoConvertida.then(result => $("#ImagemConvertida").val(result));

    });
});

function CarregarFoto() {

    $("#Foto").click();
}

function getBase64(file) {
    return new Promise((resolve, reject) => {
        const reader = new FileReader();
        reader.readAsDataURL(file);
        reader.onload = () => resolve(reader.result);
        reader.onerror = error => reject(error);
    });
}

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
                element: $('#labelData'),
                content: '<strong><div align="center" class="text-info">Data</div></strong><p><div align="center">Campo data de inclusão da imagem.</div></p>',
                position: 'top'
            },
            {
                element: $('#labelDescricao'),
                content: '<strong><div align="center" class="text-info">Descrição</div></strong><p><div align="center">campo para descrição da imagem.</div></p>',
                position: 'top'
            },
            {
                element: $('#btCarregarFoto'),
                content: '<strong><div align="center" class="text-info">Carregar foto</div></strong><p><div align="center">botão para selecionar a foto que será gravada.</div></p>',
                position: 'top'
            },
            {
                element: $('#ImagemConvertida'),
                content: '<strong><div align="center" class="text-info">Imagem carregada</div></strong><p><div align="center">Local onde será exibida a foto carregada.</div></p>',
                position: 'top'
            },
            {
                element: $('#labelVolume'),
                content: '<strong><div align="center" class="text-info">Volume</div></strong><p><div align="center">Campo para descrever o volume do produto.</div></p>',
                position: 'top'
            },
            {
                element: $('#labelDep'),
                content: '<strong><div align="center" class="text-info">Departamento</div></strong><p><div align="center">Campo para seleção do departamento que o produto está relacionado.</div></p>',
                position: 'top'
            },
            {
                element: $('#labelMarca'),
                content: '<strong><div align="center" class="text-info">Marca</div></strong><p><div align="center">Campo para seleção da marca do produto.</div></p>',
                position: 'top'
            },

            {
                element: $('#labelGrupo'),
                content: '<strong><div align="center" class="text-info">Grupo</div></strong><p><div align="center">Campo para seleção do Grupo que o produto está relacionado.</div></p>',
                position: 'top'
            },
            {
                element: $('#labelSubGrupo'),
                content: '<strong><div align="center" class="text-info">SubGrupo</div></strong><p><div align="center">Campo para seleção do SubGrupo que o produto está relacionado.</div></p>',
                position: 'top'
            },
            {
                element: $('#labelTipo'),
                content: '<strong><div align="center" class="text-info">Tipo</div></strong><p><div align="center">Campo para seleção do tipo  do produto.</div></p>',
                position: 'top'
            },
            {
                element: $('#labelCateg'),
                content: '<strong><div align="center" class="text-info">Categoria</div></strong><p><div align="center">Campo para seleção da categoria do produto.</div></p>',
                position: 'top'
            },
            {
                element: $('#labelOrigem'),
                content: '<strong><div align="center" class="text-info">Origem</div></strong><p><div align="center">Campo para seleção da Origem do produto.</div></p>',
                position: 'top'
            },
            {
                element: $('#labelOrigem'),
                content: '<strong><div align="center" class="text-info">Origem</div></strong><p><div align="center">Campo para seleção da Origem do produto.</div></p>',
                position: 'top'
            },
            {
                element: $('#labelPreco'),
                content: '<strong><div align="center" class="text-info">Preço</div></strong><p><div align="center">Campo para informar preço atual do produto.</div></p>',
                position: 'top'
            },
            {
                element: $('#labelValorUltCompra'),
                content: '<strong><div align="center" class="text-info">Valor Ultima Compra</div></strong><p><div align="center">Campo para informar Valor da Última Compra do produto.</div></p>',
                position: 'top'
            },
            {
                element: $('#labelVlCustMedio'),
                content: '<strong><div align="center" class="text-info">Valor Custo Médio</div></strong><p><div align="center">Campo para informar Valor de custo médio do produto, definido pela média das ultimas compras.</div></p>',
                position: 'top'
            },
            {
                element: $('#labelUnidCompra'),
                content: '<strong><div align="center" class="text-info">Unidade Compra</div></strong><p><div align="center">Campo para configurar a unidade utilizado nas compras do produto.</div></p>',
                position: 'top'
            },

            {
                element: $('#labelUnidVenda'),
                content: '<strong><div align="center" class="text-info">Unidade Venda</div></strong><p><div align="center">Campo para configurar a unidade utilizado nas vendas do produto.</div></p>',
                position: 'top'
            },
            {
                element: $('#labelRelacCompraVenda'),
                content: '<strong><div align="center" class="text-info">Relação Compra/Venda</div></strong><p><div align="center">Valor responsavelpara realizar a conversão do produto da compra para venda e assim abater corretamente do estoque.</div></p>',
                position: 'top'
            },
            {
                element: $('#labelCFOPVenda'),
                content: '<strong><div align="center" class="text-info">CFOP Venda</div></strong><p><div align="center">Campo para selecionar o codigo CFOP a ser utilizada nas vendas.</div></p>',
                position: 'top'
            },
            {
                element: $('#labelBalanca'),
                content: '<strong><div align="center" class="text-info">Utiliza Balança</div></strong><p><div align="center">Campo para configurar se o produto utiliza balança (valor por peso).</div></p>',
                position: 'top'
            },
            {
                element: $('#labelEstoque'),
                content: '<strong><div align="center" class="text-info">Cancelamento de Venda</div></strong><p><div align="center">Campo para configurar o que acontecerá com o produto nos cancelamentos da venda (volta para o estoque ou não).</div></p>',
                position: 'top'
            },
            {
                element: $('#labelQuantMinima'),
                content: '<strong><div align="center" class="text-info">Quantidade Minima</div></strong><p><div align="center">Campo para configurar a quantidade minima do produto no estoque, caso parametrizado como "SIM" ao atingir o valor, realiza bloqueio da venda ou aviso ao usuario.</div></p>',
                position: 'top'
            },
            {
                element: $('#divAreaImposto'),
                content: '<strong><div align="center" class="text-info">Area para configurar impostos </div></strong><p><div align="center">Area reservada a configuração de impostos relacionados ao produto.</div></p>',
                position: 'top'
            },
            //{
            //    element: $('#custom-tabs-codigoFiscal-tab'),
            //    content: '<strong><div align="center" class="text-info">Codigos Fiscais</div></strong><p><div align="center">Aba para confiogurações de codigod fiscais.</div></p>',
            //    position: 'top'
            //},
            //{
            //    element: $('#labelCodigoNCM'),
            //    content: '<strong><div align="center" class="text-info">Codigo NCM</div></strong><p><div align="center">Valor relacionado ao Codigo NCM do produto que será usado no registro da venda na SEFAZ.</div></p>',
            //    position: 'top'
            //},
            //{
            //    element: $('#labelCodigoCest'),
            //    content: '<strong><div align="center" class="text-info">Codigo Cest</div></strong><p><div align="center">Valor relacionado ao Codigo Cest do produto que será usado no registro da venda na SEFAZ.</div></p>',
            //    position: 'top'
            //},
            //{
            //    element: $('#labelCodigoSefaz'),
            //    content: '<strong><div align="center" class="text-info">Codigo Sefaz</div></strong><p><div align="center">Valor do Codigo Sefaz do produto.</div></p>',
            //    position: 'top'
            //},
            //{
            //    element: $('#labelCodigoServ'),
            //    content: '<strong><div align="center" class="text-info">Codigo Serviço</div></strong><p><div align="center">Valor do Codigo de serviço do produto que será usado no registro da venda na SEFAZ.</div></p>',
            //    position: 'top'
            //},
            //{
            //    element: $('#labelCodigoANP'),
            //    content: '<strong><div align="center" class="text-info">Codigo ANP</div></strong><p><div align="center">Valor do Codigo relacionado a ANP do produto.</div></p>',
            //    position: 'top'
            //},
            //{
            //    element: $('#labelCodigoServ'),
            //    content: '<strong><div align="center" class="text-info">Codigo Serviço</div></strong><p><div align="center">Valor do Codigo de serviço do produto que será usado no registro da venda na SEFAZ.</div></p>',
            //    position: 'top'
            //},
            //{
            //    element: $('#custom-tabs-impostos-tab'),
            //    content: '<strong><div align="center" class="text-info">Impostos</div></strong><p><div align="center">Aba para configuração de impostos do produto.</div></p>',
            //    position: 'top'
            //},
            //{
            //    element: $('#vert-tabs-icms'),
            //    content: '<strong><div align="center" class="text-info">Aba ICMS</div></strong><p><div align="center">Aba para configuração de ICMS.</div></p>',
            //    position: 'top'
            //},
            //{
            //    element: $('#vert-tabs-icms-tab'),
            //    content: '<strong><div align="center" class="text-info">Aba ICMS</div></strong><p><div align="center">Aba para configuração de ICMS do produto.</div></p>',
            //    position: 'top'
            //},
            //{
            //    element: $('#labelCodigoICMSCT'),
            //    content: '<strong><div align="center" class="text-info">IMCS CT</div></strong><p><div align="center">Campo para configuirar IMCS ST do produto.</div></p>',
            //    position: 'top'
            //},
            //{
            //    element: $('#labelCodigoICMS'),
            //    content: '<strong><div align="center" class="text-info">Codigo ICMS</div></strong><p><div align="center">Campo para ICMS do produto.</div></p>',
            //    position: 'top'
            //},
            //{
            //    element: $('#labelCodigoReducaoBaseCalc'),
            //    content: '<strong><div align="center" class="text-info">Redução base de calculo ICMS</div></strong><p><div align="center">Campo para configuração do codigo de redução da base de calculo ICMS do produto.</div></p>',
            //    position: 'top'
            //},
            //{
            //    element: $('#vert-tabs-icms_st'),
            //    content: '<strong><div align="center" class="text-info">Aba ICMS ST</div></strong><p><div align="center">Aba para configuração de ICMS ST.</div></p>',
            //    position: 'top'
            //},
            //{
            //    element: $('#labelAliqICMSTST'),
            //    content: '<strong><div align="center" class="text-info">Aliquota IMCS ST</div></strong><p><div align="center">Campo de Alquota de ICMS ST.</div></p>',
            //    position: 'top'
            //},
            //{
            //    element: $('#labelRedBaseCalcICMSST'),
            //    content: '<strong><div align="center" class="text-info">Redução base de calculo IMCS ST</div></strong><p><div align="center">Campo para configuração do codigo de redução da base de calculo ICMS ST.</div></p>',
            //    position: 'top'
            //},
            //{
            //    element: $('#labelAliqMAVST'),
            //    content: '<strong><div align="center" class="text-info">Aliquota MVA ICMS ST</div></strong><p><div align="center">Campo para configuração de alquota de margem de valor agregado de ICMS ST.</div></p>',
            //    position: 'top'
            //},
            //{
            //    element: $('#vert-tabs-ipi-tab'),
            //    content: '<strong><div align="center" class="text-info">Aba IPI</div></strong><p><div align="center">Aba para configuração de IPI dos produtos.</div></p>',
            //    position: 'top'
            //},
            //{
            //    element: $('#labelCodigoIPI'),
            //    content: '<strong><div align="center" class="text-info">Codigo IPI</div></strong><p><div align="center">Campo para configuração do codigo IPI.</div></p>',
            //    position: 'top'
            //},
            //{
            //    element: $('#labelAliqIPI'),
            //    content: '<strong><div align="center" class="text-info">Aliquota IPI</div></strong><p><div align="center">Campo de Aliquota de IPI.</div></p>',
            //    position: 'top'
            //},
            //{
            //    element: $('#vert-tabs-pis-tab'),
            //    content: '<strong><div align="center" class="text-info">Aba PIS</div></strong><p><div align="center">Aba para configuração de PIS dos produtos.</div></p>',
            //    position: 'top'
            //},
            //{
            //    element: $('#labelCodigoPIS'),
            //    content: '<strong><div align="center" class="text-info">Codigo PIS</div></strong><p><div align="center">Campo para configuração do codigo PIS.</div></p>',
            //    position: 'top'
            //},
            //{
            //    element: $('#labelAliqPIS'),
            //    content: '<strong><div align="center" class="text-info">Aliquota PIS</div></strong><p><div align="center">Campo de Aliquota de PIS.</div></p>',
            //    position: 'top'
            //},
            //{
            //    element: $('#vert-tabs-cofins-tab'),
            //    content: '<strong><div align="center" class="text-info">Aba Cofins</div></strong><p><div align="center">Aba para configuração de Cofins dos produtos.</div></p>',
            //    position: 'top'
            //},
            //{
            //    element: $('#labelCodigoConfins'),
            //    content: '<strong><div align="center" class="text-info">Codigo Confins</div></strong><p><div align="center">Campo para configuração do codigo Cofins.</div></p>',
            //    position: 'top'
            //},
            //{
            //    element: $('#labelAliqCofins'),
            //    content: '<strong><div align="center" class="text-info">Aliquota Cofins</div></strong><p><div align="center">Campo de Aliquota de Cofins.</div></p>',
            //    position: 'top'
            //}
        ]);
    });
});
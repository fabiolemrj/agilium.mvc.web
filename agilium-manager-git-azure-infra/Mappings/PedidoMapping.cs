using agilium.api.business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.infra.Mappings
{
    
    public class PedidoMapping : IEntityTypeConfiguration<Pedido>
    {
        public void Configure(EntityTypeBuilder<Pedido> builder)
        {
            builder.ToTable("pedido");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("IDPEDIDO").HasColumnType("bigint").IsRequired();
            builder.Property(c => c.IDEmpresa).HasColumnName("IDEMPRESA").HasColumnType("bigint");
            builder.Property(c => c.IDFuncionario).HasColumnName("IDFUNC").HasColumnType("bigint");
            builder.Property(c => c.IDCliente).HasColumnName("IDCLIENTE").HasColumnType("bigint");
            builder.Property(c => c.IDEndereco).HasColumnName("IDENDERECO").HasColumnType("bigint");
            builder.Property(c => c.IDCaixa).HasColumnName("IDCAIXA").HasColumnType("bigint");
            builder.Property(c => c.IDPDV).HasColumnName("IDPDV").HasColumnType("bigint");
            builder.Property(c => c.CDPedido).HasColumnName("CDPEDIDO").HasColumnType("varchar(6)");
            builder.Property(c => c.DTPedido).HasColumnName("DTPEDIDO").HasColumnType("datetime");
            builder.Property(c => c.VLPedido).HasColumnName("VLPEDIDO").HasColumnType("double");
            builder.Property(c => c.VLAcres).HasColumnName("VLACRES").HasColumnType("double");
            builder.Property(c => c.VLDesc).HasColumnName("VLDESC").HasColumnType("double");
            builder.Property(c => c.VLOutros).HasColumnName("VLOUTROS").HasColumnType("double");
            builder.Property(c => c.VLTotal).HasColumnName("VLTOTAL").HasColumnType("double");
            builder.Property(c => c.DSObs).HasColumnName("DSOBS").HasColumnType("mediumtext");
            builder.Property(c => c.NUDistancia).HasColumnName("NUDISTANCIA").HasColumnType("double");
            builder.Property(c => c.DTHRConclusao).HasColumnName("DTHRCONCLUSAO").HasColumnType("datetime");

            //chaves estrangeiras
            builder
            .HasOne(tabela => tabela.Empresa)
            .WithMany(tabelaEstrangeira => tabelaEstrangeira.Pedidos)
            .HasForeignKey(tabela => new { tabela.IDEmpresa })
            .HasPrincipalKey(tabelaEstrangeira => new { tabelaEstrangeira.Id });

            builder
             .HasOne(tabela => tabela.Funcionario)
             .WithMany(tabelaEstrangeira => tabelaEstrangeira.Pedidos)
             .HasForeignKey(tabela => new { tabela.IDFuncionario })
             .HasPrincipalKey(tabelaEstrangeira => new { tabelaEstrangeira.Id });

            builder
             .HasOne(tabela => tabela.Cliente)
             .WithMany(tabelaEstrangeira => tabelaEstrangeira.Pedidos)
             .HasForeignKey(tabela => new { tabela.IDEmpresa })
             .HasPrincipalKey(tabelaEstrangeira => new { tabelaEstrangeira.Id });

            builder
             .HasOne(tabela => tabela.Endereco)
             .WithMany(tabelaEstrangeira => tabelaEstrangeira.Pedidos)
             .HasForeignKey(tabela => new { tabela.IDEndereco })
             .HasPrincipalKey(tabelaEstrangeira => new { tabelaEstrangeira.Id });

            builder
             .HasOne(tabela => tabela.Caixa)
             .WithMany(tabelaEstrangeira => tabelaEstrangeira.Pedidos)
             .HasForeignKey(tabela => new { tabela.IDCaixa })
             .HasPrincipalKey(tabelaEstrangeira => new { tabelaEstrangeira.Id });

            builder
             .HasOne(tabela => tabela.PDV)
             .WithMany(tabelaEstrangeira => tabelaEstrangeira.Pedidos)
             .HasForeignKey(tabela => new { tabela.IDPDV })
             .HasPrincipalKey(tabelaEstrangeira => new { tabelaEstrangeira.Id });
        }
    }

    public class PedidoItemMapping : IEntityTypeConfiguration<PedidoItem>
    {
        public void Configure(EntityTypeBuilder<PedidoItem> builder)
        {
            builder.ToTable("pedido_item");

            builder.HasKey(pi => pi.Id);

            builder.Property(pi => pi.Id).HasColumnName("IDITEMPEDIDO").HasColumnType("bigint").IsRequired();
            builder.Property(pi => pi.IDPedido).HasColumnName("IDPEDIDO").HasColumnType("bigint");
            builder.Property(pi => pi.IDProduto).HasColumnName("IDPRODUTO").HasColumnType("bigint");
            builder.Property(pi => pi.IDEstoque).HasColumnName("IDESTOQUE").HasColumnType("bigint)");
            builder.Property(pi => pi.IDFornecedor).HasColumnName("IDFORN").HasColumnType("bigint");
            builder.Property(pi => pi.SQItemPedido).HasColumnName("SQITEMPEDIDO").HasColumnType("int");
            builder.Property(pi => pi.VLUnit).HasColumnName("VLUNIT").HasColumnType("double");
            builder.Property(pi => pi.NUQtd).HasColumnName("NUQTD").HasColumnType("double");
            builder.Property(pi => pi.VLItem).HasColumnName("VLITEM").HasColumnType("double");
            builder.Property(pi => pi.VLAcres).HasColumnName("VLACRES").HasColumnType("double");
            builder.Property(pi => pi.VLDesc).HasColumnName("VLDESC").HasColumnType("double");
            builder.Property(pi => pi.VLOutros).HasColumnName("VLOUTROS").HasColumnType("double");
            builder.Property(pi => pi.VLTotal).HasColumnName("VLTOTAL").HasColumnType("double");
            builder.Property(pi => pi.VLCustoMedio).HasColumnName("VLCUSTOMEDIO").HasColumnType("double");
            builder.Property(pi => pi.STItemPedido).HasColumnName("STITEMPEDIDO").HasColumnType("int(11)");
            builder.Property(pi => pi.DTPrevEntrega).HasColumnName("DTPREV_ENTREGA").HasColumnType("date");
            builder.Property(pi => pi.DSObsItem).HasColumnName("DSOBSITEM").HasColumnType("varchar(200)");

            builder.HasOne(pi => pi.Estoque).WithMany(p => p.Pedidoitens).HasForeignKey(pi => pi.IDEstoque);
            builder.HasOne(pi => pi.Fornecedor).WithMany(p => p.Pedidoitens).HasForeignKey(pi => pi.IDFornecedor);
            builder.HasOne(pi => pi.Pedido).WithMany(p => p.itens).HasForeignKey(pi => pi.IDPedido);
            builder.HasOne(pi => pi.Produto).WithMany(p => p.Pedidoitens).HasForeignKey(pi => pi.IDProduto);
        }
    }

    public class PedidoSiteMercadoMapping : IEntityTypeConfiguration<PedidoSitemercado>
    {
        public void Configure(EntityTypeBuilder<PedidoSitemercado> builder)
        {
            builder.ToTable("pedido_sitemercado");

            builder.HasKey(ps => ps.Id);

            builder.Property(ps => ps.Id).HasColumnName("IDPEDIDOSM").HasColumnType("bigint").IsRequired();
            builder.Property(ps => ps.IDEmpresa).HasColumnName("IDEMPRESA").HasColumnType("bigint");
            builder.Property(ps => ps.IDVenda).HasColumnName("IDVENDA").HasColumnType("bigint");
            builder.Property(ps => ps.IDCliente).HasColumnName("IDCLIENTE").HasColumnType("bigint");
            builder.Property(ps => ps.IDLoja).HasColumnName("IDLOJA").HasColumnType("int");
            builder.Property(ps => ps.Codigo).HasColumnName("CODIGO").HasColumnType("varchar(20)");
            builder.Property(ps => ps.CodigoLoja).HasColumnName("CODIGOLOJA").HasColumnType("varchar(20)");
            builder.Property(ps => ps.DTHRPedido).HasColumnName("DTHRPEDIDO").HasColumnType("datetime");
            builder.Property(ps => ps.DTHRIniAgend).HasColumnName("DTHRINIAGEND").HasColumnType("datetime");
            builder.Property(ps => ps.DTHRFimAgend).HasColumnName("DTHRFIMAGEND").HasColumnType("datetime");
            builder.Property(ps => ps.Entrega).HasColumnName("ENTREGA").HasColumnType("int");
            builder.Property(ps => ps.CPFNAnota).HasColumnName("CPFNANOTA").HasColumnType("int");
            builder.Property(ps => ps.PessoaAutReceb).HasColumnName("PESSOAAUTRECEB").HasColumnType("varchar(50)");
            builder.Property(ps => ps.QtdItemUnico).HasColumnName("QTDITEMUNICO").HasColumnType("int");
            builder.Property(ps => ps.VLMercado).HasColumnName("VLMERCADO").HasColumnType("double");
            builder.Property(ps => ps.VLConveniencia).HasColumnName("VLCONVENIENCIA").HasColumnType("double");
            builder.Property(ps => ps.VLEntrega).HasColumnName("VLENTREGA").HasColumnType("double");
            builder.Property(ps => ps.VLRetirada).HasColumnName("VLRETIRADA").HasColumnType("double");
            builder.Property(ps => ps.VLTroco).HasColumnName("VLTROCO").HasColumnType("double");
            builder.Property(ps => ps.VLDesconto).HasColumnName("VLDESCONTO").HasColumnType("double");
            builder.Property(ps => ps.VLTotal).HasColumnName("VLTOTAL").HasColumnType("double");
            builder.Property(ps => ps.VLCorrigido).HasColumnName("VLCORRIGIDO").HasColumnType("double");
            builder.Property(ps => ps.DSJSONPedido).HasColumnName("DSJSONPEDIDO").HasColumnType("longtext");
            builder.Property(ps => ps.CDCupom).HasColumnName("CDCUPOM").HasColumnType("varchar(50)");

            builder.HasOne(ps => ps.Cliente).WithMany(p=>p.PedidosSiteMercado).HasForeignKey(ps => ps.IDCliente);
            builder.HasOne(ps => ps.Empresa).WithMany(p => p.PedidosSiteMercado).HasForeignKey(ps => ps.IDEmpresa);
            builder.HasOne(ps => ps.Venda).WithMany(p => p.PedidosSiteMercado).HasForeignKey(ps => ps.IDVenda);
        }
    }

    public class PedidoItemSiteMercadoMapping : IEntityTypeConfiguration<PedidoItemSitemercado>
    {
        public void Configure(EntityTypeBuilder<PedidoItemSitemercado> builder)
        {
            builder.ToTable("pedido_item_sitemercado");

            builder.HasKey(pism => pism.Id);

            builder.Property(pism => pism.Id).HasColumnName("IDPEDIDOITEMSM").HasColumnType("bigint").IsRequired();
            builder.Property(pism => pism.IDPedidoSitemercado).HasColumnName("IDPEDIDOSM").HasColumnType("bigint");
            builder.Property(pism => pism.IDVendaItem).HasColumnName("IDVENDA_ITEM").HasColumnType("bigint");
            builder.Property(pism => pism._ID).HasColumnName("ID").HasColumnType("int");
            builder.Property(pism => pism.IDX).HasColumnName("IDX").HasColumnType("int");
            builder.Property(pism => pism.Codigo).HasColumnName("CODIGO").HasColumnType("varchar(20)");
            builder.Property(pism => pism.CodigoLoja).HasColumnName("CODIGOLOJA").HasColumnType("varchar(20)");
            builder.Property(pism => pism.PesoVariavel).HasColumnName("PESOVARIAVEL").HasColumnType("int");
            builder.Property(pism => pism.CodigoBarras).HasColumnName("CODIGOBARRA").HasColumnType("varchar(20)");
            builder.Property(pism => pism.PLU).HasColumnName("PLU").HasColumnType("bigint(20)");
            builder.Property(pism => pism.Produto).HasColumnName("PRODUTO").HasColumnType("varchar(50)");
            builder.Property(pism => pism.IDProduto).HasColumnName("IDPRODUTO").HasColumnType("int");
            builder.Property(pism => pism.Observacao).HasColumnName("OBSERVACAO").HasColumnType("varchar(200)");
            builder.Property(pism => pism.Quantidade).HasColumnName("QUANTIDADE").HasColumnType("double");
            builder.Property(pism => pism.Quantidade3).HasColumnName("QUANTIDADE3").HasColumnType("double");
            builder.Property(pism => pism.Valor).HasColumnName("VALOR").HasColumnType("double");
            builder.Property(pism => pism.ValorTotal).HasColumnName("VLTOTAL").HasColumnType("double");
            builder.Property(pism => pism.Indisponivel).HasColumnName("INDISPONIVEL").HasColumnType("int");
            builder.Property(pism => pism.Desistencia).HasColumnName("DESISTENCIA").HasColumnType("int");

            builder.HasOne(pism => pism.PedidoSitemercado).WithMany(x=>x.PedidoItemSiteMercado).HasForeignKey(pism => pism.IDPedidoSitemercado);
            builder.HasOne(pism => pism.VendaItem).WithMany(x => x.PedidoItemSiteMercado).HasForeignKey(pism => pism.IDVendaItem);
        }
    }

    public class PedidoPagamentoMapping : IEntityTypeConfiguration<PedidoPagamento>
    {
        public void Configure(EntityTypeBuilder<PedidoPagamento> builder)
        {
            builder.ToTable("pedido_pagamento");

            builder.HasKey(pp => pp.Id);
            builder.Property(pp => pp.Id).HasColumnName("IDPEDIDOPAG").HasColumnType("bigint(20)").IsRequired();
            builder.Property(pp => pp.IDPedido).HasColumnName("IDPEDIDO").HasColumnType("bigint(20)");
            builder.Property(pp => pp.IDFormaPagamento).HasColumnName("IDFORMAPAG").HasColumnType("bigint(20)");
            builder.Property(pp => pp.IDMoeda).HasColumnName("IDMOEDA").HasColumnType("bigint(20)");
            builder.Property(pp => pp.VLPagamento).HasColumnName("VLPAG").HasColumnType("double");
            builder.Property(pp => pp.VLTroco).HasColumnName("VLTROCO").HasColumnType("double");
            builder.Property(pp => pp.DSObsPagamento).HasColumnName("DSOBSPAG").HasColumnType("varchar(200)");

            builder.HasOne(pp => pp.Pedido).WithMany(x=>x.PedidoPagamentos).HasForeignKey(pp => pp.IDPedido);
            builder.HasOne(pp => pp.FormaPagamento).WithMany(x => x.PedidoPagamentos).HasForeignKey(pp => pp.IDFormaPagamento);
            builder.HasOne(pp => pp.Moeda).WithMany(x => x.PedidoPagamentos).HasForeignKey(pp => pp.IDMoeda);
        }
    }

    public class PedidoPagamentoSitemercadoMapping : IEntityTypeConfiguration<PedidoPagamentoSitemercado>
    {
        public void Configure(EntityTypeBuilder<PedidoPagamentoSitemercado> builder)
        {
            builder.ToTable("pedido_pagamento_sitemercado");

            builder.HasKey(pps => pps.Id);
            builder.Property(pps => pps.IDPedidoSM).HasColumnName("IDPEDIDOPAGAMENTOSM").HasColumnType("bigint(20)").IsRequired();
            builder.Property(pps => pps.IDPedidoSM).HasColumnName("IDPEDIDOSM").HasColumnType("bigint(20)");
            builder.Property(pps => pps.IDMoeda).HasColumnName("IDMOEDA").HasColumnType("bigint(20)");
            builder.Property(pps => pps.IDVendaMoeda).HasColumnName("IDVENDA_MOEDA").HasColumnType("bigint(20)");
            builder.Property(pps => pps._ID).HasColumnName("ID").HasColumnType("int(11)");
            builder.Property(pps => pps.Nome).HasColumnName("NOME").HasColumnType("varchar(50)");
            builder.Property(pps => pps.Valor).HasColumnName("VALOR").HasColumnType("double");
            builder.Property(pps => pps.VLCorrigido).HasColumnName("VLCORRIGIDO").HasColumnType("double");
            builder.Property(pps => pps.Tipo).HasColumnName("TIPO").HasColumnType("varchar(15)");

            builder.HasIndex(pps => pps.IDPedidoSM).HasName("FK_PEDIDOPAGSM_PEDIDOSM_idx");
            builder.HasIndex(pps => pps.IDVendaMoeda).HasName("FK_PEDIDOPAGSM_VENDAMOEDA_idx");
            builder.HasIndex(pps => pps.IDMoeda).HasName("FK_PEDIDOPAGSM_MOEDA_idx");

            builder.HasOne(pps => pps.PedidoSitemercado).WithMany(x=>x.PedidoPagamentoSitemercados).HasForeignKey(pps => pps.IDPedidoSM);
            builder.HasOne(pps => pps.VendaMoeda).WithMany(x => x.PedidoPagamentoSitemercados).HasForeignKey(pps => pps.IDVendaMoeda);
            builder.HasOne(pps => pps.Moeda).WithMany(x => x.PedidoPagamentoSitemercados).HasForeignKey(pps => pps.IDMoeda);
        }
    }

    public class PedidoVendaMapping : IEntityTypeConfiguration<PedidoVenda>
    {
        public void Configure(EntityTypeBuilder<PedidoVenda> builder)
        {
            builder.ToTable("pedido_venda");

            builder.HasKey(pv => pv.Id).HasName("IDPEDIDOVENDA");
            builder.Property(pv => pv.Id).HasColumnName("IDPEDIDOVENDA").HasColumnType("bigint").IsRequired();
            builder.Property(pv => pv.IDPedido).HasColumnName("IDPEDIDO").HasColumnType("bigint");
            builder.Property(pv => pv.IDVenda).HasColumnName("IDVENDA").HasColumnType("bigint");

            builder.HasOne(pv => pv.Pedido).WithMany(x=>x.PedidosVendas).HasForeignKey(pv => pv.IDPedido);
            builder.HasOne(pv => pv.Venda).WithMany(x => x.PedidosVendas).HasForeignKey(pv => pv.IDVenda);
        }
    }

    public class PedidoItemVendaItemMapping : IEntityTypeConfiguration<PedidoVendaItem>
    {
        public void Configure(EntityTypeBuilder<PedidoVendaItem> builder)
        {
            builder.ToTable("pedido_item_venda_item");

            builder.HasKey(pivi => pivi.Id).HasName("PRIMARY");
            builder.Property(pivi => pivi.Id).HasColumnName("IDPEDITVENDIT").HasColumnType("bigint(20)").IsRequired();
            builder.Property(pivi => pivi.IDITEMPEDIDO).HasColumnName("IDITEMPEDIDO").HasColumnType("bigint(20)");
            builder.Property(pivi => pivi.IDVENDA_ITEM).HasColumnName("IDVENDA_ITEM").HasColumnType("bigint(20)");

            builder.HasOne(pivi => pivi.PedidoItem).WithMany(x=>x.PedidosVendaItems).HasForeignKey(pivi => pivi.IDITEMPEDIDO);
            builder.HasOne(pivi => pivi.VendaItem).WithMany(x => x.PedidosVendaItems).HasForeignKey(pivi => pivi.IDVENDA_ITEM);
        }
    }
    
}

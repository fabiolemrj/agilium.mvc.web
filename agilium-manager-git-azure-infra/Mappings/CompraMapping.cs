using agilium.api.business.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.infra.Mappings
{
    public class CompraMapping : IEntityTypeConfiguration<Compra>
    {
        public void Configure(EntityTypeBuilder<Compra> builder)
        {
            builder.ToTable("compra");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("IDCOMPRA").HasColumnType("bigint").IsRequired();

            builder.Property(c => c.IDEMPRESA).HasColumnName("IDEMPRESA").HasColumnType("bigint");
            builder.Property(c => c.IDFORN).HasColumnName("IDFORN").HasColumnType("bigint");
            builder.Property(c => c.IDTURNO).HasColumnName("IDTURNO").HasColumnType("bigint");
            builder.Property(c => c.DTCOMPRA).HasColumnName("DTCOMPRA").HasColumnType("date");
            builder.Property(c => c.DTCAD).HasColumnName("DTCAD").HasColumnType("datetime");
            builder.Property(c => c.CDCOMPRA).HasColumnName("CDCOMPRA").HasColumnType("varchar(6)");
            builder.Property(c => c.STCOMPRA).HasColumnName("STCOMPRA").HasColumnType("int");
            builder.Property(c => c.DTNF).HasColumnName("DTNF").HasColumnType("date");
            builder.Property(c => c.NUNF).HasColumnName("NUNF").HasColumnType("varchar(30)");
            builder.Property(c => c.DSSERIENF).HasColumnName("DSSERIENF").HasColumnType("varchar(30)");
            builder.Property(c => c.DSCHAVENFE).HasColumnName("DSCHAVENFE").HasColumnType("varchar(50)");
            builder.Property(c => c.TPCOMPROVANTE).HasColumnName("TPCOMPROVANTE").HasColumnType("int");
            builder.Property(c => c.NUCFOP).HasColumnName("NUCFOP").HasColumnType("int");
            builder.Property(c => c.VLICMSRETIDO).HasColumnName("VLICMSRETIDO").HasColumnType("double");
            builder.Property(c => c.VLBSCALCICMS).HasColumnName("VLBSCALCICMS").HasColumnType("double");
            builder.Property(c => c.VLICMS).HasColumnName("VLICMS").HasColumnType("double");
            builder.Property(c => c.VLBSCALCSUB).HasColumnName("VLBSCALCSUB").HasColumnType("double");
            builder.Property(c => c.VLICMSSUB).HasColumnName("VLICMSSUB").HasColumnType("double");
            builder.Property(c => c.VLISENCAO).HasColumnName("VLISENCAO").HasColumnType("double");
            builder.Property(c => c.VLTOTPROD).HasColumnName("VLTOTPROD").HasColumnType("double");
            builder.Property(c => c.VLFRETE).HasColumnName("VLFRETE").HasColumnType("double");
            builder.Property(c => c.VLSEGURO).HasColumnName("VLSEGURO").HasColumnType("double");
            builder.Property(c => c.VLDESCONTO).HasColumnName("VLDESCONTO").HasColumnType("double");
            builder.Property(c => c.VLOUTROS).HasColumnName("VLOUTROS").HasColumnType("double");
            builder.Property(c => c.VLIPI).HasColumnName("VLIPI").HasColumnType("double");
            builder.Property(c => c.VLTOTAL).HasColumnName("VLTOTAL").HasColumnType("double");
            builder.Property(c => c.DSOBS).HasColumnName("DSOBS").HasColumnType("varchar(500)");
            builder.Property(c => c.STIMPORTADA).HasColumnName("STIMPORTADA").HasColumnType("int");

            //chaves estrangeiras
            builder
                .HasMany(compra => compra.Itens)
                .WithOne(compraItem => compraItem.Compra)
                .HasForeignKey(compraItem => new { compraItem.IDCOMPRA })
                .HasPrincipalKey(compra => new { compra.Id })
                 .OnDelete(DeleteBehavior.Cascade);

            builder
           .HasMany(compra => compra.ComprasFiscais)
           .WithOne(compraFiscal => compraFiscal.Compra)
           .HasForeignKey(compraFiscal => new { compraFiscal.IDCOMPRA })
           .HasPrincipalKey(compra => new { compra.Id })
            .OnDelete(DeleteBehavior.Cascade);

        }
    }

    public class CompraItemMapping : IEntityTypeConfiguration<CompraItem>
    {
        public void Configure(EntityTypeBuilder<CompraItem> builder)
        {
            builder.ToTable("compra_item");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("IDITEM").HasColumnType("bigint").IsRequired();

            builder.Property(c => c.IDCOMPRA).HasColumnName("IDCOMPRA").HasColumnType("bigint");
            builder.Property(c => c.IDPRODUTO).HasColumnName("IDPRODUTO").HasColumnType("bigint");
            builder.Property(c => c.IDESTOQUE).HasColumnName("IDESTOQUE").HasColumnType("bigint");
            builder.Property(c => c.DSPRODUTO).HasColumnName("DSPRODUTO").HasColumnType("varchar(100)");
            builder.Property(c => c.CDEAN).HasColumnName("CDEAN").HasColumnType("varchar(50)");
            builder.Property(c => c.CDNCM).HasColumnName("CDNCM").HasColumnType("varchar(20)");
            builder.Property(c => c.CDCEST).HasColumnName("CDCEST").HasColumnType("varchar(20)");
            builder.Property(c => c.SGUN).HasColumnName("SGUN").HasColumnType("varchar(5)");
            builder.Property(c => c.NUQTD).HasColumnName("NUQTD").HasColumnType("double");
            builder.Property(c => c.NURELACAO).HasColumnName("NURELACAO").HasColumnType("double");
            builder.Property(c => c.VLUNIT).HasColumnName("VLUNIT").HasColumnType("double");
            builder.Property(c => c.VLTOTAL).HasColumnName("VLTOTAL").HasColumnType("double");
            builder.Property(c => c.DTVALIDADE).HasColumnName("DTVALIDADE").HasColumnType("date");
            builder.Property(c => c.NUCFOP).HasColumnName("NUCFOP").HasColumnType("int");
            builder.Property(c => c.VLOUTROS).HasColumnName("VLOUTROS").HasColumnType("double");
            builder.Property(c => c.VLBSRET).HasColumnName("VLBSRET").HasColumnType("double");
            builder.Property(c => c.PCICMSRET).HasColumnName("PCICMSRET").HasColumnType("double");
            builder.Property(c => c.PCREDUCAO).HasColumnName("PCREDUCAO").HasColumnType("double");
            builder.Property(c => c.CDCSTICMS).HasColumnName("CDCSTICMS").HasColumnType("varchar(20)");
            builder.Property(c => c.CDCSTPIS).HasColumnName("CDCSTPIS").HasColumnType("varchar(20)");
            builder.Property(c => c.CDCSTCOFINS).HasColumnName("CDCSTCOFINS").HasColumnType("varchar(20)");
            builder.Property(c => c.CDCSTIPI).HasColumnName("CDCSTIPI").HasColumnType("varchar(20)");
            builder.Property(c => c.VLALIQPIS).HasColumnName("VLALIQPIS").HasColumnType("double");
            builder.Property(c => c.VLALIQCOFINS).HasColumnName("VLALIQCOFINS").HasColumnType("double");
            builder.Property(c => c.VLALIQICMS).HasColumnName("VLALIQICMS").HasColumnType("double");
            builder.Property(c => c.VLALIQIPI).HasColumnName("VLALIQIPI").HasColumnType("double");
            builder.Property(c => c.VLBSCALCPIS).HasColumnName("VLBSCALCPIS").HasColumnType("double");
            builder.Property(c => c.VLBSCALCCOFINS).HasColumnName("VLBSCALCCOFINS").HasColumnType("double");
            builder.Property(c => c.VLBSCALCICMS).HasColumnName("VLBSCALCICMS").HasColumnType("double");
            builder.Property(c => c.VLBSCALCIPI).HasColumnName("VLBSCALCIPI").HasColumnType("double");
            builder.Property(c => c.VLICMS).HasColumnName("VLICMS").HasColumnType("double");
            builder.Property(c => c.VLPIS).HasColumnName("VLPIS").HasColumnType("double");
            builder.Property(c => c.VLCOFINS).HasColumnName("VLCOFINS").HasColumnType("double");
            builder.Property(c => c.VLIPI).HasColumnName("VLIPI").HasColumnType("double");
            builder.Property(c => c.CDPRODFORN).HasColumnName("CDPRODFORN").HasColumnType("varchar(30)");
            builder.Property(c => c.VLNOVOPRECOVENDA).HasColumnName("VLNOVOPRECOVENDA").HasColumnType("double");

            builder
              .HasMany(compraItem => compraItem.EstoquesHistoricos)
              .WithOne(estoqueHistorico => estoqueHistorico.CompraItem)
              .HasForeignKey(estoqueHistorico => new { estoqueHistorico.IDITEM })
              .HasPrincipalKey(compraItem => new { compraItem.Id });
            //   .OnDelete(DeleteBehavior.Cascade);


        }
    }

    public class CompraFiscalMapping : IEntityTypeConfiguration<CompraFiscal>
    {
        public void Configure(EntityTypeBuilder<CompraFiscal> builder)
        {
            builder.ToTable("compra_fiscal");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("IDCOMPRAFISCAL").HasColumnType("bigint").IsRequired();

            builder.Property(c => c.IDCOMPRA).HasColumnName("IDCOMPRA").HasColumnType("bigint");
            builder.Property(c => c.STMANIFESTO).HasColumnName("STMANIFESTO").HasColumnType("int");
            builder.Property(c => c.DSXML).HasColumnName("DSXML").HasColumnType("mediumtext");

        }
    }
}

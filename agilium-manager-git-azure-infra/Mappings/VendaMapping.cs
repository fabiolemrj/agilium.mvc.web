using agilium.api.business.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.infra.Mappings
{
    #region Venda
    public class VendaMapping : IEntityTypeConfiguration<Venda>
    {
        public void Configure(EntityTypeBuilder<Venda> builder)
        {
            builder.ToTable("venda");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("IDVENDA").HasColumnType("bigint").IsRequired();

            builder.Property(c => c.IDCAIXA).HasColumnName("IDCAIXA").HasColumnType("bigint");
            builder.Property(c => c.IDCLIENTE).HasColumnName("IDCLIENTE").HasColumnType("bigint");
            builder.Property(c => c.SQVENDA).HasColumnName("SQVENDA").HasColumnType("int");
            builder.Property(c => c.DTHRVENDA).HasColumnName("DTHRVENDA").HasColumnType("datetime");
            builder.Property(c => c.NUCPFCNPJ).HasColumnName("NUCPFCNPJ").HasColumnType("varchar(20)");
            builder.Property(c => c.VLVENDA).HasColumnName("VLVENDA").HasColumnType("double");
            builder.Property(c => c.VLACRES).HasColumnName("VLACRES").HasColumnType("double");
            builder.Property(c => c.VLDESC).HasColumnName("VLDESC").HasColumnType("double");
            builder.Property(c => c.VLTOTAL).HasColumnName("VLTOTAL").HasColumnType("double");
            builder.Property(c => c.STVENDA).HasColumnName("STVENDA").HasColumnType("int");
            builder.Property(c => c.DSINFCOMPL).HasColumnName("DSINFCOMPL").HasColumnType("varchar(500)");
            builder.Property(c => c.VLTOTIBPTFED).HasColumnName("VLTOTIBPTFED").HasColumnType("double");
            builder.Property(c => c.VLTOTIBPTEST).HasColumnName("VLTOTIBPTEST").HasColumnType("double");
            builder.Property(c => c.VLTOTIBPTMUN).HasColumnName("VLTOTIBPTMUN").HasColumnType("double");
            builder.Property(c => c.VLTOTIBPTIMP).HasColumnName("VLTOTIBPTIMP").HasColumnType("double");
            builder.Property(c => c.NUNF).HasColumnName("NUNF").HasColumnType("int");
            builder.Property(c => c.DSSERIE).HasColumnName("DSSERIE").HasColumnType("varchar(10)");
            builder.Property(c => c.TPDOC).HasColumnName("TPDOC").HasColumnType("int");
            builder.Property(c => c.STEMISSAO).HasColumnName("STEMISSAO").HasColumnType("int");
            builder.Property(c => c.DSCHAVEACESSO).HasColumnName("DSCHAVEACESSO").HasColumnType("varchar(50)");
            builder.Property(c => c.TPORIGEM).HasColumnName("TPORIGEM").HasColumnType("int"); 

            //chaves estrangeiras
            builder
                .HasMany(venda => venda.VendaItem)
                .WithOne(item => item.Venda)
                .HasForeignKey(item => new { item.Id })
                .HasPrincipalKey(venda => new { venda.Id })
                 .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasMany(venda => venda.VendaMoeda)
                .WithOne(moeda => moeda.Venda)
                .HasForeignKey(moeda => new { moeda.Id })
                .HasPrincipalKey(venda => new { venda.Id })
                 .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasMany(venda => venda.VendaCancelada)
                .WithOne(cancel => cancel.Venda)
                .HasForeignKey(cancel => new { cancel.Id })
                .HasPrincipalKey(venda => new { venda.Id })
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasMany(venda => venda.VendaEspelho)
                .WithOne(espelho => espelho.Venda)
                .HasForeignKey(espelho => new { espelho.Id })
                .HasPrincipalKey(venda => new { venda.Id })
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasMany(venda => venda.VendaFiscal)
                .WithOne(fiscal => fiscal.Venda)
                .HasForeignKey(fiscal => new { fiscal.Id })
                .HasPrincipalKey(venda => new { venda.Id })
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasMany(venda => venda.Devolucao)
                .WithOne(devolucao => devolucao.Venda)
                .HasForeignKey(devolucao => new { devolucao.Id })
                .HasPrincipalKey(venda => new { venda.Id });
            //    .OnDelete(DeleteBehavior.Cascade);

        }
    }

    public class VendaItemMapping : IEntityTypeConfiguration<VendaItem>
    {
        public void Configure(EntityTypeBuilder<VendaItem> builder)
        {
            builder.ToTable("venda_item");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("IDVENDA_ITEM").HasColumnType("bigint").IsRequired();
            builder.Property(c => c.IDVENDA).HasColumnName("IDVENDA").HasColumnType("bigint");
            builder.Property(c => c.IDPRODUTO).HasColumnName("IDPRODUTO").HasColumnType("bigint");
            builder.Property(c => c.SQITEM).HasColumnName("SQITEM").HasColumnType("int");
            builder.Property(c => c.VLUNIT).HasColumnName("VLUNIT").HasColumnType("double");
            builder.Property(c => c.NUQTD).HasColumnName("NUQTD").HasColumnType("double");
            builder.Property(c => c.VLITEM).HasColumnName("VLITEM").HasColumnType("double");
            builder.Property(c => c.VLACRES).HasColumnName("VLACRES").HasColumnType("double");
            builder.Property(c => c.VLDESC).HasColumnName("VLDESC").HasColumnType("double");
            builder.Property(c => c.VLTOTAL).HasColumnName("VLTOTAL").HasColumnType("double");
            builder.Property(c => c.VLCUSTOMEDIO).HasColumnName("VLCUSTOMEDIO").HasColumnType("double");
            builder.Property(c => c.STITEM).HasColumnName("STITEM").HasColumnType("int");
            builder.Property(c => c.PCIBPTFED).HasColumnName("PCIBPTFED").HasColumnType("double");
            builder.Property(c => c.PCIBPTEST).HasColumnName("PCIBPTEST").HasColumnType("double");
            builder.Property(c => c.PCIBPTMUN).HasColumnName("PCIBPTMUN").HasColumnType("double");
            builder.Property(c => c.PCIBPTIMP).HasColumnName("PCIBPTIMP").HasColumnType("double");

            builder
                .HasMany(vendaItem => vendaItem.DevolucaoItem)
                .WithOne(devolucaoItem => devolucaoItem.VendaItem)
                .HasForeignKey(devolucaoItem => new { devolucaoItem.Id })
                .HasPrincipalKey(vendaItem => new { vendaItem.Id });
            //    .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class VendaEspelhoMapping : IEntityTypeConfiguration<VendaEspelho>
    {
        public void Configure(EntityTypeBuilder<VendaEspelho> builder)
        {
            builder.ToTable("venda_espelho");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("IDESPELHO").HasColumnType("bigint").IsRequired();

            builder.Property(c => c.IDVENDA).HasColumnName("IDVENDA").HasColumnType("bigint");
            builder.Property(c => c.DSESPELHO).HasColumnName("DSESPELHO").HasColumnType("mediumtext");

        }
    }

    public class VendaFiscalMapping : IEntityTypeConfiguration<VendaFiscal>
    {
        public void Configure(EntityTypeBuilder<VendaFiscal> builder)
        {
            builder.ToTable("venda_fiscal");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("IDVENDAFISCAL").HasColumnType("bigint").IsRequired();

            builder.Property(c => c.IDVENDA).HasColumnName("IDVENDA").HasColumnType("bigint");
            builder.Property(c => c.TPDOC).HasColumnName("TPDOC").HasColumnType("int");
            builder.Property(c => c.DSXML).HasColumnName("DSXML").HasColumnType("mediumtext");
            builder.Property(c => c.STDOCFISCAL).HasColumnName("STDOCFISCAL").HasColumnType("int");
            builder.Property(c => c.DTHREMISSAO).HasColumnName("DTHREMISSAO").HasColumnType("datetime");

        }
    }

    public class VendaMoedaMapping : IEntityTypeConfiguration<VendaMoeda>
    {
        public void Configure(EntityTypeBuilder<VendaMoeda> builder)
        {
            builder.ToTable("venda_moeda");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("IDVENDA_MOEDA").HasColumnType("bigint").IsRequired();
            builder.Property(c => c.IDVENDA).HasColumnName("IDVENDA").HasColumnType("bigint");
            builder.Property(c => c.IDMOEDA).HasColumnName("IDMOEDA").HasColumnType("bigint");
            builder.Property(c => c.IDVALE).HasColumnName("IDVALE").HasColumnType("bigint");
            builder.Property(c => c.VLPAGO).HasColumnName("VLPAGO").HasColumnType("double");
            builder.Property(c => c.VLTROCO).HasColumnName("VLTROCO").HasColumnType("double");
            builder.Property(c => c.NUPARCELAS).HasColumnName("NUPARCELAS").HasColumnType("int");
            builder.Property(c => c.NSU).HasColumnName("NSU").HasColumnType("varchar(20)");
        }
    }

    public class VendaCanceladaMapping : IEntityTypeConfiguration<VendaCancelada>
    {
        public void Configure(EntityTypeBuilder<VendaCancelada> builder)
        {
            builder.ToTable("venda_cancel");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("IDVENDACANCEL").HasColumnType("bigint").IsRequired();

            builder.Property(c => c.IDVENDA).HasColumnName("IDVENDA").HasColumnType("bigint");
            builder.Property(c => c.IDUSUARIOCANCEL).HasColumnName("IDUSUARIOCANCEL").HasColumnType("bigint");
            builder.Property(c => c.DTHRCANCEL).HasColumnName("DTHRCANCEL").HasColumnType("datetime");
            builder.Property(c => c.DSMOTIVO).HasColumnName("DSMOTIVO").HasColumnType("varchar(500)");
            builder.Property(c => c.DSPROTOCOLO).HasColumnName("DSPROTOCOLO").HasColumnType("varchar(50)");
            builder.Property(c => c.DSXML).HasColumnName("DSXML").HasColumnType("mediumtext");

        }
    }

    #endregion

    #region Venda Temporaria
    public class VendaTemporariaMapping : IEntityTypeConfiguration<VendaTemporaria>
    {
        public void Configure(EntityTypeBuilder<VendaTemporaria> builder)
        {
            builder.ToTable("tmp_venda");
            builder.HasKey(c => c.Id);
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("IDVENDA").HasColumnType("bigint").IsRequired();

            builder.Property(c => c.IDCAIXA).HasColumnName("IDCAIXA").HasColumnType("bigint");
            builder.Property(c => c.IDCLIENTE).HasColumnName("IDCLIENTE").HasColumnType("bigint");
            builder.Property(c => c.SQVENDA).HasColumnName("SQVENDA").HasColumnType("int");
            builder.Property(c => c.DTHRVENDA).HasColumnName("DTHRVENDA").HasColumnType("datetime");
            builder.Property(c => c.NUCPFCNPJ).HasColumnName("NUCPFCNPJ").HasColumnType("varchar(20)");
            builder.Property(c => c.VLVENDA).HasColumnName("VLVENDA").HasColumnType("double");
            builder.Property(c => c.VLACRES).HasColumnName("VLACRES").HasColumnType("double");
            builder.Property(c => c.VLDESC).HasColumnName("VLDESC").HasColumnType("double");
            builder.Property(c => c.VLTOTAL).HasColumnName("VLTOTAL").HasColumnType("double");
            builder.Property(c => c.STVENDA).HasColumnName("STVENDA").HasColumnType("int");
            builder.Property(c => c.DSINFCOMPL).HasColumnName("DSINFCOMPL").HasColumnType("varchar(500)");
            builder.Property(c => c.VLTOTIBPTFED).HasColumnName("VLTOTIBPTFED").HasColumnType("double");
            builder.Property(c => c.VLTOTIBPTEST).HasColumnName("VLTOTIBPTEST").HasColumnType("double");
            builder.Property(c => c.VLTOTIBPTMUN).HasColumnName("VLTOTIBPTMUN").HasColumnType("double");
            builder.Property(c => c.VLTOTIBPTIMP).HasColumnName("VLTOTIBPTIMP").HasColumnType("double");
            builder.Property(c => c.NUNF).HasColumnName("NUNF").HasColumnType("int");
            builder.Property(c => c.DSSERIE).HasColumnName("DSSERIE").HasColumnType("varchar(10)");
            builder.Property(c => c.TPDOC).HasColumnName("TPDOC").HasColumnType("int");
            builder.Property(c => c.STEMISSAO).HasColumnName("STEMISSAO").HasColumnType("int");
            builder.Property(c => c.DSCHAVEACESSO).HasColumnName("DSCHAVEACESSO").HasColumnType("varchar(50)");
            builder.Property(c => c.TPORIGEM).HasColumnName("TPORIGEM").HasColumnType("int");

            builder
               .HasMany(vendatemp => vendatemp.VendaTemporariaEspelho)
               .WithOne(espelhotemp => espelhotemp.Venda)
               .HasForeignKey(espelhotemp => new { espelhotemp.Id })
               .HasPrincipalKey(venda => new { venda.Id })
               .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasMany(vendatemp => vendatemp.VendaTemporariaItem)
                .WithOne(itemtemp => itemtemp.Venda)
                .HasForeignKey(itemtemp => new { itemtemp.Id })
                .HasPrincipalKey(venda => new { venda.Id })
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasMany(vendaMoeda => vendaMoeda.VendaTemporariaMoeda)
                .WithOne(moedatemp => moedatemp.Venda)
                .HasForeignKey(moedatemp => new { moedatemp.Id })
                .HasPrincipalKey(venda => new { venda.Id })
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class VendaTemporariaEspelhoMapping : IEntityTypeConfiguration<VendaTemporariaEspelho>
    {
        public void Configure(EntityTypeBuilder<VendaTemporariaEspelho> builder)
        {
            builder.ToTable("tmp_venda_espelho");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("IDESPELHO").HasColumnType("bigint").IsRequired();

            builder.Property(c => c.IDVENDA).HasColumnName("IDVENDA").HasColumnType("bigint");
            builder.Property(c => c.DSESPELHO).HasColumnName("DSESPELHO").HasColumnType("mediumtext");

        }
    }

    public class VendaTemporariaItemMapping : IEntityTypeConfiguration<VendaTemporariaItem>
    {
        public void Configure(EntityTypeBuilder<VendaTemporariaItem> builder)
        {
            builder.ToTable("tmp_venda_item");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("IDVENDA_ITEM").HasColumnType("bigint").IsRequired();
            builder.Property(c => c.IDVENDA).HasColumnName("IDVENDA").HasColumnType("bigint");
            builder.Property(c => c.IDPRODUTO).HasColumnName("IDPRODUTO").HasColumnType("bigint");
            builder.Property(c => c.SQITEM).HasColumnName("SQITEM").HasColumnType("int");
            builder.Property(c => c.VLUNIT).HasColumnName("VLUNIT").HasColumnType("double");
            builder.Property(c => c.NUQTD).HasColumnName("NUQTD").HasColumnType("double");
            builder.Property(c => c.VLITEM).HasColumnName("VLITEM").HasColumnType("double");
            builder.Property(c => c.VLACRES).HasColumnName("VLACRES").HasColumnType("double");
            builder.Property(c => c.VLDESC).HasColumnName("VLDESC").HasColumnType("double");
            builder.Property(c => c.VLTOTAL).HasColumnName("VLTOTAL").HasColumnType("double");
            builder.Property(c => c.VLCUSTOMEDIO).HasColumnName("VLCUSTOMEDIO").HasColumnType("double");
            builder.Property(c => c.STITEM).HasColumnName("STITEM").HasColumnType("int");
            builder.Property(c => c.PCIBPTFED).HasColumnName("PCIBPTFED").HasColumnType("double");
            builder.Property(c => c.PCIBPTEST).HasColumnName("PCIBPTEST").HasColumnType("double");
            builder.Property(c => c.PCIBPTMUN).HasColumnName("PCIBPTMUN").HasColumnType("double");
            builder.Property(c => c.PCIBPTIMP).HasColumnName("PCIBPTIMP").HasColumnType("double");

        }
    }

    public class VendaTemporariaMoedaMapping : IEntityTypeConfiguration<VendaTemporariaMoeda>
    {
        public void Configure(EntityTypeBuilder<VendaTemporariaMoeda> builder)
        {
            builder.ToTable("tmp_venda_moeda");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("IDVENDA_MOEDA").HasColumnType("bigint").IsRequired();
            builder.Property(c => c.IDVENDA).HasColumnName("IDVENDA").HasColumnType("bigint");
            builder.Property(c => c.IDMOEDA).HasColumnName("IDMOEDA").HasColumnType("bigint");
            builder.Property(c => c.IDVALE).HasColumnName("IDVALE").HasColumnType("bigint");
            builder.Property(c => c.VLPAGO).HasColumnName("VLPAGO").HasColumnType("double");
            builder.Property(c => c.VLTROCO).HasColumnName("VLTROCO").HasColumnType("double");
            builder.Property(c => c.NUPARCELAS).HasColumnName("NUPARCELAS").HasColumnType("int");
            builder.Property(c => c.NSU).HasColumnName("NSU").HasColumnType("varchar(20)");

        }
    }
    #endregion
}

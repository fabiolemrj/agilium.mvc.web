using agilium.api.business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.infra.Mappings
{
    public class CaixaMapping : IEntityTypeConfiguration<Caixa>
    {
        public void Configure(EntityTypeBuilder<Caixa> builder)
        {
            builder.ToTable("caixa");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("IDCAIXA").HasColumnType("bigint").IsRequired();

            builder.Property(c => c.IDEMPRESA).HasColumnName("IDEMPRESA").HasColumnType("bigint");
            builder.Property(c => c.IDTURNO).HasColumnName("IDTURNO").HasColumnType("bigint");
            builder.Property(c => c.IDPDV).HasColumnName("IDPDV").HasColumnType("bigint");
            builder.Property(c => c.IDFUNC).HasColumnName("IDFUNC").HasColumnType("bigint");
            builder.Property(c => c.SQCAIXA).HasColumnName("SQCAIXA").HasColumnType("int");
            builder.Property(c => c.STCAIXA).HasColumnName("STCAIXA").HasColumnType("int");
            builder.Property(c => c.DTHRABT).HasColumnName("DTHRABT").HasColumnType("datetime");
            builder.Property(c => c.VLABT).HasColumnName("VLABT").HasColumnType("double");
            builder.Property(c => c.DTHRFECH).HasColumnName("DTHRFECH").HasColumnType("datetime");
            builder.Property(c => c.VLFECH).HasColumnName("VLFECH").HasColumnType("double");

            //chaves estrangeiras
            builder
                .HasMany(caixa => caixa.CaixaMoeda)
                .WithOne(moedas => moedas.Caixa)
                .HasForeignKey(moedas => new { moedas.IDCAIXA })
                .HasPrincipalKey(caixa => new { caixa.Id });

            builder
                .HasMany(caixa => caixa.CaixaMovimento)
                .WithOne(mov => mov.Caixa)
                .HasForeignKey(mov => new { mov.IDCAIXA })
                .HasPrincipalKey(caixa => new { caixa.Id });

            builder
              .HasMany(caixa => caixa.Venda)
              .WithOne(venda => venda.Caixa)
              .HasForeignKey(venda => new { venda.IDCAIXA })
              .HasPrincipalKey(caixa => new { caixa.Id });

            builder
           .HasMany(caixa => caixa.VendaTemporaria)
           .WithOne(vendaTemp => vendaTemp.Caixa)
           .HasForeignKey(vendaTemp => new { vendaTemp.IDCAIXA })
           .HasPrincipalKey(caixa => new { caixa.Id });
        }
    }

    public class CaixaMoedaMapping : IEntityTypeConfiguration<CaixaMoeda>
    {
        public void Configure(EntityTypeBuilder<CaixaMoeda> builder)
        {
            builder.ToTable("caixa_moeda");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("IDCAIXA_MOEDA").HasColumnType("bigint").IsRequired();
            builder.Property(c => c.IDCAIXA).HasColumnName("IDCAIXA").HasColumnType("bigint");
            builder.Property(c => c.IDMOEDA).HasColumnName("IDMOEDA").HasColumnType("bigint");
            builder.Property(c => c.VLMOEDAORIGINAL).HasColumnName("VLMOEDAORIGINAL").HasColumnType("double");
            builder.Property(c => c.VLMOEDACORRECAO).HasColumnName("VLMOEDACORRECAO").HasColumnType("double");
            builder.Property(c => c.IDUSUARIOCORRECAO).HasColumnName("IDUSUARIOCORRECAO").HasColumnType("bigint");
            builder.Property(c => c.DTHRCORRECAO).HasColumnName("DTHRCORRECAO").HasColumnType("datetime");

        }
    }

    public class CaixaMovimentoMapping : IEntityTypeConfiguration<CaixaMovimento>
    {
        public void Configure(EntityTypeBuilder<CaixaMovimento> builder)
        {
            builder.ToTable("caixa_mov");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("IDCAIXA_MOV").HasColumnType("bigint").IsRequired();
            builder.Property(c => c.IDCAIXA).HasColumnName("IDCAIXA").HasColumnType("bigint");

            builder.Property(c => c.TPMOV).HasColumnName("TPMOV").HasColumnType("int");
            builder.Property(c => c.DSMOV).HasColumnName("DSMOV").HasColumnType("varchar(50)");
            builder.Property(c => c.VLMOV).HasColumnName("VLMOV").HasColumnType("double");
            builder.Property(c => c.STMOV).HasColumnName("STMOV").HasColumnType("int");
        }
    }

}

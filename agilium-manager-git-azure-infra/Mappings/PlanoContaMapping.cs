using agilium.api.business.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.infra.Mappings
{
    public class PlanoContaMapping : IEntityTypeConfiguration<PlanoConta>
    {
        public void Configure(EntityTypeBuilder<PlanoConta> builder)
        {
            builder.ToTable("planoconta");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("IDCONTA").HasColumnType("bigint").IsRequired();


            builder.Property(c => c.IDEMPRESA).HasColumnName("IDEMPRESA").HasColumnType("bigint");
            builder.Property(c => c.IDCONTAPAI).HasColumnName("IDCONTAPAI").HasColumnType("bigint");
            builder.Property(c => c.CDCONTA).HasColumnName("CDCONTA").HasColumnType("varchar(20)");
            builder.Property(c => c.DSCONTA).HasColumnName("DSCONTA").HasColumnType("varchar(50)");
            builder.Property(c => c.TPCONTA).HasColumnName("TPCONTA").HasColumnType("int");
            builder.Property(c => c.STCONTA).HasColumnName("STCONTA").HasColumnType("int");

            builder
            .HasMany(planoconta => planoconta.PlanoContasFilho)
            .WithOne(planocontafilho => planocontafilho.PlanoContaPai)
            .HasForeignKey(planocontafilho => new { planocontafilho.IDCONTAPAI })
            .HasPrincipalKey(planoconta => new { planoconta.Id });

            builder
                 .HasMany(planoconta => planoconta.PlanoContaLancamentos)
                 .WithOne(planocontalanc => planocontalanc.PlanoConta)
                 .HasForeignKey(planocontalanc => new { planocontalanc.IDCONTA })
                 .HasPrincipalKey(planoconta => new { planoconta.Id });

            builder
                 .HasMany(planoconta => planoconta.PlanoContaSaldos)
                 .WithOne(planocontasaldo => planocontasaldo.PlanoConta)
                 .HasForeignKey(planocontasaldo => new { planocontasaldo.IDCONTA })
                 .HasPrincipalKey(planoconta => new { planoconta.Id });

            builder
                .HasMany(planoconta => planoconta.ContaPagar)
                .WithOne(contapagar => contapagar.PlanoConta)
                .HasForeignKey(contaPagar => new { contaPagar.IDCONTA })
                .HasPrincipalKey(planoconta => new { planoconta.Id });

            builder
               .HasMany(planoconta => planoconta.ContaReceber)
               .WithOne(contaReceber => contaReceber.PlanoConta)
               .HasForeignKey(contaReceber => new { contaReceber.IDCONTA })
               .HasPrincipalKey(planoconta => new { planoconta.Id });

        }
    }

    public class PlanoContaSaldoMapeamento : IEntityTypeConfiguration<PlanoContaSaldo>
    {
        public void Configure(EntityTypeBuilder<PlanoContaSaldo> builder)
        {
            builder.ToTable("planoconta_saldo");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("IDSALDO").HasColumnType("bigint").IsRequired();

            builder.Property(c => c.IDCONTA).HasColumnName("IDCONTA").HasColumnType("bigint");
            builder.Property(c => c.DTHRATU).HasColumnName("DTHRATU").HasColumnType("datetime");
            builder.Property(c => c.NUANOMESREF).HasColumnName("NUANOMESREF").HasColumnType("int");
            builder.Property(c => c.VLSALDO).HasColumnName("VLSALDO").HasColumnType("double");

        }
    }

    public class PlanoContaLancamentoMapping : IEntityTypeConfiguration<PlanoContaLancamento>
    {
        public void Configure(EntityTypeBuilder<PlanoContaLancamento> builder)
        {
            builder.ToTable("planoconta_lanc");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("IDLANC").HasColumnType("bigint").IsRequired();

            builder.Property(c => c.IDCONTA).HasColumnName("IDCONTA").HasColumnType("bigint");
            builder.Property(c => c.DTCAD).HasColumnName("DTCAD").HasColumnType("datetime");
            builder.Property(c => c.DTREF).HasColumnName("DTREF").HasColumnType("date");
            builder.Property(c => c.NUANOMESREF).HasColumnName("NUANOMESREF").HasColumnType("int");
            builder.Property(c => c.DSLANC).HasColumnName("DSLANC").HasColumnType("varchar(500)");
            builder.Property(c => c.VLLANC).HasColumnName("VLLANC").HasColumnType("double");
            builder.Property(c => c.TPLANC).HasColumnName("TPLANC").HasColumnType("int");
            builder.Property(c => c.STLANC).HasColumnName("STLANC").HasColumnType("int");

            builder
              .HasMany(planoContaLanc => planoContaLanc.EstoquesHistoricos)
              .WithOne(estoqueHistorico => estoqueHistorico.PlanoContaLancamento)
              .HasForeignKey(estoqueHistorico => new { estoqueHistorico.IDLANC })
              .HasPrincipalKey(planoContaLanc => new { planoContaLanc.Id });

            builder
             .HasMany(planoContaLanc => planoContaLanc.ContaPagar)
             .WithOne(contaPagar => contaPagar.PlanoContaLancamento)
             .HasForeignKey(contaPagar => new { contaPagar.IDCONTAPAI })
             .HasPrincipalKey(planoContaLanc => new { planoContaLanc.Id });

            builder
               .HasMany(planoconta => planoconta.ContaReceber)
               .WithOne(contaReceber => contaReceber.PlanoContaLancamento)
               .HasForeignKey(contaReceber => new { contaReceber.IDCONTA })
               .HasPrincipalKey(planoconta => new { planoconta.Id });

        }
    }
}

using agilium.api.business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.infra.Mappings
{
    public class MoedaMapping : IEntityTypeConfiguration<Moeda>
    {
        public void Configure(EntityTypeBuilder<Moeda> builder)
        {
            builder.ToTable("moeda");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("IDMOEDA").HasColumnType("bigint").IsRequired();
            builder.Property(c => c.IDEMPRESA).HasColumnName("IDEMPRESA").HasColumnType("bigint");
            builder.Property(c => c.CDMOEDA).HasColumnName("CDMOEDA").HasColumnType("varchar(6)");
            builder.Property(c => c.DSMOEDA).HasColumnName("DSMOEDA").HasColumnType("varchar(50)");
            builder.Property(c => c.STMOEDA).HasColumnName("STMOEDA").HasColumnType("int");
            builder.Property(c => c.TPDOCFISCAL).HasColumnName("TPDOCFISCAL").HasColumnType("int");
            builder.Property(c => c.TPMOEDA).HasColumnName("TPMOEDA").HasColumnType("int");
            builder.Property(c => c.PCTAXA).HasColumnName("PCTAXA").HasColumnType("double");
            builder.Property(c => c.STTROCO).HasColumnName("STTROCO").HasColumnType("int");
            builder.Property(c => c.COR_BOTAO).HasColumnName("COR_BOTAO").HasColumnType("varchar(20)");
            builder.Property(c => c.COR_FONTE).HasColumnName("COR_FONTE").HasColumnType("varchar(20)");
            builder.Property(c => c.TECLA_ATALHO).HasColumnName("TECLA_ATALHO").HasColumnType("varchar(10)");

            builder
             .HasMany(moeda => moeda.CaixaMoeda)
            .WithOne(cm => cm.Moeda)
            .HasForeignKey(cm => new { cm.IDMOEDA })
            .HasPrincipalKey(moeda => new { moeda.Id });

            builder
              .HasMany(moeda => moeda.VendaMoeda)
             .WithOne(vm => vm.Moeda)
             .HasForeignKey(vm => new { vm.IDMOEDA })
             .HasPrincipalKey(moeda => new { moeda.Id });

            builder
                .HasMany(moeda => moeda.MoedasSiteMercados)
               .WithOne(moedaSiteMerc => moedaSiteMerc.Moeda)
               .HasForeignKey(moedaSiteMerc => new { moedaSiteMerc.IDMOEDA })
               .HasPrincipalKey(moeda => new { moeda.Id });

            builder
                .HasMany(moeda => moeda.VendaTemporariaMoeda)
               .WithOne(moedaSiteMerc => moedaSiteMerc.Moeda)
               .HasForeignKey(moedaSiteMerc => new { moedaSiteMerc.IDMOEDA })
               .HasPrincipalKey(moeda => new { moeda.Id });
        }
    }
}

using agilium.api.business.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.infra.Mappings
{
    public class MotivoDevolucaoMapping : IEntityTypeConfiguration<MotivoDevolucao>
    {
        public void Configure(EntityTypeBuilder<MotivoDevolucao> builder)
        {
            builder.ToTable("motivo_devolucao");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("IDMOTDEV").HasColumnType("bigint").IsRequired();
            builder.Property(c => c.IDEMPRESA).HasColumnName("IDEMPRESA").HasColumnType("bigint");

            builder.Property(c => c.DSMOTDEV).HasColumnName("DSMOTDEV").HasColumnType("varchar(50)");
            builder.Property(c => c.STMOTDEV).HasColumnName("STMOTDEV").HasColumnType("int");

            //chaves estrangeiras
            builder
            .HasOne(motivoDev => motivoDev.Empresa)
            .WithMany(empresa => empresa.MotivosDevolucao)
            .HasForeignKey(devolucao => new { devolucao.IDEMPRESA })
            .HasPrincipalKey(motivoDev => new { motivoDev.Id });

            builder
               .HasMany(motivoDevolucao => motivoDevolucao.Devolucao)
               .WithOne(devolucao => devolucao.MotivoDevolucao)
               .HasForeignKey(devolucao => new { devolucao.IDMOTDEV })
               .HasPrincipalKey(motivoDevolucao => new { motivoDevolucao.Id });

        }
    }

    public class DevolucaoMapping : IEntityTypeConfiguration<Devolucao>
    {
        public void Configure(EntityTypeBuilder<Devolucao> builder)
        {
            builder.ToTable("devolucao");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).HasColumnName("IDDEV").HasColumnType("bigint").IsRequired();

            builder.Property(c => c.IDEMPRESA).HasColumnName("IDEMPRESA").HasColumnType("bigint");
            builder.Property(c => c.IDVENDA).HasColumnName("IDVENDA").HasColumnType("bigint");
            builder.Property(c => c.IDCLIENTE).HasColumnName("IDCLIENTE").HasColumnType("bigint");
            builder.Property(c => c.IDMOTDEV).HasColumnName("IDMOTDEV").HasColumnType("bigint");
            builder.Property(c => c.IDVALE).HasColumnName("IDVALE").HasColumnType("bigint");
            builder.Property(c => c.CDDEV).HasColumnName("CDDEV").HasColumnType("varchar(6)");
            builder.Property(c => c.DTHRDEV).HasColumnName("DTHRDEV").HasColumnType("datetime");
            builder.Property(c => c.VLTOTALDEV).HasColumnName("VLTOTALDEV").HasColumnType("double");
            builder.Property(c => c.DSOBSDEV).HasColumnName("DSOBSDEV").HasColumnType("varchar(200)");
            builder.Property(c => c.STDEV).HasColumnName("STDEV").HasColumnType("int");

            //chaves estrangeiras
            builder
             .HasMany(devolucao => devolucao.DevolucaoItem)
             .WithOne(devolucaoItem => devolucaoItem.Devolucao)
             .HasForeignKey(devolucaoItem => new { devolucaoItem.IDDEV })
             .HasPrincipalKey(devolucao => new { devolucao.Id })
             .OnDelete(DeleteBehavior.Cascade);

        }
    }

    public class DevolucaoItemMapping : IEntityTypeConfiguration<DevolucaoItem>
    {
        public void Configure(EntityTypeBuilder<DevolucaoItem> builder)
        {
            builder.ToTable("devolucao_item");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).HasColumnName("IDDEV_ITEM").HasColumnType("bigint").IsRequired();

            builder.Property(c => c.IDDEV).HasColumnName("IDDEV").HasColumnType("bigint");
            builder.Property(c => c.IDVENDA_ITEM).HasColumnName("IDVENDA_ITEM").HasColumnType("bigint");
            builder.Property(c => c.NUQTD).HasColumnName("NUQTD").HasColumnType("double");
            builder.Property(c => c.VLITEM).HasColumnName("VLITEM").HasColumnType("double");
        }
    }
}

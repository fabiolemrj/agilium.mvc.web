using agilium.api.business.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.infra.Mappings
{
    public class TurnoMapping : IEntityTypeConfiguration<Turno>
    {
        public void Configure(EntityTypeBuilder<Turno> builder)
        {
            builder.ToTable("turno");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("IDTURNO").HasColumnType("bigint").IsRequired();

            builder.Property(c => c.IDEMPRESA).HasColumnName("IDEMPRESA").HasColumnType("bigint");
            builder.Property(c => c.IDUSUARIOINI).HasColumnName("IDUSUARIOINI").HasColumnType("bigint");
            builder.Property(c => c.IDUSUARIOFIM).HasColumnName("IDUSUARIOFIM").HasColumnType("bigint");
            builder.Property(c => c.DTTURNO).HasColumnName("DTTURNO").HasColumnType("date");
            builder.Property(c => c.NUTURNO).HasColumnName("NUTURNO").HasColumnType("int");
            builder.Property(c => c.DTHRINI).HasColumnName("DTHRINI").HasColumnType("datetime");
            builder.Property(c => c.DTHRFIM).HasColumnName("DTHRFIM").HasColumnType("datetime");
            builder.Property(c => c.DSOBS).HasColumnName("DSOBS").HasColumnType("text");

            builder
               .HasMany(turno => turno.Caixas)
               .WithOne(compra => compra.Turno)
               .HasForeignKey(compra => new { compra.IDTURNO })
               .HasPrincipalKey(turno => new { turno.Id });

            builder
              .HasMany(turno => turno.Compras)
              .WithOne(compra => compra.Turno)
              .HasForeignKey(compra => new { compra.IDEMPRESA })
              .HasPrincipalKey(turno => new { turno.Id });

        }
    }

    public class TurnoPrecoMapping : IEntityTypeConfiguration<TurnoPreco>
    {
        public void Configure(EntityTypeBuilder<TurnoPreco> builder)
        {
            builder.ToTable("turno_preco");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("IDTURNO_PRECO").HasColumnType("bigint").IsRequired();

            builder.Property(c => c.IDPRODUTO).HasColumnName("IDPRODUTO").HasColumnType("bigint");
            builder.Property(c => c.NUTURNO).HasColumnName("NUTURNO").HasColumnType("int");
            builder.Property(c => c.TPDIFERENCA).HasColumnName("TPDIFERENCA").HasColumnType("int");
            builder.Property(c => c.TPVALOR).HasColumnName("TPVALOR").HasColumnType("int");
            builder.Property(c => c.NUVALOR).HasColumnName("NUVALOR").HasColumnType("double");
            builder.Property(c => c.NMUSUARIO).HasColumnName("NMUSUARIO").HasColumnType("varchar(50)");
            builder.Property(c => c.DTHRCAD).HasColumnName("DTHRCAD").HasColumnType("datetime");

        }
    }
}

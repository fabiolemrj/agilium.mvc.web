using agilium.api.business.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.infra.Mappings
{
    public class InventarioMapping : IEntityTypeConfiguration<Inventario>
    {
        public void Configure(EntityTypeBuilder<Inventario> builder)
        {
            builder.ToTable("inventario");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("IDINVENT").HasColumnType("bigint").IsRequired();

            builder.Property(c => c.IDEMPRESA).HasColumnName("IDEMPRESA").HasColumnType("bigint");
            builder.Property(c => c.IDESTOQUE).HasColumnName("IDESTOQUE").HasColumnType("bigint");
            builder.Property(c => c.CDINVENT).HasColumnName("CDINVENT").HasColumnType("varchar(6)");
            builder.Property(c => c.DSINVENT).HasColumnName("DSINVENT").HasColumnType("varchar(50)");
            builder.Property(c => c.DTINVENT).HasColumnName("DTINVENT").HasColumnType("date");
            builder.Property(c => c.STINVENT).HasColumnName("STINVENT").HasColumnType("int");
            builder.Property(c => c.DSOBS).HasColumnName("DSOBS").HasColumnType("varchar(500)");
            builder.Property(c => c.TPANALISE).HasColumnName("TPANALISE").HasColumnType("int");

            //chaves estrangeiras
            builder
               .HasMany(inventario => inventario.InventarioItem)
               .WithOne(inventarioItem => inventarioItem.Inventario)
               .HasForeignKey(inventarioItem => new { inventarioItem.IDINVENT })
               .HasPrincipalKey(inventario => new { inventario.Id })
               .OnDelete(DeleteBehavior.Cascade);

        }
    }
    public class InventarioItemMapping : IEntityTypeConfiguration<InventarioItem>
    {
        public void Configure(EntityTypeBuilder<InventarioItem> builder)
        {
            builder.ToTable("inventario_item");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("IDINVENTITEM").HasColumnType("bigint").IsRequired();

            builder.Property(c => c.IDINVENT).HasColumnName("IDINVENT").HasColumnType("bigint");
            builder.Property(c => c.IDPRODUTO).HasColumnName("IDPRODUTO").HasColumnType("bigint");
            builder.Property(c => c.IDPERDA).HasColumnName("IDPERDA").HasColumnType("bigint");
            builder.Property(c => c.IDUSUARIOANALISE).HasColumnName("IDUSUARIOANALISE").HasColumnType("bigint");
            builder.Property(c => c.DTHRANALISE).HasColumnName("DTHRANALISE").HasColumnType("datetime");
            builder.Property(c => c.NUQTDANALISE).HasColumnName("NUQTDANALISE").HasColumnType("double");
            builder.Property(c => c.NUQTDESTOQUE).HasColumnName("NUQTDESTOQUE").HasColumnType("double");
            builder.Property(c => c.VLCUSTOMEDIO).HasColumnName("VLCUSTOMEDIO").HasColumnType("double");

        }
    }
}

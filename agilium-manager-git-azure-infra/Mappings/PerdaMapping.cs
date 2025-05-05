using agilium.api.business.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.infra.Mappings
{
    public class PerdaMapping : IEntityTypeConfiguration<Perda>
    {
        public void Configure(EntityTypeBuilder<Perda> builder)
        {
            builder.ToTable("perda");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("IDPERDA").HasColumnType("bigint").IsRequired();

            builder.Property(c => c.IDEMPRESA).HasColumnName("IDEMPRESA").HasColumnType("bigint");
            builder.Property(c => c.IDESTOQUE).HasColumnName("IDESTOQUE").HasColumnType("bigint");
            builder.Property(c => c.IDESTOQUEHST).HasColumnName("IDESTOQUEHST").HasColumnType("bigint");
            builder.Property(c => c.IDPRODUTO).HasColumnName("IDPRODUTO").HasColumnType("bigint");
            builder.Property(c => c.IDUSUARIO).HasColumnName("IDUSUARIO").HasColumnType("bigint");
            builder.Property(c => c.CDPERDA).HasColumnName("CDPERDA").HasColumnType("varchar(6)");
            builder.Property(c => c.DTHRPERDA).HasColumnName("DTHRPERDA").HasColumnType("datetime");
            builder.Property(c => c.TPPERDA).HasColumnName("TPPERDA").HasColumnType("int");
            builder.Property(c => c.TPMOV).HasColumnName("TPMOV").HasColumnType("int");
            builder.Property(c => c.NUQTDPERDA).HasColumnName("NUQTDPERDA").HasColumnType("double");
            builder.Property(c => c.VLCUSTOMEDIO).HasColumnName("VLCUSTOMEDIO").HasColumnType("double");
            builder.Property(c => c.DSOBS).HasColumnName("DSOBS").HasColumnType("varchar(200)");

            builder
             .HasMany(perda => perda.InventarioItem)
             .WithOne(inventarioItem => inventarioItem.Perda)
             .HasForeignKey(inventarioItem => new { inventarioItem.IDPERDA })
             .HasPrincipalKey(perda => new { perda.Id });
        }
    }
}

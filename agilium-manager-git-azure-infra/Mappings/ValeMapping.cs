using agilium.api.business.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.infra.Mappings
{
    public class ValeMapping : IEntityTypeConfiguration<Vale>
    {
        public void Configure(EntityTypeBuilder<Vale> builder)
        {
            builder.ToTable("vale");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("IDVALE").HasColumnType("bigint").IsRequired();

            builder.Property(c => c.IDEMPRESA).HasColumnName("IDEMPRESA").HasColumnType("bigint");
            builder.Property(c => c.IDCLIENTE).HasColumnName("IDCLIENTE").HasColumnType("bigint");
            builder.Property(c => c.CDVALE).HasColumnName("CDVALE").HasColumnType("varchar(6)");
            builder.Property(c => c.DTHRVALE).HasColumnName("DTHRVALE").HasColumnType("datetime");
            builder.Property(c => c.TPVALE).HasColumnName("TPVALE").HasColumnType("int");
            builder.Property(c => c.STVALE).HasColumnName("STVALE").HasColumnType("int");
            builder.Property(c => c.VLVALE).HasColumnName("VLVALE").HasColumnType("double");
            builder.Property(c => c.CDBARRA).HasColumnName("CDBARRA").HasColumnType("varchar(10)");


            builder
                 .HasMany(vale => vale.VendaMoeda)
                 .WithOne(vendaMoed => vendaMoed.Vale)
                 .HasForeignKey(vendaMoed => new { vendaMoed.IDVALE })
                 .HasPrincipalKey(vale => new { vale.Id });


            //builder
            //     .HasMany(vale => vale.VendaTemporariaMoeda)
            //     .WithOne(vendaMoedaTemp => vendaMoedaTemp.Vale)
            //     .HasForeignKey(vendaMoedaTemp => new { vendaMoedaTemp.IDVALE })
            //     .HasPrincipalKey(vale => new { vale.Id })
            //      .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasMany(vale => vale.Devolucao)
                .WithOne(devolucao => devolucao.Vale)
                .HasForeignKey(devolucao => new { devolucao.IDVALE })
                .HasPrincipalKey(vale => new { vale.Id });
            //     .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

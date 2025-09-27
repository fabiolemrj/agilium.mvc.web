using agilium.api.business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.infra.Mappings
{
    public class LicencaMapping : IEntityTypeConfiguration<Licenca>
    {
        public void Configure(EntityTypeBuilder<Licenca> builder)
        {
            builder.ToTable("licenca");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("IDLICENCA").HasColumnType("bigint").IsRequired();
            builder.Property(c => c.IDEMPRESA).HasColumnName("IDEMPRESA").HasColumnType("bigint");
            builder.Property(c => c.K1).HasColumnName("k1").HasColumnType("varchar(50)");
            builder.Property(c => c.K2).HasColumnName("k2").HasColumnType("varchar(50)");
            builder.Property(c => c.K3).HasColumnName("k3").HasColumnType("varchar(50)");
            builder.Property(c => c.K4).HasColumnName("k4").HasColumnType("varchar(20)");
            builder.Property(c => c.K5).HasColumnName("k5").HasColumnType("varchar(20)");
            builder.Property(c => c.K6).HasColumnName("k6").HasColumnType("varchar(50)");
            builder.Property(c => c.K7).HasColumnName("k7").HasColumnType("varchar(30)");
        }
    }
}

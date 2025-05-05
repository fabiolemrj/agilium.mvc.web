using agilium.api.business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.infra.Mappings
{
    public class ConfigMapping : IEntityTypeConfiguration<agilium.api.business.Models.Config>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<agilium.api.business.Models.Config> builder)
        {
            builder.ToTable("config");
            builder.HasKey(c => c.CHAVE);

            builder.Property(c => c.CHAVE).HasColumnName("CHAVE").HasColumnType("varchar(30)").IsRequired();

            builder.Property(c => c.IDEMPRESA).HasColumnName("IDEMPRESA").HasColumnType("bigint");
            builder.Property(c => c.VALOR).HasColumnName("VALOR").HasColumnType("varchar(100)");

            //campos padrao da entidade que nao existem na tabela

            builder.Ignore(c => c.Id);

        }
    }

    public class ConfigImagemMapping : IEntityTypeConfiguration<ConfigImagem>
    {
        public void Configure(EntityTypeBuilder<ConfigImagem> builder)
        {

            builder.ToTable("config_img");
            builder.HasKey(c => c.CHAVE);

            builder.Property(c => c.CHAVE).HasColumnName("CHAVE").HasColumnType("varchar(30)").IsRequired();

            builder.Property(c => c.IDEMPRESA).HasColumnName("IDEMPRESA").HasColumnType("bigint");
            builder.Property(c => c.IMG).HasColumnName("IMG").HasColumnType("mediumblob");
            builder.Ignore(c => c.Id);
        }
    }
}

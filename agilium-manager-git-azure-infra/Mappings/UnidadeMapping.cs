using agilium.api.business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.infra.Mappings
{
    public class UnidadeMapping : IEntityTypeConfiguration<Unidade>
    {
        public void Configure(EntityTypeBuilder<Unidade> builder)
        {
            builder.ToTable("unidade");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("IDUNIDADE").HasColumnType("bigint").IsRequired();

            builder.Property(c => c.Codigo).HasColumnName("NMSIGLA").HasColumnType("varchar(5)");
            builder.Property(c => c.Descricao).HasColumnName("DSSIGLA").HasColumnType("varchar(30)");
            builder.Property(c => c.Ativo).HasColumnName("STATIVO").HasColumnType("int");

        }
    }
}

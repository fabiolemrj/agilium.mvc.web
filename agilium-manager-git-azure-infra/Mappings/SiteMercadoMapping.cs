using agilium.api.business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.infra.Mappings
{
    public class SiteMercadoMapping : IEntityTypeConfiguration<ProdutoSiteMercado>
    {
        public void Configure(EntityTypeBuilder<ProdutoSiteMercado> builder)
        {
            builder.ToTable("prod_sitemercado");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("IDPROD_SITEMERCADO").HasColumnType("bigint").IsRequired();
            builder.Property(c => c.IDEMPRESA).HasColumnName("IDEMPRESA").HasColumnType("bigint");
            builder.Property(c => c.DSPROD).HasColumnName("DSPROD").HasColumnType("varchar(50)");
            builder.Property(c => c.NUQTDATACADO).HasColumnName("NUQTDATACADO").HasColumnType("double");
            builder.Property(c => c.VLATACADO).HasColumnName("VLATACADO").HasColumnType("double");
            builder.Property(c => c.VLCOMPRA).HasColumnName("VLCOMPRA").HasColumnType("double");
            builder.Property(c => c.VLPROMOCAO).HasColumnName("VLPROMOCAO").HasColumnType("double");
            builder.Property(c => c.STDISPSITE).HasColumnName("STDISPSITE").HasColumnType("int");
            builder.Property(c => c.STVALIDADEPROXIMA).HasColumnName("STVALIDADEPROXIMA").HasColumnType("int");
            builder.Property(c => c.DTHRATU).HasColumnName("DTHRATU").HasColumnType("datetime");

        }
    }

    public class MoedaSiteMercadoMapping : IEntityTypeConfiguration<MoedaSiteMercado>
    {
        public void Configure(EntityTypeBuilder<MoedaSiteMercado> builder)
        {
            builder.ToTable("moeda_sitemercado");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("IDMOEDASM").HasColumnType("bigint").IsRequired();
            builder.Property(c => c.IDEMPRESA).HasColumnName("IDEMPRESA").HasColumnType("bigint");
            builder.Property(c => c.IDMOEDA).HasColumnName("IDMOEDA").HasColumnType("bigint");
            builder.Property(c => c.IDSM).HasColumnName("IDSM").HasColumnType("int");
            builder.Property(c => c.DTHRCAD).HasColumnName("DTHRCAD").HasColumnType("datetime");
        }
    }
}

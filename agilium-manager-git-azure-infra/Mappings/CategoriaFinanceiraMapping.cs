using agilium.api.business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.infra.Mappings
{
    internal class CategoriaFinanceiraMapping : IEntityTypeConfiguration<CategoriaFinanceira>
    {
        public void Configure(EntityTypeBuilder<CategoriaFinanceira> builder)
        {
            builder.ToTable("categ_financ");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("IDCATEG").HasColumnType("bigint").IsRequired();
            builder.Property(c => c.NMCATEG).HasColumnName("NMCATEG").HasColumnType("varchar(20)");
            builder.Property(c => c.STCATEG).HasColumnName("STCATEG").HasColumnType("int");

            builder
                .HasMany(categFinanc => categFinanc.ContaPagar)
                .WithOne(contaPagar => contaPagar.CategFinanc)
                .HasForeignKey(contaPagar => new { contaPagar.IDCATEG_FINANC })
                .HasPrincipalKey(categFinanc => new { categFinanc.Id });

            builder
                .HasMany(categFinanc => categFinanc.ContaReceber)
                .WithOne(contaReceber => contaReceber.CategFinanc)
                .HasForeignKey(contaReceber => new { contaReceber.IDCATEG_FINANC })
                .HasPrincipalKey(categFinanc => new { categFinanc.Id });
        }
    }
}

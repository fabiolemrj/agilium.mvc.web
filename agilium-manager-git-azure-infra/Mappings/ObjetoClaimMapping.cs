using agilium.api.business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.infra.Mappings
{
    public class ObjetoClaimMapping : IEntityTypeConfiguration<ObjetoClaim>
    {
        public void Configure(EntityTypeBuilder<ObjetoClaim> builder)
        {
            builder.ToTable("claimslist");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).HasColumnName("claimId").HasColumnType("bigint").IsRequired();

            builder.Property(c => c.ClaimType).HasColumnName("claimType").HasColumnType("varchar(10)").IsRequired();
        }
    }
}

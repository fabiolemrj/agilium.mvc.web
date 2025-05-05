using agilium.api.business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.infra.Mappings
{
    public class PontoVendaMapping : IEntityTypeConfiguration<PontoVenda>
    {
        public void Configure(EntityTypeBuilder<PontoVenda> builder)
        {
            builder.ToTable("pdv");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("IDPDV").HasColumnType("bigint").IsRequired();
            builder.Property(c => c.IDEMPRESA).HasColumnName("IDEMPRESA").HasColumnType("bigint");
            builder.Property(c => c.IDESTOQUE).HasColumnName("IDESTOQUE").HasColumnType("bigint");
            builder.Property(c => c.CDPDV).HasColumnName("CDPDV").HasColumnType("varchar(6)");
            builder.Property(c => c.DSPDV).HasColumnName("DSPDV").HasColumnType("varchar(50)");
            builder.Property(c => c.STPDV).HasColumnName("STPDV").HasColumnType("int");
            builder.Property(c => c.NMMAQUINA).HasColumnName("NMMAQUINA").HasColumnType("varchar(50)");
            builder.Property(c => c.DSCAMINHO_CERT).HasColumnName("DSCAMINHO_CERT").HasColumnType("varchar(255)");
            builder.Property(c => c.DSSENHA_CERT).HasColumnName("DSSENHA_CERT").HasColumnType("varchar(30)");
            builder.Property(c => c.DSPORTAIMPRESSORA).HasColumnName("DSPORTAIMPRESSORA").HasColumnType("varchar(20)");
            builder.Property(c => c.DSPORTABAL).HasColumnName("DSPORTABAL").HasColumnType("varchar(20)");
            builder.Property(c => c.CDMODELOBAL).HasColumnName("CDMODELOBAL").HasColumnType("int");
            builder.Property(c => c.CDHANDSHAKEBAL).HasColumnName("CDHANDSHAKEBAL").HasColumnType("int");
            builder.Property(c => c.CDPARITYBAL).HasColumnName("CDPARITYBAL").HasColumnType("int");
            builder.Property(c => c.CDSERIALSTOPBITBAL).HasColumnName("CDSERIALSTOPBITBAL").HasColumnType("int");
            builder.Property(c => c.NUBAUDRATEBAL).HasColumnName("NUBAUDRATEBAL").HasColumnType("int");
            builder.Property(c => c.NUDATABITBAL).HasColumnName("NUDATABITBAL").HasColumnType("int");
            builder.Property(c => c.STGAVETA).HasColumnName("STGAVETA").HasColumnType("int");

            builder
                .HasMany(pdv => pdv.Caixas)
                .WithOne(caixa => caixa.PontoVenda)
                .HasForeignKey(caixa => new { caixa.IDPDV })
                .HasPrincipalKey(pdv => new { pdv.Id });
        }
    }
}

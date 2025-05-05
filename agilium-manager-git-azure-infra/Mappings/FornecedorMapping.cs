using agilium.api.business.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.infra.Mappings
{
    public class FornecedorMapping : IEntityTypeConfiguration<Fornecedor>
    {
        public void Configure(EntityTypeBuilder<Fornecedor> builder)
        {
            builder.ToTable("fornecedor");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).HasColumnName("IDFORN").HasColumnType("bigint").IsRequired();

            builder.Property(c => c.IDENDERECO).HasColumnName("IDENDERECO").HasColumnType("bigint");
            builder.Property(c => c.CDFORN).HasColumnName("CDFORN").HasColumnType("varchar(6)");
            builder.Property(c => c.NMRZSOCIAL).HasColumnName("NMRZSOCIAL").HasColumnType("varchar(70)");
            builder.Property(c => c.NMFANTASIA).HasColumnName("NMFANTASIA").HasColumnType("varchar(70)");
            builder.Property(c => c.TPPESSOA).HasColumnName("TPPESSOA").HasColumnType("varchar(1)");
            builder.Property(c => c.NUCPFCNPJ).HasColumnName("NUCPFCNPJ").HasColumnType("varchar(20)");
            builder.Property(c => c.DSINSCR).HasColumnName("DSINSCR").HasColumnType("varchar(20)");
            builder.Property(c => c.TPFISCAL).HasColumnName("TPFISCAL").HasColumnType("int");
            builder.Property(c => c.STFORNEC).HasColumnName("STFORNEC").HasColumnType("int");

            builder
              .HasMany(fornec => fornec.FornecedoresContatos)
              .WithOne(fornecCont => fornecCont.Fornecedor)
              .HasForeignKey(fornecCont => new { fornecCont.IDFORN })
              .HasPrincipalKey(fornec => new { fornec.Id });

            builder
             .HasMany(fornec => fornec.ContaPagar)
             .WithOne(contaPagar => contaPagar.Fornecedor)
             .HasForeignKey(contaPagar => new { contaPagar.IDFORNEC })
             .HasPrincipalKey(fornec => new { fornec.Id });


            builder
             .HasMany(fornec => fornec.Compras)
             .WithOne(compra => compra.Fornecedor)
             .HasForeignKey(compra => new { compra.IDFORN })
             .HasPrincipalKey(fornec => new { fornec.Id });
        }

    }

    public class FornecedorContatoMapping : IEntityTypeConfiguration<FornecedorContato>
    {
        public void Configure(EntityTypeBuilder<FornecedorContato> builder)
        {
            builder.ToTable("forn_contato");
            builder.HasKey(c => new { c.IDCONTATO, c.IDFORN });
            builder.Property(c => c.IDCONTATO).HasColumnName("IDCONTATO").HasColumnType("bigint").IsRequired();
            builder.Property(c => c.IDFORN).HasColumnName("IDFORN").HasColumnType("bigint").IsRequired();

            builder.Ignore(c => c.Id);

            builder
               .HasOne(fornec => fornec.Contato)
               .WithMany(fornecCont => fornecCont.FornecedoresContatos)
               .HasForeignKey(fornecCont => new { fornecCont.IDCONTATO })
               .HasPrincipalKey(fornec => new { fornec.Id });

        }
    }
}

using agilium.api.business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.infra.Mappings
{
    public class ContatoMapping : IEntityTypeConfiguration<Contato>
    {
        public void Configure(EntityTypeBuilder<Contato> builder)
        {
            builder.ToTable("contato");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("IDCONTATO").HasColumnType("bigint").IsRequired();
            builder.Property(c => c.TPCONTATO).HasColumnName("TPCONTATO").HasColumnType("int").IsRequired();
            builder.Property(c => c.DESCR1).HasColumnName("DESCR1").HasColumnType("varchar(100)");
            builder.Property(c => c.DESCR2).HasColumnName("DESCR2").HasColumnType("varchar(100)");
            builder.Property(c => c.DataCadastro).HasColumnName("DTHRATU").HasColumnType("datetime");

            builder
               .HasMany(cliente => cliente.ClienteContatos)
               .WithOne(cliContato => cliContato.Contato)
               .HasForeignKey(cliContato => new { cliContato.IDCONTATO })
               .HasPrincipalKey(cliente => new { cliente.Id })
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class ContatoEmpresaMapping : IEntityTypeConfiguration<ContatoEmpresa>
    {
        public void Configure(EntityTypeBuilder<ContatoEmpresa> builder)
        {
            builder.ToTable("empresa_contato");

            builder.HasKey(c => new { c.IDCONTATO, c.IDEMPRESA });

            builder.Property(c => c.IDCONTATO).HasColumnName("IDCONTATO").HasColumnType("bigint").IsRequired();
            builder.Property(c => c.IDEMPRESA).HasColumnName("IDEMPRESA").HasColumnType("bigint").IsRequired();

            builder.Ignore(c => c.Id);

            builder
            .HasOne(contatoEmpresa => contatoEmpresa.Contato)
            .WithMany(contato => contato.ContatoEmpresas)
            .HasForeignKey(contatoEmpresa => new { contatoEmpresa.IDCONTATO })
            .HasPrincipalKey(contato => new { contato.Id });

            builder
            .HasOne(contatoEmpresa => contatoEmpresa.Empresa)
            .WithMany(empresa => empresa.ContatoEmpresas)
            .HasForeignKey(contatoEmpresa => new { contatoEmpresa.IDEMPRESA })
            .HasPrincipalKey(empresa => new { empresa.Id });
        }
    }
}

using agilium.api.business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.infra.Mappings
{
    public class EmpresaAuthMapping : IEntityTypeConfiguration<EmpresaAuth>
    {
        public void Configure(EntityTypeBuilder<EmpresaAuth> builder)
        {
            builder.ToTable("emp_auth");

            builder.HasKey(c => new { c.IDEMPRESA, c.IDUSUARIO });


            builder.Property(c => c.IDUSUARIO).HasColumnName("IDUSUARIO").HasColumnType("bigint").IsRequired();
            builder.Property(c => c.IDEMPRESA).HasColumnName("IDEMPRESA").HasColumnType("bigint").IsRequired();

            //campos padrao da entidade que nao existem na tabela
            builder.Ignore(c => c.Id);

            //builder
            //   .HasOne(empresaAuth => empresaAuth.Empresa)
            //   .WithMany(empresa => empresa.EmpresasAuth)
            //   .HasForeignKey(empresaAuth => new { empresaAuth.IDEMPRESA })
            //   .HasPrincipalKey(empresa => new { empresa.Id });

            builder
                .HasOne(empresa => empresa.Usuario)
                .WithMany(usuario => usuario.EmpresasAuth)
                .HasForeignKey(empresa => new { empresa.IDUSUARIO })
                .HasPrincipalKey(usuario => new { usuario.Id });


        }
    }
}

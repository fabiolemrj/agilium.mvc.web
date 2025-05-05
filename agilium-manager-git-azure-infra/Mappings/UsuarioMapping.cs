using agilium.api.business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.infra.Mappings
{
    public class UsuarioMapping : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("ca_usuarios");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("id_usuario").HasColumnType("bigint").IsRequired();

            builder.Property(c => c.nome).HasColumnName("nome").HasColumnType("varchar(100)");
            builder.Property(c => c.cpf).HasColumnName("cpf").HasColumnType("varchar(14)");
            builder.Property(c => c.ender).HasColumnName("ender").HasColumnType("varchar(100)");
            builder.Property(c => c.num).HasColumnName("num").HasColumnType("varchar(20)");
            builder.Property(c => c.compl).HasColumnName("compl").HasColumnType("varchar(35)");
            builder.Property(c => c.bairro).HasColumnName("bairro").HasColumnType("varchar(40)");
            builder.Property(c => c.cep).HasColumnName("cep").HasColumnType("varchar(9)");
            builder.Property(c => c.cidade).HasColumnName("cidade").HasColumnType("varchar(40)");
            builder.Property(c => c.uf).HasColumnName("uf").HasColumnType("varchar(2)");
            builder.Property(c => c.tel1).HasColumnName("tel1").HasColumnType("varchar(20)");
            builder.Property(c => c.cel).HasColumnName("cel").HasColumnType("varchar(20)");
            builder.Property(c => c.dtnasc).HasColumnName("dtnasc").HasColumnType("date");
            builder.Property(c => c.usuario).HasColumnName("usuario").HasColumnType("varchar(20)");
            builder.Property(c => c.senha).HasColumnName("senha").HasColumnType("varchar(40)");
            builder.Property(c => c.email).HasColumnName("email").HasColumnType("varchar(100)");
            builder.Property(c => c.foto).HasColumnName("foto").HasColumnType("varchar(100)");
            builder.Property(c => c.tel2).HasColumnName("tel2").HasColumnType("varchar(20)");
            builder.Property(c => c.ativo).HasColumnName("ativo").HasColumnType("varchar(1)");
            builder.Property(c => c.DataCadastro).HasColumnName("dtcad").HasColumnType("date");
            builder.Property(c => c.id_perfil).HasColumnName("id_perfil").HasColumnType("int");
            builder.Property(c => c.idUserAspNet).HasColumnName("idUserAspNet").HasColumnType("varchar(255)");
            builder.Property(c => c.idPerfil).HasColumnName("idPerfil").HasColumnType("bigint");

            builder
            .HasMany(usuario => usuario.Funcionarios)
            .WithOne(func => func.Usuario)
            .HasForeignKey(func => new { func.IDUSUARIO })
            .HasPrincipalKey(usuario => new { usuario.Id });

            builder
               .HasMany(usuario => usuario.ContaPagar)
               .WithOne(contaPagar => contaPagar.Usuario)
               .HasForeignKey(contaPagar => new { contaPagar.IDUSUARIO })
               .HasPrincipalKey(usuario => new { usuario.Id });

            builder
               .HasMany(usuario => usuario.ContaReceber)
               .WithOne(contaReceber => contaReceber.Usuario)
               .HasForeignKey(contaReceber => new { contaReceber.IDUSUARIO })
               .HasPrincipalKey(usuario => new { usuario.Id });

            builder
               .HasMany(usuario => usuario.TurnoInicial)
               .WithOne(turno => turno.UsuarioInicial)
               .HasForeignKey(turno => new { turno.IDUSUARIOINI })
               .HasPrincipalKey(usuario => new { usuario.Id });

            builder
               .HasMany(usuario => usuario.TurnoFinal)
               .WithOne(turno => turno.UsuarioFinal)
               .HasForeignKey(turno => new { turno.IDUSUARIOFIM })
               .HasPrincipalKey(usuario => new { usuario.Id });

            builder
               .HasMany(usuario => usuario.CaixaMoeda)
               .WithOne(turno => turno.UsuarioCorrecao)
               .HasForeignKey(turno => new { turno.IDUSUARIOCORRECAO })
               .HasPrincipalKey(usuario => new { usuario.Id });

            builder
                  .HasMany(usuario => usuario.Perdas)
                  .WithOne(perda => perda.Usuario)
                  .HasForeignKey(perda => new { perda.IDUSUARIO })
                  .HasPrincipalKey(usuario => new { usuario.Id });

            builder
                 .HasMany(usuario => usuario.InventarioItem)
                 .WithOne(inventarioItem => inventarioItem.UsuarioAnalise)
                 .HasForeignKey(inventarioItem => new { inventarioItem.IDUSUARIOANALISE });
        }
    }
}

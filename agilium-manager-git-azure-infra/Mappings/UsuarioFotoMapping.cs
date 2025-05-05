using agilium.api.business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.infra.Config.Mappings
{
    public class UsuarioFotoMapping : IEntityTypeConfiguration<UsuarioFotoEntity>
    {
        public void Configure(EntityTypeBuilder<UsuarioFotoEntity> builder)
        {
            builder.ToTable("ca_usuarios_fotos");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).HasColumnName("id_usuarioFoto").HasColumnType("bigint").IsRequired();
            builder.Property(c => c.IdUsuario).HasColumnName("id_usuario").HasColumnType("bigint");
            builder.Property(c => c.IdUsuarioAspNet).HasColumnName("IdUsuarioAspNet").HasColumnType("varchar(255)");
            builder.Property(c => c.Imagem).HasColumnName("imagem").HasColumnType("mediumblob");
            builder.Property(c => c.DataCadastro).HasColumnName("dtcad").HasColumnType("date");
            builder.Property(c => c.NomeArquivo).HasColumnName("nomeArquivo").HasColumnType("varchar(255)");
        }
    }
}

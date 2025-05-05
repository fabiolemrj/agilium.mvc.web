using agilium.api.business.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.infra.Mappings
{
    public class CaModeloMapping : IEntityTypeConfiguration<CaModelo>
    {
        public void Configure(EntityTypeBuilder<CaModelo> builder)
        {
            builder.HasKey(p => p.Id);
            builder.ToTable("camodelo");

            builder.Property(c => c.Id).IsRequired().HasColumnName("idModelo").HasColumnType("bigint").IsRequired();
            builder.Property(c => c.idPerfil).HasColumnName("idPerfil").HasColumnType("bigint");
            builder.Property(c => c.idPermissao).HasColumnName("idPermissao").HasColumnType("bigint");
            builder.Property(c => c.Incluir).HasColumnName("inc").HasColumnType("varchar(1)");
            builder.Property(c => c.Alterar).HasColumnName("alt").HasColumnType("varchar(1)");
            builder.Property(c => c.Excluir).HasColumnName("exc").HasColumnType("varchar(1)");
            builder.Property(c => c.Relatorio).HasColumnName("rel").HasColumnType("varchar(1)");
            builder.Property(c => c.Consulta).HasColumnName("con").HasColumnType("varchar(1)");

           // builder.HasAlternateKey(c => new { c.idPerfil, c.idPermissao });
            // builder.HasOne(x => x.CaPerfil).WithMany(x => x.Modelos);
            // builder.HasOne(x => x.CaPermissaoItem).WithMany(x => x.Modelos);
        }
    }

    public class CaPerfilMapping : IEntityTypeConfiguration<CaPerfil>
    {
        public void Configure(EntityTypeBuilder<CaPerfil> builder)
        {
            builder.HasKey(p => p.Id);
            builder.ToTable("caperfil");

            builder.Property(c => c.Id).IsRequired().HasColumnName("idperfil").HasColumnType("bigint").IsRequired();
            builder.Property(c => c.Descricao).HasColumnName("descricao").HasColumnType("varchar(20)");
            builder.Property(c => c.Situacao).HasColumnName("situacao").HasColumnType("varchar(1)");
            builder.Property(c => c.idEmpresa).IsRequired().HasColumnName("idempresa").HasColumnType("bigint");

            builder.HasMany(p => p.Modelos)
                .WithOne(m => m.CaPerfil)
                .HasForeignKey(m => m.idPerfil)
                .HasPrincipalKey(p => p.Id);
        }
    }

    public class CaPermissaoItemMapping : IEntityTypeConfiguration<CaPermissaoItem>
    {
        public void Configure(EntityTypeBuilder<CaPermissaoItem> builder)
        {
            builder.HasKey(p => p.Id);
            builder.ToTable("capermissaoitem");

            builder.Property(c => c.Id).IsRequired().HasColumnName("idpermissao").HasColumnType("bigint").IsRequired();
            builder.Property(c => c.Descricao).HasColumnName("descricao").HasColumnType("varchar(20)");
            builder.Property(c => c.Situacao).HasColumnName("situacao").HasColumnType("varchar(1)");

            builder.HasMany(p => p.Modelos)
                .WithOne(m => m.CaPermissaoItem)
                .HasForeignKey(m => m.idPermissao)
                .HasPrincipalKey(p => p.Id);
        }
    }

    #region CaManager

    public class CaPerfilManagerMapping : IEntityTypeConfiguration<CaPerfiManager>
    {
        public void Configure(EntityTypeBuilder<CaPerfiManager> builder)
        {
            builder.HasKey(p => p.IdPerfil);
            builder.ToTable("ca_perfis");

            builder.Property(c => c.IdPerfil).IsRequired().HasColumnName("id_perfil").HasColumnType("int").IsRequired();
            builder.Property(c => c.Descricao).HasColumnName("descricao").HasColumnType("varchar(50)");

            builder.Ignore(c => c.Id);
        }
    }

    public class CaPermissaoManagerMapping : IEntityTypeConfiguration<CaPermissaoManager>
    {
        public void Configure(EntityTypeBuilder<CaPermissaoManager> builder)
        {
            builder.ToTable("ca_permissoes");
              builder.HasKey(p => new { p.IdPerfil,p.IdArea});

            builder.Property(c => c.IdPerfil).IsRequired().HasColumnName("id_perfil").HasColumnType("int");
            builder.Property(c => c.IdArea).HasColumnName("id_area").HasColumnType("int").IsRequired();
    
            builder.Ignore(c => c.Id);

            builder.HasOne(p => p.CaPerfiManager)
                .WithMany(m => m.CaPermissaoManagers)
                .HasForeignKey(m => m.IdPerfil)
                .HasPrincipalKey(p => p.IdPerfil);

            builder.HasOne(p => p.CaAreaManager)
                .WithMany(m => m.CaPermissaoManagers)
                .HasForeignKey(m => m.IdArea)
                .HasPrincipalKey(p => p.IdArea);
        }
    }

    public class CaAreaManagerMapping : IEntityTypeConfiguration<CaAreaManager>
    {
        public void Configure(EntityTypeBuilder<CaAreaManager> builder)
        {
            builder.HasKey(p => p.IdArea);
            builder.ToTable("ca_areas");

            builder.Property(c => c.IdArea).IsRequired().HasColumnName("id_area").HasColumnType("int").IsRequired();
            builder.Property(c => c.titulo).HasColumnName("titulo").HasColumnType("varchar(50)");
            builder.Property(c => c.hierarquia).IsRequired().HasColumnName("hierarquia").HasColumnType("int");
            builder.Property(c => c.ordem).IsRequired().HasColumnName("ordem").HasColumnType("int");
            builder.Property(c => c.subitem).IsRequired().HasColumnName("subitem").HasColumnType("int");
            builder.Property(c => c.idtag).IsRequired().HasColumnName("idtag").HasColumnType("int");
            
            builder.Ignore(c => c.Id);

        }
    }
    #endregion
}

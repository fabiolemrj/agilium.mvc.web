using agilium.api.business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.infra.Mappings
{
    public class EstoqueMapping : IEntityTypeConfiguration<Estoque>
    {
        public void Configure(EntityTypeBuilder<Estoque> builder)
        {
            builder.ToTable("estoque");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("IDESTOQUE").HasColumnType("bigint").IsRequired();
            builder.Property(c => c.idEmpresa).HasColumnName("IDEMPRESA").HasColumnType("bigint");
            builder.Property(c => c.Descricao).HasColumnName("DSESTOQUE").HasColumnType("varchar(50)");
            builder.Property(x => x.Tipo).HasColumnType("int").HasColumnName("TPESTOQUE");
            builder.Property(c => c.Capacidade).HasColumnName("NUCAPACIDADE").HasColumnType("double");
            builder.Property(x => x.STESTOQUE).HasColumnType("int").HasColumnName("STESTOQUE");

            //chaves estrangeiras
            builder
                .HasMany(empresa => empresa.PontosVendas)
               .WithOne(func => func.Estoque)
               .HasForeignKey(func => new { func.IDESTOQUE})
               .HasPrincipalKey(empresa => new { empresa.Id });

            builder
              .HasMany(estoque => estoque.EstoqueProdutos)
              .WithOne(produto => produto.Estoque)
              .HasForeignKey(produto => new { produto.IDESTOQUE })
              .HasPrincipalKey(estoque => new { estoque.Id });

            builder
              .HasMany(estoque => estoque.EstoqueHistoricos)
              .WithOne(historico => historico.Estoque)
              .HasForeignKey(historico => new { historico.IDESTOQUE })
              .HasPrincipalKey(estoque => new { estoque.Id });

            builder
              .HasMany(estoque => estoque.Perdas)
              .WithOne(perda => perda.Estoque)
              .HasForeignKey(perda => new { perda.IDESTOQUE })
              .HasPrincipalKey(estoque => new { estoque.Id });

            builder
              .HasMany(estoque => estoque.CompraItems)
              .WithOne(compraItem => compraItem.Estoque)
              .HasForeignKey(compraItem => new { compraItem.IDESTOQUE })
              .HasPrincipalKey(estoque => new { estoque.Id });

            builder
                 .HasMany(estoque => estoque.Inventarios)
                 .WithOne(inventario => inventario.Estoque)
                 .HasForeignKey(inventario => new { inventario.IDESTOQUE })
                 .HasPrincipalKey(estoque => new { estoque.Id });
        }
    }

    public class EstoqueProdutoMapping : IEntityTypeConfiguration<EstoqueProduto>
    {
        public void Configure(EntityTypeBuilder<EstoqueProduto> builder)
        {
            builder.ToTable("estoque_prod");
            builder.HasKey(c => c.Id);
            builder.HasAlternateKey(c => new { c.IDESTOQUE, c.IDPRODUTO });

            builder.Property(c => c.Id).HasColumnName("IDESTOQUE_PROD").HasColumnType("bigint").IsRequired();
            builder.Property(c => c.IDPRODUTO).HasColumnName("IDPRODUTO").HasColumnType("bigint");
            builder.Property(c => c.IDESTOQUE).HasColumnName("IDESTOQUE").HasColumnType("bigint");
            builder.Property(c => c.NUQTD).HasColumnName("NUQTD").HasColumnType("double");

        }
    }

    public class EstoqueHistoricoMapping : IEntityTypeConfiguration<EstoqueHistorico>
    {
        public void Configure(EntityTypeBuilder<EstoqueHistorico> builder)
        {
            builder.ToTable("estoquehst");
            builder.HasKey(c => c.Id);
         
            builder.Property(c => c.Id).HasColumnName("IDESTOQUEHST").HasColumnType("bigint").IsRequired();
            builder.Property(c => c.IDESTOQUE).HasColumnName("IDESTOQUE").HasColumnType("bigint");
            builder.Property(c => c.IDPRODUTO).HasColumnName("IDPRODUTO").HasColumnType("bigint");
            builder.Property(c => c.IDITEM).HasColumnName("IDITEM").HasColumnType("bigint");
            builder.Property(c => c.IDLANC).HasColumnName("IDLANC").HasColumnType("bigint");
            builder.Property(c => c.DTHRHST).HasColumnName("DTHRHST").HasColumnType("datetime");
            builder.Property(c => c.NMUSUARIO).HasColumnName("NMUSUARIO").HasColumnType("varchar(50)");
            builder.Property(c => c.TPHST).HasColumnName("TPHST").HasColumnType("int");
            builder.Property(c => c.DSHST).HasColumnName("DSHST").HasColumnType("varchar(250)");
            builder.Property(c => c.QTDHST).HasColumnName("QTDHST").HasColumnType(" double");

            builder
                .HasMany(estoqueHst => estoqueHst.Perdas)
                .WithOne(perda => perda.EstoqueHistorico)
                .HasForeignKey(perda => new { perda.IDESTOQUEHST })
                .HasPrincipalKey(estoqueHst => new { estoqueHst.Id });
            //.OnDelete(DeleteBehavior.Cascade);

            //builder
            //    .HasMany(estoque => estoque.CompraItems)
            //    .WithOne(compraItem => compraItem.Estoque)
            //    .HasForeignKey(compraItem => new { compraItem.IDESTOQUE })
            //    .HasPrincipalKey(estoque => new { estoque.Id })

        }
    }
}

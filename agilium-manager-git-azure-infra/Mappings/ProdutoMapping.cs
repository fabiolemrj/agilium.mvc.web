using agilium.api.business.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.infra.Mappings
{
    public class ProdutoMapping : IEntityTypeConfiguration<Produto>
    {
        public void Configure(EntityTypeBuilder<Produto> builder)
        {
            builder.ToTable("produto");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("IDPRODUTO").HasColumnType("bigint").IsRequired();
            builder.Property(c => c.idEmpresa).HasColumnName("IDEMPRESA").HasColumnType("bigint");

            builder.Property(c => c.IDGRUPO).HasColumnName("IDGRUPO").HasColumnType("bigint");
            builder.Property(c => c.CDPRODUTO).HasColumnName("CDPRODUTO").HasColumnType("varchar(6)");
            builder.Property(c => c.NMPRODUTO).HasColumnName("NMPRODUTO").HasColumnType("varchar(50)");
            builder.Property(c => c.CTPRODUTO).HasColumnName("CTPRODUTO").HasColumnType("varchar(1)");
            builder.Property(c => c.TPPRODUTO).HasColumnName("TPPRODUTO").HasColumnType("int");
            builder.Property(c => c.UNCOMPRA).HasColumnName("UNCOMPRA").HasColumnType("varchar(5)");
            builder.Property(c => c.UNVENDA).HasColumnName("UNVENDA").HasColumnType("varchar(5)");
            builder.Property(c => c.NURELACAO).HasColumnName("NURELACAO").HasColumnType("int");
            builder.Property(c => c.NUPRECO).HasColumnName("NUPRECO").HasColumnType("double");
            builder.Property(c => c.NUQTDMIN).HasColumnName("NUQTDMIN").HasColumnType("double");
            builder.Property(c => c.CDSEFAZ).HasColumnName("CDSEFAZ").HasColumnType("varchar(20)");
            builder.Property(c => c.CDANP).HasColumnName("CDANP").HasColumnType("varchar(20)");
            builder.Property(c => c.CDNCM).HasColumnName("CDNCM").HasColumnType("varchar(20)");
            builder.Property(c => c.CDCEST).HasColumnName("CDCEST").HasColumnType("varchar(20)");
            builder.Property(c => c.CDSERV).HasColumnName("CDSERV").HasColumnType("varchar(20)");
            builder.Property(c => c.STPRODUTO).HasColumnName("STPRODUTO").HasColumnType("int");
            builder.Property(c => c.VLULTIMACOMPRA).HasColumnName("VLULTIMACOMPRA").HasColumnType("double");
            builder.Property(c => c.VLCUSTOMEDIO).HasColumnName("VLCUSTOMEDIO").HasColumnType("double");
            builder.Property(c => c.PCIBPTFED).HasColumnName("PCIBPTFED").HasColumnType("double");
            builder.Property(c => c.PCIBPTEST).HasColumnName("PCIBPTEST").HasColumnType("double");
            builder.Property(c => c.PCIBPTMUN).HasColumnName("PCIBPTMUN").HasColumnType("double");
            builder.Property(c => c.PCIBPTIMP).HasColumnName("PCIBPTIMP").HasColumnType("double");
            builder.Property(c => c.NUCFOP).HasColumnName("NUCFOP").HasColumnType("int");
            builder.Property(c => c.STORIGEMPROD).HasColumnName("STORIGEMPROD").HasColumnType("int");
            builder.Property(c => c.DSCOFINS_CST).HasColumnName("DSICMS_CST").HasColumnType("varchar(5)");
            builder.Property(c => c.PCICMS_ALIQ).HasColumnName("PCICMS_ALIQ").HasColumnType("double");
            builder.Property(c => c.PCICMS_REDUCBC).HasColumnName("PCICMS_REDUCBC").HasColumnType("double");
            builder.Property(c => c.PCICMSST_ALIQ).HasColumnName("PCICMSST_ALIQ").HasColumnType("double");
            builder.Property(c => c.PCICMSST_MVA).HasColumnName("PCICMSST_MVA").HasColumnType("double");
            builder.Property(c => c.PCICMSST_REDUCBC).HasColumnName("PCICMSST_REDUCBC").HasColumnType("double");
            builder.Property(c => c.DSIPI_CST).HasColumnName("DSIPI_CST").HasColumnType("varchar(5)");
            builder.Property(c => c.PCIPI_ALIQ).HasColumnName("PCIPI_ALIQ").HasColumnType("double");
            builder.Property(c => c.DSPIS_CST).HasColumnName("DSPIS_CST").HasColumnType("varchar(5)");
            builder.Property(c => c.PCPIS_ALIQ).HasColumnName("PCPIS_ALIQ").HasColumnType("double");
            builder.Property(c => c.DSCOFINS_CST).HasColumnName("DSCOFINS_CST").HasColumnType("varchar(5)");
            builder.Property(c => c.PCCOFINS_ALIQ).HasColumnName("PCCOFINS_ALIQ").HasColumnType("double");
            builder.Property(c => c.STESTOQUE).HasColumnName("STESTOQUE").HasColumnType("int");
            builder.Property(c => c.STBALANCA).HasColumnName("STBALANCA").HasColumnType("int");
            builder.Property(c => c.IDDEP).HasColumnName("IDDEP").HasColumnType("bigint");
            builder.Property(c => c.IDMARCA).HasColumnName("IDMARCA").HasColumnType("bigint");
            builder.Property(c => c.IDSUBGRUPO).HasColumnName("IDSUBGRUPO").HasColumnType("bigint");
            builder.Property(c => c.DSVOLUME).HasColumnName("DSVOLUME").HasColumnType("varchar(20)");

            builder.Ignore(c => c.FLG_IFOOD);

            //chaves estrangeiras
            builder
                .HasMany(produto => produto.ProdutoCodigoBarras)
                .WithOne(prodCodBarra => prodCodBarra.Produto)
                .HasForeignKey(prodCodBarra => new { prodCodBarra.IDPRODUTO })
                .HasPrincipalKey(produto => new { produto.Id });

            builder
               .HasMany(produto => produto.ProdutoPrecos)
               .WithOne(prodPreco => prodPreco.Produto)
               .HasForeignKey(prodPreco => new { prodPreco.idProduto })
               .HasPrincipalKey(produto => new { produto.Id });

            builder
              .HasOne(produto => produto.ProdutoDepartamento)
              .WithMany(prodDep => prodDep.Produtos)
              .HasForeignKey(prod => new { prod.IDDEP })
              .HasPrincipalKey(prodDep => new { prodDep.Id });

            builder
              .HasOne(produto => produto.ProdutoMarca)
              .WithMany(prodDep => prodDep.Produtos)
              .HasForeignKey(prod => new { prod.IDMARCA })
              .HasPrincipalKey(prodDep => new { prodDep.Id });

            builder
              .HasOne(produto => produto.SubGrupoProduto)
              .WithMany(prodDep => prodDep.Produtos)
              .HasForeignKey(prod => new { prod.IDSUBGRUPO })
              .HasPrincipalKey(prodDep => new { prodDep.Id });

            builder
              .HasOne(produto => produto.GrupoProduto)
              .WithMany(prodDep => prodDep.Produtos)
              .HasForeignKey(prod => new { prod.IDGRUPO })
              .HasPrincipalKey(prodDep => new { prodDep.Id });


            builder
               .HasMany(produto => produto.ProdutosComposicoes)
               .WithOne(prodComposicao => prodComposicao.Produto)
               .HasForeignKey(prodComposicao => new { prodComposicao.idProduto })
               .HasPrincipalKey(produto => new { produto.Id });

            builder
               .HasMany(produto => produto.ProdutoSiteMercado)
               .WithOne(prodSiteMerc => prodSiteMerc.Produto)
               .HasForeignKey(prodSiteMerc => new { prodSiteMerc.IDPRODUTO })
               .HasPrincipalKey(produto => new { produto.Id });

            builder
               .HasMany(produto => produto.ProdutoFotos)
               .WithOne(prodFoto => prodFoto.Produto)
               .HasForeignKey(prodFoto => new { prodFoto.idProduto })
               .HasPrincipalKey(produto => new { produto.Id });
           
            builder
              .HasMany(prod => prod.EstoqueProdutos)
              .WithOne(estoque => estoque.Produto)
              .HasForeignKey(estoque => new { estoque.IDPRODUTO })
              .HasPrincipalKey(prod => new { prod.Id });

            builder
              .HasMany(produto => produto.EstoqueHistoricos)
              .WithOne(historico => historico.Produto)
              .HasForeignKey(historico => new { historico.IDPRODUTO })
              .HasPrincipalKey(produto => new { produto.Id });

            builder
                .HasMany(cliente => cliente.ClientePrecos)
                .WithOne(clipreco => clipreco.Produto)
                .HasForeignKey(clipreco => new { clipreco.IDPRODUTO })
                .HasPrincipalKey(produto => new { produto.Id });

            builder
                .HasMany(produto => produto.TurnoPrecos)
                .WithOne(turnoPreco => turnoPreco.Produto)
                .HasForeignKey(turnoPreco => new { turnoPreco.IDPRODUTO })
                .HasPrincipalKey(produto => new { produto.Id });

            builder
                .HasMany(produto => produto.CompraItems)
                .WithOne(compraItem => compraItem.Produto)
                .HasForeignKey(compraItem => new { compraItem.IDPRODUTO })
                .HasPrincipalKey(produto => new { produto.Id });
            builder
                .HasMany(produto => produto.Perdas)
                .WithOne(perda => perda.Produto)
                .HasForeignKey(perda => new { perda.IDPRODUTO })
                .HasPrincipalKey(produto => new { produto.Id });

            builder
              .HasMany(produto => produto.VendaItem)
              .WithOne(item => item.Produto)
              .HasForeignKey(item => new { item.IDPRODUTO })
              .HasPrincipalKey(produto => new { produto.Id });

            builder
        .HasMany(produto => produto.VendaTemporariaItem).WithOne(itemtemp => itemtemp.Produto)
        .HasForeignKey(itemtemp => new { itemtemp.IDPRODUTO});

            builder
                .HasMany(produto => produto.InventarioItem)
                .WithOne(inventarioItem => inventarioItem.Produto)
                .HasForeignKey(inventarioItem => new { inventarioItem.IDPRODUTO })
                .HasPrincipalKey(produto => new { produto.Id });
        }

    }

    public class ProdutoDepartamentoMapping : IEntityTypeConfiguration<ProdutoDepartamento>
    {
        public void Configure(EntityTypeBuilder<ProdutoDepartamento> builder)
        {
            builder.ToTable("prod_departamento");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("IDDEP").HasColumnType("bigint").IsRequired();
            builder.Property(c => c.idEmpresa).HasColumnName("IDEMPRESA").HasColumnType("bigint");
            builder.Property(c => c.CDDEP).HasColumnName("CDDEP").HasColumnType("varchar(6)");
            builder.Property(c => c.NMDEP).HasColumnName("NMDEP").HasColumnType("varchar(50)");
            builder.Property(c => c.STDEP).HasColumnName("STDEP").HasColumnType("int");

            //chaves estrangeiras
            builder
                .HasOne(prodDep => prodDep.Empresa)
                .WithMany(empresa => empresa.ProdutosDepartamentos)
                .HasForeignKey(prodDep => new { prodDep.idEmpresa })
                .HasPrincipalKey(empresa => new { empresa.Id });

          
        }
    }

    public class ProdutoMarcaMapping : IEntityTypeConfiguration<ProdutoMarca>
    {
        public void Configure(EntityTypeBuilder<ProdutoMarca> builder)
        {
            builder.ToTable("prod_marca");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("IDMARCA").HasColumnType("bigint").IsRequired();
            builder.Property(c => c.idEmpresa).HasColumnName("IDEMPRESA").HasColumnType("bigint");
            builder.Property(c => c.CDMARCA).HasColumnName("CDMARCA").HasColumnType("varchar(6)");
            builder.Property(c => c.NMMARCA).HasColumnName("NMMARCA").HasColumnType("varchar(30)");
            builder.Property(c => c.STMARCA).HasColumnName("STMARCA").HasColumnType("int");

            //chaves estrangeiras
            builder
                .HasOne(prodDep => prodDep.Empresa)
                .WithMany(empresa => empresa.ProdutosMarcas)
                .HasForeignKey(prodDep => new { prodDep.idEmpresa })
                .HasPrincipalKey(empresa => new { empresa.Id });
        }
    }

    public class GrupoProdutoMapping : IEntityTypeConfiguration<GrupoProduto>
    {
        public void Configure(EntityTypeBuilder<GrupoProduto> builder)
        {
            builder.ToTable("prod_grupo");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("IDGRUPO").HasColumnType("bigint").IsRequired();
            builder.Property(c => c.idEmpresa).HasColumnName("IDEMPRESA").HasColumnType("bigint").IsRequired();
            builder.Property(c => c.Nome).HasColumnName("NMGRUPO").HasColumnType("varchar(50)");
            builder.Property(c => c.CDGRUPO).HasColumnName("CDGRUPO").HasColumnType("varchar(6)");
            builder.Property(x => x.StAtivo).HasColumnType("int").HasColumnName("STATIVO");

            //chaves estrangeiras
            builder
                .HasMany(grupo => grupo.SubGrupoProdutos)
                .WithOne(subGrupo => subGrupo.GrupoProduto)
                .HasForeignKey(subGrupo => new { subGrupo.IDGRUPO })
                .HasPrincipalKey(grupo => new { grupo.Id });

            builder
      .HasOne(grupo => grupo.Empresa)
      .WithMany(empresa => empresa.GrupoProduto)
      .HasForeignKey(grupo => new { grupo.idEmpresa })
      .HasPrincipalKey(empresa => new { empresa.Id });
        }
    }

    public class SubGrupoProdutoMapping : IEntityTypeConfiguration<SubGrupoProduto>
    {
        public void Configure(EntityTypeBuilder<SubGrupoProduto> builder)
        {
            builder.ToTable("prod_subgrupo");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("IDSUBGRUPO").HasColumnType("bigint").IsRequired();
            builder.Property(c => c.IDGRUPO).HasColumnName("IDGRUPO").HasColumnType("bigint").IsRequired();
            builder.Property(c => c.NMSUBGRUPO).HasColumnName("NMSUBGRUPO").HasColumnType("varchar(50)");
            builder.Property(x => x.STATIVO).HasColumnType("int").HasColumnName("STATIVO");
                    
        }
    }

    public class ProdutoComposicaoMapping : IEntityTypeConfiguration<ProdutoComposicao>
    {
        public void Configure(EntityTypeBuilder<ProdutoComposicao> builder)
        {
            builder.ToTable("prod_composicao");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("IDCOMPOSICAO").HasColumnType("bigint").IsRequired();
            builder.Property(c => c.idProduto).HasColumnName("IDPRODUTO").HasColumnType("bigint");
            builder.Property(c => c.idProdutoComposicao).HasColumnName("IDPRODUTO_COMP").HasColumnType("bigint");
            builder.Property(c => c.idEstoque).HasColumnName("IDESTOQUE").HasColumnType("bigint");
            builder.Property(c => c.Quantidade).HasColumnName("NUQTD").HasColumnType("double");
            builder.Property(c => c.Preco).HasColumnName("NUPRECO").HasColumnType("double");

        }
    }

    public class ProdutoCodigoBarraMapping : IEntityTypeConfiguration<ProdutoCodigoBarra>
    {
        public void Configure(EntityTypeBuilder<ProdutoCodigoBarra> builder)
        {
            builder.ToTable("prod_barra");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("IDPROD_BARRA").HasColumnType("bigint").IsRequired();
            builder.Property(c => c.IDPRODUTO).HasColumnName("IDPRODUTO").HasColumnType("bigint");
            builder.Property(c => c.CDBARRA).HasColumnName("CDBARRA").HasColumnType("varchar(50)");

        }
    }

    public class ProdutoPrecoMapping : IEntityTypeConfiguration<ProdutoPreco>
    {
        public void Configure(EntityTypeBuilder<ProdutoPreco> builder)
        {
            builder.ToTable("prod_preco");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("IDPROD_PRECO").HasColumnType("bigint").IsRequired();
            builder.Property(c => c.idProduto).HasColumnName("IDPRODUTO").HasColumnType("bigint");
            builder.Property(c => c.Usuario).HasColumnName("USUARIO").HasColumnType("varchar(50)");
            builder.Property(c => c.Preco).HasColumnName("NUPRECO_NEW").HasColumnType("double");
            builder.Property(c => c.PrecoAnterior).HasColumnName("NUPRECO_OLD").HasColumnType("double");
            builder.Property(c => c.DataPreco).HasColumnName("DTPROD_PRECO").HasColumnType("datetime");


        }
    }

    public class ProdutoFotoMapping : IEntityTypeConfiguration<ProdutoFoto>
    {
        public void Configure(EntityTypeBuilder<ProdutoFoto> builder)
        {
            builder.ToTable("prod_foto");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("IDPROD_FOTO").HasColumnType("bigint").IsRequired();
            builder.Property(c => c.idProduto).HasColumnName("IDPRODUTO").HasColumnType("bigint");
            builder.Property(c => c.Descricao).HasColumnName("DSFOTO").HasColumnType("mediumtext");
            builder.Property(c => c.Foto).HasColumnName("FOTO").HasColumnType("mediumblob");
            builder.Property(c => c.Data).HasColumnName("DTHRCADFOTO").HasColumnType("datetime");

        }
    }

}

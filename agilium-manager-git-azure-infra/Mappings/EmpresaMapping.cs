using agilium.api.business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.infra.Mappings
{
    public class EmpresaMapping : IEntityTypeConfiguration<Empresa>
    {
        public void Configure(EntityTypeBuilder<Empresa> builder)
        {
            builder.ToTable("empresa");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("IDEMPRESA").HasColumnType("bigint").IsRequired();
            builder.Property(c => c.IDENDERECO).HasColumnName("IDENDERECO").HasColumnType("bigint").IsRequired();
            builder.Property(c => c.CDEMPRESA).HasColumnName("CDEMPRESA").HasColumnType("varchar(6)");
            builder.Property(c => c.NMRZSOCIAL).HasColumnName("NMRZSOCIAL").HasColumnType("varchar(70)");
            builder.Property(c => c.NMFANTASIA).HasColumnName("NMFANTASIA").HasColumnType("varchar(70)");
            builder.Property(c => c.DSINSCREST).HasColumnName("DSINSCREST").HasColumnType("varchar(20)");
            builder.Property(c => c.DSINSCRESTVINC).HasColumnName("DSINSCRESTVINC").HasColumnType("varchar(20)");
            builder.Property(c => c.DSINSCRMUN).HasColumnName("DSINSCRMUN").HasColumnType("varchar(20)");
            builder.Property(c => c.NMDISTRIBUIDORA).HasColumnName("NMDISTRIBUIDORA").HasColumnType("varchar(50)");
            builder.Property(c => c.NUREGJUNTACOM).HasColumnName("NUREGJUNTACOM").HasColumnType("varchar(20)");
            builder.Property(c => c.NUCAPARM).HasColumnName("NUCAPARM").HasColumnType("decimal(10,3)");
            builder.Property(c => c.STMICROEMPRESA).HasColumnName("STMICROEMPRESA").HasColumnType("int");
            builder.Property(c => c.STLUCROPRESUMIDO).HasColumnName("STLUCROPRESUMIDO").HasColumnType("int");
            builder.Property(c => c.TPEMPRESA).HasColumnName("TPEMPRESA").HasColumnType("int");
            builder.Property(c => c.CRT).HasColumnName("CRT").HasColumnType("int");
            builder.Property(c => c.IDCSC).HasColumnName("IDCSC").HasColumnType("varchar(10)");
            builder.Property(c => c.CSC).HasColumnName("CSC").HasColumnType("varchar(40)");
            builder.Property(c => c.NUCNAE).HasColumnName("NUCNAE").HasColumnType("varchar(10)");
            builder.Property(c => c.IDLOJA_SITEMARCADO).HasColumnName("IDLOJA_SITEMERCADO").HasColumnType("varchar(20)");
            builder.Property(c => c.CLIENTID_SITEMERCADO).HasColumnName("CLIENTID_SITEMERCADO").HasColumnType("varchar(20)");
            builder.Property(c => c.CLIENTSECRET_SITEMERCADO).HasColumnName("CLIENTSECRET_SITEMERCADO").HasColumnType("varchar(20)");
            builder.Property(c => c.CSC_HOMOL).HasColumnName("CSC_HOMOL").HasColumnType("varchar(40)");


            builder
            .HasOne(empresa => empresa.Endereco)
            .WithMany(endereco => endereco.Empresas)
            .HasForeignKey(empresa => new { empresa.IDENDERECO })
            .HasPrincipalKey(endereco => new { endereco.Id });

            builder
               .HasMany(empresa => empresa.Configuracoes)
               .WithOne(endereco => endereco.Empresa)
               .HasForeignKey(empresa => new { empresa.IDEMPRESA })
               .HasPrincipalKey(endereco => new { endereco.Id });

            builder
               .HasMany(empresa => empresa.EmpresasAuth)
               .WithOne(auth => auth.Empresa)
               .HasForeignKey(auth => new { auth.IDEMPRESA })
               .HasPrincipalKey(empresa => new { empresa.Id });

            builder
                 .HasMany(empresa => empresa.ConfigImagem)
                .WithOne(configImg => configImg.Empresa)
                .HasForeignKey(configImg => new { configImg.IDEMPRESA })
                .HasPrincipalKey(empresa => new { empresa.Id });

            builder
                 .HasMany(empresa => empresa.Perfil)
                .WithOne(perfil => perfil.Empresa)
                .HasForeignKey(perfil => new { perfil.idEmpresa })
                .HasPrincipalKey(empresa => new { empresa.Id });

            builder
                 .HasMany(empresa => empresa.Estoques)
                .WithOne(estoque => estoque.Empresa)
                .HasForeignKey(estoque => new { estoque.idEmpresa })
                .HasPrincipalKey(empresa => new { empresa.Id });

            builder
                 .HasMany(empresa => empresa.Funcionarios)
                .WithOne(func => func.Empresa)
                .HasForeignKey(func => new { func.IDEMPRESA })
                .HasPrincipalKey(empresa => new { empresa.Id });


            builder
                 .HasMany(empresa => empresa.Moedas)
                .WithOne(func => func.Empresa)
                .HasForeignKey(func => new { func.IDEMPRESA })
                .HasPrincipalKey(empresa => new { empresa.Id });

            builder
                .HasMany(empresa => empresa.PontosVendas)
               .WithOne(func => func.Empresa)
               .HasForeignKey(func => new { func.IDEMPRESA })
               .HasPrincipalKey(empresa => new { empresa.Id });

            builder
                .HasMany(empresa => empresa.Produtos)
               .WithOne(func => func.Empresa)
               .HasForeignKey(func => new { func.idEmpresa })
               .HasPrincipalKey(empresa => new { empresa.Id });

            builder
             .HasMany(empresa => empresa.PlanoContas)
             .WithOne(turno => turno.Empresa)
             .HasForeignKey(turno => new { turno.IDEMPRESA })
             .HasPrincipalKey(empresa => new { empresa.Id });

            builder
                .HasMany(empresa => empresa.ContaPagar)
                .WithOne(contaPagar => contaPagar.Empresa)
                .HasForeignKey(contaPagar => new { contaPagar.IDEMPRESA })
                .HasPrincipalKey(empresa => new { empresa.Id });

            builder
              .HasMany(empresa => empresa.ContaReceber)
              .WithOne(contaReceber => contaReceber.Empresa)
              .HasForeignKey(contaReceber => new { contaReceber.IDEMPRESA })
              .HasPrincipalKey(empresa => new { empresa.Id });

            builder
             .HasMany(empresa => empresa.NotaFiscalInutil)
             .WithOne(licenca => licenca.Empresa)
             .HasForeignKey(licenca => new { licenca.IDEMPRESA })
             .HasPrincipalKey(empresa => new { empresa.Id });

            builder
                .HasMany(empresa => empresa.Turnos)
                .WithOne(turno => turno.Empresa)
                .HasForeignKey(turno => new { turno.IDEMPRESA })
                .HasPrincipalKey(empresa => new { empresa.Id });

            builder
               .HasMany(empresa => empresa.Caixas)
               .WithOne(caixa => caixa.Empresa)
               .HasForeignKey(caixa => new { caixa.IDEMPRESA })
               .HasPrincipalKey(empresa => new { empresa.Id });

            builder
             .HasMany(empresa => empresa.Vales)
             .WithOne(vale => vale.Empresa)
             .HasForeignKey(vale => new { vale.IDEMPRESA })
             .HasPrincipalKey(empresa => new { empresa.Id });

            builder
            .HasMany(empresa => empresa.Perdas)
            .WithOne(perda => perda.Empresa)
            .HasForeignKey(perda => new { perda.IDEMPRESA })
            .HasPrincipalKey(empresa => new { empresa.Id });

            builder
           .HasMany(empresa => empresa.Devolucao)
           .WithOne(devolucao => devolucao.Empresa)
           .HasForeignKey(devolucao => new { devolucao.IDEMPRESA })
           .HasPrincipalKey(empresa => new { empresa.Id });

            builder
            .HasMany(empresa => empresa.Compras)
            .WithOne(compra => compra.Empresa)
            .HasForeignKey(compra => new { compra.IDEMPRESA })
            .HasPrincipalKey(empresa => new { empresa.Id });

            builder
               .HasMany(empresa => empresa.Inventarios)
               .WithOne(inventario => inventario.Empresa)
               .HasForeignKey(inventario => new { inventario.IDEMPRESA })
               .HasPrincipalKey(empresa => new { empresa.Id });


            builder
              .HasMany(empresa => empresa.ProdutoSiteMercado)
             .WithOne(produtoSiteMercado => produtoSiteMercado.Empresa)
             .HasForeignKey(produtoSiteMercado => new { produtoSiteMercado.IDEMPRESA })
             .HasPrincipalKey(empresa => new { empresa.Id });

            builder
              .HasMany(empresa => empresa.MoedasSiteMercados)
             .WithOne(moedaSiteMerc => moedaSiteMerc.Empresa)
             .HasForeignKey(moedaSiteMerc => new { moedaSiteMerc.IDEMPRESA })
             .HasPrincipalKey(empresa => new { empresa.Id });
        }
    }
}

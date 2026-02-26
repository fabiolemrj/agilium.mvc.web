using agilium.api.business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace agilium.api.infra.Mappings
{
    public class EmpresaMapping : IEntityTypeConfiguration<Empresa>
    {
        public void Configure(EntityTypeBuilder<Empresa> builder)
        {
            builder.ToTable("empresa");
            builder.HasKey(c => c.Id);

            // ===================== PROPRIEDADES =====================
            builder.Property(c => c.Id)
                   .HasColumnName("IDEMPRESA")
                   .HasColumnType("bigint")
                   .IsRequired();

            builder.Property(c => c.IDENDERECO)
                   .HasColumnName("IDENDERECO")
                   .HasColumnType("bigint")
                   .IsRequired();

            builder.Property(c => c.CDEMPRESA)
                   .HasColumnName("CDEMPRESA")
                   .HasColumnType("varchar(6)");

            builder.Property(c => c.NMRZSOCIAL)
                   .HasColumnName("NMRZSOCIAL")
                   .HasColumnType("varchar(70)");

            builder.Property(c => c.NMFANTASIA)
                   .HasColumnName("NMFANTASIA")
                   .HasColumnType("varchar(70)");

            builder.Property(c => c.DSINSCREST)
                   .HasColumnName("DSINSCREST")
                   .HasColumnType("varchar(20)");

            builder.Property(c => c.DSINSCRESTVINC)
                   .HasColumnName("DSINSCRESTVINC")
                   .HasColumnType("varchar(20)");

            builder.Property(c => c.DSINSCRMUN)
                   .HasColumnName("DSINSCRMUN")
                   .HasColumnType("varchar(20)");

            builder.Property(c => c.NMDISTRIBUIDORA)
                   .HasColumnName("NMDISTRIBUIDORA")
                   .HasColumnType("varchar(50)");

            builder.Property(c => c.NUREGJUNTACOM)
                   .HasColumnName("NUREGJUNTACOM")
                   .HasColumnType("varchar(20)");

            builder.Property(c => c.NUCAPARM)
                   .HasColumnName("NUCAPARM")
                   .HasColumnType("decimal(10,3)");

            builder.Property(c => c.STMICROEMPRESA)
                   .HasColumnName("STMICROEMPRESA")
                   .HasColumnType("int");

            builder.Property(c => c.STLUCROPRESUMIDO)
                   .HasColumnName("STLUCROPRESUMIDO")
                   .HasColumnType("int");

            builder.Property(c => c.TPEMPRESA)
                   .HasColumnName("TPEMPRESA")
                   .HasColumnType("int");

            builder.Property(c => c.CRT)
                   .HasColumnName("CRT")
                   .HasColumnType("int");

            builder.Property(c => c.IDCSC)
                   .HasColumnName("IDCSC")
                   .HasColumnType("varchar(10)");

            builder.Property(c => c.CSC)
                   .HasColumnName("CSC")
                   .HasColumnType("varchar(40)");

            builder.Property(c => c.NUCNAE)
                   .HasColumnName("NUCNAE")
                   .HasColumnType("varchar(10)");

            // OBS: propriedade do modelo chama IDLOJA_SITEMARCADO, coluna no DB é IDLOJA_SITEMERCADO
            builder.Property(c => c.IDLOJA_SITEMARCADO)
                   .HasColumnName("IDLOJA_SITEMERCADO")
                   .HasColumnType("varchar(20)");

            builder.Property(c => c.CLIENTID_SITEMERCADO)
                   .HasColumnName("CLIENTID_SITEMERCADO")
                   .HasColumnType("varchar(20)");

            builder.Property(c => c.CLIENTSECRET_SITEMERCADO)
                   .HasColumnName("CLIENTSECRET_SITEMERCADO")
                   .HasColumnType("varchar(20)");

            builder.Property(c => c.CSC_HOMOL)
                   .HasColumnName("CSC_HOMOL")
                   .HasColumnType("varchar(40)");

            // ===================== RELACIONAMENTOS =====================

            // Empresa -> Endereco (Empresa tem FK IDENDERECO)
            builder.HasOne(empresa => empresa.Endereco)
                   .WithMany(endereco => endereco.Empresas)
                   .HasForeignKey(empresa => empresa.IDENDERECO)
                   .HasPrincipalKey(endereco => endereco.Id);

            // Para as coleções dependentes, usar o nome da FK no lado dependente (coluna DB).
            // Isso evita depender do nome exato da propriedade na entidade dependente.
            // Ex.: Configuracoes (tabela config) terá coluna IDEMPRESA
            builder.HasMany(empresa => empresa.Configuracoes)
                   .WithOne(cfg => cfg.Empresa)
                   .HasForeignKey("IDEMPRESA")
                   .HasPrincipalKey(empresa => empresa.Id);

            builder.HasMany(empresa => empresa.EmpresasAuth)
                   .WithOne(auth => auth.Empresa)
                   .HasForeignKey("IDEMPRESA")
                   .HasPrincipalKey(empresa => empresa.Id);

            builder.HasMany(empresa => empresa.ConfigImagem)
                   .WithOne(cfgImg => cfgImg.Empresa)
                   .HasForeignKey("IDEMPRESA")
                   .HasPrincipalKey(empresa => empresa.Id);

            builder.HasMany(empresa => empresa.Perfil)
                   .WithOne(perfil => perfil.Empresa)
                   .HasForeignKey("IDEMPRESA")
                   .HasPrincipalKey(empresa => empresa.Id);

            builder.HasMany(empresa => empresa.Estoques)
                   .WithOne(estoque => estoque.Empresa)
                   .HasForeignKey("IDEMPRESA")
                   .HasPrincipalKey(empresa => empresa.Id);

            builder.HasMany(empresa => empresa.Funcionarios)
                   .WithOne(func => func.Empresa)
                   .HasForeignKey("IDEMPRESA")
                   .HasPrincipalKey(empresa => empresa.Id);

            builder.HasMany(empresa => empresa.Moedas)
                   .WithOne(moeda => moeda.Empresa)
                   .HasForeignKey("IDEMPRESA")
                   .HasPrincipalKey(empresa => empresa.Id);

            builder.HasMany(empresa => empresa.PontosVendas)
                   .WithOne(pv => pv.Empresa)
                   .HasForeignKey("IDEMPRESA")
                   .HasPrincipalKey(empresa => empresa.Id);

            builder.HasMany(empresa => empresa.Produtos)
                   .WithOne(prod => prod.Empresa)
                   .HasForeignKey("IDEMPRESA")
                   .HasPrincipalKey(empresa => empresa.Id);

            builder.HasMany(empresa => empresa.PlanoContas)
                   .WithOne(pc => pc.Empresa)
                   .HasForeignKey("IDEMPRESA")
                   .HasPrincipalKey(empresa => empresa.Id);

            builder.HasMany(empresa => empresa.ContaPagar)
                   .WithOne(cp => cp.Empresa)
                   .HasForeignKey("IDEMPRESA")
                   .HasPrincipalKey(empresa => empresa.Id);

            builder.HasMany(empresa => empresa.ContaReceber)
                   .WithOne(cr => cr.Empresa)
                   .HasForeignKey("IDEMPRESA")
                   .HasPrincipalKey(empresa => empresa.Id);

            builder.HasMany(empresa => empresa.NotaFiscalInutil)
                   .WithOne(nfi => nfi.Empresa)
                   .HasForeignKey("IDEMPRESA")
                   .HasPrincipalKey(empresa => empresa.Id);

            builder.HasMany(empresa => empresa.Turnos)
                   .WithOne(turno => turno.Empresa)
                   .HasForeignKey("IDEMPRESA")
                   .HasPrincipalKey(empresa => empresa.Id);

            builder.HasMany(empresa => empresa.Caixas)
                   .WithOne(caixa => caixa.Empresa)
                   .HasForeignKey("IDEMPRESA")
                   .HasPrincipalKey(empresa => empresa.Id);

            builder.HasMany(empresa => empresa.Vales)
                   .WithOne(vale => vale.Empresa)
                   .HasForeignKey("IDEMPRESA")
                   .HasPrincipalKey(empresa => empresa.Id);

            builder.HasMany(empresa => empresa.Perdas)
                   .WithOne(perda => perda.Empresa)
                   .HasForeignKey("IDEMPRESA")
                   .HasPrincipalKey(empresa => empresa.Id);

            builder.HasMany(empresa => empresa.Devolucao)
                   .WithOne(dev => dev.Empresa)
                   .HasForeignKey("IDEMPRESA")
                   .HasPrincipalKey(empresa => empresa.Id);

            builder.HasMany(empresa => empresa.Compras)
                   .WithOne(compra => compra.Empresa)
                   .HasForeignKey("IDEMPRESA")
                   .HasPrincipalKey(empresa => empresa.Id);

            builder.HasMany(empresa => empresa.Inventarios)
                   .WithOne(inv => inv.Empresa)
                   .HasForeignKey("IDEMPRESA")
                   .HasPrincipalKey(empresa => empresa.Id);

            builder.HasMany(empresa => empresa.ProdutoSiteMercado)
                   .WithOne(psm => psm.Empresa)
                   .HasForeignKey("IDEMPRESA")
                   .HasPrincipalKey(empresa => empresa.Id);

            builder.HasMany(empresa => empresa.MoedasSiteMercados)
                   .WithOne(msm => msm.Empresa)
                   .HasForeignKey("IDEMPRESA")
                   .HasPrincipalKey(empresa => empresa.Id);
        }
    }
}

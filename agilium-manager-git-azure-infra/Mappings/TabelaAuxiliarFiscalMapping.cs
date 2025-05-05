using agilium.api.business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.infra.Mappings
{
    public class CfopMapping : IEntityTypeConfiguration<Cfop>
    {
        public void Configure(EntityTypeBuilder<Cfop> builder)
        {
            builder.ToTable("cfop");
            builder.HasKey(c => c.CDCFOP);

            builder.Property(c => c.CDCFOP).HasColumnName("CDCFOP").HasColumnType("int").IsRequired();
            builder.Property(c => c.DSCFOP).HasColumnName("DSCFOP").HasColumnType("varchar(400)");
            builder.Property(c => c.TPCFOP).HasColumnName("TPCFOP").HasColumnType("varchar(1)");

            //campos padrao da entidade que nao existem na tabela
            builder.Ignore(c => c.Id);
        }
    }

    public class CestNcmMapping : IEntityTypeConfiguration<CestNcm>
    {
        public void Configure(EntityTypeBuilder<CestNcm> builder)
        {
            builder.ToTable("cest_ncm");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("IDCEST_NCM").HasColumnType("bigint").IsRequired();
            builder.Property(c => c.CDCEST).HasColumnName("CDCEST").HasColumnType("varchar(15)");
            builder.Property(c => c.CDNCM).HasColumnName("CDNCM").HasColumnType("varchar(15)");
            builder.Property(c => c.DSDESCR).HasColumnName("DSDESCR").HasColumnType("varchar(600)");

        }
    }

    public class IbptMapping : IEntityTypeConfiguration<Ibpt>
    {
        public void Configure(EntityTypeBuilder<Ibpt> builder)
        {
            builder.ToTable("ibpt");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("IDIBPT").HasColumnType("bigint").IsRequired();

            builder.Property(c => c.NCM).HasColumnName("NCM").HasColumnType("varchar(20)");
            builder.Property(c => c.EX).HasColumnName("EX").HasColumnType("int");
            builder.Property(c => c.TIPO).HasColumnName("TIPO").HasColumnType("int");
            builder.Property(c => c.DESCRICAO).HasColumnName("DESCRICAO").HasColumnType("varchar(255)");
            builder.Property(c => c.NACIONALFEDERAL).HasColumnName("NACIONALFEDERAL").HasColumnType("double");
            builder.Property(c => c.IMPORTADOSFEDERAL).HasColumnName("IMPORTADOSFEDERAL").HasColumnType("double");
            builder.Property(c => c.ESTADUAL).HasColumnName("ESTADUAL").HasColumnType("double");
            builder.Property(c => c.MUNICIPAL).HasColumnName("MUNICIPAL").HasColumnType("double");
            builder.Property(c => c.INICIOVIG).HasColumnName("INICIOVIG").HasColumnType("date");
            builder.Property(c => c.FIMVIG).HasColumnName("FIMVIG").HasColumnType("date");
            builder.Property(c => c.VERSAO).HasColumnName("VERSAO").HasColumnType("varchar(10)");

        }
    }

    public class CsosnMapping : IEntityTypeConfiguration<Csosn>
    {
        public void Configure(EntityTypeBuilder<Csosn> builder)
        {
            builder.ToTable("csosn");
            builder.HasKey(c => c.CST);

            builder.Property(c => c.CST).HasColumnName("CST").HasColumnType("varchar(5)").IsRequired();
            builder.Property(c => c.DESCR).HasColumnName("DESCR").HasColumnType("varchar(255)");

            //campos padrao da entidade que nao existem na tabela
            builder.Ignore(c => c.Id);
        }
    }

    public class CstMapping : IEntityTypeConfiguration<Cst>
    {
        public void Configure(EntityTypeBuilder<Cst> builder)
        {
            builder.ToTable("cst");
            builder.HasKey(c => c.CST);

            builder.Property(c => c.CST).HasColumnName("CST").HasColumnType("varchar(5)").IsRequired();
            builder.Property(c => c.DESCR).HasColumnName("DESCR").HasColumnType("varchar(255)");

            //campos padrao da entidade que nao existem na tabela
            builder.Ignore(c => c.Id);
        }
    }

    public class NcmMapping : IEntityTypeConfiguration<Ncm>
    {
        public void Configure(EntityTypeBuilder<Ncm> builder)
        {
            builder.ToTable("ncm");
            builder.HasKey(c => c.CDNCM);

            builder.Property(c => c.CDNCM).HasColumnName("CDNCM").HasColumnType("varchar(15)").IsRequired();
            builder.Property(c => c.DSDESCR).HasColumnName("DSDESCR").HasColumnType("varchar(300)");

            //campos padrao da entidade que nao existem na tabela
            builder.Ignore(c => c.Id);
        }
    }

}

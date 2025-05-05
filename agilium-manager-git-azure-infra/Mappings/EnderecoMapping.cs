using agilium.api.business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.infra.Mappings
{
    public class EnderecoMapping : IEntityTypeConfiguration<Endereco>
    {
        public void Configure(EntityTypeBuilder<Endereco> builder)
        {
            builder.ToTable("endereco");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("IDENDERECO").HasColumnType("bigint(20)").IsRequired();
            builder.Property(c => c.Logradouro).HasColumnName("ENDER").HasColumnType("varchar(90)");
            builder.Property(c => c.Complemento).HasColumnType("varchar(40)").HasColumnName("COMPL");
            builder.Property(c => c.Numero).HasColumnType("varchar(20)").HasColumnName("NUM");
            builder.Property(c => c.Bairro).HasColumnType("varchar(75)").HasColumnName("BAIRRO");
            builder.Property(c => c.Cidade).HasColumnType("varchar(65)").HasColumnName("CIDADE");
            builder.Property(c => c.Uf).HasColumnType("varchar(2)").HasColumnName("UF");
            builder.Property(c => c.Pais).HasColumnType("varchar(30)").HasColumnName("PAIS");

            builder.Property(x => x.Ibge).HasColumnType("int").HasColumnName("IBGE");
            builder.Property(x => x.PontoReferencia).HasColumnType("varchar(100)").HasColumnName("DSPTREF");

            builder
          .HasMany(ender => ender.Fornecedores)
          .WithOne(fornec => fornec.Endereco)
          .HasForeignKey(fornec => new { fornec.IDENDERECO })
          .HasPrincipalKey(ender => new { ender.Id });

            builder
          .HasMany(cliEnd => cliEnd.ClienteEndereco)
          .WithOne(cli => cli.Endereco)
          .HasForeignKey(cli => new { cli.IDENDERECO })
          .HasPrincipalKey(cliEnd => new { cliEnd.Id });

            builder
             .HasMany(cliEnd => cliEnd.ClienteEnderecoCobranca)
             .WithOne(cli => cli.EnderecoCobranca)
             .HasForeignKey(cli => new { cli.IDENDERECOCOB })
             .HasPrincipalKey(cliEnd => new { cliEnd.Id }) ;

            builder
             .HasMany(cliEnd => cliEnd.ClienteEnderecoEntrega)
             .WithOne(cli => cli.EnderecoEntrega)
             .HasForeignKey(cli => new { cli.IDENDERECONTREGA })
             .HasPrincipalKey(cliEnd => new { cliEnd.Id });


            builder
             .HasMany(cliEnd => cliEnd.ClienteEnderecoFaturamento)
             .WithOne(cli => cli.EnderecoFaturamento)
             .HasForeignKey(cli => new { cli.IDENDERECOFAT })
             .HasPrincipalKey(cliEnd => new { cliEnd.Id });

            builder
             .HasMany(ender => ender.Funcionarios)
             .WithOne(func => func.Endereco)
             .HasForeignKey(func => new { func.IDENDERECO })
             .HasPrincipalKey(ender => new { ender.Id });
        }
    }

    public class CepMapping : IEntityTypeConfiguration<Cep>
    {
        public void Configure(EntityTypeBuilder<Cep> builder)
        {
            builder.ToTable("ceps");
            builder.HasKey(c => c.id_logradouro);
            builder.Property(c => c.id_logradouro).HasColumnName("id_logradouro").HasColumnType("int").IsRequired();
            builder.Property(c => c.id_cidade).HasColumnName("id_cidade").HasColumnType("int");
            builder.Property(c => c.uf).HasColumnName("uf").HasColumnType("varchar(2)");
            builder.Property(c => c.cidade).HasColumnName("cidade").HasColumnType("varchar(65)");
            builder.Property(c => c.bairro).HasColumnName("bairro").HasColumnType("varchar(75)");
            builder.Property(c => c.ibge).HasColumnName("ibge").HasColumnType("int");
            builder.Property(c => c.tipo).HasColumnName("tipo").HasColumnType("varchar(20)");
            builder.Property(c => c.Numero).HasColumnName("cep").HasColumnType("varchar(8)");
            builder.Property(c => c.Endereco).HasColumnName("ender").HasColumnType("varchar(90)");

            builder.Ignore(x => x.Id);


        }
    }
}

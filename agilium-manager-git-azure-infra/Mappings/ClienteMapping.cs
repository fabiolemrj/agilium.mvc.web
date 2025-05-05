using agilium.api.business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.infra.Mappings
{
    public class ClienteMapping : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> builder)
        {
            builder.ToTable("cliente");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("IDCLIENTE").HasColumnType("bigint").IsRequired();

            builder.Property(c => c.CDCLIENTE).HasColumnName("CDCLIENTE").HasColumnType("varchar(6)");
            builder.Property(c => c.NMCLIENTE).HasColumnName("NMCLIENTE").HasColumnType("varchar(70)");
            builder.Property(c => c.TPCLIENTE).HasColumnName("TPCLIENTE").HasColumnType("varchar(1)");
            builder.Property(c => c.DTCAD).HasColumnName("DTCAD").HasColumnType("datetime");
            builder.Property(c => c.IDENDERECO).HasColumnName("IDENDERECO").HasColumnType("bigint");
            builder.Property(c => c.IDENDERECOCOB).HasColumnName("IDENDERECOCOB").HasColumnType("bigint");
            builder.Property(c => c.IDENDERECOFAT).HasColumnName("IDENDERECOFAT").HasColumnType("bigint");
            builder.Property(c => c.IDENDERECONTREGA).HasColumnName("IDENDERECOENTREGA").HasColumnType("bigint");
            builder.Property(c => c.STCLIENTE).HasColumnName("STCLIENTE").HasColumnType("int");
            builder.Property(c => c.STPUBEMAIL).HasColumnName("STPUBEMAIL").HasColumnType("int");
            builder.Property(c => c.STPUBSMS).HasColumnName("STPUBSMS").HasColumnType("int");

            //chaves estrangeiras
            builder
                .HasOne(cliente => cliente.ClientesPFs)
                .WithOne(ad => ad.Cliente)
                .HasForeignKey<ClientePF>(ad => ad.Id);

            builder
            .HasOne(cliente => cliente.ClientesPJs)
            .WithOne(ad => ad.Cliente)
            .HasForeignKey<ClientePJ>(ad => ad.Id);

            builder
                .HasMany(cliente => cliente.ClienteContatos)
                .WithOne(cliContato => cliContato.Cliente)
                .HasForeignKey(cliContato => new { cliContato.IDCLIENTE })
                .HasPrincipalKey(cliente => new { cliente.Id });

            builder
                .HasMany(cliente => cliente.ClientePrecos)
                .WithOne(clipreco => clipreco.Cliente)
                .HasForeignKey(clipreco => new { clipreco.IDCLIENTE });

            builder
              .HasMany(cliente => cliente.ContaReceber)
              .WithOne(contaReceber => contaReceber.Cliente)
              .HasForeignKey(contaReceber => new { contaReceber.IDCLIENTE })
              .HasPrincipalKey(cliente => new { cliente.Id });

            builder
              .HasMany(cliente => cliente.Vales)
              .WithOne(vale => vale.Cliente)
              .HasForeignKey(vale => new { vale.IDCLIENTE })
              .HasPrincipalKey(cliente => new { cliente.Id });

            builder
             .HasMany(cliente => cliente.Venda)
             .WithOne(venda => venda.Cliente)
             .HasForeignKey(venda => new { venda.IDCLIENTE })
             .HasPrincipalKey(cliente => new { cliente.Id });


            builder
                 .HasMany(cliente => cliente.Devolucao)
                 .WithOne(devolucao => devolucao.Cliente)
                 .HasForeignKey(devolucao => new { devolucao.IDCLIENTE })
                 .HasPrincipalKey(cliente => new { cliente.Id });

            builder
                  .HasMany(cliente => cliente.VendaTemporaria)
                  .WithOne(vendaTemp => vendaTemp.Cliente)
                  .HasForeignKey(vendaTemp => new { vendaTemp.IDCLIENTE })
                  .HasPrincipalKey(cliente => new { cliente.Id });
        }
    }

    public class ClientePFMapeamento : IEntityTypeConfiguration<ClientePF>
    {
        public void Configure(EntityTypeBuilder<ClientePF> builder)
        {
            builder.ToTable("clientepf");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("IDCLIENTE").HasColumnType("bigint").IsRequired();
            builder.Property(c => c.NURG).HasColumnName("NURG").HasColumnType("varchar(20)");
            builder.Property(c => c.DTNASC).HasColumnName("DTNASC").HasColumnType("date");
            builder.Property(c => c.NUCPF).HasColumnName("NUCPF").HasColumnType("varchar(15)");
        }
    }

    public class ClientePJMapeamento : IEntityTypeConfiguration<ClientePJ>
    {
        public void Configure(EntityTypeBuilder<ClientePJ> builder)
        {
            builder.ToTable("clientepj");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("IDCLIENTE").HasColumnType("bigint").IsRequired();

            builder.Property(c => c.NMRZSOCIAL).HasColumnName("NMRZSOCIAL").HasColumnType("varchar(70)");
            builder.Property(c => c.DSINSCREST).HasColumnName("DSINSCREST").HasColumnType("varchar(20)");
            builder.Property(c => c.NUCNPJ).HasColumnName("NUCNPJ").HasColumnType("varchar(20)");

        }
    }

    public class ClienteContatoMapeamento : IEntityTypeConfiguration<ClienteContato>
    {
        public void Configure(EntityTypeBuilder<ClienteContato> builder)
        {
            builder.ToTable("cli_contato");
            builder.HasKey(c => new { c.IDCLIENTE, c.IDCONTATO });

            builder.Property(c => c.IDCONTATO).HasColumnName("IDCONTATO").HasColumnType("bigint").IsRequired();
            builder.Property(c => c.IDCLIENTE).HasColumnName("IDCLIENTE").HasColumnType("bigint").IsRequired();


            //campos padrao da entidade que nao existem na tabela
            builder.Ignore(c => c.Id);
        }
    }

    public class ClientePrecoMapeamento : IEntityTypeConfiguration<ClientePreco>
    {
        public void Configure(EntityTypeBuilder<ClientePreco> builder)
        {
            builder.ToTable("cli_preco");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("IDCLI_PRECO").HasColumnType("bigint").IsRequired();

            builder.Property(c => c.IDPRODUTO).HasColumnName("IDPRODUTO").HasColumnType("bigint");
            builder.Property(c => c.IDCLIENTE).HasColumnName("IDCLIENTE").HasColumnType("bigint");
            builder.Property(c => c.TPDIFERENCA).HasColumnName("TPDIFERENCA").HasColumnType("int");
            builder.Property(c => c.TPVALOR).HasColumnName("TPVALOR").HasColumnType("int");
            builder.Property(c => c.NUVALOR).HasColumnName("NUVALOR").HasColumnType("double");
            builder.Property(c => c.DTHRCAD).HasColumnName("DTHRCAD").HasColumnType("datetime");
            builder.Property(c => c.NmUsuario).HasColumnName("NmUsuario").HasColumnType("varchar(50)");
        }
    }
}

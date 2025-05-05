using agilium.api.business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.infra.Mappings
{
    public class FormaPagamentoMapping : IEntityTypeConfiguration<FormaPagamento>
    {

        public void Configure(EntityTypeBuilder<FormaPagamento> builder)
        {
            builder.ToTable("forma_pagamento");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("IDFORMAPAG").HasColumnType("bigint").IsRequired();
            builder.Property(c => c.IDEmpresa).HasColumnName("IDEMPRESA").HasColumnType("bigint");
            builder.Property(c => c.DSFormaPagamento).HasColumnName("DSFORMAPAG").HasColumnType("varchar(50)");
            builder.Property(c => c.STFormaPagamento).HasColumnName("STFORMAPAG").HasColumnType("int");

            //chaves estrangeiras
            builder
            .HasOne(motivoDev => motivoDev.Empresa)
            .WithMany(empresa => empresa.FormasPagamento)
            .HasForeignKey(devolucao => new { devolucao.IDEmpresa})
            .HasPrincipalKey(motivoDev => new { motivoDev.Id });

        }
    }
}

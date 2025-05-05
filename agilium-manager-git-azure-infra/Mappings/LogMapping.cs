using agilium.api.business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.infra.Mappings
{
    public class LogMapping : IEntityTypeConfiguration<LogSistema>
    {
        public void Configure(EntityTypeBuilder<LogSistema> builder)
        {
            builder.ToTable("log_sist");
            builder.HasKey(c => c.id_log);

            builder.Property(c => c.id_log).HasColumnName("id_log").HasColumnType("int").IsRequired();
            builder.Property(c => c.usuario).HasColumnName("usuario").HasColumnType("varchar(50)");
            builder.Property(c => c.descr).HasColumnName("descr").HasColumnType("text");
            builder.Property(c => c.controle).HasColumnName("controle").HasColumnType("varchar(50)");
            builder.Property(c => c.maquina).HasColumnName("maquina").HasColumnType("varchar(50)");
            builder.Property(c => c.data_log).HasColumnName("data_log").HasColumnType("date");
            builder.Property(c => c.hora_log).HasColumnName("hora_log").HasColumnType("varchar(10)");
            builder.Property(c => c.SQL_log).HasColumnName("SQL_log").HasColumnType("text");
            builder.Property(c => c.so).HasColumnName("so").HasColumnType("varchar(255)");

            builder.Ignore(c => c.Id);
        }
    }

    public class LogErroMapping : IEntityTypeConfiguration<LogErro>
    {
        public void Configure(EntityTypeBuilder<LogErro> builder)
        {
            builder.ToTable("logerro");
            builder.HasKey(c => c.id_logerro);

            builder.Property(c => c.id_logerro).HasColumnName("id_logerro").HasColumnType("int").IsRequired();
            builder.Property(c => c.usuario).HasColumnName("usuario").HasColumnType("varchar(50)");
            builder.Property(c => c.erro).HasColumnName("erro").HasColumnType("text");
            builder.Property(c => c.controle).HasColumnName("controle").HasColumnType("varchar(50)");
            builder.Property(c => c.maquina).HasColumnName("maquina").HasColumnType("varchar(50)");
            builder.Property(c => c.Data_erro).HasColumnName("Data_erro").HasColumnType("date");
            builder.Property(c => c.Hora_erro).HasColumnName("Hora_erro").HasColumnType("varchar(10)");
            builder.Property(c => c.SQL_erro).HasColumnName("SQL_erro").HasColumnType("text");

            builder.Ignore(c => c.Id);
        }
    }
}

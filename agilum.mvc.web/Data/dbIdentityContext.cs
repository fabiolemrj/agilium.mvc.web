using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using agilum.mvc.web.ViewModels;
using System.Reflection.Emit;

namespace agilum.mvc.web.Data
{
    public class dbIdentityContext : IdentityDbContext<AppUserAgiliumIdentity>
    {
        public dbIdentityContext(DbContextOptions<dbIdentityContext> options): base(options){  }

        public DbSet<RefreshToken> RefreshTokens { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
          
            base.OnModelCreating(modelBuilder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            // IdentityUserRole (chave composta)
           
            modelBuilder.Entity<AppUserAgiliumIdentity>(e =>
            {
                e.ToTable("AspNetUsers"); e.HasKey(ur => new { ur.Id });
            });

            // IdentityUserRole (chave composta)
            modelBuilder.Entity<IdentityUserRole<string>>(e =>
            {
                e.ToTable("aspnetuserroles");
                e.HasKey(ur => new { ur.UserId, ur.RoleId });
            });

            // IdentityUserLogin (chave composta)
            modelBuilder.Entity<IdentityUserLogin<string>>(e =>
            {
                e.ToTable("aspnetuserlogins");
                e.HasKey(l => new { l.LoginProvider, l.ProviderKey });
            });

            // IdentityUserClaim
            modelBuilder.Entity<IdentityUserClaim<string>>(e =>
            {
                e.ToTable("aspnetuserclaims");
            });

            // IdentityRoleClaim
            modelBuilder.Entity<IdentityRoleClaim<string>>(e =>
            {
                e.ToTable("aspnetroleclaims");
            });

            // IdentityUserToken (chave composta)
            modelBuilder.Entity<IdentityUserToken<string>>(e =>
            {
                e.ToTable("aspnetusertokens");
                e.HasKey(t => new { t.UserId, t.LoginProvider, t.Name });
            });

            // Suas entidades customizadas
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                entity.SetTableName(entity.GetTableName().ToLower());
                foreach (var property in entity.GetProperties())
                    property.SetColumnName(property.GetColumnName().ToLower());
                foreach (var key in entity.GetKeys())
                    key.SetName(key.GetName().ToLower());
                foreach (var fk in entity.GetForeignKeys())
                    fk.SetConstraintName(fk.GetConstraintName().ToLower());
                foreach (var index in entity.GetIndexes())
                    index.SetName(index.GetName().ToLower());
            }
        }

    }

    public class AppUserAgiliumIdentity : IdentityUser
    {
        [Column("cpf")]
        [MaxLength(15)]
        [Display(Name = "CPF")]
        public string CPF { get; set; }
        [Column("name")]
        [MaxLength(255)]
        [Display(Name = "Nome")]
        public string Nome { get; set; }
        [Display(Name = "active")]
        public int Ativo { get; set; }
    }
}

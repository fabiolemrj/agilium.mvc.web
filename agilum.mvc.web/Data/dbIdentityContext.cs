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
using agilium.api.business.Models;

namespace agilum.mvc.web.Data
{
    public class dbIdentityContext : IdentityDbContext<CaUsuarioIdentity>
    {
        public dbIdentityContext(DbContextOptions<dbIdentityContext> options): base(options){  }

        public DbSet<RefreshToken> RefreshTokens { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
          
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CaUsuarioIdentity>(e =>
            {
                e.ToTable("aspnetusers");
                e.HasKey(ur => ur.Id);

                // Configura a navegação Usuario como não mapeada no Identity context
                // para evitar que o EF Core descubra todo o modelo de negócio
                e.Ignore(c => c.Usuario);
            });

            modelBuilder.Entity<IdentityUserRole<string>>(e =>
            {
                e.ToTable("aspnetuserroles");
                e.HasKey(ur => new { ur.UserId, ur.RoleId });
            });

            modelBuilder.Entity<IdentityUserLogin<string>>(e =>
            {
                e.ToTable("aspnetuserlogins");
                e.HasKey(l => new { l.LoginProvider, l.ProviderKey });
            });

            modelBuilder.Entity<IdentityUserClaim<string>>(e =>
            {
                e.ToTable("aspnetuserclaims");
            });

            modelBuilder.Entity<IdentityRoleClaim<string>>(e =>
            {
                e.ToTable("aspnetroleclaims");
            });

            modelBuilder.Entity<IdentityUserToken<string>>(e =>
            {
                e.ToTable("aspnetusertokens");
                e.HasKey(t => new { t.UserId, t.LoginProvider, t.Name });
            });

            modelBuilder.Entity<IdentityRole<string>>(e =>
            {
                e.ToTable("aspnetroles");
            });

            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
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
}

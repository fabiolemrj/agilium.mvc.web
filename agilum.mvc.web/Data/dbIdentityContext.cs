using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using agilium.api.manager.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using agilium.api.manager.ViewModels;

namespace agilum.mvc.web.Data
{
    public class dbIdentityContext : IdentityDbContext<AppUserAgiliumIdentity>
    {
        public dbIdentityContext(DbContextOptions<dbIdentityContext> options): base(options){  }

        public DbSet<RefreshToken> RefreshTokens { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
          
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
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

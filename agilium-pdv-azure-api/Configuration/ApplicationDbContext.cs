using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using agilium.api.pdv.ViewModels;

namespace agilium.api.pdv.Configuration
{
    public class ApplicationDbContext : IdentityDbContext<AppUserAgilium>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            //   builder.Entity<IdentityUser>().ToTable("aspnetusers").HasKey(x => x.Id);
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
    public class AppUserAgilium : IdentityUser
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

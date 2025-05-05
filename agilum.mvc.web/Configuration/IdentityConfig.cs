using agilium.api.manager.Data;
using agilum.mvc.web.Data;
using agilum.mvc.web.Extensions;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace agilum.mvc.web.Configuration
{
    public static class IdentityConfig
    {
        public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("dbIdentityContextConnection");

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services
                .AddDbContext<dbIdentityContext>(
                    options => options.UseMySql(connectionString)
                    .EnableSensitiveDataLogging(true)
                    .EnableDetailedErrors(true));

            services.AddDefaultIdentity<AppUserAgiliumIdentity>()
                .AddEntityFrameworkStores<dbIdentityContext>()
                .AddErrorDescriber<IdentityMensagensPortugues>()
                .AddDefaultTokenProviders();


            return services;
        }
    }


}

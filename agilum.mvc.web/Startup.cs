using agilium.api.infra.Context;
using agilum.mvc.web.Configuration;
using agilum.mvc.web.Extensions;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Globalization;


namespace agilum.mvc.web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
               .AddJsonOptions(options =>
               {
                   options.JsonSerializerOptions.PropertyNamingPolicy = null;
               });

            services.AddDbContext<AgiliumContext>(options =>
            {
                var versaobd_major = Convert.ToInt32(Configuration.GetConnectionString("versaobd-major"));
                var versaobd_minor = Convert.ToInt32(Configuration.GetConnectionString("versaobd-minor"));
                var versaobd_build = Convert.ToInt32(Configuration.GetConnectionString("versaobd-build"));

                options.UseMySql(Configuration.GetConnectionString("ConnectionDb"),
                      b => b.MigrationsAssembly("agilium.mvc.web"));
                options.EnableSensitiveDataLogging(sensitiveDataLoggingEnabled:true);
                options.EnableDetailedErrors(detailedErrorsEnabled:true);
 

            });

            services.AddControllersWithViews();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.ResolveDependencies(Configuration);
            services.AddIdentityConfiguration(Configuration);
            

            services.AddRazorPages();
            services.AddMvcConfiguration();
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddConsole()
                    .AddFilter(DbLoggerCategory.Database.Command.Name, LogLevel.Information);
                loggingBuilder.AddDebug();
            });
            services.AddAutoMapper(typeof(Startup));
            //services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(3);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;

           
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.Use(async (context, next) =>
            {
                foreach (var header in context.Request.Headers)
                {
                    logger.LogInformation("{Header}: {Value}", header.Key, header.Value);
                }
                await next.Invoke();
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseSession();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<EmpresaSelecionadaMiddleware>();
            app.UseMiddleware<ExceptionMiddleware>();

            //app.UseGlobalizationConfig();
            var cultura = new CultureInfo("pt-BR");
            cultura.NumberFormat.NumberDecimalSeparator = ",";
            cultura.NumberFormat.NumberGroupSeparator = ".";
            CultureInfo.DefaultThreadCurrentCulture = cultura;
            CultureInfo.DefaultThreadCurrentUICulture = cultura;

            var dateformat = new DateTimeFormatInfo
            {
                ShortDatePattern = "dd/MM/yyyy",
                LongDatePattern = "dd/MM/yyyy hh:mm:ss tt"
            };
            cultura.DateTimeFormat = dateformat;

            var supportedCultures = new[] { cultura };
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("pt-BR"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapControllerRoute("areas", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapAreaControllerRoute("Back", "Back", "back/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });

           
        }
    }
}
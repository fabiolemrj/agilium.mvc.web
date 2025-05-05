using agilium.api.infra.Context;
using agilium.api.pdv.Configuration;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace agilium.api.pdv
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IHostEnvironment hostEnvironment)
        {
            var builder = new ConfigurationBuilder()
              .SetBasePath(hostEnvironment.ContentRootPath)
              .AddJsonFile("appsettings.json", true, true)
              .AddJsonFile($"appsettings.{hostEnvironment.EnvironmentName}.json", true, true)
              .AddEnvironmentVariables();

            if (hostEnvironment.IsDevelopment())
            {
                //builder.AddUserSecrets<Startup>();
            }

            Configuration = builder.Build();
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var versaobd_major = Convert.ToInt32(Configuration.GetConnectionString("versaobd-major"));
            var versaobd_minor = Convert.ToInt32(Configuration.GetConnectionString("versaobd-minor"));
            var versaobd_build = Convert.ToInt32(Configuration.GetConnectionString("versaobd-build"));
            
            //var serverVersion = new MySqlServerVersion(new Version(versaobd_major, versaobd_minor, versaobd_build));

            services.AddDbContext<AgiliumContext>(options =>
            {
                options.UseMySql(Configuration.GetConnectionString("DefaultConnection"),
                      b => b.MigrationsAssembly("agilium.api.pdv"));
            });

            //services.ResolveLogger(Configuration);

            services.AddIdentityConfig(Configuration);

            services.AddAutoMapper(typeof(Startup));

            services.AddApiConfig();

            services.AddSwaggerConfig();

            services.ResolveDependencies(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            app.UseApiConfig(env);

            //app.UseLoggerConfig(env, Configuration);


            app.UseSwaggerConfig(provider);
        }
    }
}

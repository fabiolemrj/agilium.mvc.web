using KissLog;
using KissLog.AspNetCore;
using KissLog.CloudListeners.Auth;
using KissLog.CloudListeners.RequestLogsListener;
using KissLog.Formatters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace agilium.api.manager.Configuration
{
    public static class KissLoggerConfig
    {
    //    public static IServiceCollection ResolveLogger(this IServiceCollection services, IConfiguration configuration)
    //    {
    //        services.AddHttpContextAccessor();
    //        services.AddScoped<IKLogger>((provider) => Logger.Factory.Get());
    //        services.AddLogging(logging =>
    //        {
    //            logging.AddKissLog(options =>
    //            {
    //                options.Formatter = (FormatterArgs args) =>
    //                {
    //                    if (args.Exception == null)
    //                        return args.DefaultValue;

    //                    string exceptionStr = new ExceptionFormatter().Format(args.Exception, args.Logger);

    //                    return string.Join(Environment.NewLine, new[] { args.DefaultValue, exceptionStr });
    //                };
    //            });
    //        });

    //        return services;
    //    }

    //    public static IApplicationBuilder UseLoggerConfig(this IApplicationBuilder app, IWebHostEnvironment env, IConfiguration configuration)
    //    {
    //        //IApplicationBuilder applicationBuilder = app.UseKissLogMiddleware(options => ConfigureKissLog(options, configuration));
    //        IApplicationBuilder applicationBuilder = app.UseKissLogMiddleware(options => ConfigureKissLog(configuration));
    //        return app;
    //    }

    //    private static void ConfigureKissLog(IOptionsBuilder options, IConfiguration configuration)
    //    {
    //        var organizationId = configuration.GetSection("KissLog.OrganizationId").Value;
    //        var applicationId = configuration.GetSection("KissLog.ApplicationId").Value;
    //        var apiUrl = configuration.GetSection("KissLog.ApiUrl").Value;
    //        KissLogConfiguration.Listeners
    //            .Add(new RequestLogsApiListener(new Application(organizationId, applicationId))
    //            {
    //                ApiUrl = apiUrl
    //            });
    //    }

    //    private static void ConfigureKissLog(IConfiguration configuration)
    //    {
    //        KissLogConfiguration.Listeners
    //            .Add(new CustomMongoDbListener(configuration["KissLog.BdLog"], "LogBack"));
    //    }
    }

    
}

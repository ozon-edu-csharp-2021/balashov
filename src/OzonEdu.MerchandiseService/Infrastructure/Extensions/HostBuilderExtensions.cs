using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using OzonEdu.MerchandiseService.Infrastructure.Filters;
using OzonEdu.MerchandiseService.Infrastructure.Interceptors;
using OzonEdu.MerchandiseService.Infrastructure.StartupFilters;

namespace OzonEdu.MerchandiseService.Infrastructure.Extensions
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder AddInfrastructure(this IHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddSingleton<IStartupFilter, LoggingStartupFilter>();

                services.AddSingleton<IStartupFilter, HealthCheckStartupFilter>();
                services.AddSingleton<IStartupFilter, VersionStartupFilter>();

                services.AddSingleton<IStartupFilter, SwaggerStartupFilter>();

                services.AddSwaggerGen(options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo {Title = "OzonEdu.MerchandiseService", Version = "v1"});
                    options.CustomSchemaIds(x => x.FullName);
                });
            });
            return builder;
        }

        public static IHostBuilder AddHttp(this IHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddControllers(options => options.Filters.Add<GlobalExceptionFilter>());
            });

            return builder;
        }

        public static IHostBuilder AddGrpc(this IHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddGrpc(options => options.Interceptors.Add<LoggingInterceptor>());
            });

            return builder;
        }
    }
}
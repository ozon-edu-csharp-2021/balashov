using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using OzonEdu.MerchandiseService.Infrastructure.Middleware;

namespace OzonEdu.MerchandiseService.Infrastructure.StartupFilters
{
    public class LoggingStartupFilter : IStartupFilter
    {
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return app =>
            {
                app.UseMiddleware<RequestResponseLoggingMiddleware>();
                next(app);
            };
        }
    }
}
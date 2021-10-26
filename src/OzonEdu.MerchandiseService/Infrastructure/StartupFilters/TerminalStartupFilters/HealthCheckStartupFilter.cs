using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using OzonEdu.MerchandiseService.Infrastructure.Middleware;

namespace OzonEdu.MerchandiseService.Infrastructure.StartupFilters
{
    public class HealthCheckStartupFilter : IStartupFilter
    {
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return app =>
            {
                app.Map("/ready", builder => builder.UseMiddleware<HealthCheckReadyMiddleware>());
                app.Map("/live", builder => builder.UseMiddleware<HealthCheckLiveMiddleware>());
                next(app);
            };
        }
    }
}

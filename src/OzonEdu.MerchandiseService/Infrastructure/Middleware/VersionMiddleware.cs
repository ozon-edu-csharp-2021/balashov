using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Assembly = System.Reflection.Assembly;

namespace OzonEdu.MerchandiseService.Infrastructure.Middleware
{
    public class VersionMiddleware
    {
        public VersionMiddleware(RequestDelegate next)
        {
        }

        public async Task InvokeAsync(HttpContext context)
        {
            //TODO: проверить serviceName
            var version = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "no version";
            var serviceName = Assembly.GetExecutingAssembly().GetName().Name;
            var versionString = $"version:{version}, serviceName:{serviceName}";

            await context.Response.WriteAsync(versionString);
        }
    }
}
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OzonEdu.MerchandiseService.GrpcServices;
using OzonEdu.MerchandiseService.Services;

namespace OzonEdu.MerchandiseService
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IMerchService, MerchService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<MerchGrpcService>();
                endpoints.MapControllers();
            });
        }
    }
}
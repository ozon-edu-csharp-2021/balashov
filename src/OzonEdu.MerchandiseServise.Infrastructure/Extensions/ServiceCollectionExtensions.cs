using MediatR;
using Microsoft.Extensions.DependencyInjection;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchAggregate;
using OzonEdu.MerchandiseServise.Infrastructure.FakeData;
using OzonEdu.MerchandiseServise.Infrastructure.Handlers;

namespace OzonEdu.MerchandiseServise.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddMediatR(typeof(BDSHandler).Assembly);

            return services;
        }
        
        public static IServiceCollection AddInfrastructureRepositories(this IServiceCollection services)
        {
            services.AddScoped<IMerchRepository, MerchRepository>();
            return services;
        }
    }
}
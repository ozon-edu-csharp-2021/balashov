using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using OzonEdu.MerchandiseService.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.ManagerAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchAggregate;
using OzonEdu.MerchandiseService.Infrastructure.FakeData;
using OzonEdu.MerchandiseService.Infrastructure.Handlers;

namespace OzonEdu.MerchandiseService.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            //services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddMediatR(typeof(GetIssuedMerchInfoQueryHandler).Assembly);
            services.AddMediatR(typeof(RequestMerchCommandHandler).Assembly);


            return services;
        }
        
        public static IServiceCollection AddInfrastructureRepositories(this IServiceCollection services)
        {
            services.AddScoped<IMerchRepository, FakeMerchRepository>();

            services.AddScoped<IManagerRepository, FakeManagerRepository>();

            services.AddScoped<IEmployeeRepository, FakeEmployeeRepository>();

            return services;
        }
    }
}
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using OzonEdu.MerchandiseService.BackgroundServices;
using OzonEdu.MerchandiseService.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.ManagerAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchAggregate;
using OzonEdu.MerchandiseService.Domain.Contracts;
using OzonEdu.MerchandiseService.Infrastructure.Configuration;
using OzonEdu.MerchandiseService.Infrastructure.ExternalDataSources;
using OzonEdu.MerchandiseService.Infrastructure.InterfacesToExternals;
using OzonEdu.MerchandiseService.Infrastructure.InterfacesToExternals.FakeExternals;
using OzonEdu.MerchandiseService.Infrastructure.MessageBroker;
using OzonEdu.MerchandiseService.Infrastructure.Repositories.Implementation;
using OzonEdu.MerchandiseService.Infrastructure.Repositories.Infrastructure;
using OzonEdu.MerchandiseService.Infrastructure.Repositories.Infrastructure.Interfaces;

namespace OzonEdu.MerchandiseService.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMediator(this IServiceCollection services)
        {
            services.AddMediatR(typeof(Startup), typeof(DatabaseConnectionOptions));
            return services;
        }

        public static IServiceCollection AddDatabaseComponents(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<DatabaseConnectionOptions>(configuration.GetSection(nameof(DatabaseConnectionOptions)));
            services.AddScoped<IDbConnectionFactory<NpgsqlConnection>, NpgsqlConnectionFactory>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IChangeTracker, ChangeTracker>();
            
            return services;
        }

        public static IServiceCollection AddExternals(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IEmailServer, EmailServer>();
            //services.AddScoped<IEmailServer, FakeEmailServer>();

            services.AddScoped<IEmployeeServer, EmployeeServer>();

            //services.AddScoped<IStockApiServer, StockApiServer>();
            services.AddScoped<IStockApiServer, FakeStockApiServer>();

            var externalsConnectionsOptions = configuration.GetSection("ExternalServers");
            services.Configure<ExternalConnectionOptions>(externalsConnectionsOptions);

            return services;
        }

        public static IServiceCollection AddKafkaServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<KafkaConfiguration>(configuration);
            services.AddSingleton<IProducerBuilderWrapper, ProducerBuilderWrapper>();
            services.AddSingleton<IConsumerBuilderWrapper, ConsumerBuilderWrapper>();

            services.AddHostedService<EmployeeKafkaConsumerBackground>();
            services.AddHostedService<StockApiKafkaConsumerBackground>();

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
            services.AddScoped<IMerchRepository, MerchRepository>();
            services.AddScoped<IManagerRepository, ManagerRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            //services.AddScoped<IMerchRepository, FakeMerchRepository>();
            //services.AddScoped<IManagerRepository, FakeManagerRepository>();
            //services.AddScoped<IEmployeeRepository, FakeEmployeeRepository>();
            //services.AddScoped<IUnitOfWork, FakeUnitOfWork>();

            return services;
        }
    }
}
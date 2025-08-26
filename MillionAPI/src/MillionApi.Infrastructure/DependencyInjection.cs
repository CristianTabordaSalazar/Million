using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using MillionApi.Infrastructure.Persistence;
using MillionApi.Infrastructure.Repositories;
using MillionApi.Infrastructure.Options;
using MillionApi.Application.Common.Interfaces.Persistence;
using MillionApi.Infrastructure.Indexes;

namespace MillionApi.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MongoSettings>(configuration.GetSection(MongoSettings.SectionName));

            services.AddSingleton<IMongoClient>(sp =>
            {
                var opts = sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<MongoSettings>>().Value;
                return new MongoClient(opts.ConnectionString);
            });

            MongoMappings.Register();
            services.AddSingleton<MongoDbContext>();

            services.AddSingleton<IIndexesInitializer, PropertyIndexesInitializer>();
            services.AddSingleton<IIndexesInitializer, OwnerIndexesInitializer>();
            services.AddSingleton<IIndexesInitializer, PropertyImageIndexesInitializer>();
            services.AddSingleton<IIndexesInitializer, PropertyTraceIndexesInitializer>();

            services.AddScoped<IPropertyRepository, PropertyRepository>();

            return services;
        }

        public static async Task UseInfrastructureAsync(this IServiceProvider sp, CancellationToken ct = default)
        {
            var initializers = sp.GetServices<IIndexesInitializer>();
            foreach (var init in initializers)
                await init.CreateAsync(ct);
        }
    }
}
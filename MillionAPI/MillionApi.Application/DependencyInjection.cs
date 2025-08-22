using Microsoft.Extensions.DependencyInjection;
using MillionApi.Application.Service.Property;
using MillionApi.Application.Services.Property;

namespace MillionApi.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IPropertyService, PropertyService>();

            return services;
        }
    }
}
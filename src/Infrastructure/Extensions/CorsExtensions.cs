using StoreKit.Application.Settings;
using StoreKit.Infrastructure.Persistence.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace StoreKit.Infrastructure.Extensions
{
    public static class CorsExtensions
    {
        internal static IServiceCollection AddCorsPolicy(this IServiceCollection services)
        {
            var corsSettings = services.GetOptions<CorsSettings>(nameof(CorsSettings));
            return services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", policy =>
                {
                    policy
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()
                        .AllowAnyOrigin();
                });
            });
        }
    }
}
using StoreKit.Application.Settings;
using StoreKit.Infrastructure.Middlewares;
using StoreKit.Infrastructure.Persistence.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace StoreKit.Infrastructure.Extensions
{
    public static class MiddlewareExtensions
    {
        internal static IApplicationBuilder UseMiddlewares(this IApplicationBuilder app, IConfiguration config)
        {
            app.UseMiddleware<ExceptionMiddleware>();
            if (config.GetValue<bool>("MiddlewareSettings:EnableLocalization")) app.UseMiddleware<LocalizationMiddleware>();
            if (config.GetValue<bool>("MiddlewareSettings:EnableRequestLogging")) app.UseMiddleware<RequestLoggingMiddleware>();
            return app;
        }

        internal static IServiceCollection AddMiddlewares(this IServiceCollection services, IConfiguration config)
        {
            var middlewareSettings = services.GetOptions<MiddlewareSettings>(nameof(MiddlewareSettings));
            services.AddSingleton<ExceptionMiddleware>();
            if (middlewareSettings.EnableLocalization) services.AddSingleton<LocalizationMiddleware>();
            if (middlewareSettings.EnableRequestLogging) services.AddSingleton<RequestLoggingMiddleware>();
            return services;
        }
    }
}
using StoreKit.Application.Abstractions.Services.General;
using StoreKit.Infrastructure.Identity.Permissions;
using StoreKit.Infrastructure.Localizer;
using StoreKit.Infrastructure.Mappings;
using StoreKit.Infrastructure.Persistence;
using StoreKit.Infrastructure.Persistence.Extensions;
using StoreKit.Infrastructure.Services.General;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Localization;

namespace StoreKit.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            MapsterSettings.Configure();
            if (config.GetSection("CacheSettings:PreferRedis").Get<bool>())
            {
                services.AddDistributedRedisCache(options =>
                {
                    options.Configuration = config.GetSection("CacheSettings:RedisURL").Get<string>();
                    options.ConfigurationOptions = new StackExchange.Redis.ConfigurationOptions()
                    {
                        AbortOnConnectFail = true
                    };
                });
            }
            else
            {
                services.AddDistributedMemoryCache();
            }

            services.TryAdd(ServiceDescriptor.Singleton<ICacheService, CacheService>());
            services.AddHealthCheckExtension();
            services.AddLocalization();
            services.AddServices(config);
            services.AddSettings(config);
            services.AddPermissions(config);
            services.AddIdentity(config);
            services.AddMultitenancy<TenantManagementDbContext, ApplicationDbContext>(config);
            services.AddHangfireServer();
            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddMiddlewares(config);
            services.AddSwaggerDocumentation(config);
            services.AddCorsPolicy();
            services.AddApiVersioning(config =>
           {
               config.DefaultApiVersion = new ApiVersion(1, 0);
               config.AssumeDefaultVersionWhenUnspecified = true;
               config.ReportApiVersions = true;
           });
            services.AddSingleton<IStringLocalizerFactory, JsonStringLocalizerFactory>();
            return services;
        }

        public static IServiceCollection AddPermissions(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>()
                .AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
            return services;
        }
    }
}
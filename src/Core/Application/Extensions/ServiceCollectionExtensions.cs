using StoreKit.Application.Abstractions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace StoreKit.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<IRequestValidator>();
            return services;
        }
    }
}
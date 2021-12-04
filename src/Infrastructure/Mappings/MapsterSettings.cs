using StoreKit.Infrastructure.Identity.Models;
using StoreKit.Shared.DTOs.Identity;
using Mapster;

namespace StoreKit.Infrastructure.Mappings
{
    public static class MapsterSettings
    {
        public static void Configure()
        {
            // here we will define the type conversion / Custom-mapping
            // More details at https://github.com/MapsterMapper/Mapster/wiki/Custom-mapping
            TypeAdapterConfig<ApplicationRoleClaim, PermissionDto>.NewConfig().Map(dest => dest.Permission, src => src.ClaimValue);

        }
    }
}
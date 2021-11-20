using StoreKit.Domain.Contracts;

namespace StoreKit.Application.Constants
{
    public static class CacheKeys
    {
        public static string GetCacheKey<T>(object id)
        where T : BaseEntity
        {
            return $"{typeof(T).Name}-{id}";
        }

        public static string GetCacheKey(string name, object id)
        {
            return $"{name}-{id}";
        }
    }
}
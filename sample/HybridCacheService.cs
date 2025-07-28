using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.DependencyInjection;

namespace Sample;

public class HybridCacheService(HybridCache hybridCache) : ICacheService
{
    public async Task<string> GetOrSet(string key, Func<Task<string>> func)
    {
        return await hybridCache.GetOrCreateAsync(
            key,
            async cancel => await func());
    }

    public async Task Remove(string key)
    {
        await hybridCache.RemoveAsync(key);
    }
}

public static class HybridCacheServiceServiceExtensions
{
    public static IServiceCollection AddHybridCacheService(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddHybridCache();
        return serviceCollection
            .AddStackExchangeRedisCache(options =>
            {
                options.Configuration = "localhost";
                options.InstanceName = "sample";
            })
            .AddMemoryCache()
            .AddSingleton<ICacheService, HybridCacheService>();
    }
}
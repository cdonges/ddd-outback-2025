using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.DependencyInjection;
using ZiggyCreatures.Caching.Fusion;
using ZiggyCreatures.Caching.Fusion.Backplane.StackExchangeRedis;
using ZiggyCreatures.Caching.Fusion.Serialization.SystemTextJson;

namespace Sample.Cache;

public class FusionCacheService(IFusionCache fusionCache) : ICacheService
{
    public async Task<string> GetOrSet(string key, Func<Task<string>> func)
    {
        return await fusionCache.GetOrSetAsync(
            key,
            async cancel => await func());
    }

    public async Task Remove(string key)
    {
        await fusionCache.RemoveAsync(key);
    }
}

public static class FusionCacheServiceExtensions
{
    public static IServiceCollection AddFusionCacheService(this IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddFusionCache()
            .WithSerializer(
                new FusionCacheSystemTextJsonSerializer()
            )
            .WithDistributedCache(
                new RedisCache(new RedisCacheOptions { Configuration = "localhost" })
            )
            .WithBackplane(
                new RedisBackplane(new RedisBackplaneOptions { Configuration = "localhost" })
            );

    return serviceCollection.AddSingleton<ICacheService, FusionCacheService>();
    }
}
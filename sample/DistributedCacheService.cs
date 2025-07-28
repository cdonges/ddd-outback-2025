using System.Text;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;

namespace Sample;

public class DistributedCacheService(IDistributedCache distributedCache) : ICacheService
{
    public async Task<string> GetOrSet(string key, Func<Task<string>> func)
    {
        var bytes = await distributedCache.GetAsync(key);
        if (bytes == null)
        {
            string val = await func();
            await distributedCache.SetAsync(key, Encoding.UTF8.GetBytes(val));
            return val;
        }

        return Encoding.UTF8.GetString(bytes);
    }

    public async Task Remove(string key)
    {
        await distributedCache.RemoveAsync(key);
    }
}

public static class DistributedCacheServiceExtensions
{
    public static IServiceCollection AddDistributedCacheService(this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddStackExchangeRedisCache(options =>
            {
                options.Configuration = "localhost";
                options.InstanceName = "sample";
            })
            .AddSingleton<ICacheService, DistributedCacheService>();
    }
}
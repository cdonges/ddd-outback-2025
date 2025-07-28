using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;

namespace Sample;

public class MemoryCacheService(IMemoryCache memoryCache) : ICacheService
{
    public async Task<string> GetOrSet(string key, Func<Task<string>> func)
    {
        return (await memoryCache.GetOrCreateAsync(key, async key => await func()))!;
    }

    public Task Remove(string key)
    {
        memoryCache.Remove(key);

        return Task.CompletedTask;
    }
}

public static class MemoryCacheServiceExtensions
{
    public static IServiceCollection AddDMemoryCacheService(this IServiceCollection serviceCollection)
    {
        return serviceCollection.AddMemoryCache().AddSingleton<ICacheService, MemoryCacheService>();
    }
}
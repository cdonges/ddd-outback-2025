
using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;

namespace Sample;

public class ConcurrentDictionaryCache : ICacheService
{
    private ConcurrentDictionary<string, string> dictionary = new ConcurrentDictionary<string, string>();

    public async Task<string> GetOrSet(string key, Func<Task<string>> func)
    {
        if (!dictionary.ContainsKey(key))
        {
            dictionary[key] = await func();
        }

        return dictionary[key];
    }

    public Task Remove(string key)
    {
        dictionary.TryRemove(key, out _);

        return Task.CompletedTask;
    }
}

public static class ConcurrentDictionaryCacheExtensions
{
    public static IServiceCollection AddConcurrentDictionaryCache(this IServiceCollection serviceCollection)
    {
        return serviceCollection.AddSingleton<ICacheService, ConcurrentDictionaryCache>();
    }
}
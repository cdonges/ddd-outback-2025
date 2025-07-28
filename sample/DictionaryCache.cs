using Microsoft.Extensions.DependencyInjection;

namespace Sample;

public class DictionaryCache : ICacheService
{
    private Dictionary<string, string> dictionary = new Dictionary<string, string>();

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
        if (dictionary.ContainsKey(key))
        {
            dictionary.Remove(key);
        }

        return Task.CompletedTask;
    }
}

public static class DictionaryCacheExtensions
{
    public static IServiceCollection AddDictionaryCache(this IServiceCollection serviceCollection)
    {
        return serviceCollection.AddSingleton<ICacheService, DictionaryCache>();
    }
}
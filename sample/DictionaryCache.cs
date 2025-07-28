
using Microsoft.Extensions.DependencyInjection;

namespace Sample;

public class DictionaryCache : ICacheService
{
    private Dictionary<string, string> dictionary = new Dictionary<string, string>();

    public async Task<string> GetOrSet(string key, Func<string> func)
    {
        await Task.CompletedTask;

        if (!dictionary.ContainsKey(key))
        {
            dictionary[key] = func();
        }

        return dictionary[key];
    }

    public async Task Remove(string key)
    {
        await Task.CompletedTask;

        if (dictionary.ContainsKey(key))
        {
            dictionary.Remove(key);
        }
    }
}

public static class DictionaryCacheExtensions
{
    public static IServiceCollection AddDictionaryCache(this IServiceCollection serviceCollection)
    {
        return serviceCollection.AddSingleton<ICacheService, DictionaryCache>();
    }
}
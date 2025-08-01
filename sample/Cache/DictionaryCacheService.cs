using Microsoft.Extensions.DependencyInjection;

namespace Sample.Cache;

public class DictionaryCacheService : ICacheService
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
    public static IServiceCollection AddDictionaryCacheService(this IServiceCollection serviceCollection)
    {
        return serviceCollection.AddSingleton<ICacheService, DictionaryCacheService>();
    }
}
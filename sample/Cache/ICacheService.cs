namespace Sample.Cache;

public interface ICacheService
{
    Task<string> GetOrSet(string key, Func<Task<string>> func);

    Task Remove(string key);
}
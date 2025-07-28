namespace Sample;

public interface ICacheService
{
    Task<string> GetOrSet(string key, Func<string> func);

    Task Remove(string key);
}
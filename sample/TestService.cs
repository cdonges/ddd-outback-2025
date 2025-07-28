
namespace Sample;

public class TestService(ICacheService cacheService) : ITestService
{
    public async Task RunAsync()
    {
        var val = await cacheService.GetOrSet("hello", () => "world");
        Console.WriteLine(val);
        await Task.CompletedTask;
    }
}
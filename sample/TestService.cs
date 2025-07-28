namespace Sample;

public class TestService(ICacheService cacheService, IDashboardService dashboardService) : ITestService
{
    public async Task RunAsync()
    {
        var userId = Guid.NewGuid();
        var val = await cacheService.GetOrSet(userId.ToString(), async () => await dashboardService.Get(userId));
        Console.WriteLine(val);
        await Task.CompletedTask;
    }
}
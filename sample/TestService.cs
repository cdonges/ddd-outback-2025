using System.Diagnostics;
using Sample.Cache;

namespace Sample;

public class TestService(ICacheService cacheService, IDashboardService dashboardService, Settings settings, IResultsService resultsService) : ITestService
{
    public async Task RunAsync()
    {
        var tasks = Enumerable.Range(0, 100).Select(_ => Task.Run(async () =>
        {
            await Task.Delay(Random.Shared.Next(10 * 1000));
            var sw = Stopwatch.StartNew();
            var userId = dashboardService.RandomId();
            var val = await cacheService.GetOrSet(userId.ToString(), async () => await dashboardService.Get(userId, settings.Id));
            sw.Stop();
            resultsService.AddTiming(settings.Id, sw.Elapsed);
        }));

        await Task.WhenAll(tasks);
    }
}
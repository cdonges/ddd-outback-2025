using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Sample.Cache;

namespace Sample;

public class Program
{
    public static async Task Main(string[] args)
    {
        var containerCount = 10;
        IResultsService resultsService = new ResultsService(containerCount);
        IDashboardService dashboardService = new DashboardService(resultsService);
        var containers = Enumerable.Range(0, containerCount).Select(x => new HostContainer(x)).ToList();
        await Task.WhenAll(containers.Select(x => x.Run(dashboardService, resultsService)));
        resultsService.Print();
    }
}
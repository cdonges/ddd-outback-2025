using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using sample.Enums;
using Sample.Cache;

namespace Sample;

public class Program
{
    public static async Task Main(string[] args)
    {
        Console.WriteLine("Select cache type");
        foreach (var cacheType in Enum.GetValues(typeof(CacheTypeEnum)).Cast<CacheTypeEnum>())
        {
            Console.WriteLine($"{(int)cacheType} - {cacheType.ToString()}");
        }

        var selection = Console.ReadLine();

        if (!int.TryParse(selection, out int cacheTypeInt))
        {
            Console.WriteLine("Bad input");
            return;
        }

        var containerCount = 5;
        IResultsService resultsService = new ResultsService(containerCount);
        IDashboardService dashboardService = new DashboardService(resultsService);
        var containers = Enumerable.Range(0, containerCount).Select(x => new HostContainer(x, (CacheTypeEnum)cacheTypeInt, dashboardService, resultsService)).ToList();
        await Task.WhenAll(containers.Select(x => x.Run()));
        resultsService.Print();
    }
}
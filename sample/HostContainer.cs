using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Sample;
using Sample.Cache;

namespace Sample;

public class HostContainer(int id)
{
    public async Task Run(IDashboardService dashboardService, IResultsService resultsService)
    {
        var builder = Host.CreateApplicationBuilder();

        builder.Services.AddLogging(loggerBuilder =>
            {
                loggerBuilder.ClearProviders();
                loggerBuilder.AddConsole();
                loggerBuilder.AddFilter("Microsoft", LogLevel.None);
                loggerBuilder.AddFilter("ZiggyCreatures.Caching.Fusion.FusionCache", LogLevel.None);
            })
            .AddScoped<ITestService, TestService>()
            .AddSingleton(_ => dashboardService)
            .AddSingleton(_ => resultsService)
            .AddSingleton(new Settings() { Id = id })
            .AddDMemoryCacheService()
            .AddHostedService<ProgramService>();

        using var host = builder.Build();
        await host.RunAsync();
    }
}
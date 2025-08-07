using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Sample;
using Sample.Cache;

namespace Sample;

public class HostContainer(int id, CacheTypeEnum cacheType)
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
            .AddHostedService<ProgramService>();

        switch (cacheType)
        {
                case CacheTypeEnum.Dictionary: builder.Services.AddDictionaryCacheService(); break;
                case CacheTypeEnum.ConcurrentDictionary: builder.Services.AddConcurrentDictionaryCacheService(); break;
                case CacheTypeEnum.Memory: builder.Services.AddMemoryCacheService(); break;
                case CacheTypeEnum.Distributed: builder.Services.AddDistributedCacheService(); break;
                case CacheTypeEnum.Hybrid: builder.Services.AddHybridCacheService(); break;
                case CacheTypeEnum.Fusion: builder.Services.AddFusionCacheService(); break;
        }

        using var host = builder.Build();
        await host.RunAsync();
    }
}
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Sample;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder();

        builder.Services.AddLogging(loggerBuilder =>
            {
                loggerBuilder.ClearProviders();
                loggerBuilder.AddConsole();
                loggerBuilder.AddFilter("Microsoft", LogLevel.None);
            })
            .AddScoped<ITestService, TestService>()
            .AddFusionCacheService()
            .AddHostedService<ProgramService>();

        using var host = builder.Build();
        await host.RunAsync();
    }
}
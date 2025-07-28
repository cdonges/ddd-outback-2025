using Microsoft.Extensions.Hosting;

namespace Sample;

public class ProgramService(ITestService testService, IHostApplicationLifetime hostApplicationLifetime) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await testService.RunAsync();
        hostApplicationLifetime.StopApplication();
    }
}
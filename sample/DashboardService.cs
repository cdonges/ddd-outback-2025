using System.Text.Json;

namespace Sample;

public class DashboardService : IDashboardService
{
    public List<Guid> ValidIds { get; private set; }
    private IResultsService resultsService;

    public DashboardService(IResultsService resultsService)
    {
        ValidIds = Enumerable.Range(0, 10).Select(x => Guid.NewGuid()).ToList();
        this.resultsService = resultsService;
    }

    public Guid RandomId()
    {
        return ValidIds[Random.Shared.Next(10)];
    }

    public async Task<string> Get(Guid userId, int id)
    {
        resultsService.AddCacheMiss(id);
        var userDashboard = new { UserId = userId, Title = $"Dashboard for {userId}", Content = "" };
        await Task.Delay(2000);
        return JsonSerializer.Serialize(userDashboard);
    }
}
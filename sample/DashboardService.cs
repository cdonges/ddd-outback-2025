using System.Text.Json;

namespace Sample;

public class DashboardService : IDashboardService
{
    public async Task<string> Get(Guid userId)
    {
        var userDashboard = new { UserId = userId, Title = $"Dashboard for {userId}", Content = "" };
        await Task.Delay(5 * 1000);
        return JsonSerializer.Serialize(userDashboard);
    }
}
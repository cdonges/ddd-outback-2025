namespace Sample;

public interface IDashboardService
{
    Task<string> Get(Guid userId, int id);

    Guid RandomId();
}
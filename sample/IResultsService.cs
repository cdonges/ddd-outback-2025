namespace Sample;

public interface IResultsService
{
    void AddTiming(int id, TimeSpan elapsed);
    void AddCacheMiss(int id);

    void Print();
}
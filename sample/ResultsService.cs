using System.Collections.Concurrent;
using System.Text;
using Sample;
using Spectre.Console;

namespace Sample;

public class ResultsService : IResultsService
{
    Dictionary<int, ConcurrentBag<TimeSpan>> timings;
    ConcurrentBag<int> cacheMisses;

    public ResultsService(int count)
    {
        cacheMisses = new ConcurrentBag<int>();
        timings = Enumerable.Range(0, count).ToDictionary(x => x, y => new ConcurrentBag<TimeSpan>());
    }

    public void AddCacheMiss(int id)
    {
        Console.WriteLine($"{id}  - cache miss");
        cacheMisses.Add(id);
    }

    public void AddTiming(int id, TimeSpan elapsed)
    {
        Console.WriteLine($"{id} - {elapsed.TotalMilliseconds}");
        timings[id].Add(elapsed);
    }

    public void Print()
    {
        var table = new Table();
        table.AddColumn(new TableColumn("Machine").Alignment(Justify.Right));
        table.AddColumn(new TableColumn("Cache Miss").Alignment(Justify.Right));
        table.AddColumn(new TableColumn("Min").Alignment(Justify.Right));
        table.AddColumn(new TableColumn("Max").Alignment(Justify.Right));
        table.AddColumn(new TableColumn("Avg").Alignment(Justify.Right));

        foreach (var id in timings.Keys)
        {
            table.AddRow(
                id.ToString(),
                cacheMisses.Count(x => x == id).ToString(),
                timings[id].Select(x => x.TotalMilliseconds).Min().ToString("F"),
                timings[id].Select(x => x.TotalMilliseconds).Max().ToString("F"),
                timings[id].Select(x => x.TotalMilliseconds).Average().ToString("F"));
        }

        Console.WriteLine();

        var allTimings = timings.SelectMany(x => x.Value);
        table.AddRow(
            "all",
            cacheMisses.Count().ToString(),
            allTimings.Select(x => x.TotalMilliseconds).Min().ToString("F"),
            allTimings.Select(x => x.TotalMilliseconds).Max().ToString("F"),
            allTimings.Select(x => x.TotalMilliseconds).Average().ToString("F"));


        AnsiConsole.Write(table);
    }
}
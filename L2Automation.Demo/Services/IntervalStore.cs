using L2Automation.Demo.Domain;

namespace L2Automation.Demo.Services;

public class IntervalStore
{
    private readonly List<StateInterval> _intervals = [];

    public StateInterval? GetActive(string stationId)
        => _intervals.LastOrDefault(i => i.StationId == stationId && i.EndUtc == null);

    public void Add(StateInterval interval) => _intervals.Add(interval);

    public IReadOnlyList<StateInterval> GetAll() => _intervals.ToList();
}
using L2Automation.Demo.Domain;

namespace L2Automation.Demo.Services;

public class ProductionStore
{
    private readonly Dictionary<string, ProductionSnapshot> _byStation = new();

    public ProductionSnapshot Get(string stationId)
    {
        if (!_byStation.TryGetValue(stationId, out var snap))
        {
            snap = new ProductionSnapshot { StationId = stationId };
            _byStation[stationId] = snap;
        }

        return snap;
    }

    public void IncrementGood(string stationId, int amount = 1)
    {
        var s = Get(stationId);
        s.GoodCount += amount;
        s.LastUpdateUtc = DateTime.UtcNow;
    }

    public void IncrementScrap(string stationId, int amount = 1)
    {
        var s = Get(stationId);
        s.ScrapCount += amount;
        s.LastUpdateUtc = DateTime.UtcNow;
    }
}
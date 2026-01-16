namespace L2Automation.Demo.Domain;

public class ProductionSnapshot
{
    public string StationId { get; set; } = "ST01";
    public int GoodCount { get; set; }
    public int ScrapCount { get; set; }
    public DateTime LastUpdateUtc { get; set; } = DateTime.UtcNow;
}
namespace L2Automation.Demo.Domain;

public class StateInterval
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string StationId { get; set; } = "ST01";
    public MachineState State { get; set; }

    public DateTime StartUtc { get; set; }
    public DateTime? EndUtc { get; set; } // null = aktivan interval
}
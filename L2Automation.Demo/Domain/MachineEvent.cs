namespace L2Automation.Demo.Domain;

public class MachineEvent
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime TimestampUtc { get; set; } = DateTime.UtcNow;
    public string StationId { get; set; } = "ST01";
    public MachineEventType EventType { get; set; }
    public string? Payload { get; set; }
}
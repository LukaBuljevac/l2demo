using L2Automation.Demo.Domain;

namespace L2Automation.Demo.Services;

public class EventStore
{
    private readonly List<MachineEvent> _events = [];

    public void Add(MachineEvent machineEvent)
    {
        _events.Add(machineEvent);
    }

    public IReadOnlyList<MachineEvent> GetAll()
    {
        return _events.OrderByDescending(e => e.TimestampUtc).ToList();
    }
}
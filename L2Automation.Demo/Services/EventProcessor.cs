using L2Automation.Demo.Domain;

namespace L2Automation.Demo.Services;

public class EventProcessor
{
    private readonly EventStore _events;
    private readonly MachineStateService _state;
    private readonly IntervalStore _intervals;
    private readonly ProductionStore _production;

    public EventProcessor(EventStore events, MachineStateService state, IntervalStore intervals, ProductionStore production)
    {
        _events = events;
        _state = state;
        _intervals = intervals;
        _production = production;
    }

    public MachineState Process(MachineEvent ev)
    {
        // 1) spremi raw event (historian)
        _events.Add(ev);

        // 2) brojači proizvodnje
        if (ev.EventType == MachineEventType.PartProduced) _production.IncrementGood(ev.StationId);
        if (ev.EventType == MachineEventType.ScrapProduced) _production.IncrementScrap(ev.StationId);

        // 3) state transition
        var prev = _state.CurrentState;
        _state.ApplyEvent(ev.EventType);
        var next = _state.CurrentState;

        // 4) intervali: ako se stanje promijenilo, zatvori stari i otvori novi
        if (prev != next)
        {
            var active = _intervals.GetActive(ev.StationId);
            if (active != null)
                active.EndUtc = ev.TimestampUtc;

            _intervals.Add(new StateInterval
            {
                StationId = ev.StationId,
                State = next,
                StartUtc = ev.TimestampUtc,
                EndUtc = null
            });
        }
        else
        {
            // ako nema aktivnog intervala (npr. prvi event), otvori ga
            if (_intervals.GetActive(ev.StationId) == null)
            {
                _intervals.Add(new StateInterval
                {
                    StationId = ev.StationId,
                    State = next,
                    StartUtc = ev.TimestampUtc,
                    EndUtc = null
                });
            }
        }

        return next;
    }
}

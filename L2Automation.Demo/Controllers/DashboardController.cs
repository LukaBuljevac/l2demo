using L2Automation.Demo.Domain;
using L2Automation.Demo.Services;
using Microsoft.AspNetCore.Mvc;

namespace L2Automation.Demo.Controllers;

[ApiController]
public class DashboardController : ControllerBase
{
    private readonly MachineStateService _state;
    private readonly IntervalStore _intervals;
    private readonly ProductionStore _production;

    public DashboardController(MachineStateService state, IntervalStore intervals, ProductionStore production)
    {
        _state = state;
        _intervals = intervals;
        _production = production;
    }

    [HttpGet("api/status")]
    public IActionResult GetStatus([FromQuery] string stationId = "ST01")
    {
        var active = _intervals.GetActive(stationId);
        return Ok(new
        {
            StationId = stationId,
            State = _state.CurrentState,
            ActiveInterval = active
        });
    }

    [HttpGet("api/production/today")]
    public IActionResult GetProductionToday([FromQuery] string stationId = "ST01")
    {
        // Za in-memory demo: vraćamo trenutne brojače
        // Kasnije u SQL-u: filtriramo po datumu/shiftu.
        return Ok(_production.Get(stationId));
    }

    [HttpGet("api/downtime/today")]
    public IActionResult GetDowntimeToday([FromQuery] string stationId = "ST01")
    {
        var from = DateTime.UtcNow.Date;
        var to = DateTime.UtcNow;

        var intervals = _intervals
            .GetAll()
            .Where(i => i.StationId == stationId)
            .Select(i => new
            {
                i.State,
                Start = i.StartUtc,
                End = i.EndUtc ?? to
            })
            .Where(i => i.End > from && i.Start < to)
            .Select(i =>
            {
                var s = i.Start < from ? from : i.Start;
                var e = i.End > to ? to : i.End;
                return new { i.State, Duration = e - s };
            })
            .GroupBy(x => x.State)
            .Select(g => new
            {
                State = g.Key,
                Seconds = (int)g.Sum(x => x.Duration.TotalSeconds)
            })
            .OrderByDescending(x => x.Seconds)
            .ToList();

        return Ok(new { StationId = stationId, FromUtc = from, ToUtc = to, Totals = intervals });
    }
}

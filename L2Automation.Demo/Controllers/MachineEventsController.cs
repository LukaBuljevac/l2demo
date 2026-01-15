using L2Automation.Demo.Domain;
using L2Automation.Demo.Services;
using Microsoft.AspNetCore.Mvc;

namespace L2Automation.Demo.Controllers;

[ApiController]
[Route("api/events")]
public class MachineEventsController : ControllerBase
{
    private readonly EventStore _eventStore;
    private readonly MachineStateService _stateService;

    public MachineEventsController(EventStore eventStore, MachineStateService stateService)
    {
        _eventStore = eventStore;
        _stateService = stateService;
    }

    [HttpPost]
    public IActionResult PostEvent(MachineEvent machineEvent)
    {
        _stateService.ApplyEvent(machineEvent.EventType);
        _eventStore.Add(machineEvent);

        return Ok(new
        {
            State = _stateService.CurrentState,
            EventId = machineEvent.Id
        });
    }

    [HttpGet]
    public IActionResult GetEvents()
    {
        return Ok(_eventStore.GetAll());
    }
}
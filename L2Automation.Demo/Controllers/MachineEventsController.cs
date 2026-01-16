using L2Automation.Demo.Domain;
using L2Automation.Demo.Services;
using Microsoft.AspNetCore.Mvc;

namespace L2Automation.Demo.Controllers;

[ApiController]
[Route("api/events")]
public class MachineEventsController : ControllerBase
{
    private readonly EventProcessor _processor;
    private readonly EventStore _eventStore;

    public MachineEventsController(EventProcessor processor, EventStore eventStore)
    {
        _processor = processor;
        _eventStore = eventStore;
    }

    [HttpPost]
    public IActionResult PostEvent(MachineEvent machineEvent)
    {
        var state = _processor.Process(machineEvent);

        return Ok(new
        {
            State = state,
            EventId = machineEvent.Id
        });
    }

    [HttpGet]
    public IActionResult GetEvents() => Ok(_eventStore.GetAll());
}
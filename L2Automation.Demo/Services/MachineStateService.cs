using L2Automation.Demo.Domain;

namespace L2Automation.Demo.Services;

public class MachineStateService
{
    public MachineState CurrentState { get; private set; } = MachineState.Stopped;

    public void ApplyEvent(MachineEventType eventType)
    {
        CurrentState = (CurrentState, eventType) switch
        {
            (_, MachineEventType.Start) => MachineState.Running,
            (_, MachineEventType.Stop) => MachineState.Stopped,

            (MachineState.Running, MachineEventType.AlarmRaised) => MachineState.Alarm,
            (MachineState.Alarm, MachineEventType.AlarmCleared) => MachineState.Running,

            (_, MachineEventType.ChangeoverStart) => MachineState.Changeover,
            (MachineState.Changeover, MachineEventType.ChangeoverEnd) => MachineState.Running,

            _ => CurrentState
        };
    }
}
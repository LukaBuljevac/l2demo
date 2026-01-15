namespace L2Automation.Demo.Domain;

public enum MachineEventType
{
    Start,
    Stop,
    AlarmRaised,
    AlarmCleared,
    PartProduced,
    ScrapProduced,
    ChangeoverStart,
    ChangeoverEnd
}
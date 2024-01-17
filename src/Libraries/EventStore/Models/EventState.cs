namespace EventStore.Models;
public enum EventState
{
    NotPublished = 0,
    InProgress = 1,
    Published = 2,
    Failed = 3
}

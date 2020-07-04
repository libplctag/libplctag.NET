namespace libplctag
{
    public enum Events
    {
        PLCTAG_EVENT_READ_STARTED = 1,
        PLCTAG_EVENT_READ_COMPLETED = 2,
        PLCTAG_EVENT_WRITE_STARTED = 3,
        PLCTAG_EVENT_WRITE_COMPLETED = 4,
        PLCTAG_EVENT_ABORTED = 5,
        PLCTAG_EVENT_DESTROYED = 6
    }
}
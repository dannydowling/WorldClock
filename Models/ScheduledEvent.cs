using System;
using System.Drawing;

namespace WorldClock
{
    public enum EventType
    {
        FlightArrival,
        FlightDeparture,
        Meeting,
        Reminder
    }

    public class ScheduledEvent
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime EventTime { get; set; }
        public string Description { get; set; }
        public EventType Type { get; set; }
        public Color IndicatorColor { get; set; }
        public TimeZoneInfo TimeZone { get; set; }

        public ScheduledEvent()
        {
            Id = Guid.NewGuid();
            // Default color based on event type
            IndicatorColor = Color.Blue;
        }

        // Get the event time in UTC
        public DateTime GetEventTimeUtc()
        {
            return TimeZoneInfo.ConvertTimeToUtc(EventTime, TimeZone);
        }

        // Get the event time in a specific time zone
        public DateTime GetEventTimeIn(TimeZoneInfo targetTimeZone)
        {
            DateTime utcTime = GetEventTimeUtc();
            return TimeZoneInfo.ConvertTimeFromUtc(utcTime, targetTimeZone);
        }
    }
}
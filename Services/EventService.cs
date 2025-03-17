using System;
using System.Collections.Generic;
using System.Linq;

namespace WorldClock
{
    public class EventService
    {
        private List<ScheduledEvent> _events;

        public EventService()
        {
            _events = new List<ScheduledEvent>();
        }

        public void AddEvent(ScheduledEvent scheduledEvent)
        {
            _events.Add(scheduledEvent);
        }
        public void ClearEvents()
        {
            _events.Clear();
        }

        public void RemoveEvent(Guid eventId)
        {
            _events.RemoveAll(e => e.Id == eventId);
        }

        public List<ScheduledEvent> GetAllEvents()
        {
            return _events;
        }

        public List<ScheduledEvent> GetEventsForTimeZone(TimeZoneInfo timeZoneInfo)
        {
            return _events.Where(e => e.TimeZone.Id == timeZoneInfo.Id).ToList();
        }

        public List<ScheduledEvent> GetEventsForDay(DateTime date, TimeZoneInfo timeZoneInfo)
        {
            // Get all events and convert times to the specified time zone
            return _events
                .Select(e => new
                {
                    Event = e,
                    TimeInZone = e.GetEventTimeIn(timeZoneInfo)
                })
                .Where(x => x.TimeInZone.Date == date.Date)
                .OrderBy(x => x.TimeInZone)
                .Select(x => x.Event)
                .ToList();
        }
    }
}
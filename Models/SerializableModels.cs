using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using WorldClock;

namespace WorldClock
{
    // Serializable models for saving/loading data

    [Serializable]
    public class SerializableTimeZone
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string TimeZoneId { get; set; }

        public SerializableTimeZone()
        {
            // Default constructor for serialization
        }

        public SerializableTimeZone(TimeZoneModel model)
        {
            Name = model.Name;
            DisplayName = model.DisplayName;
            TimeZoneId = model.TimeZoneInfo.Id;
        }

        public TimeZoneModel ToTimeZoneModel()
        {
            try
            {
                var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(TimeZoneId);
                return new TimeZoneModel(Name, DisplayName, timeZoneInfo);
            }
            catch (Exception)
            {
                // Fallback to UTC if the time zone isn't found
                return new TimeZoneModel(Name, DisplayName, TimeZoneInfo.Utc);
            }
        }
    }

    [Serializable]
    public class SerializableEvent
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime EventTime { get; set; }
        public string Description { get; set; }
        public EventType Type { get; set; }

        // Store color as ARGB value
        public int ColorArgb { get; set; }

        // Store time zone ID instead of the TimeZoneInfo object
        public string TimeZoneId { get; set; }

        public SerializableEvent()
        {
            // Default constructor for serialization
        }

        public SerializableEvent(ScheduledEvent scheduledEvent)
        {
            Id = scheduledEvent.Id;
            Title = scheduledEvent.Title;
            EventTime = scheduledEvent.EventTime;
            Description = scheduledEvent.Description;
            Type = scheduledEvent.Type;
            ColorArgb = scheduledEvent.IndicatorColor.ToArgb();
            TimeZoneId = scheduledEvent.TimeZone.Id;
        }

        public ScheduledEvent ToScheduledEvent()
        {
            var result = new ScheduledEvent
            {
                Id = Id,
                Title = Title,
                EventTime = EventTime,
                Description = Description,
                Type = Type,
                IndicatorColor = Color.FromArgb(ColorArgb)
            };

            try
            {
                result.TimeZone = TimeZoneInfo.FindSystemTimeZoneById(TimeZoneId);
            }
            catch (Exception)
            {
                // Fallback to UTC if the time zone isn't found
                result.TimeZone = TimeZoneInfo.Utc;
            }

            return result;
        }
    }

    [Serializable]
    public class ApplicationData
    {
        public List<SerializableTimeZone> TimeZones { get; set; }
        public List<SerializableEvent> Events { get; set; }

        public ApplicationData()
        {
            TimeZones = new List<SerializableTimeZone>();
            Events = new List<SerializableEvent>();
        }
    }
}
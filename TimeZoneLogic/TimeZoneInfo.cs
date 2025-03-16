// File: Models/TimeZoneInfo.cs
using System;

namespace WorldClock.Models
{
    public class TimeZoneModel
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public TimeZoneInfo TimeZoneInfo { get; set; }

        public TimeZoneModel(string name, string displayName, TimeZoneInfo timeZoneInfo)
        {
            Name = name;
            DisplayName = displayName;
            TimeZoneInfo = timeZoneInfo;
        }

        public DateTime GetCurrentTime()
        {
            return TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo);
        }
    }
}
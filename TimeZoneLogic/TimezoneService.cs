// -- TimezoneDetector/Services/TimezoneService.cs --
using System;
using System.Collections.Generic;
using System.Linq;

namespace WorldClock
{
    /// <summary>
    /// Service class to handle timezone operations
    /// </summary>
    public class TimezoneService
    {
        private Dictionary<string, TimeZoneInfo> _availableTimezones;

        public TimezoneService()
        {
            // Initialize the dictionary of available timezones
            LoadAvailableTimezones();
        }

        /// <summary>
        /// Load all system timezones into a dictionary
        /// </summary>
        private void LoadAvailableTimezones()
        {
            _availableTimezones = new Dictionary<string, TimeZoneInfo>();

            foreach (TimeZoneInfo zone in TimeZoneInfo.GetSystemTimeZones().OrderBy(z => z.BaseUtcOffset))
            {
                string displayName = $"{zone.DisplayName} ({FormatOffset(zone.BaseUtcOffset)})";
                _availableTimezones.Add(displayName, zone);
            }
        }

        /// <summary>
        /// Get all available timezone display names
        /// </summary>
        public List<string> GetAvailableTimezones()
        {
            return _availableTimezones.Keys.ToList();
        }

        /// <summary>
        /// Get the local timezone display name
        /// </summary>
        public string GetLocalTimezoneName()
        {
            return TimeZoneInfo.Local.DisplayName;
        }

        /// <summary>
        /// Get the local UTC offset
        /// </summary>
        public string GetLocalUtcOffset()
        {
            return TimeZoneInfo.Local.GetUtcOffset(DateTime.Now).ToString();
        }

        /// <summary>
        /// Get a default timezone to select from the popular timezones
        /// </summary>
        public string GetDefaultTimezone()
        {
            // Try to find a popular timezone from the list
            string[] popularZones = {
                "Pacific Standard Time",
                "Eastern Standard Time",
                "GMT Standard Time",
                "Central Europe Standard Time",
                "India Standard Time",
                "Tokyo Standard Time",
                "AUS Eastern Standard Time"
            };

            foreach (string popularZone in popularZones)
            {
                try
                {
                    TimeZoneInfo zone = TimeZoneInfo.FindSystemTimeZoneById(popularZone);
                    string key = _availableTimezones.Keys.FirstOrDefault(k => k.Contains(zone.StandardName));
                    if (key != null)
                    {
                        return key;
                    }
                }
                catch
                {
                    // Ignore if timezone not found
                }
            }

            // Try to find local timezone as fallback
            string localKey = _availableTimezones.Keys
                .FirstOrDefault(k => k.Contains(TimeZoneInfo.Local.StandardName));

            return localKey;
        }

        /// <summary>
        /// Get detailed information about a specific timezone
        /// </summary>
        public TimezoneInfoDTO GetTimezoneInfo(string timezoneKey)
        {
            if (!_availableTimezones.ContainsKey(timezoneKey))
            {
                return new TimezoneInfoDTO
                {
                    DisplayName = "Unknown timezone",
                    CurrentTime = "-",
                    OffsetFromLocal = "-"
                };
            }

            TimeZoneInfo selectedZone = _availableTimezones[timezoneKey];
            DateTime now = DateTime.Now;
            DateTime utcNow = DateTime.UtcNow;
            DateTime zoneTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, selectedZone);

            TimeSpan offsetFromLocal = selectedZone.GetUtcOffset(utcNow) -
                                      TimeZoneInfo.Local.GetUtcOffset(now);

            return new TimezoneInfoDTO
            {
                DisplayName = selectedZone.DisplayName,
                CurrentTime = zoneTime.ToString(),
                OffsetFromLocal = $"{FormatOffset(offsetFromLocal)} hours"
            };
        }

        /// <summary>
        /// Format a TimeSpan offset in a readable format (+/-HH:MM)
        /// </summary>
        private string FormatOffset(TimeSpan offset)
        {
            string sign = offset.TotalHours >= 0 ? "+" : "-";
            int hours = Math.Abs((int)offset.TotalHours);
            int minutes = Math.Abs(offset.Minutes);

            return $"{sign}{hours:D2}:{minutes:D2}";
        }
    }
}
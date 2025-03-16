// File: Services/TimeZoneService.cs - Update to get all system time zones
using System;
using System.Collections.Generic;
using System.Linq;
using WorldClock.Models;

namespace WorldClock.Services
{
    public class TimeZoneService
    {
        private List<TimeZoneModel> _timeZones;

        public TimeZoneService()
        {
            InitializeTimeZones();
        }

        private void InitializeTimeZones()
        {
            _timeZones = new List<TimeZoneModel>
            {
                new TimeZoneModel("UTC", "UTC (Coordinated Universal Time)", TimeZoneInfo.Utc),
                new TimeZoneModel("GMT", "London (GMT)", TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time")),
                new TimeZoneModel("CET", "Paris (Central European Time)", TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time")),
                new TimeZoneModel("NZST", "Tauranga (New Zealand)", TimeZoneInfo.FindSystemTimeZoneById("New Zealand Standard Time")),
                new TimeZoneModel("AEDT", "Melbourne (Australia)", TimeZoneInfo.FindSystemTimeZoneById("AUS Eastern Standard Time")),
                new TimeZoneModel("AKST", "Juneau (Alaska)", TimeZoneInfo.FindSystemTimeZoneById("Alaskan Standard Time")),
                new TimeZoneModel("GST", "Dubai (UAE)", TimeZoneInfo.FindSystemTimeZoneById("Arabian Standard Time"))
                
            };
        }

        // Gets all available system time zones for the dropdown
        public List<KeyValuePair<string, string>> GetAllAvailableTimeZones()
        {
            var availableTimeZones = TimeZoneInfo.GetSystemTimeZones()
                .OrderBy(tz => tz.DisplayName)
                .Select(tz => new KeyValuePair<string, string>(
                    tz.Id,
                    $"{tz.DisplayName} ({tz.BaseUtcOffset:hh\\:mm})"
                ))
                .ToList();

            return availableTimeZones;
        }

        // Add a new time zone to the collection
        public void AddTimeZone(string id, string displayName)
        {
            // Check if already exists
            if (_timeZones.Any(tz => tz.TimeZoneInfo.Id == id))
                return;

            try
            {
                var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(id);
                string abbreviation = GetTimeZoneAbbreviation(timeZoneInfo);

                var model = new TimeZoneModel(
                    abbreviation,
                    displayName,
                    timeZoneInfo
                );

                _timeZones.Add(model);
            }
            catch (Exception)
            {
                // Handle exception if time zone not found
            }
        }

        // Helper to generate abbreviation
        private string GetTimeZoneAbbreviation(TimeZoneInfo tzi)
        {
            var words = tzi.DisplayName.Split(' ');
            if (words.Length > 1)
            {
                // Try to extract abbreviation from display name
                string abbr = string.Concat(words.Where(w => char.IsUpper(w[0])).Select(w => w[0]));
                if (abbr.Length > 0)
                    return abbr;
            }

            // Fallback to offset-based abbreviation
            return $"UTC{tzi.BaseUtcOffset:hh\\:mm}";
        }

        // Remove a time zone from the collection
        public void RemoveTimeZone(string name)
        {
            _timeZones.RemoveAll(tz => tz.Name == name);
        }

        public List<TimeZoneModel> GetAllTimeZones()
        {
            return _timeZones;
        }

        public TimeZoneModel GetTimeZoneByName(string name)
        {
            return _timeZones.Find(tz => tz.Name == name);
        }
    }
}
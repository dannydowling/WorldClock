﻿using System;
using System.Collections.Generic;
using System.Linq;


namespace WorldClock
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

        public void ClearTimeZones()
        {
            _timeZones.Clear();
        }

        public void AddTimeZoneModel(TimeZoneModel model)
        {
            _timeZones.Add(model);
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
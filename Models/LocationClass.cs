using System;
using WorldClock;

namespace WorldClock
{
    public class LocationClass
    {
        public string country { get; set; }
        public string state { get; set; }
        public string city { get; set; }
        public string name { get; set; }
        public string icao { get; set; }
        public double lat { get; set; }
        public double lon { get; set; }

        // Additional properties for our app
        public string TimeZoneId { get; set; }

        // This will be used as the display text in dropdown
        public string DisplayText => $"{icao} - {name} ({city}, {country})";

        // This will be used as the value in dropdown
        public string Code => icao;

        // Convert to our Airport class
        public Airport ToAirport()
        {
            return new Airport
            {
                Code = icao,
                Name = name,
                City = city,
                Country = country,
                Latitude = lat,
                Longitude = lon,
                TimeZoneId = TimeZoneId ?? GetEstimatedTimeZoneId() // Use estimated if not set
            };
        }

        // Estimate timezone based on longitude (very rough estimate)
        private string GetEstimatedTimeZoneId()
        {
            // Very simple estimation based on longitude
            if (lon > 142.5) return "Tokyo Standard Time"; // Far East
            else if (lon > 112.5) return "China Standard Time"; // China
            else if (lon > 52.5) return "Arabian Standard Time"; // Middle East
            else if (lon > 7.5) return "Central European Standard Time"; // Europe
            else if (lon > -22.5) return "GMT Standard Time"; // UK
            else if (lon > -67.5) return "Eastern Standard Time"; // Eastern US
            else if (lon > -112.5) return "Pacific Standard Time"; // Western US
            else if (lon > -157.5) return "Alaskan Standard Time"; // Alaska
            else if (lon > -172.5) return "Hawaiian Standard Time"; // Hawaii
            else return "New Zealand Standard Time"; // NZ/Dateline

            // Note: This is a very rough estimation and should be improved
            // Consider using a proper timezone mapping library or database
        }
    }
}
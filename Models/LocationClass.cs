using System;
using WorldClock;

namespace WorldClock
{
    public class LocationClass
    {
        public string country { get; set; }
        public string? state { get; set; }
        public string city { get; set; }
        public string name { get; set; }
        public string icao { get; set; }
        public double lat { get; set; }
        public double lon { get; set; }


        public string TimeZoneId { get; set; }

        // dropdown
        public string DisplayText => $"{icao} - {name} ({city}, {country})";

        public string Code => icao;

        
        public Airport ToAirport(LocationClass location)
        {
            return new Airport
            {
                Code = icao,
                Name = name,
                City = city,
                Country = country,
                Latitude = lat,
                Longitude = lon,
                TimeZoneId = TimeZoneId ??GetTimeZone(country, state) // Use estimated if not set
            };
        }

        private string GetTimeZone(string country, string? state)
        {
            var locationToTimezone = new LocationToTimezone();
            return locationToTimezone.GetTimezoneUsingTimeZoneConverter(country, state);
        }
    }
}
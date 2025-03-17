using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace WorldClock
{
    enum PickerType { Country, State, City}
    internal class LocationLogic
    {
        private ReadZipFileLogic zipFileLogic = new ReadZipFileLogic();

        public LocationLogic()
        {
            var cacheDir = Path.GetTempPath();
            if (!File.Exists(cacheDir + "/Locations.Zip"))
            {
                File.WriteAllBytes(cacheDir + "/Locations.Zip", Properties.Resources.locations);
            }
        }

        internal IEnumerable<string> GetCountries()
        { return zipFileLogic.ReadEntriesInZipFile("Countries/", PickerType.Country).Distinct(); }

        internal IEnumerable<string> GetStates(string country)
        {
            return zipFileLogic.ReadEntriesInZipFile($"Countries/{country}/", PickerType.State).Distinct();
        }

        internal IEnumerable<string> GetCities(string country, string stateName)
        { return zipFileLogic.ReadEntriesInZipFile($"Countries/{country}/{stateName}/", PickerType.City).Distinct(); }

        internal IEnumerable<string> GetAirports(string countryName, string stateName, string cityName)
        { return LocationsLookup(countryName, stateName, cityName).Select(x => x.name).Distinct(); }

        internal List<LocationClass> LocationsLookup(string countryName, string stateName, string cityName)
        { return zipFileLogic.ReadSpecificEntryInZipFile($"Countries/{countryName}/{stateName}/{cityName}/{cityName}.txt"); }

        internal LocationClass SpecificLocationLookup(string countryName, string stateName, string cityName, string airportName)
        { return LocationsLookup(countryName, stateName, cityName).Single(x => x.name == airportName); }
    }
}

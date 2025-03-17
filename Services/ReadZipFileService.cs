using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldClock
{

    enum PickerType { Country, State, City }
    internal class ReadZipFileLogic
    {
        public ReadZipFileLogic()
        {
            var cacheDir = Path.GetTempPath();
            if (!File.Exists(cacheDir + "/Locations.Zip"))
            {
                File.WriteAllBytes(cacheDir + "/Locations.Zip", Properties.Resources.locations);
            }
        }

        internal IEnumerable<string> GetCountries()
        { return ReadEntriesInZipFile("Countries/", PickerType.Country).Distinct(); }

        internal IEnumerable<string> GetStates(string country)
        {
            return ReadEntriesInZipFile($"Countries/{country}/", PickerType.State).Distinct();
        }

        internal IEnumerable<string> GetCities(string country, string stateName)
        { return ReadEntriesInZipFile($"Countries/{country}/{stateName}/", PickerType.City).Distinct(); }

        internal IEnumerable<string> GetAirports(string countryName, string stateName, string cityName)
        { return LocationsLookup(countryName, stateName, cityName).Select(x => x.name).Distinct(); }

        internal List<LocationClass> LocationsLookup(string countryName, string stateName, string cityName)
        { return ReadSpecificEntryInZipFile($"Countries/{countryName}/{stateName}/{cityName}/{cityName}.txt"); }

        internal LocationClass SpecificLocationLookup(string countryName, string stateName, string cityName, string airportName)
        { return LocationsLookup(countryName, stateName, cityName).Single(x => x.name == airportName); }

        private IEnumerable<string> ReadEntriesInZipFile(string path, PickerType type)
        {
            var cacheDir = Path.GetTempPath();

            using (ZipArchive archive = ZipFile.Open((cacheDir + "/Locations.Zip"), ZipArchiveMode.Read))
            {
                List<ZipArchiveEntry> entries = new List<ZipArchiveEntry>();

                List<string> deepestFolderNames = new List<string>();
                StringBuilder sb = new StringBuilder();



                foreach (ZipArchiveEntry entry in archive.Entries.Where(x => x.FullName.StartsWith(path)))
                {
                    entries.Add(entry);
                    sb.Clear();
                    sb.Append(entry.FullName);
                    sb.Remove(0, path.Length);
                    deepestFolderNames.Add(sb.ToString());
                }

                for (int i = 0; i < entries.Count; i++)
                {
                    if (type == PickerType.Country)
                    {
                        //split the part of the remaining path up by folders
                        string[] subStrings = deepestFolderNames[i].Split('/');

                        foreach (var item in subStrings)
                        {
                            if (subStrings[0] != "")
                            {
                                if (subStrings[0] != "Countries")
                                {
                                    //add each folder name to the collection
                                    deepestFolderNames[i] = subStrings[0];
                                }
                            }

                        }
                    }
                    if (type == PickerType.State)
                    {
                        //split the part of the remaining path up by folders
                        string[] subStrings = deepestFolderNames[i].Split('/');

                        foreach (var item in subStrings)
                        {
                            if (subStrings[0] != "")
                            {
                                //add each folder name to the collection
                                deepestFolderNames[i] = subStrings[0];
                            }
                        }
                    }
                    if (type == PickerType.City)
                    {
                        //split the part of the remaining path up by folders
                        string[] subStrings = deepestFolderNames[i].Split('/');

                        foreach (var item in subStrings)
                        {
                            //add each folder name to the collection
                            deepestFolderNames[i] = subStrings[0];
                        }
                    }
                }
                // var result = deepestFolderNames.Distinct().Where(x => archive.GetEntry(x).Name != null);

                //and return it.
                return deepestFolderNames.Distinct();
            }
        }

        public List<LocationClass> ReadSpecificEntryInZipFile(string path)
        {
            var cacheDir = Path.GetTempPath();

            using (ZipArchive archive = ZipFile.Open((cacheDir + "/Locations.Zip"), ZipArchiveMode.Read))
            {
                List<LocationClass> locationlist = new List<LocationClass>();
                foreach (ZipArchiveEntry entry in archive.Entries.Where(x => x.FullName.StartsWith(path)))
                {

                    LocationClass locationClass = new LocationClass();

                    using Stream fileContents = entry.Open();
                    using (var reader = new StreamReader(fileContents))
                    {
                        string content = reader.ReadToEnd();
                        locationlist = JArray.Parse(content).Select(x => new LocationClass
                        {
                            country = x["country"].ToString(),
                            state = x["state"].ToString(),
                            city = x["city"].ToString(),
                            name = x["name"].ToString(),
                            icao = x["icao"].ToString(),
                            lat = Convert.ToDouble(x["lat"]),
                            lon = Convert.ToDouble(x["lon"])

                        }).ToList();
                    }
                }
                return locationlist;
            }
        }
    }
}
using System;
using TimeZoneConverter;
using System.Text.Json;
using System.Threading.Tasks;

namespace WorldClock
{
    public class LocationToTimezone
    {
     
        public string GetTimezoneUsingTimeZoneConverter(string country, string state = "")
        {        

            try
            {                
                string windowsTimezoneId;

                if (country.ToUpper() == "US" && !string.IsNullOrEmpty(state))
                {
                    
                    windowsTimezoneId = MapUSStateToTimezone(state);
                }
                else
                {
                    
                    windowsTimezoneId = MapCountryToTimezone(country);
                }               

                return windowsTimezoneId;
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        private string MapUSStateToTimezone(string stateAbbreviation)
        {
            
            switch (stateAbbreviation.ToUpper())
            {
                case "AL":
                case "AR":
                case "FL":
                case "GA":
                case "IL":
                case "IN":
                case "KY":
                case "LA":
                case "MI":
                case "MO":
                case "MS":
                case "TN":
                case "SC":
                case "NC":
                    return "Central Standard Time"; 

                case "CT":
                case "DE":
                case "ME":
                case "MD":
                case "MA":
                case "NH":
                case "NJ":
                case "NY":
                case "OH":
                case "PA":
                case "RI":
                case "VT":
                case "VA":
                case "WV":
                case "DC":
                    return "Eastern Standard Time";

                case "AZ":
                case "CO":
                case "ID":
                case "KS":
                case "MT":
                case "NE":
                case "NM":
                case "ND":
                case "OK":
                case "SD":
                case "TX":
                case "UT":
                case "WY":
                    return "Mountain Standard Time";

                case "CA":
                case "NV":
                case "OR":
                case "WA":
                    return "Pacific Standard Time";

                case "AK":
                    return "Alaskan Standard Time";

                case "HI":
                    return "Hawaiian Standard Time";

                default:
                    return "Eastern Standard Time"; // Default fallback
            }
        }

        private string MapCountryToTimezone(string countryCode)
        {
            // Very simplified mapping, only includes a few examples
            switch (countryCode.ToUpper())
            {
                // Europe
                case "GB":
                case "IE":
                case "PT":
                case "IS":
                    return "GMT Standard Time"; // UTC+0

                case "DE":
                case "FR":
                case "IT":
                case "ES":
                case "NL":
                case "BE":
                case "DK":
                case "SE":
                case "NO":
                case "PL":
                case "CZ":
                case "AT":
                case "CH":
                case "SK":
                case "HU":
                case "SI":
                case "HR":
                case "BA":
                case "ME":
                case "AL":
                case "MK":
                case "RS":
                case "XK":
                case "LU":
                case "MC":
                case "SM":
                case "VA":
                case "MT":
                    return "Central European Standard Time"; // UTC+1

                case "FI":
                case "EE":
                case "LV":
                case "LT":
                case "UA":
                case "RO":
                case "BG":
                case "GR":
                case "CY":
                case "MD":
                case "TR":
                    return "E. Europe Standard Time"; // UTC+2

                case "BY":
                case "RU": // Western part of Russia
                    return "Russian Standard Time"; // UTC+3

                // Africa
                case "MA":
                case "DZ":
                case "TN":
                case "LY":
                case "EG":
                    return "Egypt Standard Time"; // UTC+2

                case "NG":
                case "CD":
                case "SS":
                case "SD":
                case "ET":
                case "KE":
                case "TZ":
                case "UG":
                case "SO":
                case "RW":
                case "BI":
                case "ZA":
                case "NA":
                case "BW":
                case "ZW":
                case "MZ":
                case "ZM":
                case "MW":
                    return "E. Africa Standard Time"; // UTC+3

                case "SN":
                case "GH":
                case "CI":
                case "ML":
                case "NE":
                case "BF":
                case "GN":
                    return "Greenwich Standard Time"; // UTC+0

                // Asia
                case "IL":
                case "JO":
                case "LB":
                case "SY":
                case "IQ":
                case "SA":
                case "YE":
                case "QA":
                case "BH":
                case "KW":
                case "AE":
                case "OM":
                    return "Arab Standard Time"; // UTC+3

                case "IR":
                case "AF":
                    return "Iran Standard Time"; // UTC+3:30/UTC+4:30

                case "PK":
                    return "Pakistan Standard Time"; // UTC+5

                case "IN":
                    return "India Standard Time"; // UTC+5:30

                case "BD":
                case "BT":
                case "NP":
                    return "Bangladesh Standard Time"; // UTC+6

                case "MM":
                case "TH":
                case "KH":
                case "LA":
                case "VN":
                    return "SE Asia Standard Time"; // UTC+7

                case "CN": // Most of China
                    return "China Standard Time"; // UTC+8

                case "HK":
                case "MO":
                case "TW":
                case "MY":
                case "SG":
                case "PH":
                case "ID": // Most of Indonesia
                    return "Singapore Standard Time"; // UTC+8

                case "JP":
                case "KR":
                case "KP":
                    return "Tokyo Standard Time"; // UTC+9

                // Oceania
                case "AU": // Most of Australia
                    return "AUS Eastern Standard Time"; // UTC+10

                case "NZ":
                case "FJ":
                    return "New Zealand Standard Time"; // UTC+12

                // Americas
                case "BR": // Most of Brazil
                    return "E. South America Standard Time"; // UTC-3

                case "AR":
                case "CL":
                case "PY":
                case "UY":
                    return "Argentina Standard Time"; // UTC-3

                case "VE":
                case "BO":
                case "GY":
                case "SR":
                case "GF":
                    return "SA Eastern Standard Time"; // UTC-4

                case "CO":
                case "PE":
                case "EC":
                case "PA":
                    return "SA Pacific Standard Time"; // UTC-5

                case "MX": // Most of Mexico
                    return "Central Standard Time (Mexico)"; // UTC-6

                case "CA": // Most of Canada
                    return "Eastern Standard Time"; // UTC-5

                // Default
                default:
                    return "UTC"; // Default fallback
            }
        }
    }
}
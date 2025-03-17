using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace WorldClock
{
    public partial class FlightCalculatorForm : Form
    {
        private TimeZoneModel _timeZoneModel;
        private Dictionary<string, Airport> _airports;

        // UI Controls
        private ComboBox _originAirportComboBox;
        private ComboBox _destinationAirportComboBox;
        private Label _distanceResultLabel;
        private Label _flightTimeResultLabel;
        private Label _localDepartureTimeLabel;
        private Label _localArrivalTimeLabel;
        private DateTimePicker _departureDateTimePicker;
        private Button _calculateButton;

        public FlightCalculatorForm(TimeZoneModel timeZoneModel)
        {
            _timeZoneModel = timeZoneModel;
            _airports = LoadAirportData();
            InitializeComponent();
            InitializeCustomComponents();
            this.Text = $"Flight Calculator - {_timeZoneModel.DisplayName}";
        }

        
        private void InitializeCustomComponents()
        {
            this.Padding = new Padding(20);

            // Origin airport controls
            Label originLabel = new Label
            {
                Text = "Origin Airport:",
                Location = new Point(20, 30),
                AutoSize = true
            };

            _originAirportComboBox = new ComboBox
            {
                Location = new Point(150, 27),
                Width = 300,
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            // Destination airport controls
            Label destinationLabel = new Label
            {
                Text = "Destination Airport:",
                Location = new Point(20, 70),
                AutoSize = true
            };

            _destinationAirportComboBox = new ComboBox
            {
                Location = new Point(150, 67),
                Width = 300,
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            // Departure date/time controls
            Label departureLabel = new Label
            {
                Text = "Departure Time:",
                Location = new Point(20, 110),
                AutoSize = true
            };

            _departureDateTimePicker = new DateTimePicker
            {
                Location = new Point(150, 107),
                Width = 300,
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "MM/dd/yyyy hh:mm tt"
            };

            // Calculate button
            _calculateButton = new Button
            {
                Text = "Calculate",
                Location = new Point(200, 150),
                Width = 120,
                Height = 30
            };
            _calculateButton.Click += CalculateButton_Click;

            // Results panel
            GroupBox resultsPanel = new GroupBox
            {
                Text = "Flight Results",
                Location = new Point(20, 200),
                Size = new Size(460, 170),
                Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom
            };

            // Distance result
            Label distanceLabel = new Label
            {
                Text = "Distance:",
                Location = new Point(20, 30),
                AutoSize = true
            };

            _distanceResultLabel = new Label
            {
                Text = "-",
                Location = new Point(150, 30),
                AutoSize = true,
                Font = new Font(this.Font, FontStyle.Bold)
            };

            // Flight time result
            Label flightTimeLabel = new Label
            {
                Text = "Flight Time:",
                Location = new Point(20, 60),
                AutoSize = true
            };

            _flightTimeResultLabel = new Label
            {
                Text = "-",
                Location = new Point(150, 60),
                AutoSize = true,
                Font = new Font(this.Font, FontStyle.Bold)
            };

            // Local departure time
            Label localDepartureLabel = new Label
            {
                Text = "Local Departure:",
                Location = new Point(20, 90),
                AutoSize = true
            };

            _localDepartureTimeLabel = new Label
            {
                Text = "-",
                Location = new Point(150, 90),
                AutoSize = true
            };

            // Local arrival time
            Label localArrivalLabel = new Label
            {
                Text = "Local Arrival:",
                Location = new Point(20, 120),
                AutoSize = true
            };

            _localArrivalTimeLabel = new Label
            {
                Text = "-",
                Location = new Point(150, 120),
                AutoSize = true
            };

            // Add controls to results panel
            resultsPanel.Controls.Add(distanceLabel);
            resultsPanel.Controls.Add(_distanceResultLabel);
            resultsPanel.Controls.Add(flightTimeLabel);
            resultsPanel.Controls.Add(_flightTimeResultLabel);
            resultsPanel.Controls.Add(localDepartureLabel);
            resultsPanel.Controls.Add(_localDepartureTimeLabel);
            resultsPanel.Controls.Add(localArrivalLabel);
            resultsPanel.Controls.Add(_localArrivalTimeLabel);

            // Add controls to main form
            this.Controls.Add(originLabel);
            this.Controls.Add(_originAirportComboBox);
            this.Controls.Add(destinationLabel);
            this.Controls.Add(_destinationAirportComboBox);
            this.Controls.Add(departureLabel);
            this.Controls.Add(_departureDateTimePicker);
            this.Controls.Add(_calculateButton);
            this.Controls.Add(resultsPanel);

            // Populate airport data
            PopulateAirportComboBoxes();
        }

        private void PopulateAirportComboBoxes()
        {
            _originAirportComboBox.Items.Clear();
            _destinationAirportComboBox.Items.Clear();

            foreach (var airport in _airports.Values)
            {
                string displayText = $"{airport.Code} - {airport.Name} ({airport.City})";
                _originAirportComboBox.Items.Add(new { Text = displayText, Value = airport.Code });
                _destinationAirportComboBox.Items.Add(new { Text = displayText, Value = airport.Code });
            }

            _originAirportComboBox.DisplayMember = "Text";
            _originAirportComboBox.ValueMember = "Value";
            _destinationAirportComboBox.DisplayMember = "Text";
            _destinationAirportComboBox.ValueMember = "Value";

            // Try to select a default airport for origin based on the timezone
            string tzId = _timeZoneModel.TimeZoneInfo.Id;
            foreach (var airport in _airports.Values)
            {
                if (airport.TimeZoneId == tzId)
                {
                    for (int i = 0; i < _originAirportComboBox.Items.Count; i++)
                    {
                        dynamic item = _originAirportComboBox.Items[i];
                        if (item.Value == airport.Code)
                        {
                            _originAirportComboBox.SelectedIndex = i;
                            break;
                        }
                    }
                    break;
                }
            }

            // Select first items if nothing else selected
            if (_originAirportComboBox.SelectedIndex == -1 && _originAirportComboBox.Items.Count > 0)
                _originAirportComboBox.SelectedIndex = 0;

            if (_destinationAirportComboBox.SelectedIndex == -1 && _destinationAirportComboBox.Items.Count > 0)
                _destinationAirportComboBox.SelectedIndex = 0;
        }

        private void CalculateButton_Click(object sender, EventArgs e)
        {
            if (_originAirportComboBox.SelectedItem == null || _destinationAirportComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please select both origin and destination airports.");
                return;
            }

            try
            {
                // Get selected airports
                dynamic originItem = _originAirportComboBox.SelectedItem;
                dynamic destItem = _destinationAirportComboBox.SelectedItem;

                string originCode = originItem.Value;
                string destCode = destItem.Value;

                Airport originAirport = _airports[originCode];
                Airport destAirport = _airports[destCode];

                // Calculate distance
                double distance = CalculateDistance(
                    originAirport.Latitude, originAirport.Longitude,
                    destAirport.Latitude, destAirport.Longitude);

                // Calculate flight time (assume average speed of 500 mph)
                double speed = 500; // mph
                double flightHours = distance / speed;

                // Format flight time
                TimeSpan flightTime = TimeSpan.FromHours(flightHours);
                string flightTimeStr = $"{(int)flightTime.TotalHours}h {flightTime.Minutes}m";

                // Get departure time and explicitly set it as Unspecified kind
                DateTime departureTime = DateTime.SpecifyKind(_departureDateTimePicker.Value, DateTimeKind.Unspecified);

                // Get the time zone info objects
                TimeZoneInfo originTimeZone = TimeZoneInfo.FindSystemTimeZoneById(originAirport.TimeZoneId);
                TimeZoneInfo destTimeZone = TimeZoneInfo.FindSystemTimeZoneById(destAirport.TimeZoneId);

                // A more reliable approach is to use DateTimeOffset which explicitly handles time zones
                DateTimeOffset departureDto = new DateTimeOffset(departureTime, originTimeZone.GetUtcOffset(departureTime));
                DateTimeOffset departureUtcDto = departureDto.ToUniversalTime();
                DateTimeOffset arrivalUtcDto = departureUtcDto.AddHours(flightHours);

                // Convert to local times at origin and destination
                DateTimeOffset localDepartureDto = departureDto;
                DateTimeOffset localArrivalDto = TimeZoneInfo.ConvertTime(arrivalUtcDto, destTimeZone);

                // Get the final DateTime values (keeping them as DateTimeKind.Local)
                DateTime localDepartureTime = localDepartureDto.LocalDateTime;
                DateTime localArrivalTime = localArrivalDto.LocalDateTime;

                // Update results
                _distanceResultLabel.Text = $"{distance:N0} miles";
                _flightTimeResultLabel.Text = flightTimeStr;
                _localDepartureTimeLabel.Text = localDepartureTime.ToString("MM/dd/yyyy hh:mm tt");
                _localArrivalTimeLabel.Text = localArrivalTime.ToString("MM/dd/yyyy hh:mm tt");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error calculating flight data: {ex.Message}", "Calculation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Calculate distance between two points using the Haversine formula
        private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double EarthRadiusMiles = 3958.8; // Earth's radius in miles

            // Convert to radians
            double lat1Rad = DegreesToRadians(lat1);
            double lon1Rad = DegreesToRadians(lon1);
            double lat2Rad = DegreesToRadians(lat2);
            double lon2Rad = DegreesToRadians(lon2);

            // Calculate differences
            double latDiff = lat2Rad - lat1Rad;
            double lonDiff = lon2Rad - lon1Rad;

            // Haversine formula
            double a = Math.Sin(latDiff / 2) * Math.Sin(latDiff / 2) +
                       Math.Cos(lat1Rad) * Math.Cos(lat2Rad) *
                       Math.Sin(lonDiff / 2) * Math.Sin(lonDiff / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double distance = EarthRadiusMiles * c;

            return distance;
        }

        private double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180;
        }

        // Sample airport data
        private Dictionary<string, Airport> LoadAirportData()
        {
            var airports = new Dictionary<string, Airport>
            {
                {"JFK", new Airport
                    {
                        Code = "JFK",
                        Name = "John F. Kennedy International Airport",
                        City = "New York",
                        Country = "USA",
                        Latitude = 40.6413,
                        Longitude = -73.7781,
                        TimeZoneId = "Eastern Standard Time"
                    }
                },
                {"LAX", new Airport
                    {
                        Code = "LAX",
                        Name = "Los Angeles International Airport",
                        City = "Los Angeles",
                        Country = "USA",
                        Latitude = 33.9416,
                        Longitude = -118.4085,
                        TimeZoneId = "Pacific Standard Time"
                    }
                },
                {"LHR", new Airport
                    {
                        Code = "LHR",
                        Name = "Heathrow Airport",
                        City = "London",
                        Country = "UK",
                        Latitude = 51.4700,
                        Longitude = -0.4543,
                        TimeZoneId = "GMT Standard Time"
                    }
                },
                {"CDG", new Airport
                    {
                        Code = "CDG",
                        Name = "Charles de Gaulle Airport",
                        City = "Paris",
                        Country = "France",
                        Latitude = 49.0097,
                        Longitude = 2.5479,
                        TimeZoneId = "Central European Standard Time"
                    }
                },
                {"DXB", new Airport
                    {
                        Code = "DXB",
                        Name = "Dubai International Airport",
                        City = "Dubai",
                        Country = "UAE",
                        Latitude = 25.2532,
                        Longitude = 55.3657,
                        TimeZoneId = "Arabian Standard Time"
                    }
                },
                {"NRT", new Airport
                    {
                        Code = "NRT",
                        Name = "Narita International Airport",
                        City = "Tokyo",
                        Country = "Japan",
                        Latitude = 35.7720,
                        Longitude = 140.3929,
                        TimeZoneId = "Tokyo Standard Time"
                    }
                },
                {"SYD", new Airport
                    {
                        Code = "SYD",
                        Name = "Sydney Airport",
                        City = "Sydney",
                        Country = "Australia",
                        Latitude = -33.9399,
                        Longitude = 151.1753,
                        TimeZoneId = "AUS Eastern Standard Time"
                    }
                },
                {"JNU", new Airport
                    {
                        Code = "JNU",
                        Name = "Juneau International Airport",
                        City = "Juneau",
                        Country = "USA",
                        Latitude = 58.3550,
                        Longitude = -134.5760,
                        TimeZoneId = "Alaskan Standard Time"
                    }
                },
                {"TRG", new Airport
                    {
                        Code = "TRG",
                        Name = "Tauranga Airport",
                        City = "Tauranga",
                        Country = "New Zealand",
                        Latitude = -37.6731,
                        Longitude = 176.1956,
                        TimeZoneId = "New Zealand Standard Time"
                    }
                },
                {"MEL", new Airport
                    {
                        Code = "MEL",
                        Name = "Melbourne Airport",
                        City = "Melbourne",
                        Country = "Australia",
                        Latitude = -37.6690,
                        Longitude = 144.8410,
                        TimeZoneId = "AUS Eastern Standard Time"
                    }
                }
            };

            return airports;
        }
    }

    // Airport data class
    public class Airport
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string TimeZoneId { get; set; }
    }
}
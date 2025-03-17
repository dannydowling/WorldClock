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
        private Label _distanceResultLabel;
        private Label _flightTimeResultLabel;
        private Label _localDepartureTimeLabel;
        private Label _localArrivalTimeLabel;
        private DateTimePicker _departureDateTimePicker;
        private Button _calculateButton;
        private ComboBox _originCountryComboBox;
        private ComboBox _originStateComboBox;
        private ComboBox _originCityComboBox;
        private ComboBox _originAirportComboBox;

        private ComboBox _destCountryComboBox;
        private ComboBox _destStateComboBox;
        private ComboBox _destCityComboBox;
        private ComboBox _destAirportComboBox;        

        private ReadZipFileLogic _locationLogic;
        private bool _isLoading = false;

        public FlightCalculatorForm(TimeZoneModel timeZoneModel)
        {
            _timeZoneModel = timeZoneModel;
            
            InitializeComponent();
            InitializeCustomComponents();
            _locationLogic = new ReadZipFileLogic();
            LoadCountriesAsync();
            this.Text = $"Flight Calculator - {_timeZoneModel.DisplayName}";
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // FlightCalculatorForm
            // 
            this.ClientSize = new System.Drawing.Size(500, 650);
            this.Name = "FlightCalculatorForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Flight Calculator";
            this.ResumeLayout(false);
        }

        private void InitializeCustomComponents()
        {
            this.Padding = new Padding(20);

            // Origin section title
            Label originSectionLabel = new Label
            {
                Text = "Origin Airport:",
                Location = new Point(20, 20),
                AutoSize = true,
                Font = new Font(this.Font, FontStyle.Bold)
            };

            // Origin airport selection controls
            Label originCountryLabel = new Label
            {
                Text = "Country:",
                Location = new Point(40, 50),
                AutoSize = true
            };

            _originCountryComboBox = new ComboBox
            {
                Location = new Point(140, 47),
                Width = 200,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            _originCountryComboBox.SelectedIndexChanged += OriginCountryComboBox_SelectedIndexChanged;

            Label originStateLabel = new Label
            {
                Text = "State/Region:",
                Location = new Point(40, 80),
                AutoSize = true
            };

            _originStateComboBox = new ComboBox
            {
                Location = new Point(140, 77),
                Width = 200,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Enabled = false
            };
            _originStateComboBox.SelectedIndexChanged += OriginStateComboBox_SelectedIndexChanged;

            Label originCityLabel = new Label
            {
                Text = "City:",
                Location = new Point(40, 110),
                AutoSize = true
            };

            _originCityComboBox = new ComboBox
            {
                Location = new Point(140, 107),
                Width = 200,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Enabled = false
            };
            _originCityComboBox.SelectedIndexChanged += OriginCityComboBox_SelectedIndexChanged;

            Label originAirportLabel = new Label
            {
                Text = "Airport:",
                Location = new Point(40, 140),
                AutoSize = true
            };

            _originAirportComboBox = new ComboBox
            {
                Location = new Point(140, 137),
                Width = 300,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Enabled = false
            };

            // Destination section title
            Label destSectionLabel = new Label
            {
                Text = "Destination Airport:",
                Location = new Point(20, 180),
                AutoSize = true,
                Font = new Font(this.Font, FontStyle.Bold)
            };

            // Destination airport selection controls
            Label destCountryLabel = new Label
            {
                Text = "Country:",
                Location = new Point(40, 210),
                AutoSize = true
            };

            _destCountryComboBox = new ComboBox
            {
                Location = new Point(140, 207),
                Width = 200,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            _destCountryComboBox.SelectedIndexChanged += DestCountryComboBox_SelectedIndexChanged;

            Label destStateLabel = new Label
            {
                Text = "State/Region:",
                Location = new Point(40, 240),
                AutoSize = true
            };

            _destStateComboBox = new ComboBox
            {
                Location = new Point(140, 237),
                Width = 200,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Enabled = false
            };
            _destStateComboBox.SelectedIndexChanged += DestStateComboBox_SelectedIndexChanged;

            Label destCityLabel = new Label
            {
                Text = "City:",
                Location = new Point(40, 270),
                AutoSize = true
            };

            _destCityComboBox = new ComboBox
            {
                Location = new Point(140, 267),
                Width = 200,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Enabled = false
            };
            _destCityComboBox.SelectedIndexChanged += DestCityComboBox_SelectedIndexChanged;

            Label destAirportLabel = new Label
            {
                Text = "Airport:",
                Location = new Point(40, 300),
                AutoSize = true
            };

            _destAirportComboBox = new ComboBox
            {
                Location = new Point(140, 297),
                Width = 300,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Enabled = false
            };

            // Departure date/time controls
            Label departureLabel = new Label
            {
                Text = "Departure Time:",
                Location = new Point(20, 340),
                AutoSize = true
            };

            _departureDateTimePicker = new DateTimePicker
            {
                Location = new Point(140, 337),
                Width = 200,
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "MM/dd/yyyy hh:mm tt"
            };

            // Calculate button
            _calculateButton = new Button
            {
                Text = "Calculate",
                Location = new Point(200, 380),
                Width = 120,
                Height = 30
            };
            _calculateButton.Click += CalculateButton_Click;

            // Results panel
            GroupBox resultsPanel = new GroupBox
            {
                Text = "Flight Results",
                Location = new Point(20, 430),
                Size = new Size(480, 170),
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
            this.Controls.Add(originSectionLabel);
            this.Controls.Add(originCountryLabel);
            this.Controls.Add(_originCountryComboBox);
            this.Controls.Add(originStateLabel);
            this.Controls.Add(_originStateComboBox);
            this.Controls.Add(originCityLabel);
            this.Controls.Add(_originCityComboBox);
            this.Controls.Add(originAirportLabel);
            this.Controls.Add(_originAirportComboBox);

            this.Controls.Add(destSectionLabel);
            this.Controls.Add(destCountryLabel);
            this.Controls.Add(_destCountryComboBox);
            this.Controls.Add(destStateLabel);
            this.Controls.Add(_destStateComboBox);
            this.Controls.Add(destCityLabel);
            this.Controls.Add(_destCityComboBox);
            this.Controls.Add(destAirportLabel);
            this.Controls.Add(_destAirportComboBox);

            this.Controls.Add(departureLabel);
            this.Controls.Add(_departureDateTimePicker);
            this.Controls.Add(_calculateButton);
            this.Controls.Add(resultsPanel);

            // Initialize LocationLogic and load data
            _locationLogic = new ReadZipFileLogic();

            // Load countries in the background
            LoadCountriesAsync();
        }

        // Method to load countries asynchronously
        private async void LoadCountriesAsync()
        {
            // Show loading indicator
            this.Cursor = Cursors.WaitCursor;
            _isLoading = true;

            await Task.Run(() => {
                try
                {
                    // Get countries
                    var countries = _locationLogic.GetCountries();

                    // Update UI on main thread
                    this.Invoke(new Action(() => {
                        _originCountryComboBox.Items.Clear();
                        _destCountryComboBox.Items.Clear();

                        foreach (var country in countries)
                        {
                            _originCountryComboBox.Items.Add(country);
                            _destCountryComboBox.Items.Add(country);
                        }

                        if (_originCountryComboBox.Items.Count > 0)
                        {
                            _originCountryComboBox.SelectedIndex = 0;
                            _destCountryComboBox.SelectedIndex = 0;
                        }

                        this.Cursor = Cursors.Default;
                        _isLoading = false;
                    }));
                }
                catch (Exception ex)
                {
                    // Handle error on main thread
                    this.Invoke(new Action(() => {
                        MessageBox.Show($"Error loading countries: {ex.Message}", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Cursor = Cursors.Default;
                        _isLoading = false;
                    }));
                }
            });
        }

        private async void OriginCountryComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_originCountryComboBox.SelectedItem == null ||
                _originCountryComboBox.SelectedItem.ToString().StartsWith("Loading") ||
                _originCountryComboBox.SelectedItem.ToString().StartsWith("Error"))
                return;

            // Clear subsequent selections
            _originStateComboBox.Items.Clear();
            _originCityComboBox.Items.Clear();
            _originAirportComboBox.Items.Clear();

            // Show loading indicator in the state dropdown
            _originStateComboBox.Items.Add("Loading states...");
            _originStateComboBox.SelectedIndex = 0;
            _originStateComboBox.Enabled = false;
            _originCityComboBox.Enabled = false;
            _originAirportComboBox.Enabled = false;

            // Show cursor loading indicator
            this.Cursor = Cursors.WaitCursor;

            string country = _originCountryComboBox.SelectedItem.ToString();

            // Load states for just this country
            await Task.Run(() => {
                try
                {
                    var states = _locationLogic.GetStates(country);

                    this.Invoke(new Action(() => {
                        _originStateComboBox.Items.Clear();

                        foreach (var state in states)
                        {
                            _originStateComboBox.Items.Add(state);
                        }

                        _originStateComboBox.Enabled = true;

                        if (_originStateComboBox.Items.Count > 0)
                        {
                            _originStateComboBox.SelectedIndex = 0;
                        }
                        else
                        {
                            _originStateComboBox.Items.Add("No states found");
                            _originStateComboBox.SelectedIndex = 0;
                        }

                        this.Cursor = Cursors.Default;
                    }));
                }
                catch (Exception ex)
                {
                    this.Invoke(new Action(() => {
                        _originStateComboBox.Items.Clear();
                        _originStateComboBox.Items.Add("Error loading states");
                        _originStateComboBox.SelectedIndex = 0;
                        MessageBox.Show($"Error loading states: {ex.Message}", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Cursor = Cursors.Default;
                    }));
                }
            });
        }


        private void OriginStateComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isLoading || _originStateComboBox.SelectedItem == null || _originCountryComboBox.SelectedItem == null)
                return;

            // Clear subsequent selections
            _originCityComboBox.Items.Clear();
            _originAirportComboBox.Items.Clear();

            // Disable subsequent controls until they have data
            _originCityComboBox.Enabled = false;
            _originAirportComboBox.Enabled = false;

            string country = _originCountryComboBox.SelectedItem.ToString();
            string state = _originStateComboBox.SelectedItem.ToString();

            // Show loading indicator
            this.Cursor = Cursors.WaitCursor;
            _isLoading = true;

            // Load cities in background
            Task.Run(() => {
                try
                {
                    var cities = _locationLogic.GetCities(country, state);

                    this.Invoke(new Action(() => {
                        foreach (var city in cities)
                        {
                            _originCityComboBox.Items.Add(city);
                        }

                        if (_originCityComboBox.Items.Count > 0)
                        {
                            _originCityComboBox.Enabled = true;
                            _originCityComboBox.SelectedIndex = 0;
                        }

                        this.Cursor = Cursors.Default;
                        _isLoading = false;
                    }));
                }
                catch (Exception ex)
                {
                    this.Invoke(new Action(() => {
                        MessageBox.Show($"Error loading cities: {ex.Message}", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Cursor = Cursors.Default;
                        _isLoading = false;
                    }));
                }
            });
        }

        private void OriginCityComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isLoading || _originCityComboBox.SelectedItem == null ||
                _originStateComboBox.SelectedItem == null || _originCountryComboBox.SelectedItem == null)
                return;

            // Clear subsequent selections
            _originAirportComboBox.Items.Clear();

            // Disable subsequent controls until they have data
            _originAirportComboBox.Enabled = false;

            string country = _originCountryComboBox.SelectedItem.ToString();
            string state = _originStateComboBox.SelectedItem.ToString();
            string city = _originCityComboBox.SelectedItem.ToString();

            // Show loading indicator
            this.Cursor = Cursors.WaitCursor;
            _isLoading = true;

            // Load airports in background
            Task.Run(() => {
                try
                {
                    var airports = _locationLogic.LocationsLookup(country, state, city);

                    this.Invoke(new Action(() => {
                        foreach (var airport in airports)
                        {
                            string displayText = $"{airport.icao} - {airport.name}";
                            _originAirportComboBox.Items.Add(new { Text = displayText, Value = airport });
                        }

                        _originAirportComboBox.DisplayMember = "Text";
                        _originAirportComboBox.ValueMember = "Value";

                        if (_originAirportComboBox.Items.Count > 0)
                        {
                            _originAirportComboBox.Enabled = true;
                            _originAirportComboBox.SelectedIndex = 0;
                        }

                        this.Cursor = Cursors.Default;
                        _isLoading = false;
                    }));
                }
                catch (Exception ex)
                {
                    this.Invoke(new Action(() => {
                        MessageBox.Show($"Error loading airports: {ex.Message}", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Cursor = Cursors.Default;
                        _isLoading = false;
                    }));
                }
            });
        }

        // Similar event handlers for destination selection
        private void DestCountryComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isLoading || _destCountryComboBox.SelectedItem == null)
                return;

            // Clear subsequent selections
            _destStateComboBox.Items.Clear();
            _destCityComboBox.Items.Clear();
            _destAirportComboBox.Items.Clear();

            // Disable subsequent controls until they have data
            _destStateComboBox.Enabled = false;
            _destCityComboBox.Enabled = false;
            _destAirportComboBox.Enabled = false;

            string country = _destCountryComboBox.SelectedItem.ToString();

            // Show loading indicator
            this.Cursor = Cursors.WaitCursor;
            _isLoading = true;

            // Load states in background
            Task.Run(() => {
                try
                {
                    var states = _locationLogic.GetStates(country);

                    this.Invoke(new Action(() => {
                        foreach (var state in states)
                        {
                            _destStateComboBox.Items.Add(state);
                        }

                        if (_destStateComboBox.Items.Count > 0)
                        {
                            _destStateComboBox.Enabled = true;
                            _destStateComboBox.SelectedIndex = 0;
                        }

                        this.Cursor = Cursors.Default;
                        _isLoading = false;
                    }));
                }
                catch (Exception ex)
                {
                    this.Invoke(new Action(() => {
                        MessageBox.Show($"Error loading states: {ex.Message}", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Cursor = Cursors.Default;
                        _isLoading = false;
                    }));
                }
            });
        }

        private void DestStateComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isLoading || _destStateComboBox.SelectedItem == null || _destCountryComboBox.SelectedItem == null)
                return;

            // Clear subsequent selections
            _destCityComboBox.Items.Clear();
            _destAirportComboBox.Items.Clear();

            // Disable subsequent controls until they have data
            _destCityComboBox.Enabled = false;
            _destAirportComboBox.Enabled = false;

            string country = _destCountryComboBox.SelectedItem.ToString();
            string state = _destStateComboBox.SelectedItem.ToString();

            // Show loading indicator
            this.Cursor = Cursors.WaitCursor;
            _isLoading = true;

            // Load cities in background
            Task.Run(() => {
                try
                {
                    var cities = _locationLogic.GetCities(country, state);

                    this.Invoke(new Action(() => {
                        foreach (var city in cities)
                        {
                            _destCityComboBox.Items.Add(city);
                        }

                        if (_destCityComboBox.Items.Count > 0)
                        {
                            _destCityComboBox.Enabled = true;
                            _destCityComboBox.SelectedIndex = 0;
                        }

                        this.Cursor = Cursors.Default;
                        _isLoading = false;
                    }));
                }
                catch (Exception ex)
                {
                    this.Invoke(new Action(() => {
                        MessageBox.Show($"Error loading cities: {ex.Message}", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Cursor = Cursors.Default;
                        _isLoading = false;
                    }));
                }
            });
        }

        private void DestCityComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isLoading || _destCityComboBox.SelectedItem == null ||
                _destStateComboBox.SelectedItem == null || _destCountryComboBox.SelectedItem == null)
                return;

            // Clear subsequent selections
            _destAirportComboBox.Items.Clear();

            // Disable subsequent controls until they have data
            _destAirportComboBox.Enabled = false;

            string country = _destCountryComboBox.SelectedItem.ToString();
            string state = _destStateComboBox.SelectedItem.ToString();
            string city = _destCityComboBox.SelectedItem.ToString();

            // Show loading indicator
            this.Cursor = Cursors.WaitCursor;
            _isLoading = true;

            // Load airports in background
            Task.Run(() => {
                try
                {
                    var airports = _locationLogic.LocationsLookup(country, state, city);

                    this.Invoke(new Action(() => {
                        foreach (var airport in airports)
                        {
                            string displayText = $"{airport.icao} - {airport.name}";
                            _destAirportComboBox.Items.Add(new { Text = displayText, Value = airport });
                        }

                        _destAirportComboBox.DisplayMember = "Text";
                        _destAirportComboBox.ValueMember = "Value";

                        if (_destAirportComboBox.Items.Count > 0)
                        {
                            _destAirportComboBox.Enabled = true;
                            _destAirportComboBox.SelectedIndex = 0;
                        }

                        this.Cursor = Cursors.Default;
                        _isLoading = false;
                    }));
                }
                catch (Exception ex)
                {
                    this.Invoke(new Action(() => {
                        MessageBox.Show($"Error loading airports: {ex.Message}", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Cursor = Cursors.Default;
                        _isLoading = false;
                    }));
                }
            });
        }

        private void CalculateButton_Click(object sender, EventArgs e)
        {
            if (_originAirportComboBox.SelectedItem == null || _destAirportComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please select both origin and destination airports.");
                return;
            }

            try
            {
                // Get selected airports
                dynamic originItem = _originAirportComboBox.SelectedItem;
                dynamic destItem = _destAirportComboBox.SelectedItem;

                LocationClass originAirport = originItem.Value;
                LocationClass destAirport = destItem.Value;

                // Convert LocationClass to Airport format for the calculator
                Airport originAirportObj = new Airport
                {
                    Code = originAirport.icao,
                    Name = originAirport.name,
                    City = originAirport.city,
                    Country = originAirport.country,
                    Latitude = originAirport.lat,
                    Longitude = originAirport.lon,
                    TimeZoneId = EstimateTimeZoneId(originAirport.lon) // Estimate timezone based on longitude
                };

                Airport destAirportObj = new Airport
                {
                    Code = destAirport.icao,
                    Name = destAirport.name,
                    City = destAirport.city,
                    Country = destAirport.country,
                    Latitude = destAirport.lat,
                    Longitude = destAirport.lon,
                    TimeZoneId = EstimateTimeZoneId(destAirport.lon) // Estimate timezone based on longitude
                };

                // Calculate distance
                double distance = CalculateDistance(
                    originAirportObj.Latitude, originAirportObj.Longitude,
                    destAirportObj.Latitude, destAirportObj.Longitude);

                // Calculate flight time (assume average speed of 500 mph)
                double speed = 500; // mph
                double flightHours = distance / speed;

                // Format flight time
                TimeSpan flightTime = TimeSpan.FromHours(flightHours);
                string flightTimeStr = $"{(int)flightTime.TotalHours}h {flightTime.Minutes}m";

                // Get departure time
                DateTime departureTime = DateTime.SpecifyKind(_departureDateTimePicker.Value, DateTimeKind.Unspecified);

                // Get the time zone info objects
                TimeZoneInfo originTimeZone = TimeZoneInfo.FindSystemTimeZoneById(originAirportObj.TimeZoneId);
                TimeZoneInfo destTimeZone = TimeZoneInfo.FindSystemTimeZoneById(destAirportObj.TimeZoneId);

                // Use DateTimeOffset for reliable timezone conversion
                DateTimeOffset departureDto = new DateTimeOffset(departureTime, originTimeZone.GetUtcOffset(departureTime));
                DateTimeOffset departureUtcDto = departureDto.ToUniversalTime();
                DateTimeOffset arrivalUtcDto = departureUtcDto.AddHours(flightHours);

                // Convert to local times at origin and destination
                DateTimeOffset localDepartureDto = departureDto;
                DateTimeOffset localArrivalDto = TimeZoneInfo.ConvertTime(arrivalUtcDto, destTimeZone);

                // Get the final DateTime values
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
        private string EstimateTimeZoneId(double lon)
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

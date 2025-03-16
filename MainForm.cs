using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using WorldClock.Models;
using WorldClock.Services;
using WorldClock.Controls;

namespace WorldClock
{
    public partial class MainForm : Form
    {
        private TimeZoneService _timeZoneService;
        private Dictionary<string, AnalogClockControl> _clockControls;

        public MainForm()
        {
            InitializeComponent();

            // Initialize service
            _timeZoneService = new TimeZoneService();

            // Initialize clock controls dictionary
            _clockControls = new Dictionary<string, AnalogClockControl>();
        }

      
        private void MainForm_Load(object sender, EventArgs e)
        {
            // Load available time zones into the combo box
            LoadTimeZoneComboBox();

            // Set up the UI
            SetupUI();

            // Start the timer
            _updateTimer.Start();
        }

        private void SetupUI()
        {
            // Clear any existing controls
            clockFlowLayoutPanel.Controls.Clear();

            // Add clocks for each time zone
            foreach (var timeZone in _timeZoneService.GetAllTimeZones())
            {
                // Create grouped panel for each clock
                Panel clockGroup = new Panel();
                clockGroup.Size = new Size(180, 240);
                clockGroup.Margin = new Padding(10);

                // Create label for the time zone name
                Label nameLabel = new Label();
                nameLabel.Text = timeZone.DisplayName;
                nameLabel.Dock = DockStyle.Top;
                nameLabel.Height = 40;
                nameLabel.TextAlign = ContentAlignment.MiddleCenter;
                nameLabel.Font = new Font("Arial", 9, FontStyle.Bold);

                // Create the analog clock control
                AnalogClockControl clock = new AnalogClockControl();
                clock.Dock = DockStyle.Fill;
                clock.TimeZoneName = timeZone.Name;

                // Store the clock control for later updates
                _clockControls.Add(timeZone.Name, clock);

                // Add controls to the group panel
                clockGroup.Controls.Add(clock);
                clockGroup.Controls.Add(nameLabel);

                // Add the group to the flow layout panel
                clockFlowLayoutPanel.Controls.Add(clockGroup);
            }

            // Update clocks initially
            UpdateClocks();
        }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            UpdateClocks();
        }

        private void UpdateClocks()
        {
            foreach (var timeZone in _timeZoneService.GetAllTimeZones())
            {
                if (_clockControls.TryGetValue(timeZone.Name, out AnalogClockControl clock))
                {
                    clock.CurrentTime = timeZone.GetCurrentTime();
                }
            }
        }

        private void LoadTimeZoneComboBox()
        {
            timeZoneComboBox.Items.Clear();
            timeZoneComboBox.DisplayMember = "Value";
            timeZoneComboBox.ValueMember = "Key";

            var availableTimeZones = _timeZoneService.GetAllAvailableTimeZones();
            foreach (var tz in availableTimeZones)
            {
                timeZoneComboBox.Items.Add(tz);
            }

            if (timeZoneComboBox.Items.Count > 0)
                timeZoneComboBox.SelectedIndex = 0;
        }

        private void timeZoneComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (timeZoneComboBox.SelectedItem is KeyValuePair<string, string> selectedTz)
            {
                // Auto-populate display name field with a friendly name
                string tzId = selectedTz.Key;
                string displayName = tzId.Split('/').LastOrDefault() ?? tzId;

                // Convert to title case and replace underscores with spaces
                displayName = System.Globalization.CultureInfo.CurrentCulture.TextInfo
                    .ToTitleCase(displayName.ToLower())
                    .Replace("_", " ");

                displayNameTextBox.Text = displayName;
            }
        }

        private void addTimeZoneButton_Click(object sender, EventArgs e)
        {
            if (timeZoneComboBox.SelectedItem is KeyValuePair<string, string> selectedTz)
            {
                string tzId = selectedTz.Key;
                string displayName = displayNameTextBox.Text.Trim();

                if (string.IsNullOrEmpty(displayName))
                {
                    MessageBox.Show("Please enter a display name for the time zone.");
                    return;
                }

                // Add the new time zone
                _timeZoneService.AddTimeZone(tzId, displayName);

                // Refresh the UI
                RefreshClocks();
            }
        }

        private void RefreshClocks()
        {
            // Clear existing controls
            clockFlowLayoutPanel.Controls.Clear();
            _clockControls.Clear();

            // Setup the UI again with the updated time zones
            SetupUI();
        }

    }
}
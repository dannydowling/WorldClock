using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;


namespace WorldClock
{
    public partial class MainForm : Form
    {
        private TimeZoneService _timeZoneService;
        private EventService _eventService;
        private Dictionary<string, ClockTabPanel> _clockPanels;

        public MainForm()
        {
            InitializeComponent();

            // Initialize services
            _timeZoneService = new TimeZoneService();
            _eventService = new EventService();

            // Initialize clock panels dictionary
            _clockPanels = new Dictionary<string, ClockTabPanel>();
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

        private void SetupUI()
        {
            // Clear any existing controls
            clockFlowLayoutPanel.Controls.Clear();

            // Add clocks for each time zone
            foreach (var timeZone in _timeZoneService.GetAllTimeZones())
            {
                // Create clock panel with tabs
                ClockTabPanel clockPanel = new ClockTabPanel(timeZone, _eventService);

                // Store the clock panel for later updates
                _clockPanels.Add(timeZone.Name, clockPanel);

                // Add the panel to the flow layout panel
                clockFlowLayoutPanel.Controls.Add(clockPanel);
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
                if (_clockPanels.TryGetValue(timeZone.Name, out ClockTabPanel panel))
                {
                    panel.UpdateClock(timeZone.GetCurrentTime());
                }
            }
        }

        private void timeZoneComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (timeZoneComboBox.SelectedItem is KeyValuePair<string, string> selectedTz)
            {
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
            _clockPanels.Clear();

            // Setup the UI again with the updated time zones
            SetupUI();
        }

        // Add this context menu for right-clicking on clocks
        private void SetupContextMenu()
        {
            ContextMenuStrip clockContextMenu = new ContextMenuStrip();
            clockContextMenu.Items.Add("Add Flight Arrival", null, AddFlightArrival_Click);
            clockContextMenu.Items.Add("Add Flight Departure", null, AddFlightDeparture_Click);
            clockContextMenu.Items.Add("Add Meeting", null, AddMeeting_Click);
            clockContextMenu.Items.Add("Add Reminder", null, AddReminder_Click);
            clockContextMenu.Items.Add("-"); // Separator
            clockContextMenu.Items.Add("Remove Time Zone", null, RemoveTimeZone_Click);

            // Set the context menu for each clock panel
            foreach (var panel in _clockPanels.Values)
            {
                panel.ContextMenuStrip = clockContextMenu;
            }
        }

        private void AddFlightArrival_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem menuItem &&
                menuItem.Owner is ContextMenuStrip menu &&
                menu.SourceControl is ClockTabPanel panel)
            {
                AddEventWithType(panel.TimeZoneModel, EventType.FlightArrival);
            }
        }

        private void AddFlightDeparture_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem menuItem &&
                menuItem.Owner is ContextMenuStrip menu &&
                menu.SourceControl is ClockTabPanel panel)
            {
                AddEventWithType(panel.TimeZoneModel, EventType.FlightDeparture);
            }
        }

        private void AddMeeting_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem menuItem &&
                menuItem.Owner is ContextMenuStrip menu &&
                menu.SourceControl is ClockTabPanel panel)
            {
                AddEventWithType(panel.TimeZoneModel, EventType.Meeting);
            }
        }

        private void AddReminder_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem menuItem &&
                menuItem.Owner is ContextMenuStrip menu &&
                menu.SourceControl is ClockTabPanel panel)
            {
                AddEventWithType(panel.TimeZoneModel, EventType.Reminder);
            }
        }

        private void AddEventWithType(TimeZoneModel timeZoneModel, EventType eventType)
        {
            var newEvent = new ScheduledEvent
            {
                Title = GetDefaultTitleForEventType(eventType),
                Type = eventType,
                EventTime = DateTime.Now,
                TimeZone = timeZoneModel.TimeZoneInfo,
                IndicatorColor = GetDefaultColorForEventType(eventType)
            };

            using (var eventForm = new EventForm(timeZoneModel, newEvent))
            {
                if (eventForm.ShowDialog() == DialogResult.OK)
                {
                    _eventService.AddEvent(eventForm.ScheduledEvent);
                    RefreshEvents();
                }
            }
        }

        private string GetDefaultTitleForEventType(EventType eventType)
        {
            switch (eventType)
            {
                case EventType.FlightArrival:
                    return "Flight Arrival";
                case EventType.FlightDeparture:
                    return "Flight Departure";
                case EventType.Meeting:
                    return "Meeting";
                case EventType.Reminder:
                    return "Reminder";
                default:
                    return "New Event";
            }
        }

        private Color GetDefaultColorForEventType(EventType eventType)
        {
            switch (eventType)
            {
                case EventType.FlightArrival:
                    return Color.Green;
                case EventType.FlightDeparture:
                    return Color.Blue;
                case EventType.Meeting:
                    return Color.Orange;
                case EventType.Reminder:
                    return Color.Red;
                default:
                    return Color.Purple;
            }
        }

        private void RemoveTimeZone_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem menuItem &&
                menuItem.Owner is ContextMenuStrip menu &&
                menu.SourceControl is ClockTabPanel panel)
            {
                if (MessageBox.Show($"Are you sure you want to remove {panel.TimeZoneModel.DisplayName}?",
                    "Remove Time Zone", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    _timeZoneService.RemoveTimeZone(panel.TimeZoneModel.Name);
                    RefreshClocks();
                }
            }
        }

        private void RefreshEvents()
        {
            foreach (var panel in _clockPanels.Values)
            {
                panel.RefreshEvents();
            }
        }
    }
}
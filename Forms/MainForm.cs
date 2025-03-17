using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace WorldClock
{
    public partial class MainForm : Form
    {
        private TimeZoneService _timeZoneService;
        private EventService _eventService;
        private DataService _dataService;
        private Dictionary<string, ClockTabPanel> _clockPanels;
        private bool _dataLoaded = false;
        private ContextMenuStrip _clockContextMenu;
        private ClockTabPanel _currentContextMenuClock;


        public MainForm()
        {
            InitializeComponent();
            this.ClientSize = new System.Drawing.Size(1100, 375);

            // Initialize services
            _timeZoneService = new TimeZoneService();
            _eventService = new EventService();
            _dataService = new DataService();

            // Initialize clock panels dictionary
            _clockPanels = new Dictionary<string, ClockTabPanel>();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {            
            _dataLoaded = _dataService.LoadData(_timeZoneService, _eventService);            
            LoadTimeZoneComboBox();
            SetupContextMenu();
            SetupUI();            
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
            clockFlowLayoutPanel.Controls.Clear();
            _clockPanels.Clear();
           
            foreach (var timeZone in _timeZoneService.GetAllTimeZones())
            {               
                ClockTabPanel clockPanel = new ClockTabPanel(timeZone, _eventService);
                // Subscribe to the RemoveRequested event
                clockPanel.RemoveRequested += ClockPanel_RemoveRequested;
                // Set the context menu for this panel
                clockPanel.ContextMenuStrip = _clockContextMenu;

                // Store the clock panel for later updates
                _clockPanels.Add(timeZone.Name, clockPanel);

                // Add the panel to the flow layout panel
                clockFlowLayoutPanel.Controls.Add(clockPanel);
            }
            
            UpdateClocks();
        }

        private void ClockPanel_RemoveRequested(object sender, TimeZoneModel timeZoneModel)
        {
            if (MessageBox.Show($"Are you sure you want to remove {timeZoneModel.DisplayName}?",
                "Remove Time Zone", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                _timeZoneService.RemoveTimeZone(timeZoneModel.Name);
                RefreshClocks();
            }
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

        private void SetupContextMenu()
        {
            // Create a single context menu to be shared by all clocks
            _clockContextMenu = new ContextMenuStrip();
            _clockContextMenu.Items.Add("Add Flight Arrival", null, AddFlightArrival_Click);
            _clockContextMenu.Items.Add("Add Flight Departure", null, AddFlightDeparture_Click);
            _clockContextMenu.Items.Add("Add Meeting", null, AddMeeting_Click);
            _clockContextMenu.Items.Add("Add Reminder", null, AddReminder_Click);
            _clockContextMenu.Items.Add("-"); // Separator
            _clockContextMenu.Items.Add("Remove Time Zone", null, RemoveTimeZone_Click);

            // The context menu needs to know which clock was clicked
            _clockContextMenu.Opening += ClockContextMenu_Opening;
        }

        private void ClockContextMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Get the control that was right-clicked
            _currentContextMenuClock = _clockContextMenu.SourceControl as ClockTabPanel;

            // If we couldn't determine which clock was clicked, cancel the menu
            if (_currentContextMenuClock == null)
            {
                e.Cancel = true;
            }
        }

        private void RemoveTimeZone_Click(object sender, EventArgs e)
        {
            // Use the currently tracked clock panel instead of trying to get it from the sender
            if (_currentContextMenuClock != null)
            {
                if (MessageBox.Show($"Are you sure you want to remove {_currentContextMenuClock.TimeZoneModel.DisplayName}?",
                    "Remove Time Zone", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    _timeZoneService.RemoveTimeZone(_currentContextMenuClock.TimeZoneModel.Name);
                    RefreshClocks();
                }
            }
        }



        private void AddFlightArrival_Click(object sender, EventArgs e)
        {
            if (_currentContextMenuClock != null)
            {
                AddEventWithType(_currentContextMenuClock.TimeZoneModel, EventType.FlightArrival);
            }
        }

        private void AddFlightDeparture_Click(object sender, EventArgs e)
        {
            if (_currentContextMenuClock != null)
            {
                AddEventWithType(_currentContextMenuClock.TimeZoneModel, EventType.FlightDeparture);
            }
        }

        private void AddMeeting_Click(object sender, EventArgs e)
        {
            if (_currentContextMenuClock != null)
            {
                AddEventWithType(_currentContextMenuClock.TimeZoneModel, EventType.Meeting);
            }
        }

        private void AddReminder_Click(object sender, EventArgs e)
        {
            if (_currentContextMenuClock != null)
            {
                AddEventWithType(_currentContextMenuClock.TimeZoneModel, EventType.Reminder);
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

        private void RefreshEvents()
        {
            foreach (var panel in _clockPanels.Values)
            {
                panel.RefreshEvents();
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            // Save data before closing
            _dataService.SaveData(_timeZoneService, _eventService);
        }
    }
}
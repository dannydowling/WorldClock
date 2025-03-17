using System;
using System.Drawing;
using System.Reflection.Metadata;
using System.Windows.Forms;

namespace WorldClock
{
    public class ClockTabPanel : Panel
    {
        private TabControl _tabControl;
        private TabPage _clockTabPage;
        private TabPage _calendarTabPage;
        private AnalogClockControl _analogClock;
        private Label _nameLabel;
        private MonthCalendar _calendar;
        private ListView _eventListView;
        private Button _addEventButton;
        private EventService _eventService;
        private TimeZoneModel _timeZoneModel;
        private TabPage _distanceTabPage;


        public AnalogClockControl AnalogClock => _analogClock;
        public TimeZoneModel TimeZoneModel => _timeZoneModel;

        public ClockTabPanel(TimeZoneModel timeZoneModel, EventService eventService)
        {
            _timeZoneModel = timeZoneModel;
            _eventService = eventService;
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            // Set panel properties
            Size = new Size(250, 300);
            Margin = new Padding(10);
            BorderStyle = BorderStyle.FixedSingle;

            // Create name label
            _nameLabel = new Label();
            _nameLabel.Text = _timeZoneModel.DisplayName;
            _nameLabel.Dock = DockStyle.Top;
            _nameLabel.Height = 40;
            _nameLabel.TextAlign = ContentAlignment.MiddleCenter;
            _nameLabel.Font = new Font("Arial", 9, FontStyle.Bold);

            // Create tab control
            _tabControl = new TabControl();
            _tabControl.Dock = DockStyle.Fill;

            // Create clock tab
            _clockTabPage = new TabPage("Clock");
            _analogClock = new AnalogClockControl();
            _analogClock.Dock = DockStyle.Fill;
            _analogClock.TimeZoneName = _timeZoneModel.Name;
            _analogClock.TimeZoneInfo = _timeZoneModel.TimeZoneInfo;
            _clockTabPage.Controls.Add(_analogClock);

            // Create calendar tab
            _calendarTabPage = new TabPage("Calendar");

            // Create flight distance tab
            _distanceTabPage = new TabPage("Flight");
            var flightPanel = new Panel();
            flightPanel.Dock = DockStyle.Fill;

            // Create a button to open the flight calculator
            Button openFlightCalculatorButton = new Button();
            openFlightCalculatorButton.Text = "Open Flight Calculator";
            openFlightCalculatorButton.Size = new Size(200, 40);
            openFlightCalculatorButton.Location = new Point(
                (flightPanel.Width - 200) / 2,
                (flightPanel.Height - 40) / 2);
            openFlightCalculatorButton.Anchor = AnchorStyles.None;
            openFlightCalculatorButton.Click += (sender, e) => {
                using (var flightCalculatorForm = new FlightCalculatorForm(_timeZoneModel))
                {
                    flightCalculatorForm.ShowDialog();
                }
            };

            flightPanel.Controls.Add(openFlightCalculatorButton);
            _distanceTabPage.Controls.Add(flightPanel);

            // Create monthly calendar
            _calendar = new MonthCalendar();
            _calendar.MaxSelectionCount = 1;
            _calendar.CalendarDimensions = new Size(1, 1);
            _calendar.Dock = DockStyle.Top;
            _calendar.Height = 160;
            _calendar.DateSelected += Calendar_DateSelected;

            // Create event list view
            _eventListView = new ListView();
            _eventListView.View = View.Details;
            _eventListView.Dock = DockStyle.Fill;
            _eventListView.FullRowSelect = true;
            _eventListView.Columns.Add("Time", 70);
            _eventListView.Columns.Add("Event", 150);
            _eventListView.DoubleClick += EventListView_DoubleClick;

            // Create add event button
            _addEventButton = new Button();
            _addEventButton.Text = "Add Event";
            _addEventButton.Dock = DockStyle.Bottom;
            _addEventButton.Click += AddEventButton_Click;

            // Add controls to calendar tab
            _calendarTabPage.Controls.Add(_eventListView);
            _calendarTabPage.Controls.Add(_addEventButton);
            _calendarTabPage.Controls.Add(_calendar);

            // Add tabs to tab control
            _tabControl.TabPages.Add(_clockTabPage);
            _tabControl.TabPages.Add(_calendarTabPage);
            _tabControl.TabPages.Add(_distanceTabPage);

            // Add controls to panel
            Controls.Add(_tabControl);
            Controls.Add(_nameLabel);

            // Update event list for current date
            UpdateEventList(_calendar.SelectionStart);
        }

        private void Calendar_DateSelected(object sender, DateRangeEventArgs e)
        {
            UpdateEventList(e.Start);
        }

        private void UpdateEventList(DateTime date)
        {
            _eventListView.Items.Clear();

            var events = _eventService.GetEventsForDay(date, _timeZoneModel.TimeZoneInfo);

            foreach (var scheduledEvent in events)
            {
                DateTime eventTime = scheduledEvent.GetEventTimeIn(_timeZoneModel.TimeZoneInfo);
                string time = eventTime.ToString("h:mm tt");

                ListViewItem item = new ListViewItem(time);
                item.SubItems.Add(scheduledEvent.Title);
                item.Tag = scheduledEvent;

                // Color-code based on event type
                switch (scheduledEvent.Type)
                {
                    case EventType.FlightArrival:
                        item.BackColor = Color.LightGreen;
                        break;
                    case EventType.FlightDeparture:
                        item.BackColor = Color.LightBlue;
                        break;
                    case EventType.Meeting:
                        item.BackColor = Color.LightYellow;
                        break;
                    case EventType.Reminder:
                        item.BackColor = Color.LightPink;
                        break;
                }

                _eventListView.Items.Add(item);
            }

            // Update the analog clock with these events
            _analogClock.ScheduledEvents = events;
        }

        private void AddEventButton_Click(object sender, EventArgs e)
        {
            using (var eventForm = new EventForm(_timeZoneModel))
            {
                if (eventForm.ShowDialog() == DialogResult.OK)
                {
                    _eventService.AddEvent(eventForm.ScheduledEvent);
                    UpdateEventList(_calendar.SelectionStart);
                }
            }
        }

        public event EventHandler<TimeZoneModel> RemoveRequested;


        private void AddRemoveButton()
        {
            Button removeButton = new Button();
            removeButton.Text = "❌";
            removeButton.Size = new Size(25, 25);
            removeButton.Location = new Point(this.Width - 30, 5);
            removeButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            removeButton.FlatStyle = FlatStyle.Flat;
            removeButton.FlatAppearance.BorderSize = 0;
            removeButton.Click += (sender, e) => RemoveRequested?.Invoke(this, _timeZoneModel);

            this.Controls.Add(removeButton);
            removeButton.BringToFront();
        }

        private void EventListView_DoubleClick(object sender, EventArgs e)
        {
            if (_eventListView.SelectedItems.Count > 0)
            {
                var selectedEvent = (ScheduledEvent)_eventListView.SelectedItems[0].Tag;

                using (var eventForm = new EventForm(_timeZoneModel, selectedEvent))
                {
                    if (eventForm.ShowDialog() == DialogResult.OK)
                    {
                        // Remove old event and add updated one
                        _eventService.RemoveEvent(selectedEvent.Id);
                        _eventService.AddEvent(eventForm.ScheduledEvent);
                        UpdateEventList(_calendar.SelectionStart);
                    }
                }
            }
        }

        public void UpdateClock(DateTime currentTime)
        {
            _analogClock.CurrentTime = currentTime;
        }

        public void RefreshEvents()
        {
            UpdateEventList(_calendar.SelectionStart);
        }
    }
}
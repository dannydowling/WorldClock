using System;
using System.Drawing;
using System.Windows.Forms;

namespace WorldClock
{
    public partial class EventForm : Form
    {
        private ScheduledEvent _scheduledEvent;
        private TimeZoneModel _timeZoneModel;

        public ScheduledEvent ScheduledEvent => _scheduledEvent;

        public EventForm(TimeZoneModel timeZoneModel, ScheduledEvent existingEvent = null)
        {
            InitializeComponent();

            _timeZoneModel = timeZoneModel;

            if (existingEvent != null)
            {                
                _scheduledEvent = existingEvent;
                Text = "Edit Event";
            }
            else
            {
                // create a new event
                _scheduledEvent = new ScheduledEvent
                {
                    TimeZone = _timeZoneModel.TimeZoneInfo,
                    EventTime = DateTime.Now
                };
                Text = "Add New Event";
            }

            PopulateEventTypes();
            LoadEventData();
        }

        private void InitializeComponent()
        {
            titleLabel = new Label();
            titleTextBox = new TextBox();
            typeLabel = new Label();
            typeComboBox = new ComboBox();
            dateTimeLabel = new Label();
            datePicker = new DateTimePicker();
            timePicker = new DateTimePicker();
            descriptionLabel = new Label();
            descriptionTextBox = new TextBox();
            colorLabel = new Label();
            colorButton = new Button();
            saveButton = new Button();
            cancelButton = new Button();
            SuspendLayout();
            // 
            // titleLabel
            // 
            titleLabel.AutoSize = true;
            titleLabel.Location = new Point(12, 15);
            titleLabel.Name = "titleLabel";
            titleLabel.Size = new Size(35, 17);
            titleLabel.TabIndex = 0;
            titleLabel.Text = "Title:";
            // 
            // titleTextBox
            // 
            titleTextBox.Location = new Point(95, 12);
            titleTextBox.Name = "titleTextBox";
            titleTextBox.Size = new Size(277, 22);
            titleTextBox.TabIndex = 1;
            // 
            // typeLabel
            // 
            typeLabel.AutoSize = true;
            typeLabel.Location = new Point(12, 45);
            typeLabel.Name = "typeLabel";
            typeLabel.Size = new Size(44, 17);
            typeLabel.TabIndex = 2;
            typeLabel.Text = "Type:";
            // 
            // typeComboBox
            // 
            typeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            typeComboBox.FormattingEnabled = true;
            typeComboBox.Location = new Point(95, 42);
            typeComboBox.Name = "typeComboBox";
            typeComboBox.Size = new Size(277, 24);
            typeComboBox.TabIndex = 3;
            typeComboBox.SelectedIndexChanged += new EventHandler(this.typeComboBox_SelectedIndexChanged);
            // 
            // dateTimeLabel
            // 
            dateTimeLabel.AutoSize = true;
            dateTimeLabel.Location = new Point(12, 75);
            dateTimeLabel.Name = "dateTimeLabel";
            dateTimeLabel.Size = new Size(77, 17);
            dateTimeLabel.TabIndex = 4;
            dateTimeLabel.Text = "Date/Time:";
            // 
            // datePicker
            // 
            datePicker.Format = DateTimePickerFormat.Short;
            datePicker.Location = new Point(95, 72);
            datePicker.Name = "datePicker";
            datePicker.Size = new Size(130, 22);
            datePicker.TabIndex = 5;
            // 
            // timePicker
            // 
            timePicker.Format = DateTimePickerFormat.Time;
            timePicker.Location = new Point(231, 72);
            timePicker.Name = "timePicker";
            timePicker.ShowUpDown = true;
            timePicker.Size = new Size(141, 22);
            timePicker.TabIndex = 6;
            // 
            // descriptionLabel
            // 
            descriptionLabel.AutoSize = true;
            descriptionLabel.Location = new Point(12, 105);
            descriptionLabel.Name = "descriptionLabel";
            descriptionLabel.Size = new Size(83, 17);
            descriptionLabel.TabIndex = 7;
            descriptionLabel.Text = "Description:";
            // 
            // descriptionTextBox
            // 
            descriptionTextBox.Location = new Point(95, 102);
            descriptionTextBox.Multiline = true;
            descriptionTextBox.Name = "descriptionTextBox";
            descriptionTextBox.Size = new Size(277, 60);
            descriptionTextBox.TabIndex = 8;
            // 
            // colorLabel
            // 
            colorLabel.AutoSize = true;
            colorLabel.Location = new Point(12, 175);
            colorLabel.Name = "colorLabel";
            colorLabel.Size = new Size(45, 17);
            colorLabel.TabIndex = 9;
            colorLabel.Text = "Color:";
            // 
            // colorButton
            // 
            colorButton.BackColor = Color.Blue;
            colorButton.Location = new Point(95, 170);
            colorButton.Name = "colorButton";
            colorButton.Size = new Size(30, 30);
            colorButton.TabIndex = 10;
            colorButton.UseVisualStyleBackColor = false;
            colorButton.Click += new EventHandler(this.colorButton_Click);
            // 
            // saveButton
            // 
            saveButton.DialogResult = DialogResult.OK;
            saveButton.Location = new Point(95, 215);
            saveButton.Name = "saveButton";
            saveButton.Size = new Size(130, 30);
            saveButton.TabIndex = 11;
            saveButton.Text = "Save";
            saveButton.UseVisualStyleBackColor = true;
            saveButton.Click += new EventHandler(this.saveButton_Click);
            // 
            // cancelButton
            // 
            cancelButton.DialogResult = DialogResult.Cancel;
            cancelButton.Location = new Point(242, 215);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new Size(130, 30);
            cancelButton.TabIndex = 12;
            cancelButton.Text = "Cancel";
            cancelButton.UseVisualStyleBackColor = true;
            // 
            // EventForm
            // 
            AcceptButton = saveButton;
            CancelButton = cancelButton;
            ClientSize = new Size(384, 257);
            Controls.Add(cancelButton);
            Controls.Add(saveButton);
            Controls.Add(colorButton);
            Controls.Add(colorLabel);
            Controls.Add(descriptionTextBox);
            Controls.Add(descriptionLabel);
            Controls.Add(timePicker);
            Controls.Add(datePicker);
            Controls.Add(dateTimeLabel);
            Controls.Add(typeComboBox);
            Controls.Add(typeLabel);
            Controls.Add(titleTextBox);
            Controls.Add(titleLabel);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "EventForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Event";
            ResumeLayout(false);
            PerformLayout();
        }

        private Label titleLabel;
        private TextBox titleTextBox;
        private Label typeLabel;
        private ComboBox typeComboBox;
        private Label dateTimeLabel;
        private DateTimePicker datePicker;
        private DateTimePicker timePicker;
        private Label descriptionLabel;
        private TextBox descriptionTextBox;
        private Label colorLabel;
        private Button colorButton;
        private Button saveButton;
        private Button cancelButton;

        private void PopulateEventTypes()
        {
            typeComboBox.Items.Clear();
            typeComboBox.Items.Add(new { Text = "Flight Arrival", Value = EventType.FlightArrival });
            typeComboBox.Items.Add(new { Text = "Flight Departure", Value = EventType.FlightDeparture });
            typeComboBox.Items.Add(new { Text = "Meeting", Value = EventType.Meeting });
            typeComboBox.Items.Add(new { Text = "Reminder", Value = EventType.Reminder });

            typeComboBox.DisplayMember = "Text";
            typeComboBox.ValueMember = "Value";
        }

        private void LoadEventData()
        {
            titleTextBox.Text = _scheduledEvent.Title;

            // Set the event type
            foreach (var item in typeComboBox.Items)
            {
                dynamic typeItem = item;
                if ((EventType)typeItem.Value == _scheduledEvent.Type)
                {
                    typeComboBox.SelectedItem = item;
                    break;
                }
            }

            // Default to first item if none selected
            if (typeComboBox.SelectedIndex == -1 && typeComboBox.Items.Count > 0)
                typeComboBox.SelectedIndex = 0;

            // Set date and time
            DateTime eventTime = _scheduledEvent.EventTime;
            datePicker.Value = eventTime.Date;
            timePicker.Value = eventTime;

            descriptionTextBox.Text = _scheduledEvent.Description;

            // Set color button
            colorButton.BackColor = _scheduledEvent.IndicatorColor;
        }

        private void typeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (typeComboBox.SelectedItem != null)
            {
                dynamic selectedType = typeComboBox.SelectedItem;
                EventType eventType = (EventType)selectedType.Value;

                // Set default color based on event type
                switch (eventType)
                {
                    case EventType.FlightArrival:
                        colorButton.BackColor = Color.Green;
                        break;
                    case EventType.FlightDeparture:
                        colorButton.BackColor = Color.Blue;
                        break;
                    case EventType.Meeting:
                        colorButton.BackColor = Color.AliceBlue;
                        break;
                    case EventType.Reminder:
                        colorButton.BackColor = Color.Red;
                        break;
                }
            }
        }

        private void colorButton_Click(object sender, EventArgs e)
        {
            using (ColorDialog colorDialog = new ColorDialog())
            {
                colorDialog.Color = colorButton.BackColor;
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    colorButton.BackColor = colorDialog.Color;
                }
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(titleTextBox.Text))
            {
                MessageBox.Show("Please enter a title for the event.", "Missing Information",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                DialogResult = DialogResult.None;
                return;
            }

            // Save event data
            _scheduledEvent.Title = titleTextBox.Text;

            if (typeComboBox.SelectedItem != null)
            {
                dynamic selectedType = typeComboBox.SelectedItem;
                _scheduledEvent.Type = (EventType)selectedType.Value;
            }

            // Combine date and time
            DateTime eventDate = datePicker.Value.Date;
            DateTime eventTime = timePicker.Value;
            _scheduledEvent.EventTime = eventDate.AddHours(eventTime.Hour).AddMinutes(eventTime.Minute);

            _scheduledEvent.Description = descriptionTextBox.Text;
            _scheduledEvent.IndicatorColor = colorButton.BackColor;
            _scheduledEvent.TimeZone = _timeZoneModel.TimeZoneInfo;
        }
    }
}


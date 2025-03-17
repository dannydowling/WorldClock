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
                // Editing an existing event
                _scheduledEvent = existingEvent;
                Text = "Edit Event";
            }
            else
            {
                // Creating a new event
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
            this.titleLabel = new System.Windows.Forms.Label();
            this.titleTextBox = new System.Windows.Forms.TextBox();
            this.typeLabel = new System.Windows.Forms.Label();
            this.typeComboBox = new System.Windows.Forms.ComboBox();
            this.dateTimeLabel = new System.Windows.Forms.Label();
            this.datePicker = new System.Windows.Forms.DateTimePicker();
            this.timePicker = new System.Windows.Forms.DateTimePicker();
            this.descriptionLabel = new System.Windows.Forms.Label();
            this.descriptionTextBox = new System.Windows.Forms.TextBox();
            this.colorLabel = new System.Windows.Forms.Label();
            this.colorButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Location = new System.Drawing.Point(12, 15);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(35, 17);
            this.titleLabel.TabIndex = 0;
            this.titleLabel.Text = "Title:";
            // 
            // titleTextBox
            // 
            this.titleTextBox.Location = new System.Drawing.Point(95, 12);
            this.titleTextBox.Name = "titleTextBox";
            this.titleTextBox.Size = new System.Drawing.Size(277, 22);
            this.titleTextBox.TabIndex = 1;
            // 
            // typeLabel
            // 
            this.typeLabel.AutoSize = true;
            this.typeLabel.Location = new System.Drawing.Point(12, 45);
            this.typeLabel.Name = "typeLabel";
            this.typeLabel.Size = new System.Drawing.Size(44, 17);
            this.typeLabel.TabIndex = 2;
            this.typeLabel.Text = "Type:";
            // 
            // typeComboBox
            // 
            this.typeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.typeComboBox.FormattingEnabled = true;
            this.typeComboBox.Location = new System.Drawing.Point(95, 42);
            this.typeComboBox.Name = "typeComboBox";
            this.typeComboBox.Size = new System.Drawing.Size(277, 24);
            this.typeComboBox.TabIndex = 3;
            this.typeComboBox.SelectedIndexChanged += new System.EventHandler(this.typeComboBox_SelectedIndexChanged);
            // 
            // dateTimeLabel
            // 
            this.dateTimeLabel.AutoSize = true;
            this.dateTimeLabel.Location = new System.Drawing.Point(12, 75);
            this.dateTimeLabel.Name = "dateTimeLabel";
            this.dateTimeLabel.Size = new System.Drawing.Size(77, 17);
            this.dateTimeLabel.TabIndex = 4;
            this.dateTimeLabel.Text = "Date/Time:";
            // 
            // datePicker
            // 
            this.datePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.datePicker.Location = new System.Drawing.Point(95, 72);
            this.datePicker.Name = "datePicker";
            this.datePicker.Size = new System.Drawing.Size(130, 22);
            this.datePicker.TabIndex = 5;
            // 
            // timePicker
            // 
            this.timePicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.timePicker.Location = new System.Drawing.Point(231, 72);
            this.timePicker.Name = "timePicker";
            this.timePicker.ShowUpDown = true;
            this.timePicker.Size = new System.Drawing.Size(141, 22);
            this.timePicker.TabIndex = 6;
            // 
            // descriptionLabel
            // 
            this.descriptionLabel.AutoSize = true;
            this.descriptionLabel.Location = new System.Drawing.Point(12, 105);
            this.descriptionLabel.Name = "descriptionLabel";
            this.descriptionLabel.Size = new System.Drawing.Size(83, 17);
            this.descriptionLabel.TabIndex = 7;
            this.descriptionLabel.Text = "Description:";
            // 
            // descriptionTextBox
            // 
            this.descriptionTextBox.Location = new System.Drawing.Point(95, 102);
            this.descriptionTextBox.Multiline = true;
            this.descriptionTextBox.Name = "descriptionTextBox";
            this.descriptionTextBox.Size = new System.Drawing.Size(277, 60);
            this.descriptionTextBox.TabIndex = 8;
            // 
            // colorLabel
            // 
            this.colorLabel.AutoSize = true;
            this.colorLabel.Location = new System.Drawing.Point(12, 175);
            this.colorLabel.Name = "colorLabel";
            this.colorLabel.Size = new System.Drawing.Size(45, 17);
            this.colorLabel.TabIndex = 9;
            this.colorLabel.Text = "Color:";
            // 
            // colorButton
            // 
            this.colorButton.BackColor = System.Drawing.Color.Blue;
            this.colorButton.Location = new System.Drawing.Point(95, 170);
            this.colorButton.Name = "colorButton";
            this.colorButton.Size = new System.Drawing.Size(30, 30);
            this.colorButton.TabIndex = 10;
            this.colorButton.UseVisualStyleBackColor = false;
            this.colorButton.Click += new System.EventHandler(this.colorButton_Click);
            // 
            // saveButton
            // 
            this.saveButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.saveButton.Location = new System.Drawing.Point(95, 215);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(130, 30);
            this.saveButton.TabIndex = 11;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(242, 215);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(130, 30);
            this.cancelButton.TabIndex = 12;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // EventForm
            // 
            this.AcceptButton = this.saveButton;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(384, 257);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.colorButton);
            this.Controls.Add(this.colorLabel);
            this.Controls.Add(this.descriptionTextBox);
            this.Controls.Add(this.descriptionLabel);
            this.Controls.Add(this.timePicker);
            this.Controls.Add(this.datePicker);
            this.Controls.Add(this.dateTimeLabel);
            this.Controls.Add(this.typeComboBox);
            this.Controls.Add(this.typeLabel);
            this.Controls.Add(this.titleTextBox);
            this.Controls.Add(this.titleLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EventForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Event";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.TextBox titleTextBox;
        private System.Windows.Forms.Label typeLabel;
        private System.Windows.Forms.ComboBox typeComboBox;
        private System.Windows.Forms.Label dateTimeLabel;
        private System.Windows.Forms.DateTimePicker datePicker;
        private System.Windows.Forms.DateTimePicker timePicker;
        private System.Windows.Forms.Label descriptionLabel;
        private System.Windows.Forms.TextBox descriptionTextBox;
        private System.Windows.Forms.Label colorLabel;
        private System.Windows.Forms.Button colorButton;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button cancelButton;

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


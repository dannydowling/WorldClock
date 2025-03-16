// -- TimezoneDetector/Forms/MainForm.cs --
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WorldClock
{
    public partial class MainForm : Form
    {
        private readonly TimezoneService _timezoneService;

        public MainForm()
        {
            InitializeComponent();
            _timezoneService = new TimezoneService();
            LoadTimezones();
        }

        private void LoadTimezones()
        {
            // Get available timezones from service
            var timezoneList = _timezoneService.GetAvailableTimezones();

            // Add to combo box
            foreach (var timezone in timezoneList)
            {
                cboTimezones.Items.Add(timezone);
            }

            // Display local timezone information
            lblLocalZoneValue.Text = _timezoneService.GetLocalTimezoneName();
            lblUtcOffsetValue.Text = _timezoneService.GetLocalUtcOffset();
            lblLocalTimeValue.Text = DateTime.Now.ToString();

            // Select an appropriate default timezone
            string defaultTimezone = _timezoneService.GetDefaultTimezone();
            if (!string.IsNullOrEmpty(defaultTimezone))
            {
                cboTimezones.SelectedItem = defaultTimezone;
            }
            else if (cboTimezones.Items.Count > 0)
            {
                cboTimezones.SelectedIndex = 0;
            }
        }

        private void CboTimezones_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateSelectedTimezone();
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            UpdateSelectedTimezone();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            lblLocalTimeValue.Text = DateTime.Now.ToString();
        }

        private void UpdateSelectedTimezone()
        {
            if (cboTimezones.SelectedItem == null)
                return;

            string selectedTimezone = cboTimezones.SelectedItem.ToString();

            // Get timezone info from service
            var timezoneInfo = _timezoneService.GetTimezoneInfo(selectedTimezone);

            // Update the UI with the timezone information
            lblSelectedZoneValue.Text = timezoneInfo.DisplayName;
            lblSelectedTimeValue.Text = timezoneInfo.CurrentTime;
            lblTimeDiffValue.Text = timezoneInfo.OffsetFromLocal;
        }
    }
}

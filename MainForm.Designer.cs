// -- TimezoneDetector/Forms/MainForm.Designer.cs --
namespace WorldClock
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            lblLocalTitle = new Label();
            lblLocalTime = new Label();
            lblLocalTimeValue = new Label();
            lblLocalZone = new Label();
            lblLocalZoneValue = new Label();
            lblUtcOffset = new Label();
            lblUtcOffsetValue = new Label();
            lblWorldTitle = new Label();
            lblSelectZone = new Label();
            cboTimezones = new ComboBox();
            pnlResults = new Panel();
            lblTimeDiffValue = new Label();
            lblTimeDiff = new Label();
            lblSelectedTimeValue = new Label();
            lblSelectedTime = new Label();
            lblSelectedZoneValue = new Label();
            lblSelectedZone = new Label();
            btnRefresh = new Button();
            timer = new System.Windows.Forms.Timer(components);
            pnlResults.SuspendLayout();
            SuspendLayout();
            // 
            // lblLocalTitle
            // 
            lblLocalTitle.AutoSize = true;
            lblLocalTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            lblLocalTitle.Location = new Point(20, 20);
            lblLocalTitle.Name = "lblLocalTitle";
            lblLocalTitle.Size = new Size(253, 21);
            lblLocalTitle.TabIndex = 0;
            lblLocalTitle.Text = "Your Local Timezone Information";
            // 
            // lblLocalTime
            // 
            lblLocalTime.AutoSize = true;
            lblLocalTime.Location = new Point(20, 50);
            lblLocalTime.Name = "lblLocalTime";
            lblLocalTime.Size = new Size(111, 15);
            lblLocalTime.TabIndex = 1;
            lblLocalTime.Text = "Current Local Time:";
            // 
            // lblLocalTimeValue
            // 
            lblLocalTimeValue.AutoSize = true;
            lblLocalTimeValue.Location = new Point(180, 50);
            lblLocalTimeValue.Name = "lblLocalTimeValue";
            lblLocalTimeValue.Size = new Size(12, 15);
            lblLocalTimeValue.TabIndex = 2;
            lblLocalTimeValue.Text = "-";
            // 
            // lblLocalZone
            // 
            lblLocalZone.AutoSize = true;
            lblLocalZone.Location = new Point(20, 75);
            lblLocalZone.Name = "lblLocalZone";
            lblLocalZone.Size = new Size(88, 15);
            lblLocalZone.TabIndex = 3;
            lblLocalZone.Text = "Your Timezone:";
            // 
            // lblLocalZoneValue
            // 
            lblLocalZoneValue.AutoSize = true;
            lblLocalZoneValue.Location = new Point(180, 75);
            lblLocalZoneValue.Name = "lblLocalZoneValue";
            lblLocalZoneValue.Size = new Size(12, 15);
            lblLocalZoneValue.TabIndex = 4;
            lblLocalZoneValue.Text = "-";
            // 
            // lblUtcOffset
            // 
            lblUtcOffset.AutoSize = true;
            lblUtcOffset.Location = new Point(20, 100);
            lblUtcOffset.Name = "lblUtcOffset";
            lblUtcOffset.Size = new Size(105, 15);
            lblUtcOffset.TabIndex = 5;
            lblUtcOffset.Text = "Current UTC Offset:";
            // 
            // lblUtcOffsetValue
            // 
            lblUtcOffsetValue.AutoSize = true;
            lblUtcOffsetValue.Location = new Point(180, 100);
            lblUtcOffsetValue.Name = "lblUtcOffsetValue";
            lblUtcOffsetValue.Size = new Size(12, 15);
            lblUtcOffsetValue.TabIndex = 6;
            lblUtcOffsetValue.Text = "-";
            // 
            // lblWorldTitle
            // 
            lblWorldTitle.AutoSize = true;
            lblWorldTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            lblWorldTitle.Location = new Point(20, 140);
            lblWorldTitle.Name = "lblWorldTitle";
            lblWorldTitle.Size = new Size(143, 21);
            lblWorldTitle.TabIndex = 7;
            lblWorldTitle.Text = "World Timezones";
            // 
            // lblSelectZone
            // 
            lblSelectZone.AutoSize = true;
            lblSelectZone.Location = new Point(20, 170);
            lblSelectZone.Name = "lblSelectZone";
            lblSelectZone.Size = new Size(94, 15);
            lblSelectZone.TabIndex = 8;
            lblSelectZone.Text = "Select Timezone:";
            // 
            // cboTimezones
            // 
            cboTimezones.DropDownStyle = ComboBoxStyle.DropDownList;
            cboTimezones.FormattingEnabled = true;
            cboTimezones.Location = new Point(180, 170);
            cboTimezones.Name = "cboTimezones";
            cboTimezones.Size = new Size(350, 23);
            cboTimezones.TabIndex = 9;
            cboTimezones.SelectedIndexChanged += CboTimezones_SelectedIndexChanged;
            // 
            // pnlResults
            // 
            pnlResults.BorderStyle = BorderStyle.FixedSingle;
            pnlResults.Controls.Add(lblTimeDiffValue);
            pnlResults.Controls.Add(lblTimeDiff);
            pnlResults.Controls.Add(lblSelectedTimeValue);
            pnlResults.Controls.Add(lblSelectedTime);
            pnlResults.Controls.Add(lblSelectedZoneValue);
            pnlResults.Controls.Add(lblSelectedZone);
            pnlResults.Location = new Point(20, 200);
            pnlResults.Name = "pnlResults";
            pnlResults.Size = new Size(540, 200);
            pnlResults.TabIndex = 10;
            // 
            // lblTimeDiffValue
            // 
            lblTimeDiffValue.AutoSize = true;
            lblTimeDiffValue.Location = new Point(160, 60);
            lblTimeDiffValue.Name = "lblTimeDiffValue";
            lblTimeDiffValue.Size = new Size(12, 15);
            lblTimeDiffValue.TabIndex = 5;
            lblTimeDiffValue.Text = "-";
            // 
            // lblTimeDiff
            // 
            lblTimeDiff.AutoSize = true;
            lblTimeDiff.Location = new Point(10, 60);
            lblTimeDiff.Name = "lblTimeDiff";
            lblTimeDiff.Size = new Size(123, 15);
            lblTimeDiff.TabIndex = 4;
            lblTimeDiff.Text = "Difference from Local:";
            // 
            // lblSelectedTimeValue
            // 
            lblSelectedTimeValue.AutoSize = true;
            lblSelectedTimeValue.Location = new Point(160, 35);
            lblSelectedTimeValue.Name = "lblSelectedTimeValue";
            lblSelectedTimeValue.Size = new Size(12, 15);
            lblSelectedTimeValue.TabIndex = 3;
            lblSelectedTimeValue.Text = "-";
            // 
            // lblSelectedTime
            // 
            lblSelectedTime.AutoSize = true;
            lblSelectedTime.Location = new Point(10, 35);
            lblSelectedTime.Name = "lblSelectedTime";
            lblSelectedTime.Size = new Size(79, 15);
            lblSelectedTime.TabIndex = 2;
            lblSelectedTime.Text = "Current Time:";
            // 
            // lblSelectedZoneValue
            // 
            lblSelectedZoneValue.AutoSize = true;
            lblSelectedZoneValue.Location = new Point(160, 10);
            lblSelectedZoneValue.Name = "lblSelectedZoneValue";
            lblSelectedZoneValue.Size = new Size(12, 15);
            lblSelectedZoneValue.TabIndex = 1;
            lblSelectedZoneValue.Text = "-";
            // 
            // lblSelectedZone
            // 
            lblSelectedZone.AutoSize = true;
            lblSelectedZone.Location = new Point(10, 10);
            lblSelectedZone.Name = "lblSelectedZone";
            lblSelectedZone.Size = new Size(105, 15);
            lblSelectedZone.TabIndex = 0;
            lblSelectedZone.Text = "Selected Timezone:";
            // 
            // btnRefresh
            // 
            btnRefresh.Location = new Point(250, 410);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(100, 30);
            btnRefresh.TabIndex = 11;
            btnRefresh.Text = "Refresh Times";
            btnRefresh.UseVisualStyleBackColor = true;
            btnRefresh.Click += BtnRefresh_Click;
            // 
            // timer
            // 
            timer.Enabled = true;
            timer.Interval = 1000;
            timer.Tick += Timer_Tick;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(584, 450);
            Controls.Add(btnRefresh);
            Controls.Add(pnlResults);
            Controls.Add(cboTimezones);
            Controls.Add(lblSelectZone);
            Controls.Add(lblWorldTitle);
            Controls.Add(lblUtcOffsetValue);
            Controls.Add(lblUtcOffset);
            Controls.Add(lblLocalZoneValue);
            Controls.Add(lblLocalZone);
            Controls.Add(lblLocalTimeValue);
            Controls.Add(lblLocalTime);
            Controls.Add(lblLocalTitle);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Timezone Detector";
            pnlResults.ResumeLayout(false);
            pnlResults.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblLocalTitle;
        private Label lblLocalTime;
        private Label lblLocalTimeValue;
        private Label lblLocalZone;
        private Label lblLocalZoneValue;
        private Label lblUtcOffset;
        private Label lblUtcOffsetValue;
        private Label lblWorldTitle;
        private Label lblSelectZone;
        private ComboBox cboTimezones;
        private Panel pnlResults;
        private Label lblTimeDiffValue;
        private Label lblTimeDiff;
        private Label lblSelectedTimeValue;
        private Label lblSelectedTime;
        private Label lblSelectedZoneValue;
        private Label lblSelectedZone;
        private Button btnRefresh;
        private System.Windows.Forms.Timer timer;
    }
}

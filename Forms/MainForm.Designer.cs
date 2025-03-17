// File: MainForm.Designer.cs
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
            if (disposing)
            {
                _updateTimer?.Stop();
                _updateTimer?.Dispose();
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
            clockFlowLayoutPanel = new FlowLayoutPanel();
            textBox1 = new TextBox();
            _updateTimer = new System.Windows.Forms.Timer(components);
            controlPanel = new Panel();
            addTimeZoneButton = new Button();
            displayNameTextBox = new TextBox();
            displayNameLabel = new Label();
            timeZoneComboBox = new ComboBox();
            selectTimeZoneLabel = new Label();
            clockFlowLayoutPanel.SuspendLayout();
            controlPanel.SuspendLayout();
            SuspendLayout();
            // 
            // clockFlowLayoutPanel
            // 
            clockFlowLayoutPanel.AutoScroll = true;
            clockFlowLayoutPanel.Controls.Add(textBox1);
            clockFlowLayoutPanel.Dock = DockStyle.Fill;
            clockFlowLayoutPanel.Location = new Point(0, 0);
            clockFlowLayoutPanel.Margin = new Padding(3, 4, 3, 4);
            clockFlowLayoutPanel.Name = "clockFlowLayoutPanel";
            clockFlowLayoutPanel.Padding = new Padding(10, 12, 10, 12);
            clockFlowLayoutPanel.Size = new Size(1100, 750);
            clockFlowLayoutPanel.TabIndex = 0;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(13, 15);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(976, 27);
            textBox1.TabIndex = 0;
            // 
            // _updateTimer
            // 
            _updateTimer.Interval = 1000;
            _updateTimer.Tick += UpdateTimer_Tick;
            // 
            // controlPanel
            // 
            controlPanel.Controls.Add(addTimeZoneButton);
            controlPanel.Controls.Add(displayNameTextBox);
            controlPanel.Controls.Add(displayNameLabel);
            controlPanel.Controls.Add(timeZoneComboBox);
            controlPanel.Controls.Add(selectTimeZoneLabel);
            controlPanel.Dock = DockStyle.Bottom;
            controlPanel.Location = new Point(0, 0);
            controlPanel.Margin = new Padding(3, 4, 3, 4);
            controlPanel.Name = "controlPanel";
            controlPanel.Size = new Size(1100, 60);
            controlPanel.TabIndex = 1;
            // 
            // addTimeZoneButton
            // 
            addTimeZoneButton.Location = new Point(958, 6);
            addTimeZoneButton.Margin = new Padding(3, 4, 3, 4);
            addTimeZoneButton.Name = "addTimeZoneButton";
            addTimeZoneButton.Size = new Size(130, 38);
            addTimeZoneButton.TabIndex = 4;
            addTimeZoneButton.Text = "Add Time Zone";
            addTimeZoneButton.UseVisualStyleBackColor = true;
            addTimeZoneButton.Click += addTimeZoneButton_Click;
            // 
            // displayNameTextBox
            // 
            displayNameTextBox.Location = new Point(686, 11);
            displayNameTextBox.Margin = new Padding(3, 4, 3, 4);
            displayNameTextBox.Name = "displayNameTextBox";
            displayNameTextBox.Size = new Size(266, 27);
            displayNameTextBox.TabIndex = 3;
            // 
            // displayNameLabel
            // 
            displayNameLabel.AutoSize = true;
            displayNameLabel.Location = new Point(575, 15);
            displayNameLabel.Name = "displayNameLabel";
            displayNameLabel.Size = new Size(105, 20);
            displayNameLabel.TabIndex = 2;
            displayNameLabel.Text = "Display Name:";
            // 
            // timeZoneComboBox
            // 
            timeZoneComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            timeZoneComboBox.FormattingEnabled = true;
            timeZoneComboBox.Location = new Point(145, 13);
            timeZoneComboBox.Margin = new Padding(3, 4, 3, 4);
            timeZoneComboBox.Name = "timeZoneComboBox";
            timeZoneComboBox.Size = new Size(424, 28);
            timeZoneComboBox.TabIndex = 1;
            timeZoneComboBox.SelectedIndexChanged += timeZoneComboBox_SelectedIndexChanged;
            // 
            // selectTimeZoneLabel
            // 
            selectTimeZoneLabel.AutoSize = true;
            selectTimeZoneLabel.Location = new Point(12, 16);
            selectTimeZoneLabel.Name = "selectTimeZoneLabel";
            selectTimeZoneLabel.Size = new Size(127, 20);
            selectTimeZoneLabel.TabIndex = 0;
            selectTimeZoneLabel.Text = "Select Time Zone:";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1100, 750);
            Controls.Add(controlPanel);
            Controls.Add(clockFlowLayoutPanel);
            Margin = new Padding(3, 4, 3, 4);
            Name = "MainForm";
            Text = "World Clocks";
            Load += MainForm_Load;
            clockFlowLayoutPanel.ResumeLayout(false);
            clockFlowLayoutPanel.PerformLayout();
            controlPanel.ResumeLayout(false);
            controlPanel.PerformLayout();
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel clockFlowLayoutPanel;
        private System.Windows.Forms.Timer _updateTimer;
        private System.Windows.Forms.Panel controlPanel;
        private System.Windows.Forms.ComboBox timeZoneComboBox;
        private System.Windows.Forms.Button addTimeZoneButton;
        private System.Windows.Forms.TextBox displayNameTextBox;
        private System.Windows.Forms.Label displayNameLabel;
        private System.Windows.Forms.Label selectTimeZoneLabel;
        private TextBox textBox1;
    }
}
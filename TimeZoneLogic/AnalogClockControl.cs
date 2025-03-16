// File: Controls/AnalogClockControl.cs
using System;
using System.Drawing;
using System.Windows.Forms;

namespace WorldClock.Controls
{
    public class AnalogClockControl : UserControl
    {
        private System.Windows.Forms.Timer _timer;
        private DateTime _currentTime;
        private Font _digitalFont;
        private string _timeZoneName;

        public string TimeZoneName
        {
            get { return _timeZoneName; }
            set
            {
                _timeZoneName = value;
                Invalidate();
            }
        }

        public DateTime CurrentTime
        {
            get { return _currentTime; }
            set
            {
                _currentTime = value;
                Invalidate();
            }
        }

        public AnalogClockControl()
        {
            // Set default size
            Size = new Size(150, 200);

            // Initialize the digital font
            _digitalFont = new Font("Arial", 10, FontStyle.Regular);

            // Set double buffered to avoid flickering
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                    ControlStyles.AllPaintingInWmPaint |
                    ControlStyles.UserPaint, true);

            // Default time is current time
            _currentTime = DateTime.Now;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // Calculate the center and radius of the clock
            int clockDiameter = Math.Min(Width, Height - 40); // Leave space for digital time
            int radius = clockDiameter / 2;
            Point center = new Point(Width / 2, radius + 10); // 10 pixels padding from top

            // Draw clock face
            g.FillEllipse(Brushes.White, center.X - radius, center.Y - radius, clockDiameter, clockDiameter);
            g.DrawEllipse(new Pen(Color.Black, 2), center.X - radius, center.Y - radius, clockDiameter, clockDiameter);

            // Draw hour marks
            for (int i = 1; i <= 12; i++)
            {
                double angle = Math.PI / 6 * (i - 3); // Start at 12 o'clock (- 3 hours)
                int markLength = radius / 10;

                // Calculate outer point
                int x1 = (int)(center.X + (radius - 5) * Math.Cos(angle));
                int y1 = (int)(center.Y + (radius - 5) * Math.Sin(angle));

                // Calculate inner point
                int x2 = (int)(center.X + (radius - 5 - markLength) * Math.Cos(angle));
                int y2 = (int)(center.Y + (radius - 5 - markLength) * Math.Sin(angle));

                g.DrawLine(new Pen(Color.Black, 2), x1, y1, x2, y2);

                // Draw hour numbers
                int numX = (int)(center.X + (radius - 20) * Math.Cos(angle));
                int numY = (int)(center.Y + (radius - 20) * Math.Sin(angle));

                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;

                g.DrawString(i.ToString(), new Font("Arial", 8), Brushes.Black, numX, numY, sf);
            }

            // Calculate hand angles
            double hourAngle = (_currentTime.Hour % 12 + _currentTime.Minute / 60.0) * 30 * Math.PI / 180;
            double minuteAngle = _currentTime.Minute * 6 * Math.PI / 180;
            double secondAngle = _currentTime.Second * 6 * Math.PI / 180;

            // Draw hour hand
            int hourHandLength = (int)(radius * 0.5);
            int hourX = (int)(center.X + hourHandLength * Math.Sin(hourAngle));
            int hourY = (int)(center.Y - hourHandLength * Math.Cos(hourAngle));
            g.DrawLine(new Pen(Color.Black, 3), center.X, center.Y, hourX, hourY);

            // Draw minute hand
            int minuteHandLength = (int)(radius * 0.7);
            int minuteX = (int)(center.X + minuteHandLength * Math.Sin(minuteAngle));
            int minuteY = (int)(center.Y - minuteHandLength * Math.Cos(minuteAngle));
            g.DrawLine(new Pen(Color.Black, 2), center.X, center.Y, minuteX, minuteY);

            // Draw second hand
            int secondHandLength = (int)(radius * 0.8);
            int secondX = (int)(center.X + secondHandLength * Math.Sin(secondAngle));
            int secondY = (int)(center.Y - secondHandLength * Math.Cos(secondAngle));
            g.DrawLine(new Pen(Color.Red, 1), center.X, center.Y, secondX, secondY);

            // Draw center point
            g.FillEllipse(Brushes.Black, center.X - 4, center.Y - 4, 8, 8);

            // Draw digital time (update to 12-hour format)
            string digitalTime = _currentTime.ToString("h:mm:ss tt"); // Changed to 12-hour format with AM/PM
            StringFormat digitalFormat = new StringFormat();
            digitalFormat.Alignment = StringAlignment.Center;
            digitalFormat.LineAlignment = StringAlignment.Center;

            RectangleF digitalRect = new RectangleF(0, center.Y + radius + 10, Width, 20);
            g.DrawString(digitalTime, _digitalFont, Brushes.Black, digitalRect, digitalFormat);         

            // Draw time zone name
            if (!string.IsNullOrEmpty(_timeZoneName))
            {
                RectangleF nameRect = new RectangleF(0, center.Y + radius + 30, Width, 20);
                g.DrawString(_timeZoneName, _digitalFont, Brushes.Blue, nameRect, digitalFormat);
            }
        }
    }
}

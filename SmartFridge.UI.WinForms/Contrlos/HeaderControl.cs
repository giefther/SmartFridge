using SmartFridge.Core.Interfaces;
using SmartFridge.Core.Models;
using SmartFridge.UI.WinForms.Styles;

namespace SmartFridge.UI.WinForms.Controls
{
    public partial class HeaderControl : UserControl
    {
        public event EventHandler LogoutClicked;

        private Label lblTime;
        private Label lblTemperature;
        private Label lblUsername;
        private Button btnLogout;
        private System.Windows.Forms.Timer timeTimer;
        private readonly ITemperatureService _temperatureService;

        public HeaderControl(User user, ITemperatureService temperatureService)
        {
            _temperatureService = temperatureService;
            InitializeComponent(user);
            InitializeTimer();
            InitializeTemperature();
        }

        private void InitializeTemperature()
        {
            var currentTemp = _temperatureService.GetCurrentTemperature();
            UpdateTemperatureDisplay(currentTemp);

            _temperatureService.TemperatureChanged += (s, temp) => UpdateTemperatureDisplay(temp);
        }
        private void UpdateTemperatureDisplay(double temperature)
        {
            if (lblTemperature != null && !lblTemperature.IsDisposed)
            {
                lblTemperature.Text = $"❄️ {temperature}°C";
            }
        }

        private void InitializeComponent(User user)
        {
            this.SuspendLayout();

            // Основной контейнер header'а - используем стили для UserControl
            this.BackColor = CustomFormStyles.DarkColor;
            this.Padding = new Padding(20, 5, 20, 10);
            this.Height = 60; 

            var leftPanel = new Panel
            {
                Dock = DockStyle.Left,
                Width = 200,
                Padding = new Padding(0, 5, 0, 5),
                BackColor = Color.Transparent
            };

            // Время
            lblTime = new Label
            {
                Text = "🕐 14:30",
                AutoSize = true,
                Location = new Point(0, 0)
            }.AsHeader().WithWhiteText();

            // Температура
            lblTemperature = new Label
            {
                Text = "❄️ 0°C",
                AutoSize = true,
                Location = new Point(0, 25)
            }.AsNormal().WithWhiteText();

            leftPanel.Controls.AddRange(new Control[] { lblTime, lblTemperature });

            var rightPanel = new Panel
            {
                Dock = DockStyle.Right,
                Width = 250,
                Padding = new Padding(0, 10, 0, 10),
                BackColor = Color.Transparent
            };

            lblUsername = new Label
            {
                Text = $"👤 {user.Username}",
                AutoSize = true,
                Location = new Point(35, 8)
            }.AsHeader().WithWhiteText();

            btnLogout = new Button
            {
                Text = "Выйти",
                Size = new Size(80, 35),
                Location = new Point(160, 3)
            }.AsLight();
            btnLogout.Click += (s, e) => LogoutClicked?.Invoke(this, e);

            rightPanel.Controls.AddRange(new Control[] { lblUsername, btnLogout });

            this.Controls.AddRange(new Control[] { leftPanel, rightPanel });

            this.ResumeLayout(false);
        }

        private void InitializeTimer()
        {
            timeTimer = new System.Windows.Forms.Timer { Interval = 1000 }; 
            timeTimer.Tick += (s, e) => UpdateTime();
            timeTimer.Start();
            UpdateTime(); 
        }

        private void UpdateTime()
        {
            if (lblTime != null && !lblTime.IsDisposed)
            {
                lblTime.Text = $"🕐 {DateTime.Now:HH:mm}";
            }
        }

        public void UpdateTemperature(int temperature)
        {
            if (lblTemperature != null && !lblTemperature.IsDisposed)
            {
                lblTemperature.Text = $"❄️ {temperature}°C";
            }
        }

        protected override void Dispose(bool disposing)
        {
            timeTimer?.Stop();
            timeTimer?.Dispose();
            base.Dispose(disposing);
        }
    }
}
using SmartFridge.Core.Interfaces;
using SmartFridge.Core.Models;
using SmartFridge.UI.WinForms.Styles;

namespace SmartFridge.UI.WinForms.Controls
{
    public partial class NotificationsControl : UserControl
    {
        private readonly IProductService _productService;
        private readonly ITemperatureService _temperatureService;
        private FlowLayoutPanel notificationsPanel;
        private Label emptyLabel;
        private Label notificationsTitle;
        private List<Notification> _currentNotificaitons;

        public NotificationsControl(IProductService productService, ITemperatureService temperatureService)
        {
            _productService = productService;
            _temperatureService = temperatureService;

            InitializeComponent();
            LoadNotifications();

            _temperatureService.TemperatureChanged += (s, temp) => RefreshNotifications();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Основной контейнер
            var notificationsContainer = new Panel().AsCard();
            notificationsContainer.Dock = DockStyle.Fill;

            // Заголовок
            notificationsTitle = new Label
            {
                Text = "🔔 Уведомления",
                Dock = DockStyle.Top,
                Height = 30,
                TextAlign = ContentAlignment.MiddleLeft
            };
            notificationsTitle.AsHeader();

            // Панель для уведомлений
            notificationsPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false
            };

            emptyLabel = new Label
            {
                Text = "Здесь будут уведомления",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };
            emptyLabel.AsNormal();
            emptyLabel.ForeColor = CustomFormStyles.SecondaryColor;

            notificationsContainer.Controls.AddRange(new Control[] {
                notificationsPanel, notificationsTitle
            });

            this.Controls.Add(notificationsContainer);
            this.ResumeLayout(false);
        }

        private List<Notification> GenerateNotifications()
        {
            var notifications = new List<Notification>();

            // Проверяем продукты
            var products = _productService.GetAllProducts();
            CheckProductNotifications(products, notifications);

            // Проверяем температуру
            var currentTemp = _temperatureService.GetCurrentTemperature();
            CheckTemperatureNotifications(currentTemp, notifications);

            return notifications;
        }
        private void CheckProductNotifications(IEnumerable<Product> products, List<Notification> notifications)
        {
            var expiredCount = _productService.GetExpiredProducts().Count();
            var soonCount = _productService.GetExpiringSoonProducts(3).Count();

            // Уведомление о просроченных продуктах
            if (expiredCount > 0)
            {
                notifications.Add(new Notification
                {
                    Message = $"У вас есть просроченные продукты ({expiredCount} шт.)",
                    Type = NotificationType.Expired
                });
            }

            // Уведомление о скоро истекающих продуктах
            if (soonCount > 0)
            {
                notifications.Add(new Notification
                {
                    Message = $"Некоторые продукты скоро испортятся ({soonCount} шт.)",
                    Type = NotificationType.SoonExpired
                });
            }
        }
        private void CheckTemperatureNotifications(double currentTemp, List<Notification> notifications)
        {
            // Низкая температура
            if (currentTemp < 2)
            {
                notifications.Add(new Notification
                {
                    Message = $"Температура низкая: {currentTemp}°C (рекомендуется +2°C...+6°C)",
                    Type = NotificationType.TooCold
                });
            }

            // Высокая температура
            if (currentTemp > 6)
            {
                notifications.Add(new Notification
                {
                    Message = $"Температура высокая: {currentTemp}°C (рекомендуется +2°C...+6°C)",
                    Type = NotificationType.TooHot
                });
            }
        }
        public void RefreshNotifications()
        {
            var notifications = GenerateNotifications();
            _currentNotificaitons = notifications;
            DisplayNotifications(notifications);
        }
        public void RedisplayNotifications()
        {
            DisplayNotifications(_currentNotificaitons);
        }
        private void LoadNotifications() => RefreshNotifications();
        private void DisplayNotifications(List<Notification> notifications)
        {
            notificationsPanel.Controls.Clear();

            if (notifications.Count == 0)
            {
                notificationsPanel.Controls.Add(emptyLabel);
                return;
            }

            foreach (var notification in notifications)
            {
                var notificationItem = CreateNotificationItem(notification);
                notificationsPanel.Controls.Add(notificationItem);
            }
        }
        private Panel CreateNotificationItem(Notification notification)
        {
            var panel = new Panel
            {
                Width = notificationsPanel.ClientSize.Width - 25,
                BackColor = GetNotificationColor(notification.Type),
            }.AsNotificationsPanel();

            // Иконка уведомления
            var iconLabel = new Label
            {
                Text = GetNotificationIcon(notification.Type),
                Location = new Point(5, 15),
                Size = new Size(30, 30),
                Font = new Font("Segoe UI", 12),
                TextAlign = ContentAlignment.MiddleCenter,
            };

            // Текст уведомления
            var textLabel = new Label
            {
                Text = notification.Message,
                Location = new Point(40, 15),
                Size = new Size(panel.Width - 50, 30),
                Font = CustomFormStyles.NormalFont,
                TextAlign = ContentAlignment.MiddleLeft,
            };

            panel.Controls.AddRange(new Control[] { iconLabel, textLabel });

            return panel;
        }
        private Color GetNotificationColor(NotificationType type)
        {
            return type switch
            {
                NotificationType.Expired => Color.FromArgb(255, 200, 200),   // Светло-красный
                NotificationType.SoonExpired => Color.FromArgb(255, 235, 200),  // Светло-оранжевый
                NotificationType.TooCold => Color.FromArgb(200, 230, 255),     // Светло-синий
                NotificationType.TooHot => Color.FromArgb(250, 145, 5),     // Светло-оранжевый
                _ => Color.FromArgb(225, 225, 225)                         // Светло-серый
            };
        }

        private string GetNotificationIcon(NotificationType type)
        {
            return type switch
            {
                NotificationType.Expired => "🚨",
                NotificationType.SoonExpired => "⚠️",
                NotificationType.TooCold => "❄️",
                NotificationType.TooHot => "🌡️",
                _ => "💡"
            };
        }
    }
}
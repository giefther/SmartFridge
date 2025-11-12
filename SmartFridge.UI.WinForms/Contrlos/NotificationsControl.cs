using SmartFridge.UI.WinForms.Styles;
using SmartFridge.Core.Interfaces;
using SmartFridge.Core.Models;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SmartFridge.UI.WinForms.Controls
{
    public partial class NotificationsControl : UserControl
    {
        private readonly IProductService _productService;
        private readonly ITemperatureService _temperatureService;
        private FlowLayoutPanel notificationsPanel;
        private Label emptyLabel;
        private Label notificationsTitle;

        public NotificationsControl(IProductService productService, ITemperatureService temperatureService)
        {
            _productService = productService;
            _temperatureService = temperatureService;

            InitializeComponent();
            // LoadNotifications() пока закомментируем - реализуем на этапе 2
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

            // Заглушка
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

        // Временный заглушка - реализуем на этапе 2
        private void LoadNotifications()
        {
            // TODO: Реализовать генерацию уведомлений
        }

        public void UpdateNotifications(List<string> notifications)
        {
            // TODO: Реализовать когда будет сервис уведомлений
        }
    }
}
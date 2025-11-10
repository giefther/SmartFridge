using SmartFridge.UI.WinForms.Styles;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SmartFridge.UI.WinForms.Controls
{
    public partial class NotificationsControl : UserControl
    {
        public event EventHandler NotificationClicked;

        private Panel notificationsContainer;
        private Label notificationsTitle;
        private Label notificationsPlaceholder;

        public NotificationsControl()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Основной контейнер
            notificationsContainer = new Panel().AsCard();
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

            // Заглушка
            notificationsPlaceholder = new Label
            {
                Text = "Здесь будут уведомления",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };
            notificationsPlaceholder.AsNormal();
            notificationsPlaceholder.ForeColor = CustomFormStyles.SecondaryColor;

            notificationsContainer.Controls.AddRange(new Control[] {
                notificationsPlaceholder, notificationsTitle
            });

            this.Controls.Add(notificationsContainer);
            this.ResumeLayout(false);
        }

        public void UpdateNotifications(List<string> notifications)
        {
            // TODO: Реализовать когда будет сервис уведомлений
        }
    }
}
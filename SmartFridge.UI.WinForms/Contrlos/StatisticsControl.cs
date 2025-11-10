using SmartFridge.UI.WinForms.Styles;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace SmartFridge.UI.WinForms.Controls
{
    public partial class StatisticsControl : UserControl
    {
        // Лейблы статистики продуктов
        public Label TotalValueLabel;
        public Label FreshValueLabel;
        public Label SoonValueLabel;
        public Label ExpiredValueLabel;

        private Panel statContainer;
        private Label statTitle;
        private Panel statsPanel;

        public StatisticsControl()
        {
            InitializeComponent();
            CreateStatContent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Основной контейнер
            statContainer = new Panel().AsCard();
            statContainer.Dock = DockStyle.Fill;

            // Заголовок
            statTitle = new Label
            {
                Text = "📊 Статистика",
                Dock = DockStyle.Top,
                Height = 30,
                TextAlign = ContentAlignment.MiddleLeft
            };
            statTitle.AsHeader();

            // Панель для статистики
            statsPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(10, 5, 10, 5)
            };

            statContainer.Controls.AddRange(new Control[] { statsPanel, statTitle });
            this.Controls.Add(statContainer);

            this.ResumeLayout(false);
        }

        private void CreateStatContent()
        {
            statsPanel.Controls.Clear();

            // Создаем элементы статистики с ref
            CreateStatItem("Всего", "0", Color.Gray, ref TotalValueLabel);
            CreateStatItem("Свежих", "0", Color.Green, ref FreshValueLabel);
            CreateStatItem("Скоро истекает", "0", Color.Orange, ref SoonValueLabel);
            CreateStatItem("Просрочено", "0", Color.Red, ref ExpiredValueLabel);
        }

        private void CreateStatItem(string title, string value, Color color, ref Label valueLabel)
        {
            var itemPanel = new Panel
            {
                Height = 25,
                Dock = DockStyle.Top,
                Margin = new Padding(0, 2, 0, 2)
            };
            statsPanel.Controls.Add(itemPanel);

            // Цветной символ ●
            var colorLabel = new Label
            {
                Text = "●",
                Location = new Point(5, 4),
                Size = new Size(15, 15),
                Font = new Font("Segoe UI", 10),
                ForeColor = color,
                TextAlign = ContentAlignment.MiddleCenter
            };
            itemPanel.Controls.Add(colorLabel);

            // Название категории
            var titleLabel = new Label
            {
                Text = title,
                Location = new Point(25, 4),
                Size = new Size(100, 18),
                Font = CustomFormStyles.SmallFont,
                ForeColor = CustomFormStyles.DarkColor,
                TextAlign = ContentAlignment.MiddleLeft
            };
            itemPanel.Controls.Add(titleLabel);

            // Значение (сохраняем ссылку)
            valueLabel = new Label
            {
                Text = value,
                Location = new Point(120, 4),
                Size = new Size(40, 18),
                Font = CustomFormStyles.NormalFont,
                ForeColor = CustomFormStyles.DarkColor,
                TextAlign = ContentAlignment.MiddleRight
            };
            itemPanel.Controls.Add(valueLabel);
        }

        public void UpdateStatistics(int total, int fresh, int soon, int expired)
        {
            if (TotalValueLabel != null) TotalValueLabel.Text = total.ToString();
            if (FreshValueLabel != null) FreshValueLabel.Text = fresh.ToString();
            if (SoonValueLabel != null) SoonValueLabel.Text = soon.ToString();
            if (ExpiredValueLabel != null) ExpiredValueLabel.Text = expired.ToString();
        }
    }
}
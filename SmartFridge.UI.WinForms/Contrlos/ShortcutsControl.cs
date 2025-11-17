using SmartFridge.UI.WinForms.Styles;

namespace SmartFridge.UI.WinForms.Controls
{
    public partial class ShortcutsControl : UserControl
    {
        private Panel shortcutsPanel;
        private Label shortcutsTitle;

        public ShortcutsControl()
        {
            InitializeComponent();
            CreateShortcutsContent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            shortcutsPanel = new Panel().AsCard();
            shortcutsPanel.Dock = DockStyle.Fill;
            shortcutsPanel.Padding = new Padding(15);

            shortcutsTitle = new Label
            {
                Text = "⌨️ Горячие клавиши",
                Dock = DockStyle.Top,
                Height = 30,
                TextAlign = ContentAlignment.MiddleLeft
            };
            shortcutsTitle.AsHeader();

            shortcutsPanel.Controls.Add(shortcutsTitle);
            this.Controls.Add(shortcutsPanel);

            this.ResumeLayout(false);
        }

        private void CreateShortcutsContent()
        {
            var contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(0, 40, 0, 0)
            };

            // Группа управления продуктами
            CreateShortcutGroup(contentPanel, "📦 Продукты:", 40,
                new[]
                {
                    ("➕", "CTRL + A", "Добавить продукт"),
                    ("➖", "CTRL + R", "Удалить продукт")
                });

            // Группа управления температурой
            CreateShortcutGroup(contentPanel, "🌡️ Температура:", 160,
                new[]
                {
                    ("☀️", "CTRL + U", "Увеличить температуру"),
                    ("❄️", "CTRL + D", "Уменьшить температуру")
                });

            shortcutsPanel.Controls.Add(contentPanel);
        }

        private void CreateShortcutGroup(Panel parent, string groupTitle, int topOffset, (string icon, string keys, string action)[] shortcuts)
        {
            var groupTitleLabel = new Label
            {
                Text = groupTitle,
                Location = new Point(0, topOffset),
                Size = new Size(250, 25),
                Font = CustomFormStyles.HeaderFont,
                ForeColor = CustomFormStyles.DarkColor
            };
            parent.Controls.Add(groupTitleLabel);

            for (int i = 0; i < shortcuts.Length; i++)
            {
                var (icon, keys, action) = shortcuts[i];
                CreateShortcutItem(parent, topOffset + 30 + (i * 40), icon, keys, action);
            }
        }

        private void CreateShortcutItem(Panel parent, int top, string icon, string keys, string action)
        {
            // Иконка
            var iconLabel = new Label
            {
                Text = icon,
                Location = new Point(0, top),
                Size = new Size(30, 25),
                Font = new Font("Segoe UI", 10),
                TextAlign = ContentAlignment.MiddleLeft
            };

            // Клавиши
            var keysLabel = new Label
            {
                Text = keys,
                Location = new Point(30, top),
                Size = new Size(100, 25),
                Font = CustomFormStyles.NormalFont,
                ForeColor = CustomFormStyles.PrimaryColor,
                TextAlign = ContentAlignment.MiddleLeft
            };

            // Действие
            var actionLabel = new Label
            {
                Text = action,
                Location = new Point(130, top),
                Size = new Size(150, 25),
                Font = CustomFormStyles.NormalFont,
                ForeColor = CustomFormStyles.DarkColor,
                TextAlign = ContentAlignment.MiddleLeft
            };

            parent.Controls.AddRange(new Control[] { iconLabel, keysLabel, actionLabel });
        }
    }
}
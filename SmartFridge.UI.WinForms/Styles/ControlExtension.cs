namespace SmartFridge.UI.WinForms.Styles
{
    /// <summary>
    /// Расширенные методы для применения стилей к элементам форм
    /// </summary>
    public static class ControlExtensions
    {
        // ===== Main Form Containers =====

        public static Panel AsLeftCentralContainer(this Panel panel)
        {
            panel.Dock = DockStyle.Left;
            panel.BackColor = Color.White;
            panel.Padding = new Padding(15);
            panel.BorderStyle = BorderStyle.FixedSingle;
            return panel;
        }

        public static Panel AsMainContentCentralContainer(this Panel panel)
        {
            panel.Dock = DockStyle.Fill;
            panel.BackColor = Color.White;
            panel.Padding = new Padding(15);
            panel.BorderStyle = BorderStyle.FixedSingle;
            return panel;
        }

        public static Panel AsRightCentralContainer(this Panel panel)
        {
            panel.Dock = DockStyle.Right;
            panel.BackColor = Color.White;
            panel.Padding = new Padding(15);
            panel.BorderStyle = BorderStyle.FixedSingle;
            return panel;
        }

        public static Panel AsTopContainer(this Panel panel)
        {
            panel.Dock = DockStyle.Top;
            panel.Height = 150;
            panel.BackColor = CustomFormStyles.PrimaryColor;
            panel.Padding = new Padding(0);
            panel.Margin = new Padding(0);
            return panel;
        }

        public static Panel AsCentralContainer(this Panel panel)
        {
            panel.Dock = DockStyle.Fill;
            panel.BackColor = CustomFormStyles.LightColor;
            panel.Padding = new Padding(20);
            panel.Margin = new Padding(0);
            return panel;
        }

        public static Panel AsBottomContainer(this Panel panel)
        {
            panel.Dock = DockStyle.Bottom;
            panel.Height = 80;
            panel.BackColor = CustomFormStyles.DarkColor;
            panel.Padding = new Padding(10);
            panel.Margin = new Padding(0);
            return panel;
        }

        // ===== Button =====
        public static Button AsClearSearch(this Button button)
        {
            button.Size = new Size(30, 25);
            button.Location = new Point(205, 5);
            button.Font = new Font("Segoe UI", 8);
            button.BackColor = Color.LightGray;
            button.FlatStyle = FlatStyle.Flat;
            return button;
        }
        public static Button AsDark(this Button button)
        {
            button.ForeColor = CustomFormStyles.SecondaryColor;
            button.BackColor = CustomFormStyles.DarkColor;
            button.FlatStyle = FlatStyle.Flat;
            button.Font = CustomFormStyles.NormalFont;
            button.FlatAppearance.BorderSize = 1;
            button.FlatAppearance.BorderColor = CustomFormStyles.SecondaryColor;
            button.Cursor = Cursors.Hand;
            button.Padding = new Padding(10, 5, 10, 5);
            return button;
        }

        public static Button AsLight(this Button button)
        {
            button.ForeColor = CustomFormStyles.PrimaryColor;
            button.BackColor = CustomFormStyles.LightColor;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 1;
            button.FlatAppearance.BorderColor = CustomFormStyles.PrimaryColor;
            button.Font = CustomFormStyles.NormalFont;
            button.Cursor = Cursors.Hand;
            button.Padding = new Padding(10, 5, 10, 5);
            return button;
        }

        public static Button AsSuccess(this Button button)
        {
            button.BackColor = CustomFormStyles.SuccessColor;
            button.ForeColor = Color.White;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Font = CustomFormStyles.NormalFont;
            button.Cursor = Cursors.Hand;
            button.Padding = new Padding(10, 5, 10, 5);
            return button;
        }

        public static Button AsDanger(this Button button)
        {
            button.BackColor = CustomFormStyles.DangerColor;
            button.ForeColor = Color.White;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Font = CustomFormStyles.NormalFont;
            button.Cursor = Cursors.Hand;
            button.Padding = new Padding(10, 5, 10, 5);
            return button;
        }

        // ===== Label =====

        public static Label WithWhiteText(this Label label)
        {
            label.ForeColor = Color.White;
            return label;
        }

        public static Label AsTitle(this Label label)
        {
            label.Font = CustomFormStyles.TitleFont;
            label.ForeColor = CustomFormStyles.DarkColor;
            return label;
        }

        public static Label AsHeader(this Label label)
        {
            label.Font = CustomFormStyles.HeaderFont;
            label.ForeColor = CustomFormStyles.DarkColor;
            return label;
        }

        public static Label AsNormal(this Label label)
        {
            label.Font = CustomFormStyles.NormalFont;
            label.ForeColor = CustomFormStyles.DarkColor;
            return label;
        }

        // ===== Panel =====

        public static Panel AsContainer(this Panel panel)
        {
            panel.BackColor = Color.White;
            panel.BorderStyle = BorderStyle.FixedSingle;
            panel.Padding = new Padding(20);
            return panel;
        }

        public static Panel AsNotificationsItem(this Panel panel)
        {
            panel.Height = 60;
            panel.Margin = new Padding(0, 2, 0, 2);
            panel.Padding = new Padding(10);
            return panel;
        }

        public static Panel AsCard(this Panel panel)
        {
            panel.BackColor = Color.White;
            panel.BorderStyle = BorderStyle.FixedSingle;
            panel.Padding = new Padding(15);
            return panel;
        }

        // ===== TextBox =====

        public static TextBox AsTextField(this TextBox textBox)
        {
            textBox.Font = CustomFormStyles.NormalFont;
            textBox.BorderStyle = BorderStyle.FixedSingle;
            textBox.BackColor = Color.White;
            return textBox;
        }

        // ===== Form =====

        public static Form AsMainForm(this Form form)
        {
            form.BackColor = CustomFormStyles.LightColor;
            form.Font = CustomFormStyles.NormalFont;
            return form;
        }

        public static Form AsDialogForm(this Form form)
        {
            form.BackColor = Color.White;
            form.Font = CustomFormStyles.NormalFont;
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.MaximizeBox = false;
            form.MinimizeBox = false;
            return form;
        }

        // ===== ComboBox =====

        public static ComboBox AsDropdown(this ComboBox comboBox)
        {
            comboBox.Font = CustomFormStyles.NormalFont;
            comboBox.FlatStyle = FlatStyle.Flat;
            comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            return comboBox;
        }

        // ===== DataGridView =====

        public static DataGridView AsTable(this DataGridView dataGrid)
        {
            dataGrid.BackgroundColor = CustomFormStyles.LightColor;
            dataGrid.Font = CustomFormStyles.NormalFont;
            dataGrid.BorderStyle = BorderStyle.None;
            dataGrid.EnableHeadersVisualStyles = false;
            dataGrid.ColumnHeadersDefaultCellStyle.BackColor = CustomFormStyles.PrimaryColor;
            dataGrid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGrid.ColumnHeadersDefaultCellStyle.Font = CustomFormStyles.NormalFont;
            dataGrid.RowHeadersVisible = false;
            dataGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            // Настройка стиля ячеек
            dataGrid.DefaultCellStyle.SelectionBackColor = CustomFormStyles.SecondaryColor;
            dataGrid.DefaultCellStyle.SelectionForeColor = CustomFormStyles.LightColor;
            dataGrid.DefaultCellStyle.Font = CustomFormStyles.NormalFont;
            dataGrid.EnableHeadersVisualStyles = false;
            dataGrid.ColumnHeadersDefaultCellStyle.BackColor = CustomFormStyles.SecondaryColor;
            dataGrid.ColumnHeadersDefaultCellStyle.ForeColor = CustomFormStyles.LightColor;
            dataGrid.ColumnHeadersDefaultCellStyle.Font = CustomFormStyles.NormalFont;
            dataGrid.ColumnHeadersDefaultCellStyle.SelectionBackColor = CustomFormStyles.SecondaryColor;
            dataGrid.ColumnHeadersDefaultCellStyle.SelectionForeColor = CustomFormStyles.LightColor;
            return dataGrid;
        }
    }
}
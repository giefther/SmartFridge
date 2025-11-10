using System.Drawing;
using System.Windows.Forms;

namespace SmartFridge.UI.WinForms.Styles
{
    /// <summary>
    /// Расширенные методы для применения стилей к элементам форм
    /// </summary>
    public static class ControlExtensions
    {
        // ===== Main Form Containers =====

        public static Panel AsTopContainer(this Panel panel)
        {
            panel.Dock = DockStyle.Top;
            panel.Height = 150; // Базовая высота, будет пересчитана в форме
            panel.BackColor = CustomFormStyles.PrimaryColor;
            panel.Padding = new Padding(0);
            panel.Margin = new Padding(0);
            return panel;
        }

        public static Panel AsHeaderContainer(this Panel panel)
        {
            panel.Dock = DockStyle.Top;
            panel.BackColor = CustomFormStyles.PrimaryColor;
            panel.Padding = new Padding(20, 5, 20, 10);
            return panel;
        }

        public static Panel AsToolbarContainer(this Panel panel)
        {
            panel.Dock = DockStyle.Bottom;
            panel.BackColor = CustomFormStyles.SecondaryColor;
            panel.Padding = new Padding(15, 8, 15, 8);
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
            panel.Height = 80; // Базовая высота, будет пересчитана в форме
            panel.BackColor = CustomFormStyles.DarkColor;
            panel.Padding = new Padding(10);
            panel.Margin = new Padding(0);
            return panel;
        }

        public static Label AsContainerLabel(this Label label)
        {
            label.Dock = DockStyle.Fill;
            label.TextAlign = ContentAlignment.MiddleCenter;
            label.Font = CustomFormStyles.NormalFont;
            return label;
        }

        public static Label AsTopContainerLabel(this Label label)
        {
            label.AsContainerLabel();
            label.ForeColor = Color.White;
            label.Font = CustomFormStyles.HeaderFont;
            return label;
        }

        public static Label AsCentralContainerLabel(this Label label)
        {
            label.AsContainerLabel();
            label.ForeColor = CustomFormStyles.DarkColor;
            label.Font = CustomFormStyles.HeaderFont;
            return label;
        }

        public static Label AsBottomContainerLabel(this Label label)
        {
            label.AsContainerLabel();
            label.ForeColor = Color.White;
            label.Font = CustomFormStyles.NormalFont;
            return label;
        }

        // ===== Button =====

        public static Button AsDark(this Button button)
        {
            button.ForeColor = CustomFormStyles.SecondaryColor;
            button.BackColor = CustomFormStyles.DarkColor;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 1;
            button.FlatAppearance.BorderColor = CustomFormStyles.SecondaryColor;
            button.Font = CustomFormStyles.NormalFont;
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

        public static Button AsWarning(this Button button)
        {
            button.BackColor = CustomFormStyles.WarningColor;
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

        public static Button WithWhiteText(this Button button)
        {
            button.ForeColor = Color.White;
            return button;
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

        public static Label AsSmall(this Label label)
        {
            label.Font = CustomFormStyles.SmallFont;
            label.ForeColor = CustomFormStyles.DarkColor;
            return label;
        }

        public static Label AsSuccess(this Label label)
        {
            label.Font = CustomFormStyles.NormalFont;
            label.ForeColor = CustomFormStyles.SuccessColor;
            return label;
        }

        public static Label AsDanger(this Label label)
        {
            label.Font = CustomFormStyles.NormalFont;
            label.ForeColor = CustomFormStyles.DangerColor;
            return label;
        }

        public static Label AsWarning(this Label label)
        {
            label.Font = CustomFormStyles.NormalFont;
            label.ForeColor = CustomFormStyles.WarningColor;
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

        public static Panel AsCard(this Panel panel)
        {
            panel.BackColor = Color.White;
            panel.BorderStyle = BorderStyle.FixedSingle;
            panel.Padding = new Padding(15);
            return panel;
        }

        public static Panel AsTransparent(this Panel panel)
        {
            panel.BackColor = Color.Transparent;
            panel.BorderStyle = BorderStyle.None;
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

        public static TextBox AsLargeTextField(this TextBox textBox)
        {
            textBox.Font = CustomFormStyles.HeaderFont;
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
            return dataGrid;
        }

        // ===== CheckBox ====

        public static CheckBox AsCheckbox(this CheckBox checkBox)
        {
            checkBox.Font = CustomFormStyles.NormalFont;
            checkBox.ForeColor = CustomFormStyles.DarkColor;
            return checkBox;
        }

        // ===== GroupBox =====

        public static GroupBox AsGroup(this GroupBox groupBox)
        {
            groupBox.Font = CustomFormStyles.HeaderFont;
            groupBox.ForeColor = CustomFormStyles.DarkColor;
            return groupBox;
        }

        // ===== Дополнительные стилевые помощники =====

        public static Control WithMargin(this Control control, int all)
        {
            control.Margin = new Padding(all);
            return control;
        }

        public static Control WithMargin(this Control control, int left, int top, int right, int bottom)
        {
            control.Margin = new Padding(left, top, right, bottom);
            return control;
        }

        public static Control WithPadding(this Control control, int all)
        {
            control.Padding = new Padding(all);
            return control;
        }

        public static Control WithPadding(this Control control, int left, int top, int right, int bottom)
        {
            control.Padding = new Padding(left, top, right, bottom);
            return control;
        }

        public static Control Centered(this Control control)
        {
            if (control.Parent != null)
            {
                control.Anchor = AnchorStyles.None;
                control.Left = (control.Parent.ClientSize.Width - control.Width) / 2;
                control.Top = (control.Parent.ClientSize.Height - control.Height) / 2;
            }
            return control;
        }
    }
}
using SmartFridge.UI.WinForms.Styles;

namespace SmartFridge.UI.WinForms.Forms
{
    partial class AddProductForm
    {
        private System.ComponentModel.IContainer components = null;
        private TableLayoutPanel mainTable;
        private Label lblName;
        private TextBox txtName;
        private Label lblCategory;
        private ComboBox cmbCategory;
        private Label lblQuantity;
        private TextBox txtQuantity;
        private Label lblUnit;
        private TextBox txtUnit;
        private Label lblExpirationDate;
        private DateTimePicker dtpExpirationDate;
        private FlowLayoutPanel buttonsPanel;
        private Button btnAdd;
        private Button btnCancel;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.mainTable = new TableLayoutPanel();
            this.lblName = new Label();
            this.txtName = new TextBox();
            this.lblCategory = new Label();
            this.cmbCategory = new ComboBox();
            this.lblQuantity = new Label();
            this.txtQuantity = new TextBox();
            this.lblUnit = new Label();
            this.txtUnit = new TextBox();
            this.lblExpirationDate = new Label();
            this.dtpExpirationDate = new DateTimePicker();
            this.buttonsPanel = new FlowLayoutPanel();
            this.btnAdd = new Button();
            this.btnCancel = new Button();

            this.SuspendLayout();

            // Main Table
            this.mainTable.Dock = DockStyle.Fill;
            this.mainTable.ColumnCount = 2;
            this.mainTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));
            this.mainTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            this.mainTable.RowCount = 6;
            this.mainTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            this.mainTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            this.mainTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            this.mainTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            this.mainTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            this.mainTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
            this.mainTable.Padding = new Padding(20);

            // Labels and Controls
            this.lblName.Text = "Название:";
            this.lblName.TextAlign = ContentAlignment.MiddleLeft;
            this.lblName.Dock = DockStyle.Fill;

            this.txtName.Dock = DockStyle.Fill;
            this.txtName.AsTextField();

            this.lblCategory.Text = "Категория:";
            this.lblCategory.TextAlign = ContentAlignment.MiddleLeft;
            this.lblCategory.Dock = DockStyle.Fill;

            this.cmbCategory.Dock = DockStyle.Fill;
            this.cmbCategory.AsDropdown();
            this.cmbCategory.Items.AddRange(new object[] {
                "Молочные",
                "Фрукты",
                "Овощи",
                "Мясо",
                "Рыба",
                "Напитки",
                "Бакалея"
            });

            this.lblQuantity.Text = "Количество:";
            this.lblQuantity.TextAlign = ContentAlignment.MiddleLeft;
            this.lblQuantity.Dock = DockStyle.Fill;

            this.txtQuantity.Dock = DockStyle.Fill;
            this.txtQuantity.AsTextField();
            this.txtQuantity.Text = "1";

            this.lblUnit.Text = "Единица:";
            this.lblUnit.TextAlign = ContentAlignment.MiddleLeft;
            this.lblUnit.Dock = DockStyle.Fill;

            this.txtUnit.Dock = DockStyle.Fill;
            this.txtUnit.AsTextField();
            this.txtUnit.Text = "шт";

            this.lblExpirationDate.Text = "Срок годности:";
            this.lblExpirationDate.TextAlign = ContentAlignment.MiddleLeft;
            this.lblExpirationDate.Dock = DockStyle.Fill;

            this.dtpExpirationDate.Dock = DockStyle.Fill;
            this.dtpExpirationDate.Format = DateTimePickerFormat.Short;
            this.dtpExpirationDate.Value = DateTime.Now.AddDays(7);

            // Buttons Panel
            this.buttonsPanel.Dock = DockStyle.Fill;
            this.buttonsPanel.FlowDirection = FlowDirection.RightToLeft;
            this.buttonsPanel.Padding = new Padding(0, 10, 0, 0);

            this.btnAdd.Text = "Добавить";
            this.btnAdd.Size = new Size(100, 35);
            this.btnAdd.AsSuccess();
            this.btnAdd.Click += new EventHandler(this.btnAdd_Click);

            this.btnCancel.Text = "Отмена";
            this.btnCancel.Size = new Size(100, 35);
            this.btnCancel.AsLight();
            this.btnCancel.Click += new EventHandler(this.btnCancel_Click);

            // Add controls to table
            this.mainTable.Controls.Add(this.lblName, 0, 0);
            this.mainTable.Controls.Add(this.txtName, 1, 0);
            this.mainTable.Controls.Add(this.lblCategory, 0, 1);
            this.mainTable.Controls.Add(this.cmbCategory, 1, 1);
            this.mainTable.Controls.Add(this.lblQuantity, 0, 2);
            this.mainTable.Controls.Add(this.txtQuantity, 1, 2);
            this.mainTable.Controls.Add(this.lblUnit, 0, 3);
            this.mainTable.Controls.Add(this.txtUnit, 1, 3);
            this.mainTable.Controls.Add(this.lblExpirationDate, 0, 4);
            this.mainTable.Controls.Add(this.dtpExpirationDate, 1, 4);
            this.mainTable.Controls.Add(this.buttonsPanel, 0, 5);
            this.mainTable.SetColumnSpan(this.buttonsPanel, 2);

            // Add buttons to panel
            this.buttonsPanel.Controls.Add(this.btnCancel);
            this.buttonsPanel.Controls.Add(this.btnAdd);

            // Form
            this.Controls.Add(this.mainTable);
            this.ResumeLayout(false);
        }
    }
}
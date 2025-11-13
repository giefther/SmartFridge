using SmartFridge.Core.Interfaces;
using SmartFridge.Core.Models;
using SmartFridge.UI.WinForms.Styles;

namespace SmartFridge.UI.WinForms.Forms
{
    public partial class AddProductForm : Form
    {
        public Product? CreatedProduct { get; private set; }
        private readonly IProductService _productService;

        public AddProductForm(IProductService productService)
        {
            _productService = productService;
            InitializeComponent();
            InitializeForm();
        }

        private void InitializeForm()
        {
            this.AsDialogForm();
            this.Text = "Добавить продукт";
            this.MinimumSize = new Size(450, 400);

            // Выбираем первую категорию по умолчанию
            if (cmbCategory.Items.Count > 0)
                cmbCategory.SelectedIndex = 0;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateForm())
                return;

            try
            {
                var product = new Product
                {
                    Name = txtName.Text.Trim(),
                    Category = new Category { Name = cmbCategory.Text },
                    Quantity = double.Parse(txtQuantity.Text),
                    Unit = txtUnit.Text.Trim(),
                    ExpirationDate = dtpExpirationDate.Value.Date
                };

                _productService.AddProduct(product);
                CreatedProduct = product;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении продукта: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Введите название продукта", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(cmbCategory.Text))
            {
                MessageBox.Show("Выберите категорию", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbCategory.Focus();
                return false;
            }

            if (!double.TryParse(txtQuantity.Text, out double quantity) || quantity <= 0)
            {
                MessageBox.Show("Введите корректное количество", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtQuantity.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtUnit.Text))
            {
                MessageBox.Show("Введите единицу измерения", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUnit.Focus();
                return false;
            }

            if (dtpExpirationDate.Value.Date < DateTime.Today)
            {
                MessageBox.Show("Срок годности не может быть в прошлом", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpExpirationDate.Focus();
                return false;
            }

            return true;
        }
    }
}
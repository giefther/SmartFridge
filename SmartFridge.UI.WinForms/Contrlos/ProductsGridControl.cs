using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SmartFridge.Core.Models;
using SmartFridge.UI.WinForms.Styles;

namespace SmartFridge.UI.WinForms.Controls
{
    public partial class ProductsGridControl : UserControl
    {
        public event EventHandler SelectionChanged;

        public DataGridView Grid => productsDataGrid;
        public Product SelectedProduct => productsDataGrid.SelectedRows.Count > 0
            ? productsDataGrid.SelectedRows[0].DataBoundItem as Product
            : null;

        private DataGridView productsDataGrid;
        private TextBox searchTextBox;
        private Label statusLabel;
        private Button btnClearSearch;
        private List<Product> _allProducts;
        private List<Product> _filteredProducts;

        public ProductsGridControl()
        {
            InitializeComponent();
            SetupDataGridColumns();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Панель поиска
            var searchPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 40,
                Padding = new Padding(0, 5, 0, 5)
            };

            // Поле поиска
            searchTextBox = new TextBox
            {
                PlaceholderText = "🔍 Поиск продуктов...",
                Dock = DockStyle.Left,
                Width = 200
            }.AsTextField();
            searchTextBox.TextChanged += SearchTextBox_TextChanged;

            // ✅ КНОПКА ОЧИСТКИ ПОИСКА
            var btnClearSearch = new Button
            {
                Text = "❌",
            }.AsClearSearch();

            btnClearSearch.Click += BtnClearSearch_Click;


            // Статус
            statusLabel = new Label
            {
                Dock = DockStyle.Right,
                TextAlign = ContentAlignment.MiddleRight,
                AutoSize = false,
                Width = 150
            }.AsNormal();


            // Добавляем элементы в searchPanel
            searchPanel.Controls.AddRange(new Control[] { searchTextBox, btnClearSearch, statusLabel });


            productsDataGrid = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoGenerateColumns = false // Исключает столбцы id, category, addeddate
            }.AsTable();
            productsDataGrid.SelectionChanged += (s, e) => SelectionChanged?.Invoke(this, e);
            productsDataGrid.RowPrePaint += ProductsDataGrid_RowPrePaint;


            this.Controls.Add(productsDataGrid); // DockStyle.Fill 
            this.Controls.Add(searchPanel);      // DockStyle.Top 

            this.ResumeLayout(false);
        }

        private void SetupDataGridColumns()
        {
            productsDataGrid.Columns.Clear();

            productsDataGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Name",
                HeaderText = "Название",
                DataPropertyName = "Name",
                Width = 150
            });

            productsDataGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Category",
                HeaderText = "Категория",
                DataPropertyName = "CategoryName",
                Width = 120
            });

            productsDataGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ExpirationDate",
                HeaderText = "Срок годности",
                DataPropertyName = "ExpirationDate",
                Width = 120,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd.MM.yyyy" }
            });

            productsDataGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Quantity",
                HeaderText = "Количество",
                DataPropertyName = "Quantity",
                Width = 80
            });

            productsDataGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Unit",
                HeaderText = "Ед.",
                DataPropertyName = "Unit",
                Width = 50
            });
        }

        private void ProductsDataGrid_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= productsDataGrid.Rows.Count)
                return;

            var row = productsDataGrid.Rows[e.RowIndex];
            var product = row.DataBoundItem as Product;

            if (product == null)
                return;

            Color backColor = GetProductRowColor(product);
            row.DefaultCellStyle.BackColor = backColor;
            row.DefaultCellStyle.ForeColor = IsLightColor(backColor) ? Color.Black : Color.White;
        }

        private Color GetProductRowColor(Product product)
        {
            var daysUntilExpiration = (product.ExpirationDate - DateTime.Today).TotalDays;

            if (daysUntilExpiration < 0)
                return Color.FromArgb(255, 200, 200); // Светло-красный
            else if (daysUntilExpiration <= 3)
                return Color.FromArgb(255, 255, 200); // Светло-жёлтый
            else
                return Color.FromArgb(200, 255, 200); // Светло-зелёный
        }

        private bool IsLightColor(Color color)
        {
            var brightness = (color.R * 0.299 + color.G * 0.587 + color.B * 0.114) / 255;
            return brightness > 0.5;
        }

        private void SearchTextBox_TextChanged(object sender, EventArgs e)
        {
            FilterProducts();
        }

        private void BtnClearSearch_Click(object sender, EventArgs e)
        {
            searchTextBox.Clear();
        }

        private void FilterProducts()
        {
            var searchText = searchTextBox.Text.Trim().ToLower();

            if (string.IsNullOrEmpty(searchText))
            {
                _filteredProducts = new List<Product>(_allProducts);
            }
            else
            {
                _filteredProducts = _allProducts
                    .Where(p => p.Name.ToLower().Contains(searchText))
                    .ToList();
            }

            productsDataGrid.DataSource = _filteredProducts;
            UpdateStatusLabel();
        }

        private void UpdateStatusLabel()
        {
            if (string.IsNullOrEmpty(searchTextBox.Text))
                statusLabel.Text = $"Всего продуктов: {_filteredProducts?.Count ?? 0}";
            else
                statusLabel.Text = $"Найдено: {_filteredProducts?.Count ?? 0}";
        }

        public void LoadProducts(IEnumerable<Product> products)
        {
            _allProducts = products?.ToList() ?? new List<Product>();
            _filteredProducts = new List<Product>(_allProducts);
            productsDataGrid.DataSource = _filteredProducts;
            UpdateStatusLabel();
        }
    }
}
using SmartFridge.Core.Interfaces;
using SmartFridge.UI.WinForms.Styles;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace SmartFridge.UI.WinForms.Controls
{
    public partial class ToolbarControl : UserControl
    {
        public event EventHandler AddProductClicked;
        public event EventHandler DeleteProductClicked;
        public event EventHandler IncreaseTempClicked;
        public event EventHandler DecreaseTempClicked;
        private readonly ITemperatureService _temperatureService;

        public bool DeleteButtonEnabled
        {
            get => btnDeleteProduct.Enabled;
            set => btnDeleteProduct.Enabled = value;
        }

        private Button btnAddProduct;
        private Button btnDeleteProduct;
        private Button btnIncreaseTemp;
        private Button btnDecreaseTemp;

        public void SimulateAddProductClick() => btnAddProduct.PerformClick();
        public void SimulateDeleteProductClick() => btnDeleteProduct.PerformClick();
        public void SimulateIncreaseTempClick() => btnIncreaseTemp.PerformClick();
        public void SimulateDecreaseTempClick() => btnDecreaseTemp.PerformClick();

        public ToolbarControl(ITemperatureService temperatureService)
        {
            _temperatureService = temperatureService;
            InitializeComponent();
            ApplyStyles();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Основной контейнер toolbar'а
            this.BackColor = CustomFormStyles.SecondaryColor;
            this.Padding = new Padding(15, 8, 15, 8);
            this.Height = 60; // Фиксированная высота

            // Левая часть - управление температурой
            var leftPanel = new Panel
            {
                Dock = DockStyle.Left,
                Width = 350,
                Padding = new Padding(20, 0, 0, 0),
                BackColor = Color.Transparent
            };

            // Кнопка уменьшения температуры
            btnDecreaseTemp = new Button
            {
                Text = "❄️ Уменьшить",
                Size = new Size(165, 45),
                Location = new Point(0, 0)
            }.AsLight();
            btnDecreaseTemp.Click += (s, e) => DecreaseTemperature(); // ← ИЗМЕНИТЬ

            // Кнопка увеличения температуры
            btnIncreaseTemp = new Button
            {
                Text = "☀️ Увеличить",
                Size = new Size(165, 45),
                Location = new Point(175, 0)
            }.AsLight();
            btnIncreaseTemp.Click += (s, e) => IncreaseTemperature(); // ← ИЗМЕНИТЬ

            leftPanel.Controls.AddRange(new Control[] { btnDecreaseTemp, btnIncreaseTemp });

            // Правая часть - управление продуктами
            var rightPanel = new Panel
            {
                Dock = DockStyle.Right,
                Width = 250,
                Padding = new Padding(0, 0, 20, 0),
                BackColor = Color.Transparent
            };

            // Кнопка добавления продукта
            btnAddProduct = new Button
            {
                Text = "➕ Добавить",
                Size = new Size(110, 45),
                Location = new Point(0, 0)
            }.AsSuccess();
            btnAddProduct.Click += (s, e) => AddProductClicked?.Invoke(this, e);

            // Кнопка удаления продукта
            btnDeleteProduct = new Button
            {
                Text = "➖ Удалить",
                Size = new Size(110, 45),
                Location = new Point(120, 0),
                Enabled = false
            }.AsDanger();
            btnDeleteProduct.Click += (s, e) => DeleteProductClicked?.Invoke(this, e);

            rightPanel.Controls.AddRange(new Control[] { btnAddProduct, btnDeleteProduct });

            this.Controls.AddRange(new Control[] { leftPanel, rightPanel });
            this.ResumeLayout(false);
        }
        private void IncreaseTemperature()
        {
            if (_temperatureService.GetCurrentTemperature() <= 9.5)
            {
                try
                {
                    _temperatureService.IncreaseTemperature();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при увеличении температуры: {ex.Message}",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void DecreaseTemperature()
        {
            if (_temperatureService.GetCurrentTemperature() >= -9.5)
            {
                try
                {
                    _temperatureService.DecreaseTemperature();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при уменьшении температуры: {ex.Message}",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ApplyStyles()
        {
            // Дополнительные стили если нужно
        }
    }
}
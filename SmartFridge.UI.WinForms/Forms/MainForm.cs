using SmartFridge.Core.Interfaces;
using SmartFridge.Core.Models;
using SmartFridge.UI.WinForms.Composition;
using SmartFridge.UI.WinForms.Styles;
using System;
using System.Windows.Forms;

namespace SmartFridge.UI.WinForms.Forms
{
    public partial class MainForm : Form
    {
        private readonly User _currentUser;
        private readonly IProductService _productService;

        // Основные контейнеры
        private Panel topContainer;
        private Panel centralContainer;
        private Panel bottomContainer;

        public MainForm(User user)
        {
            _currentUser = user ?? throw new ArgumentNullException(nameof(user));
            _productService = CompositionRoot.GetProductService(user);

            InitializeComponent();
            SetupContainers();
            ApplyStyles();
        }

        private void SetupContainers()
        {
            CreateTopContainer();
            CreateBottomContainer();
            CreateCentralContainer();
        }

        private void CreateTopContainer()
        {
            topContainer = new Panel().AsTopContainer();
            topContainer.Height = CalculatePercentageHeight(20);
            this.Controls.Add(topContainer);
        }

        private void CreateCentralContainer()
        {
            centralContainer = new Panel().AsCentralContainer();
            this.Controls.Add(centralContainer);
        }

        private void CreateBottomContainer()
        {
            bottomContainer = new Panel().AsBottomContainer();
            bottomContainer.Height = CalculatePercentageHeight(10);
            this.Controls.Add(bottomContainer);
        }

        private void ApplyStyles()
        {
            // Стиль формы
            this.AsMainForm();
            this.Text = $"Умный холодильник - {_currentUser.Username}";

            // Добавляем текст для идентификации контейнеров (временный)
            AddContainerLabels();
        }

        private void AddContainerLabels()
        {
            // Метки для визуального отличия контейнеров (убрать в продакшене)
            var topLabel = new Label
            {
                Text = "TOP CONTAINER (20%) - Будут: Header + Toolbar"
            }.AsTopContainerLabel();
            topContainer.Controls.Add(topLabel);

            var centralLabel = new Label
            {
                Text = "CENTRAL CONTAINER (70%) - Будут: Статистика + Список продуктов"
            }.AsCentralContainerLabel();
            centralContainer.Controls.Add(centralLabel);

            var bottomLabel = new Label
            {
                Text = "BOTTOM CONTAINER (10%) - Будет: Footer"
            }.AsBottomContainerLabel();
            bottomContainer.Controls.Add(bottomLabel);
        }

        private int CalculatePercentageHeight(int percentage)
        {
            return (int)(this.ClientSize.Height * (percentage / 100.0));
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            // Обновляем высоты при изменении размера формы
            if (topContainer != null)
                topContainer.Height = CalculatePercentageHeight(20);

            if (bottomContainer != null)
                bottomContainer.Height = CalculatePercentageHeight(10);
        }
    }
}
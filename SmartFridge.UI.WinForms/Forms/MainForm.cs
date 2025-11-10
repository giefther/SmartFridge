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

        // Контейнеры внутри TopContainer
        private Panel headerContainer;
        private Panel toolbarContainer;

        // Относительные величины
        private const int _topToFormPercentage = 21;
        private const int _bottomToFormPercentage = 11;
        private const int _headerToTopPercentage = 31;
        private const int _toolbarToTopPercentage = 71;

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
            topContainer.Height = CalculatePercentageHeight(this.ClientSize.Height, _topToFormPercentage);
            this.Controls.Add(topContainer);

            // Header и Toolbar внутри TopContainer
            CreateHeaderContainer();
            CreateToolbarContainer();
        }

        private void CreateHeaderContainer()
        {
            headerContainer = new Panel().AsHeaderContainer();
            headerContainer.Height = CalculatePercentageHeight(topContainer.Height, _headerToTopPercentage); 
            topContainer.Controls.Add(headerContainer);
        }

        private void CreateToolbarContainer()
        {
            toolbarContainer = new Panel().AsToolbarContainer();
            toolbarContainer.Height = CalculatePercentageHeight(topContainer.Height, _toolbarToTopPercentage);
            toolbarContainer.Dock = DockStyle.Bottom;
            topContainer.Controls.Add(toolbarContainer);
        }

        private void CreateCentralContainer()
        {
            centralContainer = new Panel().AsCentralContainer();
            this.Controls.Add(centralContainer);
        }

        private void CreateBottomContainer()
        {
            bottomContainer = new Panel().AsBottomContainer();
            bottomContainer.Height = CalculatePercentageHeight(this.ClientSize.Height, _bottomToFormPercentage);
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

        /// <summary>
        /// Вычисляет относительную высоту
        /// </summary>
        /// <param name="fromHeight">Изначальная высота, относительно которой необходимо сделать вычисление</param>
        /// <param name="percentage">Отношение в процентах к высоте</param>
        /// <returns></returns>
        private int CalculatePercentageHeight(int fromHeight, int percentage)
        {
            return (int)(fromHeight * (percentage / 100.0));
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            // Обновляем высоты при изменении размера формы
            if (topContainer != null)
                topContainer.Height = CalculatePercentageHeight(this.ClientSize.Height,_topToFormPercentage);

            if (bottomContainer != null)
                bottomContainer.Height = CalculatePercentageHeight(this.ClientSize.Height, _bottomToFormPercentage);

            if (headerContainer != null)
                headerContainer.Height = CalculatePercentageHeight(topContainer.Height, _headerToTopPercentage);

            if (toolbarContainer != null)
                toolbarContainer.Height = CalculatePercentageHeight(topContainer.Height, _toolbarToTopPercentage);
        }
    }
}
using Microsoft.VisualBasic.ApplicationServices;
using SmartFridge.Core.Interfaces;
using SmartFridge.Core.Models;
using SmartFridge.UI.WinForms.Composition;
using SmartFridge.UI.WinForms.Controls;
using SmartFridge.UI.WinForms.Styles;
using System;
using System.Windows.Forms;

namespace SmartFridge.UI.WinForms.Forms
{
    public partial class MainForm : Form
    {
        private readonly SmartFridge.Core.Models.User _currentUser;
        private readonly IProductService _productService;
        private System.Windows.Forms.Timer timeTimer;

        // Контроллы
        private ProductsGridControl productsGridControl;
        private HeaderControl headerControl;
        private ToolbarControl toolbarControl;
        private StatisticsControl statisticsControl;

        // Основные контейнеры
        private Panel topContainer;
        private Panel centralContainer;
        private Panel bottomContainer;

        // Содержимое CentralContainer
        private Panel leftCentralContainer;
        private Panel mainContentCentralContainer;
        private Panel rightCentralContainer;
        private NotificationsControl notificationsControl;

        // Относительные величины
        private const int _topToFormHeightPercentage = 21;
        private const int _bottomToFormHeightPercentage = 11;
        private const int _headerToTopHeightPercentage = 51;
        private const int _toolbarToTopHeightPercentage = 51;
        private const int _leftCentralWidthPercentage = 30;
        private const int _mainCentralWidthPercentage = 50;
        private const int _rightCentralWidthPercentage = 20;
        private const int _statToLeftHeightPercentage = 50;

        public MainForm(SmartFridge.Core.Models.User user)
        {
            _currentUser = user ?? throw new ArgumentNullException(nameof(user));
            _productService = CompositionRoot.GetProductService(user);
            var temperatureService = CompositionRoot.GetTemperatureService(user); 

            InitializeComponent();
            SetupContainers(temperatureService); 
            ApplyStyles();
            LoadProducts();
        }

        private void SetupContainers(ITemperatureService temperatureService)
        {
            CreateCentralContainer();
            CreateCentralContainers();
            CreateTopContainer(temperatureService); 
            CreateBottomContainer();
        }

        private void CreateTopContainer(ITemperatureService temperatureService)
        {
            topContainer = new Panel().AsTopContainer();
            topContainer.Height = CalculatePercentageValue(this.ClientSize.Height, _topToFormHeightPercentage);
            this.Controls.Add(topContainer);

            CreateHeaderControl(temperatureService);
            CreateToolbarControl(temperatureService); // ← ПЕРЕДАТЬ СЕРВИС
        }
        private void CreateToolbarControl(ITemperatureService temperatureService)
        {
            toolbarControl = new ToolbarControl(temperatureService) // ← ПЕРЕДАТЬ СЕРВИС
            {
                Height = CalculatePercentageValue(topContainer.Height, _toolbarToTopHeightPercentage),
                Dock = DockStyle.Bottom
            };

            // Подписываемся на события управления продуктами
            toolbarControl.AddProductClicked += (s, e) => AddProduct();
            toolbarControl.DeleteProductClicked += (s, e) => DeleteProduct();

            topContainer.Controls.Add(toolbarControl);
        }
        private void CreateNotificationsControl()
        {
            notificationsControl = new NotificationsControl
            {
                Dock = DockStyle.Fill
            };
        }
        private void CreateStatisticsControl()
        {
            statisticsControl = new StatisticsControl
            {
                Dock = DockStyle.Top,
                Height = CalculatePercentageValue(leftCentralContainer.Height, _statToLeftHeightPercentage)
            };
        }
        private void CreateHeaderControl(ITemperatureService temperatureService)
        {
            headerControl = new HeaderControl(_currentUser, temperatureService) // ← ПЕРЕДАТЬ СЕРВИС
            {
                Height = CalculatePercentageValue(topContainer.Height, _headerToTopHeightPercentage),
                Dock = DockStyle.Top
            };
            headerControl.LogoutClicked += (s, e) => Logout();
            topContainer.Controls.Add(headerControl);
        }

        private void Logout()
        {
            CompositionRoot.ClearUserCache(_currentUser.Id);
            Application.Restart();
        }

        // Обработчики событий (заглушки)
        private void LoadProducts()
        {
            var products = _productService.GetAllProducts();
            productsGridControl.LoadProducts(products);
            UpdateStatistics();
        }
        private void AddProduct()
        {
            using (var dialog = new AddProductForm(_productService))
            {
                var result = dialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    LoadProducts();
                }
            }
        }

        private void DeleteProduct()
        {
            var selectedProduct = productsGridControl.SelectedProduct;
            if (selectedProduct == null)
            {
                MessageBox.Show("Выберите продукт для удаления", "Информация",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                _productService.DeleteProduct(selectedProduct.Id);
                LoadProducts();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении продукта: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void IncreaseTemperature()
        {
            // TODO: Увеличить температуру  
            MessageBox.Show("Температура увеличена");
        }

        private void DecreaseTemperature()
        {
            // TODO: Уменьшить температуру
            MessageBox.Show("Температура уменьшена");
        }

        private void CreateCentralContainer()
        {
            centralContainer = new Panel().AsCentralContainer();
            this.Controls.Add(centralContainer);
        }

        private void CreateCentralContainers()
        {

            mainContentCentralContainer = new Panel { }.AsMainContentCentralContainer();
            centralContainer.Controls.Add(mainContentCentralContainer);

            leftCentralContainer = new Panel
            {
                Width = CalculatePercentageValue(centralContainer.Width, _leftCentralWidthPercentage)
            }.AsLeftCentralContainer();
            centralContainer.Controls.Add(leftCentralContainer);

            rightCentralContainer = new Panel
            {
                Width = CalculatePercentageValue(centralContainer.Width, _rightCentralWidthPercentage),
            }.AsRightCentralContainer();
            centralContainer.Controls.Add(rightCentralContainer);

            CreateProductsGrid();
            CreateLeftContent();
        }

        private void CreateProductsGrid()
        {
            productsGridControl = new ProductsGridControl
            {
                Dock = DockStyle.Fill
            };
            mainContentCentralContainer.Controls.Add(productsGridControl);

            // Подписываемся на событие выбора
            productsGridControl.SelectionChanged += (s, e) =>
                toolbarControl.DeleteButtonEnabled = productsGridControl.SelectedProduct != null;
        }

        private void CreateLeftContent()
        {
            // NotificationsControl - нижняя половина (50%) - ДОБАВЛЯЕМ ПЕРВЫМ
            CreateNotificationsControl();
            leftCentralContainer.Controls.Add(notificationsControl);

            // StatisticsControl - верхняя половина (50%) - ДОБАВЛЯЕМ ВТОРЫМ
            CreateStatisticsControl();
            leftCentralContainer.Controls.Add(statisticsControl);
        }

        private void UpdateStatistics()
        {
            if (_productService == null) return;

            try
            {
                var total = _productService.GetAllProducts().Count();
                var expired = _productService.GetExpiredProducts().Count();
                var soon = _productService.GetExpiringSoonProducts(3).Count();
                var fresh = total - expired - soon;

                statisticsControl?.UpdateStatistics(total, fresh, soon, expired);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка обновления статистики: {ex.Message}");
            }
        }

        private void CreateBottomContainer()
        {
            bottomContainer = new Panel().AsBottomContainer();
            bottomContainer.Height = CalculatePercentageValue(this.ClientSize.Height, _bottomToFormHeightPercentage);
            this.Controls.Add(bottomContainer);
        }

        private void ApplyStyles()
        {
            // Стиль формы
            this.AsMainForm();
            this.Text = $"Умный холодильник - {_currentUser.Username}";
        }

        /// <summary>
        /// Вычисляет относительную величину
        /// </summary>
        /// <param name="fromValue">Изначальная величина, относительно которой необходимо сделать вычисление</param>
        /// <param name="percentage">Отношение в процентах к величине</param>
        /// <returns></returns>
        private int CalculatePercentageValue(int fromValue, int percentage)
        {
            return (int)(fromValue * (percentage / 100.0));
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            // Обновляем высоты при изменении размера формы
            if (topContainer != null)
                topContainer.Height = CalculatePercentageValue(this.ClientSize.Height, _topToFormHeightPercentage);

            if (bottomContainer != null)
                bottomContainer.Height = CalculatePercentageValue(this.ClientSize.Height, _bottomToFormHeightPercentage);

            if (headerControl != null)
                headerControl.Height = CalculatePercentageValue(topContainer.Height, _headerToTopHeightPercentage);

            if (toolbarControl != null)
                toolbarControl.Height = CalculatePercentageValue(topContainer.Height, _toolbarToTopHeightPercentage);

            if (leftCentralContainer != null)
                leftCentralContainer.Width = CalculatePercentageValue(centralContainer.Width, _leftCentralWidthPercentage);

            if (rightCentralContainer != null)
                rightCentralContainer.Width = CalculatePercentageValue(centralContainer.Width, _rightCentralWidthPercentage);
            if (statisticsControl != null && leftCentralContainer != null)
                statisticsControl.Height = CalculatePercentageValue(leftCentralContainer.Height, _statToLeftHeightPercentage);
        }
    }
}
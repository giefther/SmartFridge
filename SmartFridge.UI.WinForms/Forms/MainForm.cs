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

        // Основные контейнеры
        private Panel topContainer;
        private Panel centralContainer;
        private Panel bottomContainer;

        // Содержимое CentralContainer
        private Panel leftCentralContainer;
        private Panel mainContentCentralContainer;
        private Panel rightCentralContainer;
        
        // Содержимое LeftContainer
        private Panel statContainer;
        private Panel notificationsContainer;
        private Label statTitle;
        private Label notificationsTitle;
        // Лейблы статистики продуктов
        private Label totalValueLabel;
        private Label freshValueLabel;
        private Label soonValueLabel;
        private Label expiredValueLabel;

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

            InitializeComponent();
            SetupContainers();
            ApplyStyles();
            LoadProducts();
        }

        private void SetupContainers()
        {
            CreateCentralContainer();     
            CreateCentralContainers();    
            CreateTopContainer();         
            CreateBottomContainer();      
        }

        private void CreateTopContainer()
        {
            topContainer = new Panel().AsTopContainer();
            topContainer.Height = CalculatePercentageValue(this.ClientSize.Height, _topToFormHeightPercentage);
            this.Controls.Add(topContainer);

            // Header и Toolbar внутри TopContainer
            CreateHeaderControl();
            CreateToolbarControl();  
        }
        private void CreateToolbarControl()
        {
            toolbarControl = new ToolbarControl()
            {
                Height = CalculatePercentageValue(topContainer.Height, _toolbarToTopHeightPercentage),
                Dock = DockStyle.Bottom
            };

            // Подписываемся на события
            toolbarControl.AddProductClicked += (s, e) => AddProduct();
            toolbarControl.DeleteProductClicked += (s, e) => DeleteProduct();
            toolbarControl.IncreaseTempClicked += (s, e) => IncreaseTemperature();
            toolbarControl.DecreaseTempClicked += (s, e) => DecreaseTemperature();

            topContainer.Controls.Add(toolbarControl);
        }
        private void CreateHeaderControl()
        {
            headerControl = new HeaderControl(_currentUser)
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
            // NotificationsContainer - нижняя половина (50%)
            notificationsContainer = new Panel
            {
                Dock = DockStyle.Fill, // Занимает оставшееся пространство
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(10)
            };
            leftCentralContainer.Controls.Add(notificationsContainer);

            // StatContainer - верхняя половина (50%)
            statContainer = new Panel
            {
                Dock = DockStyle.Top,
                Height = CalculatePercentageValue(leftCentralContainer.Height, _statToLeftHeightPercentage),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(10)
            };
            leftCentralContainer.Controls.Add(statContainer);

            // Заголовок для StatContainer
            statTitle = new Label
            {
                Text = "📊 Статистика",
                Dock = DockStyle.Top,
                Height = 30,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = CustomFormStyles.HeaderFont,
                ForeColor = CustomFormStyles.DarkColor
            };
            statContainer.Controls.Add(statTitle);

            // Заголовок для NotificationsContainer
            notificationsTitle = new Label
            {
                Text = "🔔 Уведомления",
                Dock = DockStyle.Top,
                Height = 30,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = CustomFormStyles.HeaderFont,
                ForeColor = CustomFormStyles.DarkColor
            };
            notificationsContainer.Controls.Add(notificationsTitle);

            // Пока что добавляем заглушки
            CreateStatContent();
            CreateNotificationsContent();
        }
        private void CreateStatItem(Panel parent, string title, string value, Color color, int topPosition, ref Label valueLabel)
        {
            var itemPanel = new Panel
            {
                Height = 25,
                Dock = DockStyle.Top,
                Margin = new Padding(0, 2, 0, 2)
            };
            parent.Controls.Add(itemPanel);

            // Цветной символ ●
            var colorLabel = new Label
            {
                Text = "●",
                Location = new Point(5, 4),
                Size = new Size(15, 15),
                Font = new Font("Segoe UI", 10),
                ForeColor = color,
                TextAlign = ContentAlignment.MiddleCenter
            };
            itemPanel.Controls.Add(colorLabel);

            // Название категории
            var titleLabel = new Label
            {
                Text = title,
                Location = new Point(25, 4),
                Size = new Size(100, 18),
                Font = CustomFormStyles.SmallFont,
                ForeColor = CustomFormStyles.DarkColor,
                TextAlign = ContentAlignment.MiddleLeft
            };
            itemPanel.Controls.Add(titleLabel);

            // Значение (сохраняем ссылку)
            valueLabel = new Label
            {
                Text = value,
                Location = new Point(120, 4),
                Size = new Size(40, 18),
                Font = CustomFormStyles.NormalFont,
                ForeColor = CustomFormStyles.DarkColor,
                TextAlign = ContentAlignment.MiddleRight
            };
            itemPanel.Controls.Add(valueLabel);
        }
        private void CreateStatContent()
        {
            // Очищаем контейнер
            statContainer.Controls.Clear();

            // Панель для статистики
            var statsPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(10, 5, 10, 5)
            };
            statContainer.Controls.Add(statsPanel);

            // Заголовок
            statTitle = new Label
            {
                Text = "📊 Статистика",
                Dock = DockStyle.Top,
                Height = 30,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = CustomFormStyles.HeaderFont,
                ForeColor = CustomFormStyles.DarkColor
            };
            statContainer.Controls.Add(statTitle);


            // Создаем элементы статистики и сохраняем ссылки на Label'ы
            CreateStatItem(statsPanel, "Всего", "0", Color.Gray, 0, ref totalValueLabel);
            CreateStatItem(statsPanel, "Свежих", "0", Color.Green, 25, ref freshValueLabel);
            CreateStatItem(statsPanel, "Скоро истекает", "0", Color.Orange, 50, ref soonValueLabel);
            CreateStatItem(statsPanel, "Просрочено", "0", Color.Red, 75, ref expiredValueLabel);

            // Обновляем статистику при загрузке
            UpdateStatistics();
        }

        private void UpdateStatistics()
        {
            if (_productService == null) return;

            try
            {
                // Используем методы сервиса для подсчета
                var total = _productService.GetAllProducts().Count();
                var expired = _productService.GetExpiredProducts().Count();
                var soon = _productService.GetExpiringSoonProducts(3).Count();
                var fresh = total - expired - soon;

                // ✅ Просто обновляем текст Label'ов через сохраненные ссылки
                if (totalValueLabel != null) totalValueLabel.Text = total.ToString();
                if (freshValueLabel != null) freshValueLabel.Text = fresh.ToString();
                if (soonValueLabel != null) soonValueLabel.Text = soon.ToString();
                if (expiredValueLabel != null) expiredValueLabel.Text = expired.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка обновления статистики: {ex.Message}");
            }
        }

        private void CreateNotificationsContent()
        {
            // Заглушка для уведомлений
            var notificationsPlaceholder = new Label
            {
                Text = "Здесь будут уведомления",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = CustomFormStyles.NormalFont,
                ForeColor = CustomFormStyles.SecondaryColor
            };
            notificationsContainer.Controls.Add(notificationsPlaceholder);
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
                topContainer.Height = CalculatePercentageValue(this.ClientSize.Height,_topToFormHeightPercentage);

            if (bottomContainer != null)
                bottomContainer.Height = CalculatePercentageValue(this.ClientSize.Height, _bottomToFormHeightPercentage);

            if(headerControl != null)
                headerControl.Height = CalculatePercentageValue(topContainer.Height, _headerToTopHeightPercentage);

            if (toolbarControl != null)
                toolbarControl.Height = CalculatePercentageValue(topContainer.Height, _toolbarToTopHeightPercentage);

            if (leftCentralContainer != null)
                leftCentralContainer.Width = CalculatePercentageValue(centralContainer.Width, _leftCentralWidthPercentage);

            if (rightCentralContainer != null)
                rightCentralContainer.Width = CalculatePercentageValue(centralContainer.Width, _rightCentralWidthPercentage);
            if (statContainer != null && leftCentralContainer != null)
            {
                statContainer.Height = CalculatePercentageValue(leftCentralContainer.Height, _statToLeftHeightPercentage);
            }
        }
    }
}
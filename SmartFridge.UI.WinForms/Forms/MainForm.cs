using Microsoft.VisualBasic.ApplicationServices;
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
        private readonly SmartFridge.Core.Models.User _currentUser;
        private readonly IProductService _productService;
        private System.Windows.Forms.Timer timeTimer;

        // Основные контейнеры
        private Panel topContainer;
        private Panel centralContainer;
        private Panel bottomContainer;

        // Содержимое TopContainer
        private Panel headerContainer;
        private Panel toolbarContainer;
        private Panel leftHeaderContainer;
        private Panel rightHeaderContainer;
        private Label lblTime;
        private Label lblTemperature;
        private Label lblUsername;
        private Button btnLogout;
        private Panel leftToolbarContainer;
        private Panel rightToolbarContainer;
        private Button btnDecreaseTemp;
        private Button btnIncreaseTemp;
        private Button btnAddProduct;
        private Button btnDeleteProduct;

        // Содержимое CentralContainer
        private Panel leftCentralContainer;
        private Panel mainContentCentralContainer;
        private Panel rightCentralContainer;
        private DataGridView productsDataGrid;
        private TextBox searchTextBox;
        private Label statusLabel;
        private List<Product> _allProducts; // Все продукты для поиска
        private List<Product> _filteredProducts; // Отфильтрованные продукты для поиска
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
            InitializeTimeTimer();
            SetupContainers();
            ApplyStyles();
        }

        private void InitializeTimeTimer()
        {
            timeTimer = new System.Windows.Forms.Timer();
            timeTimer.Interval = 1000; // 1 секунда
            timeTimer.Tick += TimeTimer_Tick;
            timeTimer.Start();
            UpdateTime(); // Первоначальное обновление
        }
        private void TimeTimer_Tick(object sender, EventArgs e)
        {
            UpdateTime();
        }

        private void UpdateTime()
        {
            if (lblTime != null && !lblTime.IsDisposed)
            {
                lblTime.Text = $"🕐 {DateTime.Now:HH:mm}";
            }
        }
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            timeTimer?.Stop();
            timeTimer?.Dispose();
            base.OnFormClosed(e);
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
            CreateHeaderContainer();
            CreateToolbarContainer();
            CreateHeaderContent();
            CreateToolbarContent();
        }

        private void CreateHeaderContainer()
        {
            headerContainer = new Panel().AsHeaderContainer();
            headerContainer.Height = CalculatePercentageValue(topContainer.Height, _headerToTopHeightPercentage); 
            topContainer.Controls.Add(headerContainer);
        }

        private void CreateToolbarContainer()
        {
            toolbarContainer = new Panel().AsToolbarContainer();
            toolbarContainer.Height = CalculatePercentageValue(topContainer.Height, _toolbarToTopHeightPercentage);
            toolbarContainer.Dock = DockStyle.Bottom;
            topContainer.Controls.Add(toolbarContainer);
        }

        private void CreateToolbarContent()
        {
            CreateLeftToolbarContainer();
            CreateRightToolbarContainer();
        }

        private void CreateLeftToolbarContainer()
        {
            leftToolbarContainer = new Panel
            {
                Dock = DockStyle.Left,
                Width = 350,
                Padding = new Padding(20, 8, 0, 8)
            };
            toolbarContainer.Controls.Add(leftToolbarContainer);

            // Кнопка уменьшения температуры
            btnDecreaseTemp = new Button
            {
                Text = "❄️ Уменьшить",
                Size = new Size(165, 45),
                Location = new Point(0, 0)
            }.AsLight();
            btnDecreaseTemp.Click += BtnDecreaseTemp_Click;
            leftToolbarContainer.Controls.Add(btnDecreaseTemp);

            // Кнопка увеличения температуры
            btnIncreaseTemp = new Button
            {
                Text = "☀️ Увеличить",
                Size = new Size(165, 45),
                Location = new Point(175, 0)
            }.AsLight();
            btnIncreaseTemp.Click += BtnIncreaseTemp_Click;
            leftToolbarContainer.Controls.Add(btnIncreaseTemp);
        }

        private void CreateRightToolbarContainer()
        {
            rightToolbarContainer = new Panel
            {
                Dock = DockStyle.Right,
                Width = 250,
                Padding = new Padding(0, 8, 20, 8)
            };
            toolbarContainer.Controls.Add(rightToolbarContainer);

            // Кнопка добавления продукта
            btnAddProduct = new Button
            {
                Text = "➕ Добавить",
                Size = new Size(110, 45),
                Location = new Point(0, 0)
            }.AsSuccess();
            btnAddProduct.Click += BtnAddProduct_Click;
            rightToolbarContainer.Controls.Add(btnAddProduct);

            // Кнопка удаления продукта
            btnDeleteProduct = new Button
            {
                Text = "➖ Удалить",
                Size = new Size(110, 45),
                Location = new Point(120, 0),
                Enabled = false // Изначально неактивна
            }.AsDanger();
            btnDeleteProduct.Click += BtnDeleteProduct_Click;
            rightToolbarContainer.Controls.Add(btnDeleteProduct);
        }

        // Обработчики событий (заглушки)
        private void BtnAddProduct_Click(object sender, EventArgs e)
        {
            using (var dialog = new AddProductForm(_productService))
            {
                var result = dialog.ShowDialog();
                if (result == DialogResult.OK) // Пользователь нажал "Добавить"
                {
                    var newProduct = dialog.CreatedProduct;
                    LoadProducts();
                }
            }
        }

        private void BtnDeleteProduct_Click(object sender, EventArgs e)
        {
            if (productsDataGrid.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите продукт для удаления", "Информация",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var selectedRow = productsDataGrid.SelectedRows[0];
            var product = selectedRow.DataBoundItem as Product;

            if (product == null)
                return;

            var productName = product.Name;
            try
            {
                _productService.DeleteProduct(product.Id);
                LoadProducts();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении продукта: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnDecreaseTemp_Click(object sender, EventArgs e)
        {
            // TODO: Уменьшить температуру
            MessageBox.Show("Температура уменьшена");
        }

        private void BtnIncreaseTemp_Click(object sender, EventArgs e)
        {
            // TODO: Увеличить температуру  
            MessageBox.Show("Температура увеличена");
        }

        private void CreateHeaderContent()
        {
            CreateLeftHeaderContainer();
            CreateRightHeaderContainer();
        }

        private void CreateLeftHeaderContainer()
        {
            leftHeaderContainer = new Panel
            {
                Dock = DockStyle.Left,
                Width = 200,
                Padding = new Padding(20, 5, 0, 5)
            };
            headerContainer.Controls.Add(leftHeaderContainer);

            // Время
            lblTime = new Label
            {
                Text = "🕐 14:30",
                AutoSize = true,
                Location = new Point(0, 0)
            }.AsHeader().WithWhiteText();
            leftHeaderContainer.Controls.Add(lblTime);

            // Температура
            lblTemperature = new Label
            {
                Text = "❄️ 0°C",
                AutoSize = true,
                Location = new Point(0, 25)
            }.AsNormal().WithWhiteText();
            leftHeaderContainer.Controls.Add(lblTemperature);
        }

        private void CreateRightHeaderContainer()
        {
            rightHeaderContainer = new Panel
            {
                Dock = DockStyle.Right,
                Width = 250,    
                Padding = new Padding(0, 10, 20, 10) 
            };
            headerContainer.Controls.Add(rightHeaderContainer);

            lblUsername = new Label
            {
                Text = $"👤{_currentUser.Username}",
                AutoSize = true,
                Location = new Point(35, 8), 
                Font = CustomFormStyles.HeaderFont 
            }.WithWhiteText();
            rightHeaderContainer.Controls.Add(lblUsername);

            btnLogout = new Button
            {
                Text = "Выйти", 
                Size = new Size(80, 45), 
                Location = new Point(160, 3)
            }.AsLight();
            btnLogout.Click += BtnLogout_Click;
            rightHeaderContainer.Controls.Add(btnLogout);
        }

        // Обработчик выхода
        private void BtnLogout_Click(object sender, EventArgs e)
        {
            CompositionRoot.ClearUserCache(_currentUser.Id);
            Application.Restart();
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

            CreateMainContent();
            CreateLeftContent();
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

        private void CreateMainContent()
        {
            // DataGridView
            productsDataGrid = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoGenerateColumns = false // Исключает столбцы id, category, addeddate
            }.AsTable();
            SetupDataGridColumns();
            mainContentCentralContainer.Controls.Add(productsDataGrid);
            // Панель для поиска и статуса
            var topPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 40,
                Padding = new Padding(0, 5, 0, 5)
            };
            mainContentCentralContainer.Controls.Add(topPanel);

            // Поле поиска
            searchTextBox = new TextBox
            {
                PlaceholderText = "🔍 Поиск продуктов...",
                Dock = DockStyle.Left,
                Width = 200
            }.AsTextField();
            searchTextBox.TextChanged += SearchTextBox_TextChanged;
            topPanel.Controls.Add(searchTextBox);

            // ✅ КНОПКА ОЧИСТКИ ПОИСКА
            var btnClearSearch = new Button
            {
                Text = "❌",
            }.AsClearSearch();

            btnClearSearch.Click += BtnClearSearch_Click;

            topPanel.Controls.Add(btnClearSearch);

            // Статус
            statusLabel = new Label
            {
                Dock = DockStyle.Right,
                TextAlign = ContentAlignment.MiddleRight,
                AutoSize = false,
                Width = 150
            }.AsNormal();
            topPanel.Controls.Add(statusLabel);

            LoadProducts();
            productsDataGrid.SelectionChanged += ProductsDataGrid_SelectionChanged;
        }
        private void BtnClearSearch_Click(object sender, EventArgs e)
        {
            searchTextBox.Clear();
        }
        private void ProductsDataGrid_SelectionChanged(object sender, EventArgs e)
        {
            btnDeleteProduct.Enabled = productsDataGrid.SelectedRows.Count > 0;
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
            productsDataGrid.RowPrePaint += ProductsDataGrid_RowPrePaint;
        }

        private void ProductsDataGrid_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= productsDataGrid.Rows.Count)
                return;

            var row = productsDataGrid.Rows[e.RowIndex];
            var product = row.DataBoundItem as Product;

            if (product == null)
                return;

            // Определяем цвет в зависимости от срока годности
            Color backColor = GetProductRowColor(product);
            row.DefaultCellStyle.BackColor = backColor;

            // Делаем текст контрастным
            row.DefaultCellStyle.ForeColor = IsLightColor(backColor) ? Color.Black : Color.White;
        }

        private Color GetProductRowColor(Product product)
        {
            var daysUntilExpiration = (product.ExpirationDate - DateTime.Today).TotalDays;

            if (daysUntilExpiration < 0)
            {
                return Color.FromArgb(255, 200, 200); // Светло-красный
            }
            else if (daysUntilExpiration <= 3)
            {
                return Color.FromArgb(255, 255, 200); // Светло-жёлтый
            }
            else
            {
                return Color.FromArgb(200, 255, 200); // Светло-зелёный
            }
        }

        private bool IsLightColor(Color color)
        {
            // Определяем, светлый ли цвет (для выбора цвета текста)
            var brightness = (color.R * 0.299 + color.G * 0.587 + color.B * 0.114) / 255;
            return brightness > 0.5;
        }

        private void LoadProducts()
        {
            _allProducts = _productService.GetAllProducts().ToList();
            _filteredProducts = new List<Product>(_allProducts);
            productsDataGrid.DataSource = _filteredProducts;
            UpdateStatusLabel(_filteredProducts.Count);
            UpdateStatistics();
        }

        private void UpdateStatusLabel(int count)
        {
            if (string.IsNullOrEmpty(searchTextBox.Text))
                statusLabel.Text = $"Всего продуктов: {count}";
            else
                statusLabel.Text = $"Найдено: {count}";
        }

        private void CreateBottomContainer()
        {
            bottomContainer = new Panel().AsBottomContainer();
            bottomContainer.Height = CalculatePercentageValue(this.ClientSize.Height, _bottomToFormHeightPercentage);
            this.Controls.Add(bottomContainer);
        }

        private void SearchTextBox_TextChanged(object sender, EventArgs e)
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

            // Обновляем таблицу
            productsDataGrid.DataSource = _filteredProducts;
            UpdateStatusLabel(_filteredProducts.Count);
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

            if (headerContainer != null)
                headerContainer.Height = CalculatePercentageValue(topContainer.Height, _headerToTopHeightPercentage);

            if (toolbarContainer != null)
                toolbarContainer.Height = CalculatePercentageValue(topContainer.Height, _toolbarToTopHeightPercentage);

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
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

        // Относительные величины
        private const int _topToFormHeightPercentage = 21;
        private const int _bottomToFormHeightPercentage = 11;
        private const int _headerToTopHeightPercentage = 51;
        private const int _toolbarToTopHeightPercentage = 51;

        public MainForm(User user)
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
            CreateTopContainer();
            CreateBottomContainer();
            CreateCentralContainer();
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
            }.AsSuccess(); // Зеленая для позитивного действия
            btnAddProduct.Click += BtnAddProduct_Click;
            rightToolbarContainer.Controls.Add(btnAddProduct);

            // Кнопка удаления продукта
            btnDeleteProduct = new Button
            {
                Text = "➖ Удалить",
                Size = new Size(110, 45),
                Location = new Point(120, 0)
            }.AsDanger(); // Красная для опасного действия
            btnDeleteProduct.Click += BtnDeleteProduct_Click;
            rightToolbarContainer.Controls.Add(btnDeleteProduct);
        }

        // Обработчики событий (заглушки)
        private void BtnAddProduct_Click(object sender, EventArgs e)
        {
            // TODO: Открыть форму добавления продукта
            MessageBox.Show("Форма добавления продукта будет здесь");
        }

        private void BtnDeleteProduct_Click(object sender, EventArgs e)
        {
            // TODO: Открыть форму добавления продукта
            MessageBox.Show("Убрали предмет");
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

            if (headerContainer != null)
                headerContainer.Height = CalculatePercentageValue(topContainer.Height, _headerToTopHeightPercentage);

            if (toolbarContainer != null)
                toolbarContainer.Height = CalculatePercentageValue(topContainer.Height, _toolbarToTopHeightPercentage);
        }
    }
}
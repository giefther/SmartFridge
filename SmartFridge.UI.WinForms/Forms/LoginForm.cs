using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SmartFridge.Core.Models;
using SmartFridge.UI.WinForms.Composition;
using SmartFridge.UI.WinForms.Styles;

namespace SmartFridge.UI.WinForms.Forms
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
            Text = "Умный Холодильник — Вход";

            // ПРИМЕНЯЕМ СТИЛИ ЧЕРЕЗ EXTENSION METHODS
            ApplyStyles();

            // Подписываемся на событие изменения размера формы
            this.SizeChanged += LoginForm_SizeChanged;

            // Центрируем контейнер при старте
            CenterContainer();
        }

        private void ApplyStyles()
        {
            // Стиль для формы
            this.BackColor = CustomFormStyles.LightColor;
            this.Font = CustomFormStyles.NormalFont;

            // Стили через extension methods (более читабельно)
            pnlContainer.AsContainer();
            lblTitle.AsTitle();
            lblUsername.AsNormal();
            lblPassword.AsNormal();
            btnLogin.AsDark();
            btnRegister.AsLight();
            txtUsername.AsTextField();
            txtPassword.AsTextField();

            // Дополнительные настройки для статуса
            lblStatus.Font = CustomFormStyles.SmallFont;
        }

        private void LoginForm_SizeChanged(object sender, EventArgs e)
        {
            CenterContainer();
        }

        private void CenterContainer()
        {
            // Центрируем контейнер по горизонтали и вертикали
            pnlContainer.Left = (this.ClientSize.Width - pnlContainer.Width) / 2;
            pnlContainer.Top = (this.ClientSize.Height - pnlContainer.Height) / 2;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            var username = txtUsername.Text.Trim();
            var password = txtPassword.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ShowError("Пожалуйста, введите имя пользователя и пароль");
                return;
            }

            var user = CompositionRoot.UserService.Authenticate(username, password);

            if (user != null)
            {
                ShowSuccess($"Добро пожаловать, {user.Username}!");
                var mainForm = new MainForm(user);
                mainForm.Show();
                this.Hide();
            }
            else
            {
                ShowError("Неверное имя пользователя или пароль");
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            var username = txtUsername.Text.Trim();
            var password = txtPassword.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ShowError("Пожалуйста, введите имя пользователя и пароль");
                return;
            }

            if (password.Length < 4)
            {
                ShowError("Пароль должен быть не менее 4 символов");
                return;
            }

            var newUser = new User
            {
                Username = username,
                Email = $"{username}@example.com"
            };

            var success = CompositionRoot.UserService.Register(newUser, password);

            if (success)
            {
                ShowSuccess("Регистрация успешна! Теперь вы можете войти.");
                txtPassword.Clear();
            }
            else
            {
                ShowError("Имя пользователя уже существует");
            }
        }

        private void ShowError(string message)
        {
            lblStatus.ForeColor = CustomFormStyles.DangerColor;
            lblStatus.Text = message;
        }

        private void ShowSuccess(string message)
        {
            lblStatus.ForeColor = CustomFormStyles.SuccessColor;
            lblStatus.Text = message;
        }
    }
}
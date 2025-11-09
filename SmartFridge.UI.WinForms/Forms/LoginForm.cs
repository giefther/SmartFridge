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

namespace SmartFridge.UI.WinForms.Forms
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
            Text = "Умный Холодильник — Вход";
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

            // ИСПОЛЬЗУЕМ COMPOSITION ROOT для получения сервиса
            var user = CompositionRoot.UserService.Authenticate(username, password);

            if (user != null)
            {
                ShowSuccess($"Добро пожаловать, {user.Username}!");
                // TODO: Open MainForm
                var mainForm = new MainForm();
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
                Email = $"{username}@example.com" // Временное решение
            };

            // ИСПОЛЬЗУЕМ COMPOSITION ROOT для получения сервиса
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
            lblStatus.ForeColor = Color.Red;
            lblStatus.Text = message;
        }

        private void ShowSuccess(string message)
        {
            lblStatus.ForeColor = Color.Green;
            lblStatus.Text = message;
        }
    }
}
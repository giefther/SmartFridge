using SmartFridge.UI.WinForms.Forms;

namespace SmartFridge.UI.WinForms
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            var loginForm = new LoginForm();
            Application.Run(loginForm);
        }
    }
}

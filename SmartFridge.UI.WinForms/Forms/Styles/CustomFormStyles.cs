using System.Drawing;

namespace SmartFridge.UI.WinForms.Styles
{
    /// <summary>
    /// Centralized style definitions for the application
    /// </summary>
    public static class CustomFormStyles
    {
        // ===== COLOR PALETTE =====

        public static Color PrimaryColor => Color.FromArgb(41, 128, 185);     // Синий
        public static Color SecondaryColor => Color.FromArgb(52, 152, 219);   // Светло-синий
        public static Color SuccessColor => Color.FromArgb(39, 174, 96);      // Зеленый
        public static Color DangerColor => Color.FromArgb(231, 76, 60);       // Красный
        public static Color WarningColor => Color.FromArgb(243, 156, 18);     // Оранжевый
        public static Color LightColor => Color.FromArgb(236, 240, 241);      // Светлый
        public static Color DarkColor => Color.FromArgb(44, 62, 80);          // Темный

        // ===== FONTS =====

        public static Font TitleFont => new Font("Segoe UI", 14, FontStyle.Bold);
        public static Font HeaderFont => new Font("Segoe UI", 12, FontStyle.Bold);
        public static Font NormalFont => new Font("Segoe UI", 9);
        public static Font SmallFont => new Font("Segoe UI", 8);

        // ===== DIMENSIONS =====

        public static Size DefaultButtonSize => new Size(120, 35);
        public static Size LargeButtonSize => new Size(150, 40);
        public static Size SmallButtonSize => new Size(80, 25);
    }
}
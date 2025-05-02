using Microsoft.VisualStudio.PlatformUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PowerSuggestion.Helpers
{
    public static class VsThemeHelper
    {
        private static readonly Uri _vsStylesUri = new("pack://application:,,,/Microsoft.VisualStudio.Platform.WindowManagement;component/Themes/ThemedDialogDefaultStyles.xaml", UriKind.Absolute);
        private static readonly Uri _vsScrollUri = new("pack://application:,,,/Microsoft.VisualStudio.Shell.UI.Internal;component/Styles/ScrollBarStyle.xaml", UriKind.Absolute);

        public static void ApplyThemeResources(FrameworkElement element)
        {
            if (element == null) return;

            try
            {
                var vsStyles = new ResourceDictionary { Source = _vsStylesUri };
                var scrollStyles = new ResourceDictionary { Source = _vsScrollUri };

                element.Resources.MergedDictionaries.Add(vsStyles);
                element.Resources.MergedDictionaries.Add(scrollStyles);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to load VS theme resources: " + ex.Message);
            }
        }
        public static System.Windows.Media.Color ToMediaColor(System.Drawing.Color color)
        {
            return System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
        }
        public static void ApplyThemedColors(FrameworkElement element)
        {
            if (element == null) return;

            if (element is Control control)
            {
                var background = ToMediaColor(VSColorTheme.GetThemedColor(EnvironmentColors.ToolWindowBackgroundColorKey));
                var foreground = ToMediaColor(VSColorTheme.GetThemedColor(EnvironmentColors.ToolWindowTextColorKey));

                control.Background = new SolidColorBrush(background);
                control.Foreground = new SolidColorBrush(foreground);
            }
        }

        public static void EnableAutoRefreshOnThemeChange(FrameworkElement element)
        {
            VSColorTheme.ThemeChanged += _ =>
            {
                Application.Current?.Dispatcher.Invoke(() =>
                {
                    element.Resources.MergedDictionaries.Clear();
                    ApplyThemeResources(element);
                    ApplyThemedColors(element);
                });
            };
        }

        public static void AttachThemeSupport(FrameworkElement element)
        {
            ApplyThemeResources(element);
            ApplyThemedColors(element);
            EnableAutoRefreshOnThemeChange(element);
        }
    }
}

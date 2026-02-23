using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace BehringerMonitor.Converters
{
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class NotNullToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value != null && (value is not string str || str != "") ? Visibility.Visible : Visibility.Collapsed;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => (Visibility)value == Visibility.Visible;
    }
}

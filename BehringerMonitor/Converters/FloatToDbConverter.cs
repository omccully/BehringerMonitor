using BehringerMonitor.Helpers;
using System.Globalization;
using System.Windows.Data;

namespace BehringerMonitor.Converters
{
    public class FloatToDbConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is float f)
            {
                return DisplayHelper.FloatToDb(f);
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

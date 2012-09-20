using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace BombaJob.Utilities.Converters
{
    public class OfferDateLongConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;
            DateTime dt = (DateTime)value;
            string date = "";
            date += dt.ToString("HH:mm");
            date += " ";
            date += AppResources.ResourceManager.GetString("weekday_" + ((int)dt.DayOfWeek + 1));
            date += ", ";
            date += dt.Day;
            date += " ";
            date += AppResources.ResourceManager.GetString("monthLong_" + dt.Month);
            return date;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

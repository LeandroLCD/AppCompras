using System;
using System.Globalization;
using Xamarin.Forms;

namespace AppCompras.Converter
{
    public class TextLengthToBolean : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value == null)
                return false;

            int x = value.ToString().Length;
            if (x == 0)
                return false;
            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

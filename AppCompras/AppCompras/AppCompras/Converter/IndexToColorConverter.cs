using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace AppCompras.Converter
{
    public class IndexToColorConverter : IValueConverter
    {
        /// <summary>
        /// Retorna un color para cada linea de un collection
        /// </summary>
        /// <param name="value">index</param>
        /// <param name="targetType"></param>
        /// <param name="parameter">ObservableCollection or List<T></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var sourse = parameter as IEnumerable<object>;
            var index = sourse.IndexOf(value);

            if (index % 2 == 0)
                return Color.White;
            return Color.FromHex("E7E6E6");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}

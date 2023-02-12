using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;
using Xamarin.Forms;

namespace AppCompras.Converter
{
    public class DisplayItemConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Type type = value.GetType();
            PropertyInfo[] properties = type.GetProperties();
            string propertyValue= string.Empty;
            foreach (PropertyInfo property in properties)
            {
                if(property.Name == parameter.ToString())
                {
                    propertyValue = (string)property.GetConstantValue();
                    break;
                }
                    
            }
            return propertyValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

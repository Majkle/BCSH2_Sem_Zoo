using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;

namespace BCSH2_Sem_Zoo.View.Markup
{
    /// <summary>
    /// Used for ComboBox Items do display the DecsriptionAttribute
    /// </summary>
    public class TypeToDescriptionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Type type)
            {
                DescriptionAttribute? descriptionAttribute = (DescriptionAttribute?)Attribute.GetCustomAttribute(type, typeof(DescriptionAttribute));

                return descriptionAttribute?.Description ?? type.Name;
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

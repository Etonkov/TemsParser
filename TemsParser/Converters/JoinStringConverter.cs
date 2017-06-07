using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace TemsParser.Converters
{
    public class JoinStringConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var separator = parameter as string ?? " ";
            return string.Join(separator, values);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            var separator = parameter as string ?? " ";

            var valueStr = value as string;

            if (valueStr != null)
	        {
		        return valueStr.Split(new[] { separator }, StringSplitOptions.None).Cast<object>().ToArray();
	        }
            else
	        {
                return null;
	        }
        }
    }
}

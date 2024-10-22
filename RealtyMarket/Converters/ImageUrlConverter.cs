using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealtyMarket.Converters
{
    public class ImageUrlConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value is string url && !string.IsNullOrEmpty(url))
            {
                return url;
            }

            if(parameter != null && parameter is string callBackUrl && !string.IsNullOrEmpty(callBackUrl))
            {
                return callBackUrl;
            }

            return "ad_logo.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

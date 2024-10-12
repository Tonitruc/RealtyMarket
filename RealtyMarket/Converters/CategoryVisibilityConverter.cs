using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealtyMarket.Converters
{
    public class CategoryVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string selectedCategory = value as string;
            string[] categories = parameter.ToString().Split(',');

            if(selectedCategory == "Не выбрано")
            {
                return false;
            }

            if (categories.Contains("Any"))
            {
                return true;
            }

            if (selectedCategory == "Квартира")
            {
                if (categories.Contains("ResidentialRealty")
                    || categories.Contains("Flat"))
                {
                    return true;
                }
            }
            else if (selectedCategory == "Дом")
            {
                if (categories.Contains("ResidentialRealty")
                    || categories.Contains("House"))
                {
                    return true;
                }
            }

            return categories.Contains(selectedCategory);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

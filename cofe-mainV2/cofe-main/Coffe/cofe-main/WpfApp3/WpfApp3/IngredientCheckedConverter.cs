using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Data;

namespace CoffeeMaker
{
    public class IngredientCheckedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var selected = value as ObservableCollection<string>;
            var ingredient = parameter as string;
            return selected != null && ingredient != null && selected.Contains(ingredient);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isChecked = (bool)value;
            var ingredient = parameter as string;
            var selected = targetType == typeof(ObservableCollection<string>) ? null : null;
            // В биндинге мы получаем саму коллекцию через DataContext, поэтому используем EventTrigger в XAML
            return Binding.DoNothing;
        }
    }
} 
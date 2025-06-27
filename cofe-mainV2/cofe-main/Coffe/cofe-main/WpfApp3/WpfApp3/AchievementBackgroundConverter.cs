using System;
using System.Windows.Data;
using System.Windows.Media;

namespace CoffeeMaker
{
    public class AchievementBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool isUnlocked)
            {
                return isUnlocked ?
                    new SolidColorBrush(Color.FromArgb(255, 222, 235, 222)) : // Зеленый для разблокированных
                    new SolidColorBrush(Color.FromArgb(255, 240, 240, 240));   // Серый для заблокированных
            }
            return new SolidColorBrush(Colors.White);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

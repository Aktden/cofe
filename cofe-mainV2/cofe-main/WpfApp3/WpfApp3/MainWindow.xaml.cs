using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace CoffeeMaker
{
    public partial class MainWindow : Window
    {
        private Dictionary<string, Ingredient> ingredients = new Dictionary<string, Ingredient>
        {
            {"Эспрессо", new Ingredient("Эспрессо", 3, 15.50m, 30)},
            {"Двойной эспрессо", new Ingredient("Двойной эспрессо", 5, 25.00m, 60)},
            {"Молоко", new Ingredient("Молоко", 1, 5.00m, 100)},
            {"Вода", new Ingredient("Вода", 0, 0m, 100)},
            {"Молочная пена", new Ingredient("Молочная пена", 2, 10.00m, 50)},
            {"Сливки", new Ingredient("Сливки", 2, 12.00m, 50)},
            {"Ванильный сироп", new Ingredient("Ванильный сироп", 1, 8.00m, 10)},
            {"Карамельный сироп", new Ingredient("Карамельный сироп", 1, 8.00m, 10)},
            {"Шоколадный сироп", new Ingredient("Шоколадный сироп", 1, 8.00m, 10)}
        };

        private List<Recipe> recipes = new List<Recipe>();
        private List<CoffeeHistoryItem> coffeeHistory = new List<CoffeeHistoryItem>();
        private UserProfile userProfile = new UserProfile();
        private List<Achievement> achievements = new List<Achievement>
        {
            new Achievement("Первый кофе", "Приготовьте ваш первый кофе", "☕"),
            new Achievement("Кофеман", "Приготовьте 5 разных кофе", "👍"),
            new Achievement("Экспериментатор", "Попробуйте 3 разных модификатора", "🧪"),
            new Achievement("Гурман", "Приготовьте кофе с оценкой 10/10", "🌟")
        };

        public MainWindow()
        {
            InitializeComponent();
            LoadDefaultRecipes();
            UpdateUserInfo();
        }

        private void LoadDefaultRecipes()
        {
            recipes.Add(new Recipe("Капучино", new List<string> { "Эспрессо", "Молоко", "Молочная пена" }));
            recipes.Add(new Recipe("Латте", new List<string> { "Эспрессо", "Молоко" }));
            recipes.Add(new Recipe("Американо", new List<string> { "Эспрессо", "Вода" }));
            recipes.Add(new Recipe("Раф", new List<string> { "Эспрессо", "Сливки", "Ванильный сироп" }));
            recipes.Add(new Recipe("Мокко", new List<string> { "Эспрессо", "Молоко", "Шоколадный сироп" }));
        }

        private void UpdateUserInfo()
        {
            txtUserLevel.Text = userProfile.Level.ToString();
            pbUserExp.Value = userProfile.Experience % 100;
            txtBalance.Text = $"{userProfile.Balance:0.00} ₽";
        }


        private async void MakeCoffee_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<Ingredient> selectedIngredients = GetSelectedIngredients();
                if (selectedIngredients.Count == 0)
                {
                    MessageBox.Show("Выберите хотя бы один ингредиент!", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                decimal cost = CalculateCost(selectedIngredients);
                if (userProfile.Balance < cost)
                {
                    MessageBox.Show("Недостаточно средств!", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                pbCoffeeProgress.Visibility = Visibility.Visible;
                await AnimateCoffeeMaking();
                pbCoffeeProgress.Visibility = Visibility.Collapsed;

                var (coffeeName, description) = IdentifyCoffee(selectedIngredients);
                int rating = CalculateRating(selectedIngredients);
                string modifier = ((ComboBoxItem)cmbModifiers.SelectedItem).Content.ToString();

                lblCoffeeName.Content = coffeeName;
                txtDescription.Text = $"{description}\nМодификатор: {modifier}";
                txtRating.Text = $"{rating}/10";
                txtCost.Text = $"Стоимость: {cost:0.00} ₽";

                userProfile.Balance -= cost;
                userProfile.Experience += rating;
                if (userProfile.Experience >= userProfile.Level * 100)
                {
                    userProfile.Level++;
                    MessageBox.Show($"Поздравляем! Вы достигли уровня {userProfile.Level}!", "Уровень",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }

                coffeeHistory.Add(new CoffeeHistoryItem
                {
                    Date = DateTime.Now,
                    Name = coffeeName,
                    Rating = rating,
                    Ingredients = selectedIngredients.Select(i => i.Name).ToList(),
                    Modifier = modifier,
                    Cost = cost
                });

                CheckAchievements();
                UpdateUserInfo();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private (string coffeeName, string description) IdentifyCoffee(List<Ingredient> selectedIngredients)
        {
            var ingredientNames = selectedIngredients.Select(i => i.Name).ToList();

            foreach (var recipe in recipes)
            {
                if (recipe.Ingredients.All(i => ingredientNames.Contains(i)))
                {
                    return (recipe.Name, recipe.Description);
                }
            }

            string name = "Кастомный кофе";
            string description = $"Уникальный напиток, приготовленный из: {string.Join(", ", ingredientNames)}";

            return (name, description);
        }

        private void RandomCoffee_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (recipes.Count == 0)
                {
                    MessageBox.Show("Нет доступных рецептов!", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                Random random = new Random();
                Recipe randomRecipe = recipes[random.Next(0, recipes.Count)];

                lblCoffeeName.Content = randomRecipe.Name;
                txtDescription.Text = randomRecipe.Description;

                // Сбрасываем все выборы
                chkEspresso.IsChecked = false;
                chkDoubleEspresso.IsChecked = false;
                chkMilk.IsChecked = false;
                chkFoam.IsChecked = false;
                chkCream.IsChecked = false;
                chkVanilla.IsChecked = false;
                chkCaramel.IsChecked = false;
                chkChocolate.IsChecked = false;
                sldWater.Value = 0;

                // Устанавливаем нужные ингредиенты
                foreach (var ingredient in randomRecipe.Ingredients)
                {
                    if (ingredient == "Эспрессо") chkEspresso.IsChecked = true;
                    if (ingredient == "Двойной эспрессо") chkDoubleEspresso.IsChecked = true;
                    if (ingredient == "Молоко") chkMilk.IsChecked = true;
                    if (ingredient == "Молочная пена") chkFoam.IsChecked = true;
                    if (ingredient == "Сливки") chkCream.IsChecked = true;
                    if (ingredient == "Ванильный сироп") chkVanilla.IsChecked = true;
                    if (ingredient == "Карамельный сироп") chkCaramel.IsChecked = true;
                    if (ingredient == "Шоколадный сироп") chkChocolate.IsChecked = true;
                    if (ingredient == "Вода") sldWater.Value = 100;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaveRecipe_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<Ingredient> selectedIngredients = GetSelectedIngredients();
                if (selectedIngredients.Count == 0)
                {
                    MessageBox.Show("Нет выбранных ингредиентов для сохранения!", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                string coffeeName = lblCoffeeName.Content.ToString();
                if (coffeeName == "Ваш кофе" || coffeeName == "Кастомный кофе")
                {
                    coffeeName = Interaction.InputBox(
                        "Введите название для вашего рецепта:",
                        "Сохранение рецепта",
                        "Мой кофе");

                    if (string.IsNullOrWhiteSpace(coffeeName)) return;
                }

                var newRecipe = new Recipe(
                    coffeeName,
                    selectedIngredients.Select(i => i.Name).ToList());

                recipes.Add(newRecipe);
                MessageBox.Show($"Рецепт '{coffeeName}' успешно сохранен!", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteRecipe_Click(object sender, RoutedEventArgs e)
        {
            if (lstRecipes.SelectedItem is Recipe selectedRecipe)
            {
                recipes.Remove(selectedRecipe);
                lstRecipes.Items.Refresh();
                MessageBox.Show("Рецепт удален!", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Выберите рецепт для удаления!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private async Task AnimateCoffeeMaking()
        {
            double targetHeight = 100;
            coffeeLevelRect.Height = 0;

            for (int i = 0; i <= 100; i++)
            {
                double height = (i / 100.0) * targetHeight;
                coffeeLevelRect.Height = height;

                await Task.Delay(30);
            }

            // Эффект волн
            DoubleAnimation waveAnimation = new DoubleAnimation
            {
                From = targetHeight - 5,
                To = targetHeight + 5,
                AutoReverse = true,
                RepeatBehavior = new RepeatBehavior(TimeSpan.FromSeconds(2)),
                Duration = TimeSpan.FromSeconds(0.5),
                EasingFunction = new SineEase()
            };
            coffeeLevelRect.BeginAnimation(Rectangle.HeightProperty, waveAnimation);

            await Task.Delay(2000);
            coffeeLevelRect.BeginAnimation(Rectangle.HeightProperty, null);
            coffeeLevelRect.Height = targetHeight;
        }

        // Эффект волн
        DoubleAnimation waveAnimation = new DoubleAnimation
        {
            From = 150 - targetHeight + 5,
            To = 150 - targetHeight - 5,
            AutoReverse = true,
            RepeatBehavior = new RepeatBehavior(TimeSpan.FromSeconds(2)),
            Duration = TimeSpan.FromSeconds(0.5),
            EasingFunction = new SineEase()
        };
        coffeeLevelRect.BeginAnimation(Canvas.TopProperty, waveAnimation);

            await Task.Delay(2000);
        coffeeLevelRect.BeginAnimation(Canvas.TopProperty);
        }

        private List<Ingredient> GetSelectedIngredients()
        {
            List<Ingredient> selected = new List<Ingredient>();

            if (chkEspresso.IsChecked == true)
                selected.Add(ingredients["Эспрессо"]);
            if (chkDoubleEspresso.IsChecked == true)
                selected.Add(ingredients["Двойной эспрессо"]);
            if (chkMilk.IsChecked == true)
                selected.Add(ingredients["Молоко"]);
            if (chkFoam.IsChecked == true)
                selected.Add(ingredients["Молочная пена"]);
            if (chkCream.IsChecked == true)
                selected.Add(ingredients["Сливки"]);
            if (chkVanilla.IsChecked == true)
                selected.Add(ingredients["Ванильный сироп"]);
            if (chkCaramel.IsChecked == true)
                selected.Add(ingredients["Карамельный сироп"]);
            if (chkChocolate.IsChecked == true)
                selected.Add(ingredients["Шоколадный сироп"]);
            if (sldWater.Value > 0)
                selected.Add(new Ingredient("Вода", 0, 0m, 100) { Amount = (int)sldWater.Value });

            return selected;
        }

        private decimal CalculateCost(List<Ingredient> ingredients)
        {
            return ingredients.Sum(i => i.Price * (i.Amount / (decimal)i.DefaultAmount));
        }

        private int CalculateRating(List<Ingredient> ingredients)
        {
            int total = ingredients.Sum(i => i.Points * (i.Amount / i.DefaultAmount));
            int count = ingredients.Count;

            if (count == 0) return 0;
            int rating = total * 10 / (count * 5);
            return Math.Min(10, rating);
        }

        private void CheckAchievements()
        {
            if (coffeeHistory.Count == 1 && !userProfile.UnlockedAchievements.Contains("Первый кофе"))
            {
                userProfile.UnlockedAchievements.Add("Первый кофе");
                ShowAchievementPopup("Первый кофе", "Приготовьте ваш первый кофе");
            }

            if (coffeeHistory.Select(c => c.Name).Distinct().Count() >= 5 &&
                !userProfile.UnlockedAchievements.Contains("Кофеман"))
            {
                userProfile.UnlockedAchievements.Add("Кофеман");
                ShowAchievementPopup("Кофеман", "Приготовьте 5 разных кофе");
            }

            if (coffeeHistory.Any(c => c.Rating == 10) &&
                !userProfile.UnlockedAchievements.Contains("Гурман"))
            {
                userProfile.UnlockedAchievements.Add("Гурман");
                ShowAchievementPopup("Гурман", "Приготовьте кофе с оценкой 10/10");
            }
        }

        private void ShowAchievementPopup(string title, string description)
        {
            MessageBox.Show($"Достижение разблокировано: {title}\n{description}", "Поздравляем!",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            createCoffeeGrid.Visibility = tabCreateCoffee.IsSelected ? Visibility.Visible : Visibility.Collapsed;
            recipesGrid.Visibility = tabRecipes.IsSelected ? Visibility.Visible : Visibility.Collapsed;
            historyGrid.Visibility = tabHistory.IsSelected ? Visibility.Visible : Visibility.Collapsed;
            achievementsGrid.Visibility = tabAchievements.IsSelected ? Visibility.Visible : Visibility.Collapsed;

            if (tabRecipes.IsSelected)
            {
                lstRecipes.ItemsSource = recipes;
                lstRecipes.Items.Refresh();
            }
            else if (tabHistory.IsSelected)
            {
                dgHistory.ItemsSource = coffeeHistory;
            }
            else if (tabAchievements.IsSelected)
            {
                icAchievements.ItemsSource = achievements.Select(a => new
                {
                    Title = a.Title,
                    Description = a.Description,
                    Icon = a.Icon,
                    IsUnlocked = userProfile.UnlockedAchievements.Contains(a.Title)
                });
            }
        
 

    public class Ingredient
    {
        public string Name { get; set; }
        public int Points { get; set; }
        public decimal Price { get; set; }
        public int DefaultAmount { get; set; }
        public int Amount { get; set; }

        public Ingredient(string name, int points, decimal price, int defaultAmount)
        {
            Name = name;
            Points = points;
            Price = price;
            DefaultAmount = defaultAmount;
            Amount = defaultAmount;
        }
    }

    public class Recipe
    {
        public string Name { get; set; }
        public List<string> Ingredients { get; set; }
        public string Description => $"Классический рецепт: {Name}. Состав: {string.Join(", ", Ingredients)}";

        public Recipe(string name, List<string> ingredients)
        {
            Name = name;
            Ingredients = ingredients;
        }
    }

    public class CoffeeHistoryItem
    {
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public int Rating { get; set; }
        public List<string> Ingredients { get; set; }
        public string Modifier { get; set; }
        public decimal Cost { get; set; }
    }

    public class UserProfile
    {
        public int Level { get; set; } = 1;
        public int Experience { get; set; }
        public decimal Balance { get; set; } = 100.00m;
        public List<string> UnlockedAchievements { get; set; } = new List<string>();
    }

    public class Achievement
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }

        public Achievement(string title, string description, string icon)
        {
            Title = title;
            Description = description;
            Icon = icon;
        }
    }

    public class AchievementBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (bool)value ?
                new SolidColorBrush(Color.FromArgb(255, 111, 78, 55)) :
                new SolidColorBrush(Color.FromArgb(255, 200, 200, 200));
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
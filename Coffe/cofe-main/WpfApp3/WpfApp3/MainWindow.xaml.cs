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
using System.Windows.Shapes;
using System.Windows.Media.Animation;

namespace CoffeeMaker
{
    public partial class MainWindow : Window
    {
        private Dictionary<string, Ingredient> ingredients = new Dictionary<string, Ingredient>
        {
            {"Эспрессо", new Ingredient("Эспрессо", 3, 15.50m, 30)},
            {"Двойной эспрессо", new Ingredient("Двойной эспрессо", 5, 25.00m, 60)},
            {"Американо", new Ingredient("Американо", 2, 10.00m, 120)},
            {"Молоко", new Ingredient("Молоко", 1, 5.00m, 100)},
            {"Вода", new Ingredient("Вода", 0, 0m, 100)},
            {"Молочная пена", new Ingredient("Молочная пена", 2, 10.00m, 50)},
            {"Сливки", new Ingredient("Сливки", 2, 12.00m, 50)},
            {"Сгущенное молоко", new Ingredient("Сгущенное молоко", 3, 15.00m, 30)},
            {"Кокосовое молоко", new Ingredient("Кокосовое молоко", 3, 18.00m, 50)},
            {"Ванильный сироп", new Ingredient("Ванильный сироп", 1, 8.00m, 10)},
            {"Карамельный сироп", new Ingredient("Карамельный сироп", 1, 8.00m, 10)},
            {"Шоколадный сироп", new Ingredient("Шоколадный сироп", 1, 8.00m, 10)},
            {"Фундучный сироп", new Ingredient("Фундучный сироп", 1, 9.00m, 10)},
            {"Кленовый сироп", new Ingredient("Кленовый сироп", 1, 9.00m, 10)},
            {"Мятный сироп", new Ingredient("Мятный сироп", 1, 9.00m, 10)},
            {"Корица", new Ingredient("Корица", 1, 5.00m, 2)},
            {"Мускатный орех", new Ingredient("Мускатный орех", 1, 6.00m, 1)},
            {"Имбирь", new Ingredient("Имбирь", 1, 7.00m, 3)},
            {"Взбитые сливки", new Ingredient("Взбитые сливки", 2, 12.00m, 30)},
            {"Шоколадная крошка", new Ingredient("Шоколадная крошка", 1, 8.00m, 5)}
        };

        private List<Recipe> recipes = new List<Recipe>();
        private List<CoffeeHistoryItem> coffeeHistory = new List<CoffeeHistoryItem>();
        private UserProfile userProfile = new UserProfile();
        private List<Achievement> achievements = new List<Achievement>
        {
            new Achievement("Первый кофе", "Приготовьте ваш первый кофе", "☕", (user) => user.CoffeesMade >= 1),
            new Achievement("Кофеман", "Приготовьте 10 разных кофе", "👍", (user) => user.UniqueCoffeesMade >= 10),
            new Achievement("Экспериментатор", "Попробуйте 5 разных модификаторов", "🧪", (user) => user.ModifiersUsed >= 5),
            new Achievement("Гурман", "Приготовьте кофе с оценкой 10/10", "🌟", (user) => user.MaxRating >= 10),
            new Achievement("Коллекционер", "Сохраните 5 рецептов", "📋", (user) => user.RecipesSaved >= 5),
            new Achievement("Кофейный магнат", "Потратьте 500 ₽ на кофе", "💰", (user) => user.TotalSpent >= 500),
            new Achievement("Ночной совенок", "Приготовьте кофе после полуночи", "🦉", (user) => user.NightCoffees >= 1),
            new Achievement("Сладкоежка", "Используйте сиропы 10 раз", "🍯", (user) => user.SyrupsUsed >= 10),
            new Achievement("Молочный барон", "Используйте молочные продукты 15 раз", "🥛", (user) => user.MilkProductsUsed >= 15),
            new Achievement("Мастер специй", "Используйте все виды специй", "🌶️", (user) => user.SpicesUsed >= 3),
            new Achievement("Кофейный художник", "Создайте 3 кастомных рецепта", "🎨", (user) => user.CustomRecipes >= 3),
            new Achievement("Дегустатор", "Попробуйте все виды кофе", "👅", (user) => user.UniqueCoffeesMade >= 15)
        };

        public MainWindow()
        {
            InitializeComponent();
            LoadDefaultRecipes();
            UpdateUserInfo();
            LoadCoffeeImage();
        }

        private void LoadDefaultRecipes()
        {
            recipes.Add(new Recipe("Капучино", new List<string> { "Эспрессо", "Молоко", "Молочная пена" }));
            recipes.Add(new Recipe("Латте", new List<string> { "Эспрессо", "Молоко" }));
            recipes.Add(new Recipe("Американо", new List<string> { "Эспрессо", "Вода" }));
            recipes.Add(new Recipe("Раф", new List<string> { "Эспрессо", "Сливки", "Ванильный сироп" }));
            recipes.Add(new Recipe("Мокко", new List<string> { "Эспрессо", "Молоко", "Шоколадный сироп" }));
            recipes.Add(new Recipe("Флэт Уайт", new List<string> { "Двойной эспрессо", "Молоко" }));
            recipes.Add(new Recipe("Макиато", new List<string> { "Эспрессо", "Молочная пена" }));
            recipes.Add(new Recipe("Глясе", new List<string> { "Эспрессо", "Мороженое" }));
            recipes.Add(new Recipe("Кокосовый латте", new List<string> { "Эспрессо", "Кокосовое молоко" }));
            recipes.Add(new Recipe("Мятный мокко", new List<string> { "Эспрессо", "Молоко", "Шоколадный сироп", "Мятный сироп" }));
        }

        private void UpdateUserInfo()
        {
            txtUserLevel.Text = userProfile.Level.ToString();
            pbUserExp.Value = userProfile.Experience % 100;
            txtBalance.Text = $"{userProfile.Balance:0.00} ₽";
            UpdateAchievements();
        }

        private void UpdateAchievements()
        {
            icAchievements.ItemsSource = achievements.Select(a => new
            {
                Title = a.Title,
                Description = a.Description,
                Icon = a.Icon,
                IsUnlocked = a.IsUnlocked,
                Progress = a.GetProgress(userProfile)
            });
        }

        private void LoadCoffeeImage()
        {
            try
            {
                var uri = new Uri("pack://application:,,,/Images/coffee-cup.png");
                imgCoffeeCup.Source = new BitmapImage(uri);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки изображения: {ex.Message}");
            }
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

                btnMakeCoffee.IsEnabled = false;
                pbCoffeeProgress.Visibility = Visibility.Visible;
                await AnimateCoffeeMaking(selectedIngredients);
                pbCoffeeProgress.Visibility = Visibility.Collapsed;
                btnMakeCoffee.IsEnabled = true;

                var (coffeeName, description) = IdentifyCoffee(selectedIngredients);
                int rating = CalculateRating(selectedIngredients);
                string modifier = ((ComboBoxItem)cmbModifiers.SelectedItem).Content.ToString();

                lblCoffeeName.Content = coffeeName;
                txtDescription.Text = $"{description}\nМодификатор: {modifier}";
                txtRating.Text = $"{rating}/10";
                txtCost.Text = $"Стоимость: {cost:0.00} ₽";

                userProfile.Balance -= cost;
                userProfile.Experience += rating;
                userProfile.CoffeesMade++;
                userProfile.TotalSpent += cost;

                if (DateTime.Now.Hour >= 0 && DateTime.Now.Hour < 6)
                    userProfile.NightCoffees++;

                if (rating == 10 && rating > userProfile.MaxRating)
                    userProfile.MaxRating = rating;

                if (!userProfile.CoffeeTypes.Contains(coffeeName))
                {
                    userProfile.CoffeeTypes.Add(coffeeName);
                    userProfile.UniqueCoffeesMade = userProfile.CoffeeTypes.Count;
                }

                if (!userProfile.UsedModifiers.Contains(modifier))
                {
                    userProfile.UsedModifiers.Add(modifier);
                    userProfile.ModifiersUsed = userProfile.UsedModifiers.Count;
                }

                // Подсчет использованных сиропов и молочных продуктов
                int syrups = selectedIngredients.Count(i => i.Name.Contains("сироп"));
                int milkProducts = selectedIngredients.Count(i => i.Name.Contains("Молоко") ||
                                                              i.Name.Contains("Сливки") ||
                                                              i.Name.Contains("пена"));
                int spices = selectedIngredients.Count(i => i.Name.Contains("Корица") ||
                                                         i.Name.Contains("орех") ||
                                                         i.Name.Contains("Имбирь"));

                userProfile.SyrupsUsed += syrups;
                userProfile.MilkProductsUsed += milkProducts;
                userProfile.SpicesUsed = Math.Max(userProfile.SpicesUsed, spices);

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
                btnMakeCoffee.IsEnabled = true;
                pbCoffeeProgress.Visibility = Visibility.Collapsed;
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
            userProfile.CustomRecipes = Math.Max(userProfile.CustomRecipes,
                coffeeHistory.Count(ch => ch.Name == "Кастомный кофе") + 1);

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
                chkAmericano.IsChecked = false;
                chkMilk.IsChecked = false;
                chkFoam.IsChecked = false;
                chkCream.IsChecked = false;
                chkCondensedMilk.IsChecked = false;
                chkCoconutMilk.IsChecked = false;
                chkVanilla.IsChecked = false;
                chkCaramel.IsChecked = false;
                chkChocolate.IsChecked = false;
                chkHazelnut.IsChecked = false;
                chkMaple.IsChecked = false;
                chkMint.IsChecked = false;
                chkCinnamon.IsChecked = false;
                chkNutmeg.IsChecked = false;
                chkGinger.IsChecked = false;
                chkWhippedCream.IsChecked = false;
                chkChocolateChips.IsChecked = false;
                sldWater.Value = 0;

                // Устанавливаем нужные ингредиенты
                foreach (var ingredient in randomRecipe.Ingredients)
                {
                    if (ingredient == "Эспрессо") chkEspresso.IsChecked = true;
                    if (ingredient == "Двойной эспрессо") chkDoubleEspresso.IsChecked = true;
                    if (ingredient == "Американо") chkAmericano.IsChecked = true;
                    if (ingredient == "Молоко") chkMilk.IsChecked = true;
                    if (ingredient == "Молочная пена") chkFoam.IsChecked = true;
                    if (ingredient == "Сливки") chkCream.IsChecked = true;
                    if (ingredient == "Сгущенное молоко") chkCondensedMilk.IsChecked = true;
                    if (ingredient == "Кокосовое молоко") chkCoconutMilk.IsChecked = true;
                    if (ingredient == "Ванильный сироп") chkVanilla.IsChecked = true;
                    if (ingredient == "Карамельный сироп") chkCaramel.IsChecked = true;
                    if (ingredient == "Шоколадный сироп") chkChocolate.IsChecked = true;
                    if (ingredient == "Фундучный сироп") chkHazelnut.IsChecked = true;
                    if (ingredient == "Кленовый сироп") chkMaple.IsChecked = true;
                    if (ingredient == "Мятный сироп") chkMint.IsChecked = true;
                    if (ingredient == "Корица") chkCinnamon.IsChecked = true;
                    if (ingredient == "Мускатный орех") chkNutmeg.IsChecked = true;
                    if (ingredient == "Имбирь") chkGinger.IsChecked = true;
                    if (ingredient == "Взбитые сливки") chkWhippedCream.IsChecked = true;
                    if (ingredient == "Шоколадная крошка") chkChocolateChips.IsChecked = true;
                    if (ingredient == "Вода") sldWater.Value = 100;
                }

                LoadCoffeeImage();
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
                userProfile.RecipesSaved = recipes.Count;
                MessageBox.Show($"Рецепт '{coffeeName}' успешно сохранен!", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);

                CheckAchievements();
                UpdateUserInfo();
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
                userProfile.RecipesSaved = recipes.Count;
                lstRecipes.Items.Refresh();
                MessageBox.Show("Рецепт удален!", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);

                CheckAchievements();
                UpdateUserInfo();
            }
            else
            {
                MessageBox.Show("Выберите рецепт для удаления!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private async Task AnimateCoffeeMaking(List<Ingredient> ingredients)
        {
            // Анимация заполнения чашки
            double totalAmount = ingredients.Sum(i => i.Amount);
            double maxHeight = 200; // Максимальная высота заполнения
            double targetHeight = Math.Min(maxHeight, totalAmount * 0.8); // Коэффициент для визуализации

            var fillAnimation = new DoubleAnimation
            {
                To = targetHeight,
                Duration = TimeSpan.FromSeconds(3),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };

            rectCoffeeFill.BeginAnimation(Rectangle.HeightProperty, fillAnimation);

            // Анимация прогресс-бара
            for (int i = 0; i <= 100; i++)
            {
                pbCoffeeProgress.Value = i;
                await Task.Delay(30);
            }
        }

        private List<Ingredient> GetSelectedIngredients()
        {
            List<Ingredient> selected = new List<Ingredient>();

            if (chkEspresso.IsChecked == true)
                selected.Add(ingredients["Эспрессо"]);
            if (chkDoubleEspresso.IsChecked == true)
                selected.Add(ingredients["Двойной эспрессо"]);
            if (chkAmericano.IsChecked == true)
                selected.Add(ingredients["Американо"]);
            if (chkMilk.IsChecked == true)
                selected.Add(ingredients["Молоко"]);
            if (chkFoam.IsChecked == true)
                selected.Add(ingredients["Молочная пена"]);
            if (chkCream.IsChecked == true)
                selected.Add(ingredients["Сливки"]);
            if (chkCondensedMilk.IsChecked == true)
                selected.Add(ingredients["Сгущенное молоко"]);
            if (chkCoconutMilk.IsChecked == true)
                selected.Add(ingredients["Кокосовое молоко"]);
            if (chkVanilla.IsChecked == true)
                selected.Add(ingredients["Ванильный сироп"]);
            if (chkCaramel.IsChecked == true)
                selected.Add(ingredients["Карамельный сироп"]);
            if (chkChocolate.IsChecked == true)
                selected.Add(ingredients["Шоколадный сироп"]);
            if (chkHazelnut.IsChecked == true)
                selected.Add(ingredients["Фундучный сироп"]);
            if (chkMaple.IsChecked == true)
                selected.Add(ingredients["Кленовый сироп"]);
            if (chkMint.IsChecked == true)
                selected.Add(ingredients["Мятный сироп"]);
            if (chkCinnamon.IsChecked == true)
                selected.Add(ingredients["Корица"]);
            if (chkNutmeg.IsChecked == true)
                selected.Add(ingredients["Мускатный орех"]);
            if (chkGinger.IsChecked == true)
                selected.Add(ingredients["Имбирь"]);
            if (chkWhippedCream.IsChecked == true)
                selected.Add(ingredients["Взбитые сливки"]);
            if (chkChocolateChips.IsChecked == true)
                selected.Add(ingredients["Шоколадная крошка"]);

            // Добавляем воду, если выбрана
            if (sldWater.Value > 0)
            {
                selected.Add(new Ingredient("Вода", 0, 0m, (int)sldWater.Value));
            }

            return selected;
        }

        private decimal CalculateCost(List<Ingredient> ingredients)
        {
            return ingredients.Sum(i => i.Cost);
        }

        private int CalculateRating(List<Ingredient> ingredients)
        {
            // Базовый рейтинг зависит от сочетания ингредиентов
            int baseRating = 5;

            // Добавляем баллы за каждый ингредиент
            baseRating += ingredients.Sum(i => i.RatingPoints);

            // Ограничиваем максимальный рейтинг 10
            return Math.Min(10, baseRating);
        }

        private void CheckAchievements()
        {
            foreach (var achievement in achievements)
            {
                if (!achievement.IsUnlocked && achievement.CheckUnlocked(userProfile))
                {
                    achievement.Unlock();
                    MessageBox.Show($"Достижение разблокировано: {achievement.Title}!\n{achievement.Description}",
                        "Поздравляем!",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
            }
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tabCreateCoffee.IsSelected)
            {
                createCoffeeGrid.Visibility = Visibility.Visible;
                recipesGrid.Visibility = Visibility.Collapsed;
                historyGrid.Visibility = Visibility.Collapsed;
                achievementsGrid.Visibility = Visibility.Collapsed;
            }
            else if (tabRecipes.IsSelected)
            {
                createCoffeeGrid.Visibility = Visibility.Collapsed;
                recipesGrid.Visibility = Visibility.Visible;
                historyGrid.Visibility = Visibility.Collapsed;
                achievementsGrid.Visibility = Visibility.Collapsed;
                lstRecipes.ItemsSource = recipes;
            }
            else if (tabHistory.IsSelected)
            {
                createCoffeeGrid.Visibility = Visibility.Collapsed;
                recipesGrid.Visibility = Visibility.Collapsed;
                historyGrid.Visibility = Visibility.Visible;
                achievementsGrid.Visibility = Visibility.Collapsed;
                dgHistory.ItemsSource = coffeeHistory;
            }
            else if (tabAchievements.IsSelected)
            {
                createCoffeeGrid.Visibility = Visibility.Collapsed;
                recipesGrid.Visibility = Visibility.Collapsed;
                historyGrid.Visibility = Visibility.Collapsed;
                achievementsGrid.Visibility = Visibility.Visible;
                UpdateAchievements();
            }
        }

        private void btnAddMoney_Click(object sender, RoutedEventArgs e)
        {
            string input = Interaction.InputBox("Введите сумму для пополнения:", "Пополнение баланса", "100");
            if (decimal.TryParse(input, out decimal amount) && amount > 0)
            {
                userProfile.Balance += amount;
                UpdateUserInfo();
                MessageBox.Show($"Баланс успешно пополнен на {amount:0.00} ₽", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Введите корректную сумму!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public class Ingredient
        {
            public string Name { get; set; }
            public int RatingPoints { get; set; }
            public decimal Cost { get; set; }
            public int Amount { get; set; } // в мл или граммах

            public Ingredient(string name, int ratingPoints, decimal cost, int amount)
            {
                Name = name;
                RatingPoints = ratingPoints;
                Cost = cost;
                Amount = amount;
            }
        }

        public class Recipe
        {
            public string Name { get; set; }
            public List<string> Ingredients { get; set; }
            public string Description => $"Состав: {string.Join(", ", Ingredients)}";

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
            public decimal Balance { get; set; } = 100m;
            public int CoffeesMade { get; set; }
            public int UniqueCoffeesMade { get; set; }
            public int ModifiersUsed { get; set; }
            public int MaxRating { get; set; }
            public int RecipesSaved { get; set; }
            public decimal TotalSpent { get; set; }
            public int NightCoffees { get; set; }
            public int SyrupsUsed { get; set; }
            public int MilkProductsUsed { get; set; }
            public int SpicesUsed { get; set; }
            public int CustomRecipes { get; set; }
            public List<string> CoffeeTypes { get; set; } = new List<string>();
            public List<string> UsedModifiers { get; set; } = new List<string>();
        }

        public class Achievement
        {
            public string Title { get; set; }
            public string Description { get; set; }
            public string Icon { get; set; }
            public Func<UserProfile, bool> CheckCondition { get; set; }
            public bool IsUnlocked { get; private set; }

            public Achievement(string title, string description, string icon, Func<UserProfile, bool> checkCondition)
            {
                Title = title;
                Description = description;
                Icon = icon;
                CheckCondition = checkCondition;
            }

            public bool CheckUnlocked(UserProfile profile) => IsUnlocked || CheckCondition(profile);

            public void Unlock() => IsUnlocked = true;

            public string GetProgress(UserProfile profile)
            {
                if (IsUnlocked) return "Разблокировано!";

                if (Title == "Первый кофе") return $"Прогресс: {Math.Min(1, profile.CoffeesMade)}/1";
                if (Title == "Кофеман") return $"Прогресс: {Math.Min(10, profile.UniqueCoffeesMade)}/10";
                if (Title == "Экспериментатор") return $"Прогресс: {Math.Min(5, profile.ModifiersUsed)}/5";
                if (Title == "Гурман") return $"Лучшая оценка: {profile.MaxRating}/10";
                if (Title == "Коллекционер") return $"Прогресс: {Math.Min(5, profile.RecipesSaved)}/5";
                if (Title == "Кофейный магнат") return $"Потрачено: {Math.Min(500, profile.TotalSpent)}/500 ₽";
                if (Title == "Ночной совенок") return profile.NightCoffees > 0 ? "Готово!" : "Еще не готовили ночью";
                if (Title == "Сладкоежка") return $"Прогресс: {Math.Min(10, profile.SyrupsUsed)}/10";
                if (Title == "Молочный барон") return $"Прогресс: {Math.Min(15, profile.MilkProductsUsed)}/15";
                if (Title == "Мастер специй") return $"Прогресс: {Math.Min(3, profile.SpicesUsed)}/3";
                if (Title == "Кофейный художник") return $"Прогресс: {Math.Min(3, profile.CustomRecipes)}/3";
                if (Title == "Дегустатор") return $"Прогресс: {Math.Min(15, profile.UniqueCoffeesMade)}/15";

                return "В процессе...";
            }
        }

        public class AchievementBackgroundConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                if (value is bool isUnlocked)
                {
                    return isUnlocked ? new SolidColorBrush(Color.FromArgb(255, 222, 235, 222)) : new SolidColorBrush(Color.FromArgb(255, 240, 240, 240));
                }
                return new SolidColorBrush(Colors.White);
            }

            public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }
    }
}

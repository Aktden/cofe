using System.Collections.ObjectModel;
using CoffeeMaker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.ComponentModel;
using System.IO;
using System.Text.Json;

namespace CoffeeMaker.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Recipe> Recipes { get; set; }
        public ObservableCollection<Ingredient> Ingredients { get; set; }
        public ObservableCollection<Achievement> Achievements { get; set; }
        public ObservableCollection<CoffeeHistoryItem> CoffeeHistory { get; set; }
        public UserProfile UserProfile { get; set; }
        public ICommand MakeCoffeeCommand { get; }
        public ICommand SaveRecipeCommand { get; }
        public ICommand RandomCoffeeCommand { get; }
        public ICommand DeleteRecipeCommand { get; }
        public ICommand AddMoneyCommand { get; }
        public ICommand SaveHistoryToJsonCommand { get; }
        public ICommand LoadHistoryFromJsonCommand { get; }
        public ICommand SaveRecipesToJsonCommand { get; }
        public ICommand LoadRecipesFromJsonCommand { get; }
        public ObservableCollection<string> SelectedIngredients { get; set; } = new ObservableCollection<string>();
        private string _selectedModifier;
        public string SelectedModifier
        {
            get => _selectedModifier;
            set { _selectedModifier = value; OnPropertyChanged(nameof(SelectedModifier)); }
        }
        public ObservableCollection<string> Modifiers { get; set; } = new ObservableCollection<string>
        {
            "Обычный",
            "Очень горячий",
            "Двойная порция",
            "С пенкой",
            "Ледяной",
            "С корицей"
        };
        private string _coffeeName = "Ваш кофе";
        public string CoffeeName { get => _coffeeName; set { _coffeeName = value; OnPropertyChanged(nameof(CoffeeName)); } }
        private string _coffeeDescription = "Выберите ингредиенты для приготовления кофе";
        public string CoffeeDescription { get => _coffeeDescription; set { _coffeeDescription = value; OnPropertyChanged(nameof(CoffeeDescription)); } }
        private string _coffeeRating = "0/10";
        public string CoffeeRating { get => _coffeeRating; set { _coffeeRating = value; OnPropertyChanged(nameof(CoffeeRating)); } }
        private string _coffeeCost = "";
        public string CoffeeCost { get => _coffeeCost; set { _coffeeCost = value; OnPropertyChanged(nameof(CoffeeCost)); } }
        private Recipe _selectedRecipe;
        public Recipe SelectedRecipe
        {
            get => _selectedRecipe;
            set { _selectedRecipe = value; OnPropertyChanged(nameof(SelectedRecipe)); }
        }
        private bool _isResultVisible = false;
        public bool IsResultVisible
        {
            get => _isResultVisible;
            set { _isResultVisible = value; OnPropertyChanged(nameof(IsResultVisible)); }
        }

        public MainViewModel()
        {
            Ingredients = new ObservableCollection<Ingredient>(new List<Ingredient>
            {
                new Ingredient("Эспрессо", 3, 15.50m, 30),
                new Ingredient("Двойной эспрессо", 5, 25.00m, 60),
                new Ingredient("Американо", 2, 10.00m, 120),
                new Ingredient("Молоко", 1, 5.00m, 100),
                new Ingredient("Вода", 0, 0m, 100),
                new Ingredient("Молочная пена", 2, 10.00m, 50),
                new Ingredient("Сливки", 2, 12.00m, 50),
                new Ingredient("Сгущенное молоко", 3, 15.00m, 30),
                new Ingredient("Кокосовое молоко", 3, 18.00m, 50),
                new Ingredient("Ванильный сироп", 1, 8.00m, 10),
                new Ingredient("Карамельный сироп", 1, 8.00m, 10),
                new Ingredient("Шоколадный сироп", 1, 8.00m, 10),
                new Ingredient("Фундучный сироп", 1, 9.00m, 10),
                new Ingredient("Кленовый сироп", 1, 9.00m, 10),
                new Ingredient("Мятный сироп", 1, 9.00m, 10),
                new Ingredient("Корица", 1, 5.00m, 2),
                new Ingredient("Мускатный орех", 1, 6.00m, 1),
                new Ingredient("Имбирь", 1, 7.00m, 3),
                new Ingredient("Взбитые сливки", 2, 12.00m, 30),
                new Ingredient("Шоколадная крошка", 1, 8.00m, 5)
            });

            Recipes = new ObservableCollection<Recipe>(new List<Recipe>
            {
                new Recipe("Капучино", new List<string> { "Эспрессо", "Молоко", "Молочная пена" }),
                new Recipe("Латте", new List<string> { "Эспрессо", "Молоко" }),
                new Recipe("Американо", new List<string> { "Эспрессо", "Вода" }),
                new Recipe("Раф", new List<string> { "Эспрессо", "Сливки", "Ванильный сироп" }),
                new Recipe("Мокко", new List<string> { "Эспрессо", "Молоко", "Шоколадный сироп" }),
                new Recipe("Флэт Уайт", new List<string> { "Двойной эспрессо", "Молоко" }),
                new Recipe("Макиато", new List<string> { "Эспрессо", "Молочная пена" }),
                new Recipe("Глясе", new List<string> { "Эспрессо", "Мороженое" }),
                new Recipe("Кокосовый латте", new List<string> { "Эспрессо", "Кокосовое молоко" }),
                new Recipe("Мятный мокко", new List<string> { "Эспрессо", "Молоко", "Шоколадный сироп", "Мятный сироп" })
            });

            Achievements = new ObservableCollection<Achievement>(new List<Achievement>
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
            });

            CoffeeHistory = new ObservableCollection<CoffeeHistoryItem>();
            UserProfile = new UserProfile();
            MakeCoffeeCommand = new RelayCommand(MakeCoffee);
            SaveRecipeCommand = new RelayCommand(SaveRecipe);
            RandomCoffeeCommand = new RelayCommand(RandomCoffee);
            DeleteRecipeCommand = new RelayCommand(DeleteRecipe);
            AddMoneyCommand = new RelayCommand(AddMoney);
            SaveHistoryToJsonCommand = new RelayCommand(SaveHistoryToJson);
            LoadHistoryFromJsonCommand = new RelayCommand(LoadHistoryFromJson);
            SaveRecipesToJsonCommand = new RelayCommand(SaveRecipesToJson);
            LoadRecipesFromJsonCommand = new RelayCommand(LoadRecipesFromJson);
        }

        private void MakeCoffee(object parameter)
        {
            if (SelectedIngredients == null || SelectedIngredients.Count == 0)
            {
                CoffeeName = "Ошибка";
                CoffeeDescription = "Выберите хотя бы один ингредиент!";
                CoffeeRating = "0/10";
                CoffeeCost = "";
                return;
            }

            var selected = Ingredients.Where(i => SelectedIngredients.Contains(i.Name)).ToList();
            decimal cost = selected.Sum(i => i.Cost);
            if (UserProfile.Balance < cost)
            {
                CoffeeName = "Ошибка";
                CoffeeDescription = "Недостаточно средств!";
                CoffeeRating = "0/10";
                CoffeeCost = $"Стоимость: {cost:0.00} ₽";
                return;
            }

            // Расчет рейтинга
            int baseRating = 5 + selected.Sum(i => i.RatingPoints);
            int rating = Math.Min(10, baseRating);

            // Определение названия и описания кофе
            var recipe = Recipes.FirstOrDefault(r => r.Ingredients.All(ing => SelectedIngredients.Contains(ing)) && r.Ingredients.Count == SelectedIngredients.Count);
            string coffeeName = recipe?.Name ?? "Кастомный кофе";
            string description = recipe != null ? $"Состав: {string.Join(", ", recipe.Ingredients)}" : $"Состав: {string.Join(", ", SelectedIngredients)}";
            if (!string.IsNullOrEmpty(SelectedModifier))
                description += $"\nМодификатор: {SelectedModifier}";

            // Обновление профиля
            UserProfile.Balance -= cost;
            UserProfile.Experience += rating;
            UserProfile.CoffeesMade++;
            UserProfile.TotalSpent += cost;
            if (DateTime.Now.Hour >= 0 && DateTime.Now.Hour < 6) UserProfile.NightCoffees++;
            if (rating == 10 && rating > UserProfile.MaxRating) UserProfile.MaxRating = rating;
            if (recipe != null && !string.IsNullOrEmpty(coffeeName))
            {
                if (UserProfile.CoffeeTypes == null) UserProfile.CoffeeTypes = new List<string>();
                if (!UserProfile.CoffeeTypes.Contains(coffeeName))
                {
                    UserProfile.CoffeeTypes.Add(coffeeName);
                    UserProfile.UniqueCoffeesMade = UserProfile.CoffeeTypes.Count;
                }
            }
            if (!string.IsNullOrEmpty(SelectedModifier))
            {
                if (UserProfile.UsedModifiers == null) UserProfile.UsedModifiers = new List<string>();
                if (!UserProfile.UsedModifiers.Contains(SelectedModifier))
                {
                    UserProfile.UsedModifiers.Add(SelectedModifier);
                    UserProfile.ModifiersUsed = UserProfile.UsedModifiers.Count;
                }
            }
            int syrups = selected.Count(i => i.Name.Contains("сироп"));
            int milkProducts = selected.Count(i => i.Name.Contains("Молоко") || i.Name.Contains("Сливки") || i.Name.Contains("пена"));
            int spices = selected.Count(i => i.Name.Contains("Корица") || i.Name.Contains("орех") || i.Name.Contains("Имбирь"));
            UserProfile.SyrupsUsed += syrups;
            UserProfile.MilkProductsUsed += milkProducts;
            UserProfile.SpicesUsed = Math.Max(UserProfile.SpicesUsed, spices);
            if (UserProfile.Experience >= UserProfile.Level * 100)
            {
                UserProfile.Level++;
            }
            // История
            CoffeeHistory.Add(new CoffeeHistoryItem
            {
                Date = DateTime.Now,
                Name = coffeeName,
                Rating = rating,
                Cost = cost
            });
            // Достижения
            foreach (var ach in Achievements)
            {
                if (!ach.IsUnlocked && ach.CheckUnlocked(UserProfile))
                {
                    ach.Unlock();
                }
            }
            // Вывод результата
            CoffeeName = coffeeName;
            CoffeeDescription = description;
            CoffeeRating = $"{rating}/10";
            CoffeeCost = $"Стоимость: {cost:0.00} ₽";
            IsResultVisible = false; IsResultVisible = true;
        }

        private void SaveRecipe(object parameter)
        {
            if (SelectedIngredients == null || SelectedIngredients.Count == 0)
                return;
            // Проверка на дубликат
            bool exists = Recipes.Any(r => r.Ingredients.Count == SelectedIngredients.Count && r.Ingredients.All(ing => SelectedIngredients.Contains(ing)));
            if (exists)
                return;
            // Генерация уникального имени
            int customCount = Recipes.Count(r => r.Name.StartsWith("Мой рецепт")) + 1;
            string name = $"Мой рецепт {customCount}";
            Recipes.Add(new Recipe(name, SelectedIngredients.ToList()));
            UserProfile.RecipesSaved++;
            // Достижения
            foreach (var ach in Achievements)
            {
                if (!ach.IsUnlocked && ach.CheckUnlocked(UserProfile))
                    ach.Unlock();
            }
        }

        private void RandomCoffee(object parameter)
        {
            if (Recipes.Count == 0) return;
            var rnd = new Random();
            var recipe = Recipes[rnd.Next(Recipes.Count)];
            SelectedIngredients.Clear();
            foreach (var ing in recipe.Ingredients)
                SelectedIngredients.Add(ing);
            SelectedModifier = Modifiers[rnd.Next(Modifiers.Count)];
            // Можно сразу вызвать MakeCoffee, если нужно сразу показать результат:
            // MakeCoffee(null);
        }

        private void DeleteRecipe(object parameter)
        {
            if (SelectedRecipe == null) return;
            if (Recipes.Contains(SelectedRecipe))
            {
                if (SelectedRecipe.Name.StartsWith("Мой рецепт"))
                    UserProfile.RecipesSaved = Math.Max(0, UserProfile.RecipesSaved - 1);
                Recipes.Remove(SelectedRecipe);
                SelectedRecipe = null;
                // Достижения
                foreach (var ach in Achievements)
                {
                    if (!ach.IsUnlocked && ach.CheckUnlocked(UserProfile))
                        ach.Unlock();
                }
            }
        }

        private void AddMoney(object parameter)
        {
            // TODO: Реализовать логику пополнения баланса (перенести из MainWindow.xaml.cs)
            UserProfile.Balance += 100;
            System.Windows.MessageBox.Show("Баланс пополнен на 100₽! (заглушка MVVM)");
        }

        private void SaveHistoryToJson(object parameter)
        {
            try
            {
                var json = JsonSerializer.Serialize(CoffeeHistory.ToList());
                File.WriteAllText("coffee_history.json", json);
                System.Windows.MessageBox.Show("История успешно сохранена в coffee_history.json");
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Ошибка сохранения: {ex.Message}");
            }
        }

        private void LoadHistoryFromJson(object parameter)
        {
            try
            {
                if (!File.Exists("coffee_history.json"))
                {
                    System.Windows.MessageBox.Show("Файл coffee_history.json не найден");
                    return;
                }
                var json = File.ReadAllText("coffee_history.json");
                var loaded = JsonSerializer.Deserialize<List<CoffeeHistoryItem>>(json);
                CoffeeHistory.Clear();
                foreach (var item in loaded)
                    CoffeeHistory.Add(item);
                System.Windows.MessageBox.Show("История успешно загружена из coffee_history.json");
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Ошибка загрузки: {ex.Message}");
            }
        }

        private void SaveRecipesToJson(object parameter)
        {
            try
            {
                var json = JsonSerializer.Serialize(Recipes.ToList());
                File.WriteAllText("recipes.json", json);
                System.Windows.MessageBox.Show("Рецепты успешно сохранены в recipes.json");
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Ошибка сохранения: {ex.Message}");
            }
        }

        private void LoadRecipesFromJson(object parameter)
        {
            try
            {
                if (!File.Exists("recipes.json"))
                {
                    System.Windows.MessageBox.Show("Файл recipes.json не найден");
                    return;
                }
                var json = File.ReadAllText("recipes.json");
                var loaded = JsonSerializer.Deserialize<List<Recipe>>(json);
                Recipes.Clear();
                foreach (var item in loaded)
                    Recipes.Add(item);
                System.Windows.MessageBox.Show("Рецепты успешно загружены из recipes.json");
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Ошибка загрузки: {ex.Message}");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
} 
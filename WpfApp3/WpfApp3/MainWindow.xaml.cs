using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media; 
using System.Windows.Media.Imaging;

namespace CoffeeMaker
{
    public partial class MainWindow : Window
    {
        private Dictionary<string, int> ingredientPoints = new Dictionary<string, int>
        {
            {"Эспрессо", 3},
            {"Двойной эспрессо", 5},
            {"Американо", 2},
            {"Молоко", 1},
            {"Молочная пена", 2},
            {"Взбитые сливки", 3},
            {"Сгущённое молоко", 4},
            {"Шоколад", 2},
            {"Корица", 1},
            {"Ванильный сироп", 2},
            {"Карамельный сироп", 2},
            {"Мёд", 1},
            {"Мускатный орех", 1},
            {"Лёд", 1},
            {"Ликёр", 5},
            {"Кокосовое молоко", 3}
        };

        private List<Recipe> recipes = new List<Recipe>();

        public MainWindow()
        {
            InitializeComponent();
            LoadDefaultRecipes();
            SetupAnimations();
        }

        private void LoadDefaultRecipes()
        {
            recipes.Add(new Recipe("Капучино", new List<string> { "Эспрессо", "Молоко", "Молочная пена" }));
            recipes.Add(new Recipe("Латте", new List<string> { "Эспрессо", "Молоко" }));
            recipes.Add(new Recipe("Мокко", new List<string> { "Эспрессо", "Молоко", "Шоколад" }));
            recipes.Add(new Recipe("Айриш кофе", new List<string> { "Эспрессо", "Взбитые сливки", "Ликёр" }));
            recipes.Add(new Recipe("Раф кофе", new List<string> { "Эспрессо", "Сгущённое молоко", "Ванильный сироп" }));
        }

        private void SetupAnimations()
        {
            // Инициализация анимаций
        }

        private void MakeCoffee_Click(object sender, RoutedEventArgs e)
        {
            List<string> selectedIngredients = GetSelectedIngredients();

            if (selectedIngredients.Count == 0)
            {
                MessageBox.Show("Выберите хотя бы один ингредиент!", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Определяем тип кофе
            var (coffeeName, description) = IdentifyCoffee(selectedIngredients);

            // Рассчитываем оценку
            int rating = CalculateRating(selectedIngredients);

            // Обновляем UI
            lblCoffeeName.Content = coffeeName;
            txtDescription.Text = description;
            txtRating.Text = $"{rating}/10";

            // Просто показываем изображение чашки без анимации
            imgCoffeeCup.Source = new BitmapImage(
                new Uri("C:\\Users\\studentcoll\\Documents\\кофейня\\coffee-cup.png"));
            imgCoffeeCup.Opacity = 1;
        }

        private List<string> GetSelectedIngredients()
        {
            List<string> ingredients = new List<string>();

            if (chkEspresso.IsChecked == true) ingredients.Add("Эспрессо");
            if (chkDoubleEspresso.IsChecked == true) ingredients.Add("Двойной эспрессо");
            if (chkAmericano.IsChecked == true) ingredients.Add("Американо");
            if (chkMilk.IsChecked == true) ingredients.Add("Молоко");
            if (chkFoam.IsChecked == true) ingredients.Add("Молочная пена");
            if (chkWhippedCream.IsChecked == true) ingredients.Add("Взбитые сливки");
            if (chkCondensedMilk.IsChecked == true) ingredients.Add("Сгущённое молоко");
            if (chkChocolate.IsChecked == true) ingredients.Add("Шоколад");
            if (chkCinnamon.IsChecked == true) ingredients.Add("Корица");
            if (chkSyrup.IsChecked == true) ingredients.Add("Ванильный сироп");
            if (chkCaramelSyrup.IsChecked == true) ingredients.Add("Карамельный сироп");
            if (chkHoney.IsChecked == true) ingredients.Add("Мёд");
            if (chkNutmeg.IsChecked == true) ingredients.Add("Мускатный орех");
            if (chkIce.IsChecked == true) ingredients.Add("Лёд");
            if (chkAlcohol.IsChecked == true) ingredients.Add("Ликёр");
            if (chkCoconutMilk.IsChecked == true) ingredients.Add("Кокосовое молоко");

            return ingredients;
        }

        private (string name, string description) IdentifyCoffee(List<string> ingredients)
        {
            // Проверяем известные рецепты
            foreach (var recipe in recipes)
            {
                if (recipe.Matches(ingredients))
                {
                    return (recipe.Name, recipe.GetDescription());
                }
            }

            // Если рецепт не найден, создаем описание на основе ингредиентов
            return CreateCustomCoffeeDescription(ingredients);
        }

        private (string name, string description) CreateCustomCoffeeDescription(List<string> ingredients)
        {
            string name = "Уникальный микс";
            string description = "Вы создали уникальную комбинацию! Состав: ";

            foreach (var ingredient in ingredients)
            {
                description += ingredient.ToLower() + ", ";
            }

            description = description.TrimEnd(',', ' ') + ".";

            // Особые случаи
            if (ingredients.Contains("Ликёр") && ingredients.Contains("Взбитые сливки"))
            {
                name = "Алкогольный десерт";
            }
            else if (ingredients.Contains("Лёд") && ingredients.Count == 1)
            {
                name = "Стакан льда";
                description = "Очень освежающий напиток!";
            }

            return (name, description);
        }

        private int CalculateRating(List<string> ingredients)
        {
            int total = 0;
            int count = 0;

            foreach (var ingredient in ingredients)
            {
                if (ingredientPoints.TryGetValue(ingredient, out int points))
                {
                    total += points;
                    count++;
                }
            }

            if (count == 0) return 0;

            // Базовый рейтинг на основе очков ингредиентов
            int rating = total * 10 / (count * 5);

            // Бонус за сочетания
            if (ingredients.Contains("Эспрессо") && ingredients.Contains("Молочная пена"))
                rating += 1;
            if (ingredients.Contains("Шоколад") && ingredients.Contains("Молоко"))
                rating += 1;
            if (ingredients.Contains("Корица") && ingredients.Contains("Мёд"))
                rating += 1;

            return Math.Min(10, rating); // Максимум 10
        }




        private void SaveRecipe_Click(object sender, RoutedEventArgs e)
        {
            List<string> ingredients = GetSelectedIngredients();

            if (ingredients.Count == 0)
            {
                MessageBox.Show("Выберите ингредиенты для сохранения рецепта!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string name = Microsoft.VisualBasic.Interaction.InputBox("Введите название рецепта:", "Сохранение рецепта", "Мой кофе");

            if (!string.IsNullOrWhiteSpace(name))
            {
                recipes.Add(new Recipe(name, ingredients));
                MessageBox.Show($"Рецепт \"{name}\" сохранён!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void RandomCoffee_Click(object sender, RoutedEventArgs e)
        {
            // Сбрасываем все выборы
            ResetIngredientChecks();

            // Выбираем случайный рецепт
            if (recipes.Count > 0)
            {
                Random rand = new Random();
                Recipe randomRecipe = recipes[rand.Next(recipes.Count)];

                // Устанавливаем соответствующие чекбоксы
                foreach (var ingredient in randomRecipe.Ingredients)
                {
                    SetIngredientCheck(ingredient, true);
                }

                // Автоматически "готовим" кофе
                MakeCoffee_Click(sender, e);
            }
        }

        private void ResetIngredientChecks()
        {
            chkEspresso.IsChecked = false;
            chkDoubleEspresso.IsChecked = false;
            chkAmericano.IsChecked = false;
            chkMilk.IsChecked = false;
            chkFoam.IsChecked = false;
            chkWhippedCream.IsChecked = false;
            chkCondensedMilk.IsChecked = false;
            chkChocolate.IsChecked = false;
            chkCinnamon.IsChecked = false;
            chkSyrup.IsChecked = false;
            chkCaramelSyrup.IsChecked = false;
            chkHoney.IsChecked = false;
            chkNutmeg.IsChecked = false;
            chkIce.IsChecked = false;
            chkAlcohol.IsChecked = false;
            chkCoconutMilk.IsChecked = false;
        }

        private void SetIngredientCheck(string ingredient, bool isChecked)
        {
            switch (ingredient)
            {
                case "Эспрессо": chkEspresso.IsChecked = isChecked; break;
                case "Двойной эспрессо": chkDoubleEspresso.IsChecked = isChecked; break;
                case "Американо": chkAmericano.IsChecked = isChecked; break;
                case "Молоко": chkMilk.IsChecked = isChecked; break;
                case "Молочная пена": chkFoam.IsChecked = isChecked; break;
                case "Взбитые сливки": chkWhippedCream.IsChecked = isChecked; break;
                case "Сгущённое молоко": chkCondensedMilk.IsChecked = isChecked; break;
                case "Шоколад": chkChocolate.IsChecked = isChecked; break;
                case "Корица": chkCinnamon.IsChecked = isChecked; break;
                case "Ванильный сироп": chkSyrup.IsChecked = isChecked; break;
                case "Карамельный сироп": chkCaramelSyrup.IsChecked = isChecked; break;
                case "Мёд": chkHoney.IsChecked = isChecked; break;
                case "Мускатный орех": chkNutmeg.IsChecked = isChecked; break;
                case "Лёд": chkIce.IsChecked = isChecked; break;
                case "Ликёр": chkAlcohol.IsChecked = isChecked; break;
                case "Кокосовое молоко": chkCoconutMilk.IsChecked = isChecked; break;
            }
        }
    }

    public class Recipe
    {
        public string Name { get; }
        public List<string> Ingredients { get; }

        public Recipe(string name, List<string> ingredients)
        {
            Name = name;
            Ingredients = ingredients;
        }

        public bool Matches(List<string> ingredientsToCheck)
        {
            // Проверяем, что все ингредиенты рецепта присутствуют
            foreach (var ingredient in Ingredients)
            {
                if (!ingredientsToCheck.Contains(ingredient))
                    return false;
            }
            return true;
        }

        public string GetDescription()
        {
            return $"Классический рецепт: {Name}. Состав: {string.Join(", ", Ingredients)}.";
        }
    }
}
using System.Collections.Generic;

namespace CoffeeMaker.Models
{
    public class Recipe
    {
        public string Name { get; set; }
        public List<string> Ingredients { get; set; }
        public string Description { get; set; }

        public Recipe(string name, List<string> ingredients, string description = "")
        {
            Name = name;
            Ingredients = ingredients;
            Description = description;
        }
    }
} 
namespace CoffeeMaker.Models
{
    public class UserProfile
    {
        public int Level { get; set; }
        public int Experience { get; set; }
        public decimal Balance { get; set; }
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
    }
} 
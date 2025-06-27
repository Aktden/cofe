namespace CoffeeMaker.Models
{
    public class Ingredient
    {
        public string Name { get; set; }
        public int RatingPoints { get; set; }
        public decimal Cost { get; set; }
        public int Amount { get; set; }

        public Ingredient(string name, int ratingPoints, decimal cost, int amount)
        {
            Name = name;
            RatingPoints = ratingPoints;
            Cost = cost;
            Amount = amount;
        }
    }
} 
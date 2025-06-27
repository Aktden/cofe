using System;

namespace CoffeeMaker.Models
{
    public class CoffeeHistoryItem
    {
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public int Rating { get; set; }
        public decimal Cost { get; set; }
    }
} 
using System.Windows;
using CoffeeMaker.Models;

namespace CoffeeMaker
{
    public partial class ProfileWindow : Window
    {
        public ProfileWindow(UserProfile profile, int achievementsUnlocked)
        {
            InitializeComponent();
            DataContext = new ProfileViewModel(profile, achievementsUnlocked);
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }

    public class ProfileViewModel
    {
        public int Level { get; set; }
        public int Experience { get; set; }
        public decimal Balance { get; set; }
        public int CoffeesMade { get; set; }
        public int UniqueCoffeesMade { get; set; }
        public int AchievementsUnlocked { get; set; }
        public ProfileViewModel(UserProfile profile, int achievementsUnlocked)
        {
            Level = profile.Level;
            Experience = profile.Experience;
            Balance = profile.Balance;
            CoffeesMade = profile.CoffeesMade;
            UniqueCoffeesMade = profile.UniqueCoffeesMade;
            AchievementsUnlocked = achievementsUnlocked;
        }
    }
} 
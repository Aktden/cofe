using System;

namespace CoffeeMaker.Models
{
    public class Achievement
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public bool IsUnlocked { get; set; }
        public Func<UserProfile, bool> CheckUnlocked { get; set; }

        public Achievement(string title, string description, string icon, Func<UserProfile, bool> checkUnlocked)
        {
            Title = title;
            Description = description;
            Icon = icon;
            CheckUnlocked = checkUnlocked;
            IsUnlocked = false;
        }

        public void Unlock()
        {
            IsUnlocked = true;
        }

        public string GetProgress(UserProfile profile)
        {
            // Пример прогресса (можно доработать)
            return "В процессе...";
        }
    }
} 
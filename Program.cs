using System;

namespace MovieLibraryOO
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var menu = new Menu();

            var userSelection = menu.GetMainMenuSelection();

            while (menu.IsValid)
            {
                menu.Process(userSelection);

                userSelection = menu.GetMainMenuSelection();
            }

            Console.WriteLine("\nThanks for using the Movie Library!");
        }
    }
}
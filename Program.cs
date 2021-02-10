using System;
using System.Collections.Generic;

namespace MovieLibraryOO
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var mm = new MovieManager();
            var menu = new Menu();

            var userSelection = menu.GetInput();

            while (menu.IsValid)
            {
                mm.Execute(userSelection);

                userSelection = menu.GetInput();
            }

            Console.WriteLine("\nThanks for using the Movie Library!");
        }
    }

    internal class Menu
    {
        private readonly List<char> _validChoices = new List<char> {'1', '2'};
        private char _exitKey = 'X';

        public Menu()
        {
            DisplayMenu();
        }

        public bool IsValid { get; set; }

        private void DisplayMenu()
        {
            Console.WriteLine("1. List movies");
            Console.WriteLine("2. Add Movie to list");
        }

        public char GetInput()
        {
            IsValid = true;

            Console.Write($"Select ({String.Join(',', _validChoices.ToArray())},{_exitKey})> ");
            var key = Console.ReadKey().KeyChar;
            while (!_validChoices.Contains(key))
            {
                if (key == _exitKey || char.ToLower(key) == char.ToLower(_exitKey))
                {
                    IsValid = false;
                    break;
                }
                Console.WriteLine();
                Console.Write("Invalid, Please ");
                Console.Write($"Select ({String.Join(',', _validChoices.ToArray())},{_exitKey})> ");
                key = Console.ReadKey().KeyChar;
            }
            
            return key;
        }
    }

    internal class MovieManager
    {
        public void Execute(char userSelection)
        {
            throw new NotImplementedException();
        }
    }
}
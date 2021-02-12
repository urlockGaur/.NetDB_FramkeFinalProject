using System;
using System.Collections.Generic;
using ConsoleTables;
using MovieLibraryOO.Data;
using MovieLibraryOO.Models;

namespace MovieLibraryOO
{
    internal class Menu
    {
        private readonly char _exitKey = 'X';
        private readonly List<char> _validChoices = new List<char> {'1', '2'};
        private MovieContext _context;

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

        public char GetMainMenuSelection()
        {
            IsValid = true;

            Console.Write($"Select ({string.Join(',', _validChoices.ToArray())},{_exitKey})> ");
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
                Console.Write($"Select ({string.Join(',', _validChoices.ToArray())},{_exitKey})> ");
                key = Console.ReadKey().KeyChar;
            }

            return key;
        }

        private Movie GetMovieDetails()
        {
            return new Movie {Title = "Marvel Man", Genres = "Action"};
        }

        public void Process(char userSelection)
        {
            switch (userSelection)
            {
                case '1':
                    // List movies
                    _context = new MovieContext();
                    Console.WriteLine();
                    ConsoleTable.From<Movie>(_context.GetMovies()).Write();
                    break;
                case '2':
                    // Ask user to enter movie details
                    var movie = GetMovieDetails();
                    _context = new MovieContext(movie);
                    _context.AddMovie();
                    Console.WriteLine($"\nYour movie {movie.Title} has been added!\n");
                    break;
            }
        }
    }
}
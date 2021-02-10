using System;
using System.Collections.Generic;

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
            return new Movie {MovieId = 99999, Title = "Marvel Man", Genres = "Action"};
        }

        public void Process(char userSelection)
        {
            switch (userSelection)
            {
                case '1':
                    // List movies
                    _context = new MovieContext();
                    _context.ListMovies();
                    break;
                case '2':
                    // Ask user to enter movie details
                    var movie = GetMovieDetails();
                    _context = new MovieContext(movie);
                    _context.AddMovie();
                    break;
            }
        }
    }

    internal class MovieContext
    {
        private readonly FileRepository _file;
        private readonly Movie _movie;

        public MovieContext(Movie movie = null)
        {
            _file = new FileRepository();
            _movie = movie;
        }

        public void AddMovie()
        {
            _file.Add(_movie);
        }

        public void ListMovies()
        {
            _file.GetAll();
        }
    }

    internal class Movie
    {
        public int MovieId { get; set; }
        public string Title { get; set; }
        public string Genres { get; set; }
    }

    internal class FileRepository
    {
        public FileRepository()
        {
            // initialize file
        }
        public void Add(Movie movie)
        {
        }

        public List<Movie> GetAll()
        {
            return new List<Movie>();
        }

        private void GetIdentity()
        {
        }
    }
}
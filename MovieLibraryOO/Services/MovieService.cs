using Microsoft.Extensions.Logging;
using MovieLibraryEntities.Dao;
using MovieLibraryEntities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieLibraryOO.Services
{
    public class MovieService : IMovieService
    {
        private readonly IRepository _repository;
        private readonly ILogger _logger;

        public MovieService(IRepository repository, ILogger<MovieService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public void AddNewMovieMenu()
        {
            Console.WriteLine("Please enter the Title of the movie you want to add: ");
            var movieTitle = Console.ReadLine();
            Console.WriteLine();

            Console.WriteLine("Please enter the Release Date of the movie (YYYY-MM-DD): ");
            Console.WriteLine();

            if (DateTime.TryParse(Console.ReadLine(), out DateTime releaseDate))
            {
                //calling AddMovie() from repository class
                var movie = _repository.AddMovie(movieTitle, releaseDate);

                if (movie != null)
                {
                    Console.Write($"Movie added: ");

                    Console.WriteLine($"Title: {movie.Title} | Release Date: {movie.ReleaseDate}");

                }
                else
                {
                    Console.WriteLine("Could not add the movie. Please review the input and try again.");

                }
            }
            else
            {
                Console.WriteLine("Invalid date formate. Please enter the release date in the correct format (YYYY-MM-DD).");

            }
        }

        public void DeleteMovieMenu()
        {
            Console.WriteLine("Enter the Id of the movie you want to delete: ");
            var deleteInput = Console.ReadLine();

            if (long.TryParse(deleteInput, out long movieIdDelete))
            {
                _repository.DeleteMovie(movieIdDelete);
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid Movie Id.");
            }
        }

        public void UpdateMovieMenu()
        {
            Console.WriteLine("Enter the Id of the movie you want to update: ");
            if (long.TryParse(Console.ReadLine(), out long movieIdToUpdate))
            {
                Console.WriteLine("Enter the new title of the movie: ");
                var updatedTitle = Console.ReadLine();

                Console.WriteLine("Enther the new Release Date of the movie (YYYY-MM-DD");
                if (DateTime.TryParse(Console.ReadLine(), out DateTime updatedReleaseDate))
                {
                    var updatedMovie = _repository.UpdateMovie(movieIdToUpdate, updatedTitle, updatedReleaseDate);

                    if (updatedMovie != null)
                    {
                        Console.WriteLine("Movie updated successfully: ");
                        _repository.MovieDetails(updatedMovie);
                    }
                    else
                    {
                        Console.WriteLine("Movie could not be updated. Please review the input and try again");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid date format. Please enter the release date in the correct format (YYYY-MM-DD) ");
                }
            }
            else
            {
                Console.WriteLine("Invalid Movie Id. Please enter a valid numberic Movie Id.");
            }
        }

        public void SearchMovieMenu()
        {
            ConsoleColor textColor = Console.ForegroundColor;
            Console.WriteLine("Search by Movie Title");
            Console.WriteLine("----------------------------------------------");
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Please enter your search terms: ");
            Console.ForegroundColor = textColor;
            var searchTerm = Console.ReadLine();

            var movies = _repository.Search(searchTerm);

            if (movies.Any())
            {
                Console.WriteLine("Search Results: ");

                foreach (var movie in movies)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Title: {movie.Title} Release Date: {movie.ReleaseDate}");
                    Console.ForegroundColor = textColor;
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                _logger.LogInformation("Movie not found. Please check your input and try again.");
                Console.ForegroundColor = textColor;
            }
        }
    }
}


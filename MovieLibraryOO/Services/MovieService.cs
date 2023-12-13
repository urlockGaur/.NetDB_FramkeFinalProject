using Microsoft.Extensions.Logging;
using MovieLibraryEntities.Dao;
using MovieLibraryEntities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console;

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
                    try
                    {
                        var updatedMovie = _repository.UpdateMovie(movieIdToUpdate, updatedTitle, updatedReleaseDate);

                        if (updatedMovie != null)
                        {
                            _logger.LogInformation("Movie updated");
                            var successMessage = new Markup("[green]Movie updated successfully: [/]");
                            AnsiConsole.MarkupLine(successMessage.ToString());
                            _repository.MovieDetails(updatedMovie);
                        }
                        else
                        {
                            var errorMessage = new Markup("[red]Movie could not be updated. Please review the input and try again.[/]");
                            AnsiConsole.MarkupLine(errorMessage.ToString());
                        }
                    }
                    catch
                    {
                        _logger.LogError("Error occurred in the UpdateMovieMenu.");
                    }
                }
                else
                {
                    var errorMessage = new Markup("[red]Invalid date format. Please enter the release date in the correct format (YYYY-MM-DD).[/]");
                    AnsiConsole.MarkupLine(errorMessage.ToString());
                }
            }
            else
            {
                var errorMessage = new Markup("[red]Invalid Movie Id. Please enter a valid numberic Movie Id.[/]");
                AnsiConsole.MarkupLine(errorMessage.ToString());
            }
        }

        public void SearchMovieMenu()
        {
            try
            {

                Console.WriteLine("Search by Movie Title");
                Console.WriteLine("----------------------------------------------");
                Console.WriteLine();


                Console.WriteLine("Please enter your search terms: ");

                var searchTerm = Console.ReadLine();

                var movies = _repository.Search(searchTerm);

                if (movies.Any())
                {
                    _logger.LogInformation("Displaying Search Results");
                    var successMessage = new Markup("[green]Search Results: [/]");
                    AnsiConsole.MarkupLine(successMessage.ToString());

                    foreach (var movie in movies)
                    {
                        var movieDetails = new Markup($"Title: [green]{movie.Title}[/] [green]Release Date: [/]{movie.ReleaseDate}");
                        AnsiConsole.MarkupLine(successMessage.ToString());

                    }
                }
                else
                {
                    var errorMessage = new Markup("[red]Movie not found. Please check your input and try again.[/]");
                    AnsiConsole.MarkupLine(errorMessage.ToString());

                }
            }
            catch
            {
                _logger.LogError("Error occurred in the SearchMovieMenu method. ");
                var errorMessage = new Markup("[red]An error while searching for the movie. Please try again...[/]");
                AnsiConsole.MarkupLine(errorMessage.ToString());

            }
        }

        public void DisplayMovieLibraryMenu()
        {
            try
            {
                Console.WriteLine("Movie Library List: ");
                Console.WriteLine("----------------------------------------------");
                Console.WriteLine();

                var movieList = _repository.GetAllMovies();
                if (movieList.Any())
                {
                    AnsiConsole.MarkupLine("[green]");
                    int moviesPerPage = 10;

                    for (int i = 0; i < movieList.Count(); i += moviesPerPage)
                    {
                        var moviesGroup = movieList.Skip(i).Take(moviesPerPage);

                        foreach (var movie in moviesGroup)
                        {
                            string genre = string.Join(", ", movie.MovieGenres.Select(x => x.Genre.Name));
                            AnsiConsole.WriteLine($"Id: {movie.Id} | Title: {movie.Title} | Release Date: {movie.ReleaseDate.ToString("MM/dd/yyy")} | Genres: {genre} ");

                        }
                        AnsiConsole.WriteLine();
                        AnsiConsole.WriteLine("Press enter to view more movies or type 'exit' to stop...");
                        var userInput = Console.ReadLine();

                        if (userInput.ToLower() == "exit")
                        {
                            AnsiConsole.Clear();
                            break;
                        }
                        AnsiConsole.Clear();
                    }
                              
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in DisplayMovieLibaryMenu");

                AnsiConsole.Foreground = Color.Red;
                AnsiConsole.WriteLine($"Error occurred: {ex.Message}");
                AnsiConsole.ResetColors();
            }
        }
    }
}


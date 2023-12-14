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
    public class MenuService : IMenuService
    {
        private readonly IRepository _repository;
        private readonly ILogger _logger;

        public MenuService(IRepository repository, ILogger<MenuService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public void AddNewMovieMenu()
        {
            try {
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
                        _logger.LogInformation("Movie successfully added to database!");
                        AnsiConsole.MarkupLine("[green]Movie added: [/]");

                        AnsiConsole.MarkupLine($"[green]Title:[/] {movie.Title} | [green]Release Date:[/] {movie.ReleaseDate}");

                    }
                    else
                    {
                        AnsiConsole.MarkupLine("[red]Could not add the movie. Please review the input and try again.[/]");

                    }
                }
                else
                {
                    AnsiConsole.MarkupLine("[red]Invalid date formate. Please enter the release date in the correct format (YYYY-MM-DD).[/]");

                }
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error occurred in AddNewMovieMenu");
                AnsiConsole.MarkupLine($"[red]An error occurred: {ex.Message}[/]");
            }
        }

        public void DeleteMovieMenu()
        {
            try
            {
                Console.WriteLine("Enter the Id of the movie you want to delete: ");
                var deleteInput = Console.ReadLine();

                if (long.TryParse(deleteInput, out long movieIdDelete))
                {
                    _repository.DeleteMovie(movieIdDelete);
                }
                else
                {
                    _logger.LogError("Invalid Id input");
                    AnsiConsole.MarkupLine("[red]Invalid Id input. Please enter a valid Movie Id.[/]");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Movie was not deleted...");
                AnsiConsole.MarkupLine("[red]An error occured in the DeleteMovieMenu.[/]");
                
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
                            _logger.LogInformation("Movie updated!");
                            AnsiConsole.MarkupLine("[green]Movie updated successfully: [/]");
                            _repository.MovieDetails(updatedMovie);
                        }
                        else
                        {
                            _logger.LogError("Movie was not updated...");
                            AnsiConsole.MarkupLine("[red]Movie could not be updated. Please review the input and try again.[/]");
                            
                        }
                    }
                    catch
                    {
                        _logger.LogError("Error occurred in the UpdateMovieMenu.");
                    }
                }
                else
                {
                    AnsiConsole.MarkupLine("[red]Invalid date format. Please enter the release date in the correct format (YYYY-MM-DD).[/]");
                    
                }
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Invalid Movie Id. Please enter a valid numberic Movie Id.[/]");
                
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
                    AnsiConsole.MarkupLine("[green]Search Results: [/]");
                    

                    foreach (var movie in movies)
                    {
                        AnsiConsole.MarkupLine($"[green]Title:[/] {movie.Title} [green]Release Date: [/]{movie.ReleaseDate}");
                        

                    }
                }
                else
                {
                     AnsiConsole.MarkupLine("[red]Movie not found. Please check your input and try again.[/]");
                    

                }
            }
            catch
            {
                _logger.LogError("Error occurred in the SearchMovieMenu method. ");
                AnsiConsole.MarkupLine("[red]An error while searching for the movie. Please try again...[/]");
                

            }
        }

        public void DisplayMovieLibraryMenu()
        {
            try
            {
                Console.WriteLine("Movie Library List");
                Console.WriteLine("----------------------------------------------");
                Console.WriteLine();

                var movieList = _repository.GetAllMovies();
                if (movieList.Any())
                {
                    
                    int moviesPerPage = 10;

                    for (int i = 0; i < movieList.Count(); i += moviesPerPage)
                    {
                        var moviesGroup = movieList.Skip(i).Take(moviesPerPage);

                        foreach (var movie in moviesGroup)
                        {
                            string genre = string.Join(", ", movie.MovieGenres.Select(x => x.Genre.Name));
                            AnsiConsole.MarkupLine($"[green]Id:[/] {movie.Id} | [green]Title:[/] {movie.Title} | [green]Release Date:[/] {movie.ReleaseDate.ToString("MM/dd/yyy")} | [green]Genres:[/] {genre}");

                        }
                        Console.WriteLine();
                        AnsiConsole.MarkupLine("[green]Press enter to view more movies or type [red]'exit'[/] to stop...[/]");
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
                AnsiConsole.MarkupLine($"[red]Error occurred: {ex.Message}[/]");
            }
        }

        public void AddNewUserMenu()
        {
            try
            {
                AnsiConsole.MarkupLine("[green]Add New User Module[/]");
                AnsiConsole.MarkupLine("[green]-------------------------[/]");
                AnsiConsole.MarkupLine("Enter [green]First[/] Name: ");
                var firstName = Console.ReadLine();

                AnsiConsole.MarkupLine("Enter [green]Last[/] Name: ");
                var lastName = Console.ReadLine();

                AnsiConsole.MarkupLine("Enter user's [green]Age[/]: ");
                if (long.TryParse(Console.ReadLine(), out long age))
                {
                    AnsiConsole.MarkupLine("Enter user [green]Gender[/]: ");
                    var gender = Console.ReadLine();

                    AnsiConsole.MarkupLine("Enter user [green]Occupation[/]: ");
                    var occupation = Console.ReadLine();

                    AnsiConsole.MarkupLine("Enter user [green]Street Address[/]: ");
                    var streetAddress = Console.ReadLine();

                    AnsiConsole.MarkupLine("Enter user [green]City[/]: ");
                    var city = Console.ReadLine();

                    AnsiConsole.MarkupLine("Enter user [green]State[/]");
                    var state = Console.ReadLine();

                    AnsiConsole.MarkupLine("Enter user [green]Zip Code[/]: ");
                    var zipcode = Console.ReadLine();

                    var newUser = _repository.AddNewUser(firstName, lastName, age, gender, occupation, streetAddress, city, state, zipcode);

                    if (newUser != null)
                    {
                        _logger.LogInformation("New user successfully added to the database.");
                        AnsiConsole.MarkupLine("[green]User Added! Success![/]");
                        DisplayUserDetailsMenu(newUser.Id);
                    }
                    else
                    {
                        AnsiConsole.MarkupLine("[red]User was not added. Please review input and try again...[/]");
                    }
                }
                else { AnsiConsole.MarkupLine("[red]Invalid Age input. Please enter a valid numeric Age.[/]");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the AddNewUserMenu");
                AnsiConsole.MarkupLine($"[red]An error occurred: {ex.Message}[/]");
            }
        }

        public void DisplayUserDetailsMenu(long userId)
        {
            try
            {
                AnsiConsole.MarkupLine("[green]Displaying User Details:[/] ");
                _repository.DisplayUserDetails(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in DisplayUserDetailsMenu...");
                AnsiConsole.MarkupLine($"[red]An error occurred: {ex.Message}[/]");
            }
        }

        public void AddUserRatingMenu()
        {
            try
            {
                Console.WriteLine("Enter the Id of the user: ");
                if (long.TryParse(Console.ReadLine(), out long userId))
                {
                    Console.WriteLine("Enter the Id of the movie: ");
                    if (long.TryParse(Console.ReadLine(), out long movieId))
                    {
                        Console.WriteLine("Enter the rating for the movie: ");
                        if (long.TryParse(Console.ReadLine(), out long rating))
                        {
                            var userRating = _repository.AddUserRating(userId, movieId, rating);

                            if (userRating != null)
                            {
                                _logger.LogInformation("User rating added succesfully!");
                                _repository.DisplayUserMovieRating(userId, movieId);
                            }
                            else
                            {
                                _logger.LogError("User rating add failed...");
                                AnsiConsole.MarkupLine("[red]User rating could not be added. Review the input and try again...[/]");
                            }
                        }
                        else
                        {
                            AnsiConsole.MarkupLine("[red]Invalid rating. Please enter a valid numeric rating.[/]");
                        }
            
                    }
                    else
                    {
                        AnsiConsole.MarkupLine("[red]Invalid movie Id. Please enter a valid numeric movie Id.[/]");
                    }
                }
                else 
                { 
                    AnsiConsole.MarkupLine("[red]Invalid user Id. Please enter a valid numeric user Id.[/]"); 
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in AddUserRatingMenu.");
                AnsiConsole.MarkupLine($"[red]An error occurredL {ex.Message}[/]");
            }
        }

        public void DisplayMovieRatingsMenu()
        {
            try
            {
                while (true) {
                    AnsiConsole.MarkupLine("[green]Movie Rating Menu[/]");
                    AnsiConsole.MarkupLine("[green]----------------------------------------------[/]");
                    AnsiConsole.MarkupLine("[green]1.[/] Display Top Rated Movies By Age Bracket");
                    AnsiConsole.MarkupLine("[green]2.[/] Display Top Rated Movies by Occupation");
                    AnsiConsole.MarkupLine("[green]3.[/] Main Menu");

                    AnsiConsole.MarkupLine("[green]Please make a selection: [/]");

                    var userInput = Console.ReadLine();

                    switch (userInput)
                    {
                        case "1":
                            var topRatedMoviesByAge = _repository.GetTopRatedMoviesByAge();
                            DisplayTopRatedMovies(topRatedMoviesByAge);

                            break;
                        case "2":
                            var topRatedMoviesByOccupation = _repository.GetTopRatedMoviesByOccupation();
                            DisplayTopRatedMovies(topRatedMoviesByOccupation);
                            break;
                        case "3":
                            return;
                        default:
                            AnsiConsole.MarkupLine("[red]Invalid selection. Please select between option 1 - 3.[/]");
                            break;
                    }
                }                
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred in the DisplayMovieRatingMenu.");
                AnsiConsole.MarkupLine($"[red]An error occurred: {ex.Message}[/]");
            }
        }

        private void DisplayTopRatedMovies<T>(List<(T, Movie, long Rating)> topRatedMovies)
        {
            if (topRatedMovies.Any())
            {
                foreach (var result in topRatedMovies)
                {
                    AnsiConsole.MarkupLine($"[green]{result.Item1}:[/] [green]{result.Item2.Title}:[/] | [green]Rating:[/] {result.Rating}");
                }
            }    
        }

        public void DisplayTopRatedMovies(List<(Occupation Occupation, Movie TopRatedMovie, long Rating)> topRatedMovies)
        {
            if (topRatedMovies.Any())
            {
                foreach (var result in topRatedMovies)
                {
                    AnsiConsole.MarkupLine($"[green]{result.Occupation.Name}:[/] [green]{result.TopRatedMovie.Title}:[/] | [green]Rating:[/] {result.Rating}");
                }
            }
        }
    }
}



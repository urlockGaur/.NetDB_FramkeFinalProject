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
                        AnsiConsole.Write("[green]Movie added: [/]");

                        AnsiConsole.WriteLine($"[green]Title:[/] {movie.Title} | [green]Release Date:[/] {movie.ReleaseDate}");

                    }
                    else
                    {
                        AnsiConsole.WriteLine("[red]Could not add the movie. Please review the input and try again.[/]");

                    }
                }
                else
                {
                    AnsiConsole.WriteLine("[red]Invalid date formate. Please enter the release date in the correct format (YYYY-MM-DD).[/]");

                }
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error occurred in AddNewMovieMenu");
                AnsiConsole.WriteLine($"[red]An error occurred: {ex.Message}[/]");
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
                    AnsiConsole.WriteLine("[red]Invalid Id input. Please enter a valid Movie Id.[/]");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Movie was not deleted...");
                AnsiConsole.WriteLine("[red]An error occured in the DeleteMovieMenu.[/]");
                
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
                            AnsiConsole.WriteLine("[green]Movie updated successfully: [/]");
                            _repository.MovieDetails(updatedMovie);
                        }
                        else
                        {
                            _logger.LogError("Movie was not updated...");
                            AnsiConsole.WriteLine("[red]Movie could not be updated. Please review the input and try again.[/]");
                            
                        }
                    }
                    catch
                    {
                        _logger.LogError("Error occurred in the UpdateMovieMenu.");
                    }
                }
                else
                {
                    AnsiConsole.WriteLine("[red]Invalid date format. Please enter the release date in the correct format (YYYY-MM-DD).[/]");
                    
                }
            }
            else
            {
                AnsiConsole.WriteLine("[red]Invalid Movie Id. Please enter a valid numberic Movie Id.[/]");
                
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
                    AnsiConsole.WriteLine("[green]Search Results: [/]");
                    

                    foreach (var movie in movies)
                    {
                        AnsiConsole.WriteLine($"Title: [green]{movie.Title}[/] [green]Release Date: [/]{movie.ReleaseDate}");
                        

                    }
                }
                else
                {
                     AnsiConsole.WriteLine("[red]Movie not found. Please check your input and try again.[/]");
                    

                }
            }
            catch
            {
                _logger.LogError("Error occurred in the SearchMovieMenu method. ");
                AnsiConsole.WriteLine("[red]An error while searching for the movie. Please try again...[/]");
                

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
                var errorMessage = $"[red]Error occurred: {ex.Message}[/]";
                AnsiConsole.MarkupLine(errorMessage);
            }
        }

        public void AddNewUserMenu()
        {
            try
            {
                AnsiConsole.WriteLine("[green]Add New User Module[/]");
                AnsiConsole.WriteLine("[green]-------------------------[/]");
                AnsiConsole.WriteLine("Enter [green]First[/] Name: ");
                var firstName = Console.ReadLine();

                AnsiConsole.WriteLine("Enter [green]Last[/] Name: ");
                var lastName = Console.ReadLine();

                AnsiConsole.WriteLine("Enter user's [green]Age[/]: ");
                if (long.TryParse(Console.ReadLine(), out long age))
                {
                    AnsiConsole.WriteLine("Enter user [green]Gender[/]: ");
                    var gender = Console.ReadLine();

                    AnsiConsole.WriteLine("Enter user [green]Occupation[/]: ");
                    var occupation = Console.ReadLine();

                    AnsiConsole.WriteLine("Enter user [green]Street Address[/]: ");
                    var streetAddress = Console.ReadLine();

                    AnsiConsole.WriteLine("Enter user [green]City[/]: ");
                    var city = Console.ReadLine();

                    AnsiConsole.WriteLine("Enter user [green]State[/]");
                    var state = Console.ReadLine();

                    AnsiConsole.WriteLine("Enter user [green]Zip Code[/]: ");
                    var zipcode = Console.ReadLine();

                    var newUser = _repository.AddNewUser(firstName, lastName, age, gender, occupation, streetAddress, city, state, zipcode);

                    if (newUser != null)
                    {
                        _logger.LogInformation("New user successfully added to the database.");
                        AnsiConsole.WriteLine("[green]User Added! Success![/]");
                        DisplayUserDetailsMenu(newUser.Id);
                    }
                    else
                    {
                        AnsiConsole.WriteLine("[red]User was not added. Please review input and try again...[/]");
                    }
                }
                else { AnsiConsole.WriteLine("[red]Invalid Age input. Please enter a valid numeric Age.[/]");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the AddNewUserMenu");
                AnsiConsole.WriteLine($"[red]An error occurred: {ex.Message}[/]");
            }
        }

        public void DisplayUserDetailsMenu(long userId)
        {
            try
            {
                AnsiConsole.WriteLine("[green]Displaying User Details:[/] ");
                _repository.DisplayUserDetails(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in DisplayUserDetailsMenu...");
                AnsiConsole.WriteLine($"[red]An error occurred: {ex.Message}[/]");
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
                                AnsiConsole.WriteLine("[red]User rating could not be added. Review the input and try again...[/]");
                            }
                        }
                        else
                        {
                            AnsiConsole.WriteLine("[red]Invalid rating. Please enter a valid numeric rating.[/]");
                        }
            
                    }
                    else
                    {
                        AnsiConsole.WriteLine("[red]Invalid movie Id. Please enter a valid numeric movie Id.[/]");
                    }
                }
                else 
                { 
                    AnsiConsole.WriteLine("[red]Invalid user Id. Please enter a valid numeric user Id.[/]"); 
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in AddUserRatingMenu.");
                AnsiConsole.WriteLine($"[red]An error occurredL {ex.Message}[/]");
            }
        }
    }
}


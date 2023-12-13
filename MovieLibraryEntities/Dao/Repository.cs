using Microsoft.EntityFrameworkCore;
using MovieLibraryEntities.Context;
using MovieLibraryEntities.Models;

namespace MovieLibraryEntities.Dao
{
    public class Repository : IRepository, IDisposable
    {
        private readonly IDbContextFactory<MovieContext> _contextFactory;
        private readonly MovieContext _context;

       // public Repository(MovieContext dbContext)
       // {
       //_context = dbContext;
       // }
        public Repository(IDbContextFactory<MovieContext> contextFactory)
        {
            _contextFactory = contextFactory;
            _context = _contextFactory.CreateDbContext();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        

        // ========================================================
        // =Helper Method=
        public Movie GetById(long id)
        {
            return _context.Movies.FirstOrDefault(x => x.Id == id);
        }

        // ========================================================
        // =Helper Method=
        public void MovieDetails(Movie movie)
        {
            Console.WriteLine($"Movie Details: ");
            Console.WriteLine($"Title: {movie.Title}");
            Console.WriteLine($"Release Date: {movie.ReleaseDate.ToString("MM/dd/yyy")}");
            Console.WriteLine();
        }

        // ========================================================
        // =Base Requirement=
        // =Adding a new movie=
        public Movie AddMovie(string title, DateTime releaseDate)
        {
            using (var db = new MovieContext())
            {
                Movie newMovie = new Movie()
                {
                    Title = title,
                    ReleaseDate = releaseDate
                };

                try
                {
                    _context.Movies.Add(newMovie);
                    _context.SaveChanges();
                    return newMovie;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error saving movie: {ex.Message}");
                    return null;
                }
            }
        }

        // ========================================================
        // =Base Requirement=
        // =Deleting a movie=
        public void DeleteMovie(long movieIdDelete)
        {
            var movieDelete = GetById(movieIdDelete);

            if (movieDelete != null)
            {
                MovieDetails(movieDelete);
                ConsoleColor textColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;

                Console.WriteLine($"Are you sure you want to delete {movieDelete.Title}: y / n ");
                Console.ForegroundColor = textColor;
                var deleteConfirmationInput = Console.ReadLine();
                if (deleteConfirmationInput.ToLower() == "y")
                {
                    try
                    {
                        var movieToDelete = _context.Movies.Find(movieIdDelete);

                        if (movieToDelete != null)
                        {
                            _context.Movies.Remove(movieToDelete);
                            _context.SaveChanges();
                            Console.WriteLine("Movie deleted successfully.");
                        }
                        else
                        {
                            Console.WriteLine("Movie not found");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"An error occured while attempting to delete the movie: {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine("Delete canceled");
                }
            }
            else
            {
                Console.WriteLine("Movie not found. Try again");
            }
        }

        // ========================================================
        // =Base Requirement=
        // =Updating a movie=
        public Movie UpdateMovie(long movieId, string updatedMovieTitle, DateTime updatedReleaseDate)
        {
            var movieToUpdate = GetById(movieId);

            if (movieToUpdate != null)
            {
                movieToUpdate.Title = updatedMovieTitle;
                movieToUpdate.ReleaseDate = updatedReleaseDate;
            
                try
                {
                    _context.SaveChanges();
                    Console.WriteLine("Movie updated successfuly");
                    MovieDetails(movieToUpdate);
                    return movieToUpdate;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error updating movie: {ex.Message}");
                    throw new Exception("Failed to update the movie...");
                }
            }
            else
            {
                Console.WriteLine("Movie not found. Could not update.");
                throw new Exception("Movie not found for update...");
            }
        }

        // ========================================================
        // =Base Requirement=
        // =Displaying movies to a list=
        public IEnumerable<Movie> GetAllMovies()
        {
            return _context.Movies.ToList();
        }

        public IEnumerable<Movie> Search(string searchString)
        {
            var allMovies = _context.Movies;
            var listOfMovies = allMovies.ToList();
            var temp = listOfMovies.Where(x => x.Title.Contains(searchString, StringComparison.CurrentCultureIgnoreCase));

            return temp;
        }
    }
}

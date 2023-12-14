using Microsoft.EntityFrameworkCore;
using MovieLibraryEntities.Context;
using MovieLibraryEntities.Models;
using System.Text.RegularExpressions;

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

                Console.WriteLine($"Are you sure you want to delete {movieDelete.Title}: y / n ");

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
        // =Updating a Movie=
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
        // =Displaying Movies to a List=
        public IEnumerable<Movie> GetAllMovies()
        {
            return _context.Movies.ToList();
        }
        // ========================================================
        // =Base Requirement=
        // =Searching for Movie=
        public IEnumerable<Movie> Search(string searchString)
        {
            var allMovies = _context.Movies;
            var listOfMovies = allMovies.ToList();
            var temp = listOfMovies.Where(x => x.Title.Contains(searchString, StringComparison.CurrentCultureIgnoreCase));

            return temp;
        }

        // ========================================================
        // =C Level Requirement=
        // =Add User - Display User Details=
        public User AddNewUser(string firstName, string lastName, long age, string gender, string zipcode, string streetAddress, string city, string state, string occupation)
        {
            using (var db = new MovieContext())
            {
                var newOccupation = new Occupation { Name = occupation };
                var newUserDetail = new UserDetail { FirstName = firstName, LastName = lastName, StreetAddress = streetAddress, City = city, State = state, };
                var newUser = new User { Age = age, Gender = gender, ZipCode = zipcode, UserDetail = newUserDetail, Occupation = newOccupation };

                try
                {
                    _context.Users.Add(newUser);
                    _context.SaveChanges();
                    return newUser;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error adding the new user: {ex.Message}");
                    return null;
                }
            }
        }

        // ========================================================
        // =C Level Requirement=
        // =Add User - Display User Details=
        public void DisplayUserDetails(long userId)
        {
            try
            {
                var user = _context.Users.Include(u => u.UserDetail).Include(u => u.Occupation).FirstOrDefault(u => u.Id == userId);
                if (user != null)
                {
                    Console.WriteLine($"User Details:");
                    Console.WriteLine($"Name: {user.UserDetail.FirstName} {user.UserDetail.LastName}");
                    Console.WriteLine($"Age: {user.Age}");
                    Console.WriteLine($"Gender: {user.Gender}");
                    Console.WriteLine($"Occupation: {user.Occupation.Name}");
                    Console.WriteLine($"Street Address: {user.UserDetail.StreetAddress}");
                    Console.WriteLine($"City: {user.UserDetail.City}");
                    Console.WriteLine($"State: {user.UserDetail.State}");
                    Console.WriteLine($"Zip Code: {user.ZipCode}");
                }
                else
                {
                    Console.WriteLine("User not found");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error displaying user details: {ex.Message}");
            }
        }

        // ========================================================
        // =B Level Requirement=
        // =Ask User to enter rating on existing movie - Display details = user, rated movie, rating=
        public UserMovie AddUserRating(long userId, long Id, long rating)
        {
            var userRating = new UserMovie
            {
                Rating = rating,
                RatedAt = DateTime.Now
            };

            try
            {
                //grab User and Movie entities
                var user = _context.Users.FirstOrDefault(u => u.Id == userId);
                var movie = _context.Movies.FirstOrDefault(u => u.Id == Id);

                if (user != null && movie != null)
                {
                    userRating.User = user;
                    userRating.Movie = movie;

                    _context.UserMovies.Add(userRating);
                    _context.SaveChanges();
                    return userRating;
                }
                else
                {
                    Console.WriteLine("User or Movie not found...");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding user rating: {ex.Message}");
                return null;
            }
        }

        // ========================================================
        // =B Level Requirement=
        // =Ask User to enter rating on existing movie - Display details = user, rated movie, rating=
        public void DisplayUserMovieRating(long userId, long movieId)
        {
            var userMovieRating = _context.UserMovies
                .Include(u => u.User)
                .Include(u => u.Movie)
                .FirstOrDefault(u => u.User.Id == userId && u.Movie.Id == movieId);

            if (userMovieRating != null)
            {
                Console.WriteLine("User Details: ");
                Console.WriteLine("-------------------------");

                // Check if User is not null before accessing properties
                if (userMovieRating.User != null)
                {
                    Console.WriteLine($"User Id: {userMovieRating.User.Id}");
                    Console.WriteLine($"Age: {userMovieRating.User.Age}");
                    Console.WriteLine($"Occupation: {userMovieRating.User.Occupation.Name}");
                }
                else
                {
                    Console.WriteLine("User details not available.");
                }

                Console.WriteLine("-------------------------");

                Console.WriteLine();

                Console.WriteLine("Movie Details: ");
                Console.WriteLine("-------------------------");

                // Check if Movie is not null before accessing properties
                if (userMovieRating.Movie != null)
                {
                    Console.WriteLine($"Title: {userMovieRating.Movie.Title}");
                    Console.WriteLine($"Release Date: {userMovieRating.Movie.ReleaseDate.ToString("MM/dd/yyyy") ?? "N/A"}");
                    Console.WriteLine($"New Rating: {userMovieRating.Rating}");
                }
                else
                {
                    Console.WriteLine("Movie details not available.");
                }

                Console.WriteLine("-------------------------");
            }
            else
            {
                Console.WriteLine($"User movie rating not found for userId: {userId} and movieId: {movieId}");
            }
        }

        // ========================================================
        // =A Level Requirement=
        // =List top rated movie by age bracket or occupation - Sort alphabetically and by rating and display just the first movie=
        public List<(string AgeBracket, Movie TopRatedMovie, long Rating)> GetTopRatedMoviesByAge()
        {
            var ageBrackets = new List<(long minAge, long maxAge, string AgeBracket)>
    {
        (0, 17, "Under 18"),
        (18, 24, "18-24"),
        (25, 34, "25-34"),
        (35, 44, "35-44"),
        (45, 54, "45-54"),
        (55, long.MaxValue, "55+"),
    };

            var results = ageBrackets
                .Select(bracket => (
                    AgeBracket: bracket.AgeBracket,
                    TopRatedMovie: _context.UserMovies
                        .Include(u => u.Movie)
                        .Include(u => u.User)
                        .Where(u => u.User.Age >= bracket.minAge && u.User.Age <= bracket.maxAge)
                        .OrderByDescending(u => u.Rating)
                        .Select(u => u.Movie)
                        .FirstOrDefault(),
                    Rating: _context.UserMovies
                        .Where(u => u.User.Age >= bracket.minAge && u.User.Age <= bracket.maxAge)
                        .OrderByDescending(u => u.Rating)
                        .Select(u => u.Rating)
                        .FirstOrDefault()
                ))
                .ToList();

            // Log information for debugging
            foreach (var result in results)
            {
                Console.WriteLine($"AgeBracket: {result.AgeBracket}, TopRatedMovie: {result.TopRatedMovie?.Title}, Rating: {result.Rating}");
            }

            return results;
        }

        // ========================================================
        // =A Level Requirement=
        // =List top rated movie by age bracket or occupation - Sort alphabetically and by rating and display just the first movie=
        public List<(string Occupation, Movie TopRatedMovie, long Rating)> GetTopRatedMoviesByOccupation()
        {
            var topRatedMoviesByOccupation = _context.UserMovies
                .Include(u => u.Movie)
                .Include(u => u.User.Occupation)
                .GroupBy(u => u.User.Occupation.Name)
                .Select(u => new
                {
                    Occupation = u.Key,
                    TopRatedMovie = u.OrderByDescending(x => x.Rating).Select(x => x.Movie).FirstOrDefault(),
                    Rating = u.Max(x => x.Rating),
                })
                .ToList();

            // Add null checks before accessing properties
            var results = topRatedMoviesByOccupation
                .Select(movie => (
                    Occupation: movie?.Occupation ?? "Unknown",
                    TopRatedMovie: movie?.TopRatedMovie,
                    Rating: movie?.Rating ?? 0
                ))
                .ToList();

            return results;
        }
    }
}


using MovieLibraryEntities.Models;

namespace MovieLibraryEntities.Dao
{
    public interface IRepository
    {
        IEnumerable<Movie> GetAllMovies();
        IEnumerable<Movie> Search(string searchString);

         void MovieDetails(Movie movie);

        Movie AddMovie(string title, DateTime releaseDate);

        void DeleteMovie(long movieIdDelete);

        Movie UpdateMovie(long movieId, string updatedMovieTitle, DateTime updatedReleaseDate);

        User AddNewUser(string firstName, string lastName, long age, string gender, string zipcode, string streetAddress, string city, string state, string occupation);
        void DisplayUserDetails(long userId);

        UserMovie AddUserRating(long userId, long movieId, long rating);
        void DisplayUserMovieRating(long userId, long movieId);


    }
}

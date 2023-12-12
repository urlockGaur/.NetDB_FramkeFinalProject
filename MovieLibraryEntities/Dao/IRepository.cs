using MovieLibraryEntities.Models;

namespace MovieLibraryEntities.Dao
{
    public interface IRepository
    {
        IEnumerable<Movie> GetAll();
        IEnumerable<Movie> Search(string searchString);

         void MovieDetails(Movie movie);

        Movie AddMovie(string title, DateTime releaseDate);

        void DeleteMovie(long movieIdDelete);

        Movie UpdateMovie(long movieId, string updatedMovieTitle, DateTime updatedReleaseDate);
    }
}

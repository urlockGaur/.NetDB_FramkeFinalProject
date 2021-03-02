using System.Collections.Generic;
using MovieLibraryOO.Models;

namespace MovieLibraryOO.Data
{
    public interface IContext
    {
        void AddMovie(Movie movie);
        List<Movie> GetMovies();
    }
}
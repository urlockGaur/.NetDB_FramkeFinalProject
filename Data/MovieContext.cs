using MovieLibraryOO.Models;

namespace MovieLibraryOO.Data
{
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
}
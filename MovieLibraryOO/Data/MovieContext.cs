using System.Collections.Generic;
using MovieLibraryOO.Models;

namespace MovieLibraryOO.Data
{
    public class MovieContext2 : IContext
    {
        private readonly IRepository _repository;

        public MovieContext2(IRepository repository)
        {
            _repository = repository;
        }

        public void AddMovie(Movie movie)
        {
            _repository.Add(movie);
        }

        public List<Movie> GetMovies()
        {
            return _repository.GetAll();
        }
    }
}
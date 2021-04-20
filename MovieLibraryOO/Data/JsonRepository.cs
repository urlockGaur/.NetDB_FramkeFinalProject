
using System.Collections.Generic;
using MovieLibraryOO.Models;
using Newtonsoft.Json;

namespace MovieLibraryOO.Data
{
    public class JsonRepository : IRepository
    {
        public void Add(Movie movie)
        {
            throw new System.NotImplementedException();
        }

        public List<Movie> GetAll()
        {
            List<Movie> movies = JsonConvert.DeserializeObject<List<Movie>>("{ title: Sample }");
            return movies;
        }
    }
}
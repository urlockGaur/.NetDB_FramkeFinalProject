using System.Collections.Generic;
using MovieLibraryOO.Models;

namespace MovieLibraryOO.Data
{
    public interface IRepository {
        void Add(Movie movie);
        List<Movie> GetAll();
    }

    // public class TestClass {
    //     public void Test() 
    //     {
    //         IRepository repo;

    //         repo.Add(new Movie());
    //         var list = repo.GetAll();

    //     }
    // }
}

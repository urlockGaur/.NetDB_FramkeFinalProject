using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieLibraryOO.Services
{
    public interface IMovieService
    {
        void AddNewMovieMenu();
        void DeleteMovieMenu();
        void UpdateMovieMenu();
        void SearchMovieMenu();
    }
}

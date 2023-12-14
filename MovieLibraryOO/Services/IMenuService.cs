using MovieLibraryEntities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieLibraryOO.Services
{
    public interface IMenuService
    {
        void AddNewMovieMenu();
        void DeleteMovieMenu();
        void UpdateMovieMenu();
        void SearchMovieMenu();
        void DisplayMovieLibraryMenu();
        void AddNewUserMenu();
        void DisplayUserDetailsMenu(long userId);

        void AddUserRatingMenu();

        void DisplayMovieRatingsMenu();

        void DisplayTopRatedMovies(List<(Occupation Occupation, Movie TopRatedMovie, long Rating)> topRatedMovies);
    }
}

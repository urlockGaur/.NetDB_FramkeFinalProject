using MovieLibraryOO.Models;

namespace MovieLibraryOO
{
    public interface IMenu
    {
        bool IsValid { get; set; }

        char GetMainMenuSelection();
        Movie GetMovieDetails();
        void Process(char userSelection);
    }
}
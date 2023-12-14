using ConsoleTables;
using Microsoft.Extensions.Logging;
using MovieLibraryEntities.Dao;
using MovieLibraryOO.Dao;
using MovieLibraryOO.Dto;
using MovieLibraryOO.Mappers;
using System;

namespace MovieLibraryOO.Services
{
    public class MainService : IMainService
    {
        private readonly ILogger<MainService> _logger;
        private readonly IMovieMapper _movieMapper;
        private readonly IRepository _repository;
        private readonly IFileService _fileService;
        private readonly IMovieService _movieService;

        public MainService(ILogger<MainService> logger, IMovieMapper movieMapper, IRepository repository, IFileService fileService, IMovieService movieService)
        {
            _logger = logger;
            _movieMapper = movieMapper;
            _repository = repository;
            _fileService = fileService;
            _movieService = movieService;
        }

        public void Invoke()
        {
            var menu = new Menu();

            Menu.MenuOptions menuChoice;
            do
            {
                menuChoice = menu.ChooseAction();

                switch (menuChoice)
                {
                    case Menu.MenuOptions.DisplayMovieLibrary:
                        _logger.LogInformation("Displaying Movie Library");
                        _movieService.DisplayMovieLibraryMenu();
                        break;                 
                    case Menu.MenuOptions.AddMovie:
                        _logger.LogInformation("Adding a N=new Movie");
                        _movieService.AddNewMovieMenu();
                        //menu.GetUserInput();
                        break;
                    case Menu.MenuOptions.UpdateMovie:
                        _logger.LogInformation("Updating an existing Movie");
                        _movieService.UpdateMovieMenu();
                        break;
                    case Menu.MenuOptions.DeleteMovie:
                        _logger.LogInformation("Deleting a Movie");
                        _movieService.DeleteMovieMenu();
                        break;
                    case Menu.MenuOptions.SearchMovie:
                        _logger.LogInformation("Searching for a movie");
                        _movieService.SearchMovieMenu();
                        break;
                    case Menu.MenuOptions.AddUser:
                        _logger.LogInformation("Adding a new User.");
                        _movieService.AddNewUserMenu();
                        break;
                }
            }
            while (menuChoice != Menu.MenuOptions.Exit);

            menu.Exit();


            Console.WriteLine("\nThanks for using the Movie Library!");

        }
    }
}
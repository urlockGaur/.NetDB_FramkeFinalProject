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
        private readonly IMenuService _menuService;

        public MainService(ILogger<MainService> logger, IMovieMapper movieMapper, IRepository repository, IFileService fileService, IMenuService menuService)
        {
            _logger = logger;
            _movieMapper = movieMapper;
            _repository = repository;
            _fileService = fileService;
            _menuService = menuService;
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
                    case Menu.MenuOptions.AddUser:
                        _logger.LogInformation("Adding a new User.");
                        _menuService.AddNewUserMenu();
                        break;
                    case Menu.MenuOptions.AddMovie:
                        _logger.LogInformation("Adding a New Movie");
                        _menuService.AddNewMovieMenu();
                        break;                                                        
                    case Menu.MenuOptions.UpdateMovie:
                        _logger.LogInformation("Updating an existing Movie");
                        _menuService.UpdateMovieMenu();
                        break;                    
                    case Menu.MenuOptions.SearchMovie:
                        _logger.LogInformation("Searching for a movie");
                        _menuService.SearchMovieMenu();
                        break;
                    case Menu.MenuOptions.DisplayMovieLibrary:
                        _logger.LogInformation("Displaying Movie Library");
                        _menuService.DisplayMovieLibraryMenu();
                        break;
                    case Menu.MenuOptions.RateMovie:
                        _logger.LogInformation("Rating a Movie");
                        _menuService.AddUserRatingMenu();
                        break;
                    case Menu.MenuOptions.ListTopRatedMovie:
                        _logger.LogInformation("Displaying Top Rated Movies by Age or Occupation.");
                        _menuService.DisplayMovieRatingsMenu();
                        break;
                    case Menu.MenuOptions.DeleteMovie:
                        _logger.LogInformation("Deleting a Movie");
                        _menuService.DeleteMovieMenu();
                        break;

                }
            }
            while (menuChoice != Menu.MenuOptions.Exit);

            menu.Exit();


            Console.WriteLine("\nThanks for using the Movie Library!");

        }
    }
}
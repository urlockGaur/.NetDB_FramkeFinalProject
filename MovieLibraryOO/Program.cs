using System;
using Microsoft.Extensions.DependencyInjection;
using MovieLibraryOO.Data;

namespace MovieLibraryOO
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // DEPENDENCY INJECTION
            var serviceProvider = new ServiceCollection()
                .AddSingleton<IRepository, FileRepository>()
                .AddSingleton<IContext, MovieContext>()
                .AddSingleton<IMenu, Menu>()
                .BuildServiceProvider();

            // ** still have a dependency here **
            // var repository = new MyNewRepository();
            // var context = new MovieContext(repository);
            // var menu = new Menu(repository, context);

            var menu = serviceProvider.GetService<IMenu>();
            var userSelection = menu.GetMainMenuSelection();

            while (menu.IsValid)
            {
                menu.Process(userSelection);

                userSelection = menu.GetMainMenuSelection();
            }

            Console.WriteLine("\nThanks for using the Movie Library!");
        }
    }
}
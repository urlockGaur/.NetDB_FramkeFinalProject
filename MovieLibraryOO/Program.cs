using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MovieLibraryOO.Context;
using MovieLibraryOO.Data;

namespace MovieLibraryOO
{
    internal class Program
    {
        private static void Main(string[] args)
        {

            using (var db = new MovieContext()) {
                var users = db.Users.Take(10).ToList();
                foreach (var user in users) {
                    System.Console.WriteLine($"({user.Id}) {user.Gender} {user.Occupation.Name}");
                }
            }

            using (var db = new MovieContext()) {
                var users = db.Users.Where(x=> x.Id == 1).ToList();
                foreach (var user in users) {
                    System.Console.WriteLine($"Added user: ({user.Id}) {user.Gender} {user.Occupation.Name}");
                }
            }

            using (var db = new MovieContext()) {
                var users = db.Users.Where(x=> x.Id == 2).Include(x=>x.UserMovies).ThenInclude(x=>x.Movie).ToList();

                foreach (var user in users) {
                    System.Console.WriteLine($"Added user: ({user.Id}) {user.Gender} {user.Occupation.Name}");

                    foreach (var movie in user.UserMovies.OrderBy(x=>x.Rating)) {
                        System.Console.WriteLine($"\t\t\t{movie.Rating} {movie.Movie.Title}");
                    }
                }
            }


            // // DEPENDENCY INJECTION
            // var serviceProvider = new ServiceCollection()
            //     .AddSingleton<IRepository, FileRepository>()
            //     .AddSingleton<IContext, MovieContext>()
            //     .AddSingleton<IMenu, Menu>()
            //     .BuildServiceProvider();

            // // ** still have a dependency here **
            // // var repository = new MyNewRepository();
            // // var context = new MovieContext(repository);
            // // var menu = new Menu(repository, context);

            // var menu = serviceProvider.GetService<IMenu>();
            // var userSelection = menu.GetMainMenuSelection();

            // while (menu.IsValid)
            // {
            //     menu.Process(userSelection);

            //     userSelection = menu.GetMainMenuSelection();
            // }

            Console.WriteLine("\nThanks for using the Movie Library!");
        }
    }
}
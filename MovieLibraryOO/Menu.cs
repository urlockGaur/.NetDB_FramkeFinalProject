﻿using System;
using System.Collections.Generic;
using System.Linq;
using Spectre.Console;

namespace MovieLibraryOO
{
    // This is just a fun implementation of Spectre.Console to present more interesting menus
    // Feel free to use it and read more at https://spectreconsole.net if you would like
    // If not, just use your own regular Console.Writeline menus as we have in the past
    public class Menu
    {
        public enum MenuOptions
        {
            AddUser,
            AddMovie,
            UpdateMovie,            
            SearchMovie,
            DisplayMovieLibrary,
            RateMovie,
            ListTopRatedMovie,
            DeleteMovie,
            Exit
        }

        private static readonly Dictionary<MenuOptions, string> ReadableDisplayNames = new Dictionary<MenuOptions, string>
        {
            { MenuOptions.AddUser, "Add New User" },
            { MenuOptions.AddMovie,"Add New Movie"  },                                
            { MenuOptions.UpdateMovie,"Update Movie Data"  },            
            { MenuOptions.SearchMovie,"Search for Movie"  },
            { MenuOptions.DisplayMovieLibrary,"Display Movie Library"  },
            { MenuOptions.RateMovie, "Rate Movie" },
            { MenuOptions.ListTopRatedMovie, "Display Top Rated" },
            { MenuOptions.DeleteMovie,"Delete Movie"  },
            { MenuOptions.Exit,"Exit"  }
        };

        public Menu() // default constructor
        {
        }

        public MenuOptions ChooseAction()
        {
            var menuOptions = Enum.GetNames(typeof(MenuOptions));

            var choices = ReadableDisplayNames.Values.ToArray();

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Choose your [green]menu action[/]:")
                    .AddChoices(choices));

            return (MenuOptions) Enum.Parse(typeof(MenuOptions), menuOptions[Array.IndexOf(choices, choice)]);
        }

        public void Exit()
        {
            AnsiConsole.Write(
                new FigletText("Thanks!")
                    .LeftAligned()
                    .Color(Color.Green));
        }

        public string GetUserResponse(string question, string highlightedText, string highlightedColor)
        {
            return AnsiConsole.Ask<string>($"{question} [{highlightedColor}]{highlightedText}[/]");
        }

        #region examples
        // example - not currently used - see https://spectreconsole.net for more fun!
        public void GetUserInput()
        {
            var name = AnsiConsole.Ask<string>("What is your [green]name[/]?");
            var semester = AnsiConsole.Prompt(
                new TextPrompt<string>("For which [green]semester[/] are you registering?")
                    .InvalidChoiceMessage("[red]That's not a valid semester[/]")
                    .DefaultValue("Spring 2022")
                    .AddChoice("Fall 2022")
                    .AddChoice("Spring 2023"));
            var classes = AnsiConsole.Prompt(
                new MultiSelectionPrompt<string>()
                    .Title("For which [green]classes[/] are you registering?")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down to reveal more classes)[/]")
                    .InstructionsText(
                        "[grey](Press [blue]<space>[/] to toggle a class, " +
                        "[green]<enter>[/] to accept)[/]")
                    .AddChoices("History", "English", "Spanish", "Math", "Computer", "Literature", "Science",
                        "Chemistry", "Economics"));
        }
        #endregion
    }
}
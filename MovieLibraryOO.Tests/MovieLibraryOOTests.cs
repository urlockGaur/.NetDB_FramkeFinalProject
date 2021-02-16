using System;
using Xunit;

namespace MovieLibraryOO.Tests
{
    public class MovieLibraryOOTests
    {
        [Fact]
        public void Menu_Valid_Success()
        {
            // arrange
            Menu menu = new Menu();
            
            // act
            var results = menu.GetMovieDetails();

            // assert
            Assert.Equal("Marvel Man", results.Title);
            Assert.Equal("Action", results.Genres);

        }
    }
}

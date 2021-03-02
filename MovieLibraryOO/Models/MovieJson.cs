using System.Collections.Generic;

namespace MovieLibraryOO.Models
{
    public class MovieJson
    {
        public int MovieId { get; set; }
        public string Title { get; set; }
        public List<string> Genres { get; set; }

        public MovieJson()
        {
            this.MovieId = 1;
            this.Title = "My Movie";
            this.Genres = new List<string>() 
            {
                "Horror", "Action"
            };
        }
    }
}
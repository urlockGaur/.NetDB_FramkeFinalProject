using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using MovieLibraryOO.Models;

namespace MovieLibraryOO.Data
{
    public class FileRepository : IRepository
    {

        private readonly string _filename = Path.Combine(Environment.CurrentDirectory, "Files", "movies.csv");
        public FileRepository()
        {
            if (!File.Exists(_filename)) throw new FileNotFoundException($"Unable to locate {_filename}");
        }

        public void Add(Movie movie)
        {
            var movies = GetAll();

            var lastMovie = movies.OrderBy(o => o.MovieId).Last();
            movie.MovieId = lastMovie.MovieId + 1;

            movies.Add(movie);

            using (var writer = new StreamWriter(_filename))
            {
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.Context.RegisterClassMap<MovieMap>();
                    csv.WriteRecords(movies);
                }
            }
        }

        public List<Movie> GetAll()
        {
            IEnumerable<Movie> records;

            using (var reader = new StreamReader(_filename))
            {
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    csv.Context.RegisterClassMap<MovieMap>();

                    records = csv.GetRecords<Movie>().ToList();
                }
            }
            return new List<Movie>(records);
        }
    }
}
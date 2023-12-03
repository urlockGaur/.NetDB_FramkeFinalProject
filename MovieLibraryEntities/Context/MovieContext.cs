using FakerDotNet;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MovieLibraryEntities.Models;

namespace MovieLibraryEntities.Context;

public class MovieContext : DbContext
{
    private readonly ILogger<MovieContext> _logger;
    public DbSet<Genre> Genres { get; set; }
    public DbSet<MovieGenre> MovieGenres { get; set; }
    public DbSet<Movie> Movies { get; set; }
    public DbSet<Occupation> Occupations { get; set; }
    public DbSet<UserDetail> UserDetails { get; set; }
    public DbSet<UserMovie> UserMovies { get; set; }
    public DbSet<User> Users { get; set; }

    public MovieContext()
    {
        var factory = LoggerFactory.Create(b => b.AddConsole());
        _logger = factory.CreateLogger<MovieContext>();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

        optionsBuilder
            .LogTo(action => _logger.LogInformation(action), LogLevel.Information)
            //.EnableSensitiveDataLogging()
            .UseSqlServer(configuration.GetConnectionString("MovieContext"), builder => builder.EnableRetryOnFailure()
            );
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var userDetails = new List<UserDetail>();

        for (var i = 1; i <= 943; i++)
        {
            userDetails.Add(new UserDetail
            {
                Id = i,
                FirstName = Faker.Name.FirstName(),
                LastName = Faker.Name.LastName(),
                StreetAddress = Faker.Address.StreetAddress(),
                City = Faker.Address.City(),
                State = Faker.Address.StateAbbr(),
                UserId = i
            });
        }

        modelBuilder.Entity<UserDetail>().HasData(userDetails);
    }
}

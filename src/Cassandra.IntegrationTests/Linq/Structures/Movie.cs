using System.Collections.Generic;
using System.Diagnostics;
using Cassandra.Data.Linq;
using Cassandra.IntegrationTests.TestBase;
using NUnit.Framework;
#pragma warning disable 618

namespace Cassandra.IntegrationTests.Linq.Structures
{
    public enum MovieRating
    {
        A,
        B,
        C
    }

    [AllowFiltering]
    [Table("coolMovies")]
    public class Movie
    {
        [Column("mainGuy")]
        public string MainActor;

        [PartitionKey(5)]
        [Column("movie_maker")]
        public string MovieMaker;

        [PartitionKey(1)]
        [Column("unique_movie_title")]
        public string Title;

        [Column("list")]
        public List<string> ExampleSet = new List<string>();

        [ClusteringKey(1)]
        [Column("director")]
        public string Director { get; set; }

        [Column("yearMade")]
        public int? Year { get; set; }

        [Cassandra.Mapping.Attributes.Column("rating",Type = typeof(int))]
        public MovieRating? Rating { get; set; }

        public Movie()
        {

        }

        public Movie(string title, string director, string mainActor, string movieMaker, int year, MovieRating rating)
        {
            Title = title;
            Director = director;
            MainActor = mainActor;
            MovieMaker = movieMaker;
            Year = year;
            Rating = rating;
        }

        public static void AssertEquals(Movie expectedMovie, Movie actualMovie)
        {
            Assert.AreEqual(expectedMovie.MainActor, actualMovie.MainActor);
            Assert.AreEqual(expectedMovie.MovieMaker, actualMovie.MovieMaker);
            Assert.AreEqual(expectedMovie.Title, actualMovie.Title);
            Assert.AreEqual(expectedMovie.ExampleSet, actualMovie.ExampleSet);
            Assert.AreEqual(expectedMovie.Director, actualMovie.Director);
            Assert.AreEqual(expectedMovie.Year, actualMovie.Year);
            Assert.AreEqual(expectedMovie.Rating, actualMovie.Rating);
        }

        public static void AssertListContains(List<Movie> expectedMovies, Movie actualMovie)
        {
            Assert.IsTrue(ListContains(expectedMovies, actualMovie));
        }

        public static bool ListContains(List<Movie> expectedMovies, Movie actualMovie)
        {
            foreach (var expectedMovie in expectedMovies)
            {
                try
                {
                    AssertEquals(actualMovie, expectedMovie);
                    return true;
                }
                catch (AssertionException) { }
            }
            return false;
        }

        public static Movie GetRandomMovie()
        {
            Movie movie = new Movie
            {
                Title = "SomeMovieTitle_" + Randomm.RandomAlphaNum(10),
                Director = "SomeMovieDirector_" + Randomm.RandomAlphaNum(10),
                MainActor = "SomeMainActor_" + Randomm.RandomAlphaNum(10),
                MovieMaker = "SomeFilmMaker_" + Randomm.RandomAlphaNum(10),
                Year = Randomm.RandomInt(),
                Rating = (MovieRating)(Randomm.RandomInt() % ((int)MovieRating.C))
            };
            return movie;
        }

        public static List<Movie> GetDefaultMovieList()
        {
            List<Movie> movieList = new List<Movie>();
            movieList.Add(new Movie("title3", "actor1", "director3", "maker3", 1988, MovieRating.A));
            movieList.Add(new Movie("title5", "actor1", "director4", "maker5", 1998, MovieRating.B));
            movieList.Add(new Movie("title1", "actor1", "director1", "maker1", 2008, MovieRating.C));
            movieList.Add(new Movie("title2", "actor2", "director2", "maker2", 2018, MovieRating.A));
            movieList.Add(new Movie("title4", "actor2", "director4", "maker4", 1888, MovieRating.B));
            return movieList;
        }

        public static void DisplayMovies(IEnumerable<Movie> result)
        {
            foreach (Movie resMovie in result)
            {
                Trace.TraceInformation("Movie={0} Director={1} MainActor={2}, Year={3}, Rating={4}",
                                  resMovie.Title, resMovie.Director, resMovie.MainActor, resMovie.Year, resMovie.Rating);
            }
        }


    }
}

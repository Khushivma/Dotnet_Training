using AssessmentQue02.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;



namespace YourNamespace.Repositories // Adjust the namespace as per your project
    {
        public class MoviesRepository
        {
            private readonly MoviesDbContext _context;

            public MoviesRepository()
            {
                _context = new MoviesDbContext(); // Create a new instance of the database context
            }

            // Method to get all movies
            public IEnumerable<Movie> GetAllMovies()
            {
                return _context.Movies.ToList();
            }

            // Method to get a movie by ID
            public Movie GetMovieById(int id)
            {
                return _context.Movies.Find(id);
            }

            // Method to add a new movie
            public void AddMovie(Movie movie)
            {
                _context.Movies.Add(movie);
                _context.SaveChanges();
            }

            // Method to update an existing movie
            public void UpdateMovie(Movie movie)
            {
                _context.Entry(movie).State = EntityState.Modified;
                _context.SaveChanges();
            }

            // Method to delete a movie by ID
            public void DeleteMovie(int id)
            {
                var movie = _context.Movies.Find(id);
                if (movie != null)
                {
                    _context.Movies.Remove(movie);
                    _context.SaveChanges();
                }
            }
        }
    }

}
}
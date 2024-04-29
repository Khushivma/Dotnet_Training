using AssessmentQue02.Models;
using System.Linq;
using System.Web.Mvc;
using AssessmentQue02.Models;
using YourNamespace.Repositories; // If using MoviesRepository

namespace AssessmentQue02.Controllers

{
    public class MoviesController : Controller
    {
        private readonly MoviesRepository _repository; // If using MoviesRepository

        // Constructor with MoviesRepository injection
        public MoviesController()
        {
            _repository = new MoviesRepository(); // Initialize the repository
        }

        // Action method to display all movies
        public ActionResult Index()
        {
            var movies = _repository.GetAllMovies(); // If using MoviesRepository
            // var movies = _context.Movies.ToList(); // If directly accessing MoviesDbContext
            return View(movies);
        }

        // Action method to display details of a specific movie
        public ActionResult Details(int id)
        {
            var movie = _repository.GetMovieById(id); // If using MoviesRepository
            // var movie = _context.Movies.Find(id); // If directly accessing MoviesDbContext
            return View(movie);
        }

        // Action method to display the form for creating a new movie
        public ActionResult Create()
        {
            return View();
        }

        // Action method to handle the HTTP POST request for creating a new movie
        [HttpPost]
        public ActionResult Create(Movie movie)
        {
            if (ModelState.IsValid)
            {
                _repository.AddMovie(movie); // If using MoviesRepository
                // _context.Movies.Add(movie); // If directly accessing MoviesDbContext
                return RedirectToAction("Index");
            }
            return View(movie);
        }

        // Action method to display the form for editing a movie
        public ActionResult Edit(int id)
        {
            var movie = _repository.GetMovieById(id); // If using MoviesRepository
            // var movie = _context.Movies.Find(id); // If directly accessing MoviesDbContext
            return View(movie);
        }

        // Action method to handle the HTTP POST request for editing a movie
        [HttpPost]
        public ActionResult Edit(Movie movie)
        {
            if (ModelState.IsValid)
            {
                _repository.UpdateMovie(movie); // If using MoviesRepository
                // _context.Entry(movie).State = EntityState.Modified; // If directly accessing MoviesDbContext
                return RedirectToAction("Index");
            }
            return View(movie);
        }

        // Action method to display the form for deleting a movie
        public ActionResult Delete(int id)
        {
            var movie = _repository.GetMovieById(id); // If using MoviesRepository
            // var movie = _context.Movies.Find(id); // If directly accessing MoviesDbContext
            return View(movie);
        }

        // Action method to handle the HTTP POST request for deleting a movie
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            _repository.DeleteMovie(id); // If using MoviesRepository
            // var movie = _context.Movies.Find(id); _context.Movies.Remove(movie); // If directly accessing MoviesDbContext
            return RedirectToAction("Index");
        }
    }
}

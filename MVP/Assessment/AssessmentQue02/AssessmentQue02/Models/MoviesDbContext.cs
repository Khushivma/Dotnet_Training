using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;


namespace AssessmentQue02.Models
{
   public class MoviesDbContext : DbContext
   {
            public DbSet<Movie> Movies { get; set; }

        internal object Entry(Movie movie)
        {
            throw new NotImplementedException();
        }
    }
}
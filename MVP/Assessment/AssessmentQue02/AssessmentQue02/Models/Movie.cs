using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AssessmentQue02.Models
{
    public class Movie
    {
        public int Mid { get; set; }

            [Required]
            [StringLength(100)]
            public string Moviename { get; set; }

            [Required]
            [DataType(DataType.Date)]
            public DateTime DateofRelease { get; set; }
    
    }

}

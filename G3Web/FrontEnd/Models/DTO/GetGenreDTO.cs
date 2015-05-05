using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GamesTore.Models
{
    public class GetGenreDTO
    {
        public string URL { get; set; }
        public int Id { get; set; }
        [Required]
        [Display(Name = "Genre Name")]
        public string Name { get; set; }
    }
}
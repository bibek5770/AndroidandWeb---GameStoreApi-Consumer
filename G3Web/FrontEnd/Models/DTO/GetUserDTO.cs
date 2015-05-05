using FrontEnd.Models;
using GamesTore.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Routing;

namespace GamesTore.Models
{
    public class GetUserDTO
    {
        public string URL { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required]
        [Display(Name = "Email Address")]
        [EmailAddress]
        public string Email { get; set; }
        public Roles role { get; set; }    
    }
   
}
using FrontEnd.Models.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontEnd.Models.DTO
{
    public class PostUserDTO
    {
     
        [Key]
        public int Id { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "Please enter a first name.")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Please enter a last name.")]
        public string LastName { get; set; }

        
        [Required(ErrorMessage = "Please enter a password")]
        [DataType(DataType.Password)]
        public string Password{ get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Please enter a valid email.")]
        [EmailAddress]
        public string Email { get; set; }


        public Roles Role { get; set; }
    }
   
}


  
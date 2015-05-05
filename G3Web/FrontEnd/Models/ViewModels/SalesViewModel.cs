using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontEnd.Models.ViewModels
{
   public class SalesViewModel
    {
        public string URL { get; set; }
        public DateTime SalesDate { get; set; }
        public decimal Total { get; set; }
        [Display(Name = "Employee Name")]
        public string EmployeeId { get; set; }

        [Display(Name = "User Sold To")]
        public string user { get; set; }
    }
}

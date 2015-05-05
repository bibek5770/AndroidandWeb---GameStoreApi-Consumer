using GamesTore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontEnd.Models.DTO
{
    public class GetSalesDTO
    {

        public string URL { get; set; }
        public int EmployeeId { get; set; }
        public DateTime SalesDate { get; set; }
        public decimal Total { get; set; }
        public GetCartDTO Cart { get; set; }
        public GetUserDTO User { get; set; }
    }
}


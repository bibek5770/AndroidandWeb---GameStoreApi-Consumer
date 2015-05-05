using GamesTore.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FrontEnd.Models.DTO
{

    public class GetCartDTO 
    {

        public string URL { get; set; }
        public int Id { get; set; }
        public bool CheckoutReady { get; set; }
        public int User_Id { get; set; }
        public List<Tuple<GetGameDTO, int>> Games { get; set; }

    }
}


using GamesTore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontEnd.Models.DTO
{
    class CreateSaleDTO
    {
        public int User_Id { get; set; }
        public List<Tuple<GetGameDTO, int>> Games { get; set; }
    }
}

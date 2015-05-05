using GamesTore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GamesTore.Models
{
    public class GetGameDTO
    {
        public string URL { get; set; }
        public int Id { get; set; }
        public string GameName { get; set; }
        public DateTime ReleaseDate { get; set; }
        public decimal Price { get; set; }
        public int InventoryStock { get; set; }
        public virtual List<GetGenreDTO> Genres { get; set; }
        public virtual List<GetTagDTO> Tags { get; set; }

    }
}
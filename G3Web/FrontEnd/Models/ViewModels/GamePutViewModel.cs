using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FrontEnd.Models.ViewModels
{
  public class GamePutViewModel
    {

        public string GameName { get; set; }
        public string URL { get; set; }
        public DateTime ReleaseDate { get; set; }
        public decimal Price { get; set; }
        public int InventoryStock { get; set; }
        public IEnumerable<SelectListItem> GameGenres { get; set; }
        public IEnumerable<SelectListItem> GameTags { get; set; }
        public IEnumerable<SelectListItem> Genres { get; set; }
        public IEnumerable<SelectListItem> Tags { get; set; }
        public List<string> SelectGameTags { get; set; }
        public List<string> SelectGameGenres { get; set; }
    }
}

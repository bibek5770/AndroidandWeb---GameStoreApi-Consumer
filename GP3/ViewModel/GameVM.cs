using System;
using System.Collections.Generic;

namespace GP3
{
	public class Game
	{
		public string URL { get; set; }
		public string GameName { get; set; }
		public DateTime ReleaseDate { get; set; }
		public Decimal Price { get; set; }
		public int InventoryStock { get; set; }
		public List<GenreVM> Genres { get; set; }
		public List<TagVM> Tags { get; set; }
	}

	public class GameVM
	{
		public string URL { get; set; }
		public string GameName { get; set; }
		public string ReleaseDate { get; set; }
		public string Price { get; set; }
		public int InventoryStock { get; set; }
		public string Genres { get; set; }
		public string Tags { get; set; }
	}

	public class GenreVM
	{
		public string URL { get; set; }
		public string Name { get; set; }

		public override string ToString ()
		{
			return string.Format ("{0}", Name);
		}
	}

	public class TagVM
	{
		public string URL { get; set; }
		public string Name { get; set; }

		public override string ToString ()
		{
			return string.Format ("{0}", Name);
		}
	}

	public class IconVm
	{
		public string MediaUrl { get; set; }
	}

	public class GetGameDTO
	{
		public string URL { get; set; }
		public string GameName { get; set; }
		public DateTime ReleaseDate { get; set; }
		public decimal Price { get; set; }
		public int InventoryStock { get; set; }
		public virtual ICollection<GetGenreDTO> Genres { get; set; }
		public virtual ICollection<GetTagDTO> Tags { get; set; }
	}

	public class SetGameDTO
	{
		public string GameName { get; set; }
		public DateTime ReleaseDate { get; set; }
		public decimal Price { get; set; }
		public int InventoryStock { get; set; }
		public virtual ICollection<CartVm.SetGenreDTO> Genres { get; set; }
		public virtual ICollection<CartVm.SetTagDTO> Tags { get; set; }
	}

	public class GetGenreDTO
	{
		public string URL { get; set; }
		public string Name { get; set; }
	}

	public class GetTagDTO
	{
		public string URL { get; set; }
		public string Name { get; set; }
	}
}
using System;
using System.Collections.Generic;

namespace GP3
{
	public class CartVm
	{
		public class SetCartDTO
		{
			public int User_Id { get; set; }
			public List<Tuple<SetGameDTO, int>> Games { get; set; }
		}

		public class SetGenreDTO
		{
			public string Name { get; set; }
		}

		public class SetTagDTO
		{
			public string Name { get; set; }
		}
	}
}
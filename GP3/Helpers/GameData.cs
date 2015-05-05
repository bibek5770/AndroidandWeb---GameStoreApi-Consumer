using System;
using System.Collections.Generic;
using RestSharp;
using System.Net;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace GP3
{
	public static class GameData
	{
		public static List<GameVM> GetGameData ()
		{
			List<Game> TempGames = new List<Game> ();
			List<GameVM> FinalGames = new List<GameVM>();

			const string url = Constants.ApiURL;
			var client = new RestClient (url);
			var request = APIGateway.CreateRequest("/games", Method.GET);
			var response = client.Execute (request);

			if (response.StatusCode == HttpStatusCode.OK) {
				TempGames = JsonConvert.DeserializeObject<List<Game>>(response.Content);
			}

			foreach(Game TempGame in TempGames){
				GameVM FinalGame = new GameVM ();
				FinalGame.URL = TempGame.URL;
				FinalGame.GameName = TempGame.GameName;
				FinalGame.Price = TempGame.Price.ToString();
				FinalGame.ReleaseDate = TempGame.ReleaseDate.Date.ToString();
				FinalGame.InventoryStock = TempGame.InventoryStock;

				var genrelist = "";
				var taglist = "";

				foreach(GenreVM eachgenre in TempGame.Genres){
					genrelist += eachgenre + ", ";	
				}


				foreach(TagVM eachtag in TempGame.Tags){
					taglist += eachtag + ", ";	
				}
					
				if(taglist.Length > 2){
					FinalGame.Tags = taglist.Substring(0, taglist.Length - 2);
				}

				if(genrelist.Length > 2){
					FinalGame.Genres = genrelist.Substring(0, genrelist.Length - 2);
				}

				//Add to FinalGame
				FinalGames.Add(FinalGame);
			}
			return FinalGames;
		}

//		public static Boolean PostGameData ()
//		{
//			List<Game> TempGames = new List<Game> ();
//			List<GameVM> FinalGames = new List<GameVM> ();
//		}
//
//		public static Boolean PutGameData ()
//		{
//			List<Game> TempGames = new List<Game> ();
//			List<GameVM> FinalGames = new List<GameVM> ();
//		}
	}
}
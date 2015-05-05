using GamesTore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Collections;
using System.Threading.Tasks;
using RestSharp;
using Newtonsoft.Json;
using System.Threading;
using System.Web.Mvc;
using FrontEnd.Models.ViewModels;

namespace FrontEnd.Controllers
{
    public class GamesController : BaseController
    {
        public async Task<ActionResult> GetGames(int? id)
        {

            var gamesRequest = CreateRequest("API/Games/", Method.GET);

            var result = await Client.ExecuteTaskAsync(gamesRequest);
            {
                if (result.StatusCode == HttpStatusCode.OK)
                {
                    var deserializedGames = JsonConvert.DeserializeObject<List<GetGameDTO>>(result.Content);
                    TempData["Title"] = "Get Games";
                    TempData["msg"] = "Request Successful";
                    return View(deserializedGames);
                }
                else
                {
                    TempData["Title"] = "Get Games";
                    TempData["msg"] = result.StatusCode;
                    return RedirectToAction("Index", "Account");
                }
            }
        }
        public async Task<ActionResult> Details(string URL)
        {
            try
            {
                if (!String.IsNullOrEmpty(URL))
                {
                    URL = URL.Substring(39);
                    var gamesRequest = CreateRequest("API/Games/{URL}", Method.GET);
                    gamesRequest.AddParameter("Id", Int16.Parse(URL));
                    var result = await Client.ExecuteTaskAsync(gamesRequest);
                    {
                        if (result.StatusCode == HttpStatusCode.OK)
                        {
                            TempData["Title"] = " Detail";
                            TempData["msg"] = "Request Successfull";
                            var deserializedGames = JsonConvert.DeserializeObject<GetGameDTO>(result.Content);
                            return View(deserializedGames);
                        }
                        else
                        {
                            TempData["Title"] = "Game Detail";
                            TempData["msg"] = "Error!!" + result.StatusCode;
                            return RedirectToAction("GetGames");
                        }
                    }
                }
                else
                {
                    TempData["Title"] = "Game Detail";
                    TempData["msg"] = "Error!! Invalid Request";
                    return RedirectToAction("GetGames");
                }

            }
            catch
            {
                TempData["Title"] = "Game Detail";
                TempData["msg"] = "Error!!Invalid Request";
                return RedirectToAction("GetGames");
            }
        }


        [System.Web.Mvc.HttpGet]
        public async Task<ActionResult> CreateGame()
        {
            if (!HttpContext.User.IsInRole("Admin"))
            {
                TempData["Title"] = "Log In ";
                TempData["msg"] = "Please log in as appopriate User";
                return RedirectToAction("Login", "Account");
            }
            try
            {
                GameViewModel Game = new GameViewModel();
                List<SelectListItem> genres = new List<SelectListItem>();
                List<SelectListItem> tags = new List<SelectListItem>();
                //for listing the genres//
                var genreRequest = CreateRequest("API/Genres/", Method.GET);
                var genreResult = await Client.ExecuteTaskAsync(genreRequest);
                {
                    var deserializedGenre = JsonConvert.DeserializeObject<List<GetGenreDTO>>(genreResult.Content);
                    foreach (var genre in deserializedGenre)
                    {
                        genres.Add(new SelectListItem { Text = genre.Name, Value = genre.Name.ToString() });
                    }
                    Game.Genres = genres;
                }
                //for listing the tags//
                var tagsRequest = CreateRequest("API/Tags/", Method.GET);
                var tagsResult = await Client.ExecuteTaskAsync(tagsRequest);
                {
                    var deserializedTags = JsonConvert.DeserializeObject<List<GetTagDTO>>(tagsResult.Content);
                    foreach (var tag in deserializedTags)
                    {
                        tags.Add(new SelectListItem { Text = tag.Name, Value = tag.Name.ToString() });
                    }
                    Game.Tags = tags;
                }

                return View(Game);
            }
            catch
            {
                TempData["Title"] = " Create User";
                TempData["msg"] = "!!Internal Error!!";
                return RedirectToAction("GetGames");
            }
        }


        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateGame(GameViewModel game)
        {
            if (!HttpContext.User.IsInRole("Admin"))
            {
                TempData["Title"] = "Log In ";
                TempData["msg"] = "Please log in as appopriate User";
                return RedirectToAction("Login", "Account");
            }
            try
            {
                if (ModelState.IsValid)
                {
                    GetGameDTO NewGame = new GetGameDTO();
                    NewGame.GameName = game.GameName;
                    NewGame.InventoryStock = game.InventoryStock;
                    NewGame.Price = game.Price;
                    NewGame.ReleaseDate = game.ReleaseDate;
                    NewGame.Tags = new List<GetTagDTO>();
                    NewGame.Genres = new List<GetGenreDTO>();
                    foreach (var tag in game.SelectGameTags)
                    {
                        NewGame.Tags.Add(new GetTagDTO { Name = tag });
                    }

                    foreach (var genre in game.SelectGameGenres)
                    {
                        NewGame.Genres.Add(new GetGenreDTO { Name = genre });
                    }
                    var request = CreateRequest("Api/Games", Method.POST);
                    request.AddBody(NewGame);
                    var result = await Client.ExecuteTaskAsync(request);
                    {
                        if (result.StatusCode == HttpStatusCode.Created)
                        {
                            TempData["Title"] = "Create Game";
                            TempData["msg"] = "Game Created Successfull";
                            return RedirectToAction("GetGames");
                        }
                        else
                        {
                            TempData["Title"] = "Create Game";
                            TempData["msg"] = "Error!! " + result.StatusCode;
                            return RedirectToAction("CreateGame");
                        }
                    }
                }
                else
                {
                    TempData["Title"] = " Create Game";
                    TempData["msg"] = "Error!!ModelState Not Valid";
                    return RedirectToAction("CreateGame");
                }

            }
            catch
            {
                TempData["Title"] = " Create Game";
                TempData["msg"] = "Games or Tags Cannot be Null";
                return RedirectToAction("CreateGame");
            }
        }


        [System.Web.Mvc.HttpGet]
        public async Task<ActionResult> EditGame(string URL)
        {
            if (!HttpContext.User.IsInRole("Admin"))
            {
                TempData["Title"] = "Log In ";
                TempData["msg"] = "Please log in as appopriate User";
                return RedirectToAction("Login", "Account");
            }
            try
            {
                if (!String.IsNullOrEmpty(URL))
                {

                    GamePutViewModel Game = new GamePutViewModel();
                    List<SelectListItem> gameGenres = new List<SelectListItem>();
                    List<SelectListItem> gameTags = new List<SelectListItem>();
                    //get genres
                    List<SelectListItem> genres = new List<SelectListItem>();
                    var genreRequest = CreateRequest("API/Genres/", Method.GET);
                    var genreResult = await Client.ExecuteTaskAsync(genreRequest);
                    {
                        var deserializedGenre = JsonConvert.DeserializeObject<List<GetGenreDTO>>(genreResult.Content);
                        foreach (var genre in deserializedGenre)
                        {
                            genres.Add(new SelectListItem { Text = genre.Name, Value = genre.Name.ToString() });
                        }
                    }
                    //get tags
                    List<SelectListItem> tags = new List<SelectListItem>();
                    var tagsRequest = CreateRequest("API/Tags/", Method.GET);
                    var tagsResult = await Client.ExecuteTaskAsync(tagsRequest);
                    {
                        var deserializedTags = JsonConvert.DeserializeObject<List<GetTagDTO>>(tagsResult.Content.ToString());
                        foreach (var tag in deserializedTags)
                        {
                            tags.Add(new SelectListItem { Text = tag.Name, Value = tag.Name.ToString() });
                        }
                    }
                    //////get game
                    var requestUrl = URL.Substring(39);
                    var rGame = CreateRequest(requestUrl, Method.GET);
                    var gameResult = await Client.ExecuteTaskAsync(rGame);
                    {
                        var deserializedGameResult = JsonConvert.DeserializeObject<GetGameDTO>(gameResult.Content);
                        foreach (var genre in deserializedGameResult.Genres)
                        {
                            gameGenres.Add(new SelectListItem { Text = genre.Name, Value = genre.Name.ToString() });
                        }

                        foreach (var tag in deserializedGameResult.Tags)
                        {
                            gameTags.Add(new SelectListItem { Text = tag.Name, Value = tag.Name.ToString() });
                        }
                        Game.URL = deserializedGameResult.URL;
                        Game.GameName = deserializedGameResult.GameName;
                        Game.ReleaseDate = deserializedGameResult.ReleaseDate;
                        Game.Price = deserializedGameResult.Price;
                        Game.InventoryStock = deserializedGameResult.InventoryStock;
                    }
                    Game.GameGenres = gameGenres;
                    Game.GameTags = gameTags;
                    Game.Tags = tags;
                    Game.Genres = genres;
                    return View(Game);
                }
                else
                {
                    TempData["Title"] = "Edit Game";
                    TempData["msg"] = "Bad Request";
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                TempData["Title"] = "Edit Game";
                TempData["msg"] = "Internal Error!!Please Try Again";
                return RedirectToAction("Index");
            }
        }

        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditGame(GamePutViewModel Game)
        {
            if (!HttpContext.User.IsInRole("Admin"))
            {
                TempData["Title"] = "Log In ";
                TempData["msg"] = "Please log in as appopriate User";
                return RedirectToAction("Login", "Account");
            }
            try
            {
                if (ModelState.IsValid)
                {
                    GetGameDTO newGame = new GetGameDTO();
                    newGame.URL = Game.URL;
                    newGame.GameName = Game.GameName;
                    newGame.InventoryStock = Game.InventoryStock;
                    newGame.Price = Game.Price;
                    newGame.ReleaseDate = Game.ReleaseDate;
                    newGame.Tags = new List<GetTagDTO>();
                    newGame.Genres = new List<GetGenreDTO>();
                    //List<GetTagDTO> tagList = new List<GetTagDTO>();
                    //List<GetGenreDTO> genreList = new List<GetGenreDTO>();

                    foreach (var tag in Game.SelectGameTags)
                    {
                        newGame.Tags.Add(new GetTagDTO { Name = tag });
                    }

                    foreach (var genre in Game.SelectGameGenres)
                    {
                        newGame.Genres.Add(new GetGenreDTO { Name = genre.ToString() });
                    }

                    //string newUrl = Game.URL.Substring(Game.URL.Length - 11);
                    string newUrl = Game.URL.Substring(39);

                    var request = CreateRequest(newUrl, Method.PUT);
                    request.AddBody(newGame);
                    var result = await Client.ExecuteTaskAsync(request);
                    if (result.StatusCode == HttpStatusCode.OK)
                    {
                        TempData["Title"] = "Edit Game";
                        TempData["msg"] = "Game Successfully Updated";
                        return RedirectToAction("GetGames");
                    }
                    else
                    {
                       
                        TempData["Title"] = "Edit Game";
                        TempData["msg"] = "Error!!"+result.StatusCode;
                        return RedirectToAction("GetGames");
                    }
                }
                else
                {
                    TempData["Title"] = "Edit Game";
                    TempData["msg"] = "Error!!InvalidModelState";
                    return RedirectToAction("GetGames");
                }
            }
            catch
            {
                TempData["Title"] = "Edit Game";
                TempData["msg"] = "Pleas choose at least one tag and one Genre For the Game";
                return RedirectToAction("GetGames");
            }
        }


        public async Task<ActionResult> DeleteGame(string URL)
        {
            if (!HttpContext.User.IsInRole("Admin"))
            {
                TempData["Title"] = "Log In ";
                TempData["msg"] = "Please log in as appopriate User";
                return RedirectToAction("Login", "Account");
            }
            try
            {
                if (!String.IsNullOrEmpty(URL))
                {
                    var requestUrl = URL.Substring(39);
                    var rGame = CreateRequest(requestUrl, Method.DELETE);
                    var gameResult = await Client.ExecuteTaskAsync(rGame);
                    if (gameResult.StatusCode == HttpStatusCode.OK)
                    {
                        TempData["Title"] = "Delete User";
                        TempData["msg"] = " Successfully Deleted";
                        return RedirectToAction("GetGames");
                    }
                    else
                    {
                        TempData["Title"] = "Delete User";
                        TempData["msg"] = "Error!!" + gameResult.StatusCode;
                        return RedirectToAction("GetGames");
                    }                    
                }
                else
                {
                    TempData["Title"] = "Delete User";
                    TempData["msg"] = "Error!!Invalid Request";
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                TempData["Title"] = "Delete User";
                TempData["msg"] = "Error!!Invalid Request";
                return RedirectToAction("Index");
            }
        }

        public async Task<ActionResult> Search(string searchString)
        {
            var gamesRequest = CreateRequest("API/Games/", Method.GET);
            var result = await Client.ExecuteTaskAsync(gamesRequest);
            {
                if (result.StatusCode == HttpStatusCode.OK)
                {
                    var deserializedGames = JsonConvert.DeserializeObject<List<GetGameDTO>>(result.Content);
                    TempData["Title"] = "Search User ";
                    TempData["msg"] = " User Found";
                    List<GetGameDTO> gameFound = deserializedGames.FindAll(x => x.GameName.ToLower().Contains(searchString.ToLower()));
                    return View(gameFound);
                }
                else
                {
                    TempData["Title"] = "Search User ";
                    TempData["msg"] = " User Not Found Found";
                    return RedirectToAction("Index");

                }
            }
        }

        //public async Task<ActionResult> AddToCart(string URL)
        //{
        //    if (!String.IsNullOrEmpty(URL))
        //    {
        //        var requestUrl = URL.Substring(22);

        //        //var requestUrl = URL.Substring(39);
        //        var rGame = CreateRequest(requestUrl, Method.GET);
        //        var gameResult = await Client.ExecuteTaskAsync(rGame);
        //        var deserializedGameResult = JsonConvert.DeserializeObject<GetGameDTO>(gameResult.Content);
        //      //  List<GetGameDTO> games = new List<GetGameDTO>();
        //        string gameId = "1";//URL.Substring(49);
        //       // games.Add(deserializedGameResult);
        //        var addToCarRequest = CreateRequest("api/carts/1", Method.PUT);
        //        addToCarRequest.AddBody(deserializedGameResult.GameName.ToString());
        //        var result = await Client.ExecuteTaskAsync(addToCarRequest);
        //    }
        //    return RedirectToAction("GetGames");
        //}

    }
}

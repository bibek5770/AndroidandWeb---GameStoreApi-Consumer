using GamesTore.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FrontEnd.Controllers
{
    public class GenreController : BaseController
    {
        // GET: Genre
        public async Task<ActionResult> Index()
        {
            var request = CreateRequest("Api/Genres/", Method.GET);
            var result = await Client.ExecuteTaskAsync(request);
            {
                if (result.StatusCode == HttpStatusCode.OK)
                {
                    var deserializedTags = JsonConvert.DeserializeObject<List<GetGenreDTO>>(result.Content);
                    if (TempData["msg"] == null)
                    {
                        TempData["Title"] = "Get All Genre";
                        TempData["msg"] = "Request Successfull";
                    }
                    return View(deserializedTags);
                }
                else
                {
                    TempData["Title"] = "Get All Genre";
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
                    string newUrl = URL.Substring(39);
                    var request = CreateRequest(newUrl, Method.GET);
                    var response = await Client.ExecuteTaskAsync(request);
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            TempData["Title"] = "Genre Detail";
                            TempData["msg"] = "Request Successfull";
                            var deserializedGenres = JsonConvert.DeserializeObject<GetGenreDTO>(response.Content);
                            return View(deserializedGenres);
                        }
                        else
                        {
                            TempData["Title"] = "Genre Detail";
                            TempData["msg"] = "Error!!Invalid Request";
                            return RedirectToAction("Index");
                        }
                    }
                }
                else
                {
                    TempData["Title"] = "Genre Detail";
                    TempData["msg"] = "Error!!Invalid Request";
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                TempData["Title"] = "Genre Detail";
                TempData["msg"] = "Error!!Invalid Request";
                return RedirectToAction("Index");
            }
        }



        // GET: Users/Create
        public ActionResult Create()
        {
            if (!HttpContext.User.IsInRole("Admin"))
            {
                TempData["Title"] = "Log In ";
                TempData["msg"] = "Please log in as appopriate User";
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        public async Task<ActionResult> Create(GetGenreDTO genre)
        {
            if (!HttpContext.User.IsInRole("Admin"))
            {
                TempData["Title"] = "Log In ";
                TempData["msg"] = "Please log in as appopriate User";
                return RedirectToAction("Login", "Account");
            }
            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    var request = CreateRequest("Api/Genres/", Method.POST);
                    request.AddObject(genre);
                    var response = await Client.ExecuteTaskAsync(request);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        TempData["Title"] = "Create Genre";
                        TempData["msg"] = "Success!!Genre Successfully Created";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                          
                        TempData["Title"] = "Create Genre";
                        TempData["msg"] = "Error!! Invalid Model";
                        return RedirectToAction("Index");
                        
                    }

                }
                return RedirectToAction("Index");
            }
            catch
            {
                TempData["Title"] = "Create Genre";
                TempData["msg"] = "Error!!Invalid Request";
                return RedirectToAction("Index");
            }
        }
        [HttpGet]
        // GET: Users/Edit/5
        public async Task<ActionResult> Edit(string URL)
        {
            try
            {
                if (!String.IsNullOrEmpty(URL))
                {
                    if (!HttpContext.User.IsInRole("Admin"))
                    {
                        TempData["Title"] = "Log In ";
                        TempData["msg"] = "Please log in as appopriate User";
                        return RedirectToAction("Login", "Account");
                    }
                    string newUrl = URL.Substring(39);
                    var request = CreateRequest(newUrl, Method.GET);
                    var result = await Client.ExecuteTaskAsync(request);
                    {
                        var respose = JsonConvert.DeserializeObject<GetGenreDTO>(result.Content);
                        return View(respose);
                    }
                }
                else
                {
                    TempData["Title"] = "Edit Genre";
                    TempData["msg"] = "Error!!Request  Not Valid";
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                TempData["Title"] = "Edit Genre";
                TempData["msg"] = "Error!!Please try again.";
                return RedirectToAction("Index");
            }
        }

        // POST: Users/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(GetGenreDTO genre)
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
                    string newUrl = genre.URL.Substring(39);
                    var request = CreateRequest(newUrl, Method.PUT);
                    //genre.Id =  newUrl.Substring(10);
                    request.AddObject(genre);
                    var result = await Client.ExecuteTaskAsync(request);
                    if (result.StatusCode == HttpStatusCode.OK)
                    {
                        TempData["Title"] = "Edit Tag";
                        TempData["msg"] = "Request Successfull";
                        return RedirectToAction("Index");

                    }else
                    {
                        TempData["Title"] = "Edit ";
                        TempData["msg"] = "Error!! " + result.StatusCode;
                        return RedirectToAction("Index");
                    }
                }
                // TODO: Add update logic here
                else
                {
                    TempData["Title"] = "Edit Genre";
                    TempData["msg"] = "Error!!Request Model Not Valid";
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                TempData["Title"] = "Error with Edit Genre";
                TempData["msg"] = "Invalid Request";
                return RedirectToAction("Index");
            }
        }

        public ActionResult Delete(string URL)
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
                    string newUrl = URL.Substring(39);
                    var request = CreateRequest(newUrl, Method.DELETE);
                    var result = Client.Execute(request);
                    if (result.StatusCode == HttpStatusCode.OK)
                    {
                        TempData["Title"] = "Delete Genre";
                        TempData["msg"] = "Genre Successfully Deleted";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["Title"] = "Delete Genre";
                        TempData["msg"] = "Error!!" + result.StatusCode;
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    TempData["Title"] = "Delete Genre";
                    TempData["msg"] = "Error!!Invalid Request";
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                TempData["Title"] = "Delete Genre";
                TempData["msg"] = "Error!!Invalid Request";
                return RedirectToAction("Index");
            }
        }
    }
}
    
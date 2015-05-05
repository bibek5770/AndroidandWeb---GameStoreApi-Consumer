using FrontEnd.Models.DTO;
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
    public class UsersController : BaseController
    {
        // GET: Users
        public async Task<ActionResult> Index()
        {
            if (HttpContext.User.IsInRole("Admin"))
            {
                var request = CreateRequest("Api/Users", Method.GET);
                var result = await Client.ExecuteTaskAsync(request);
                {
                    if (result.StatusCode == HttpStatusCode.OK)
                    {
                        TempData["Title"] = "Get All Users";
                        TempData["msg"] = "Request Successfull";
                        var response = JsonConvert.DeserializeObject<List<GetUserDTO>>(result.Content);
                        return View(response);
                    }
                    {
                        TempData["Title"] = "Get All Users";
                        TempData["msg"] = result.StatusCode;
                        return RedirectToAction("Index", "Account");
                    }
                }
            }
            else
            {
                TempData["Title"] = "Log In ";
                TempData["msg"] = "Please log in as appopriate User";
                return RedirectToAction("Login", "Account");
            }
        }

        // GET: Users/Details/5
        public async Task<ActionResult> Details(string URL)
        {
            if (HttpContext.User.IsInRole("Admin"))
            {
                try
                {
                    if (!String.IsNullOrEmpty(URL))
                    {
                        string newUrl = URL.Substring(39);
                        var request = CreateRequest(newUrl, Method.GET);
                        var result = await Client.ExecuteTaskAsync(request);
                        if (result.StatusCode == HttpStatusCode.OK)
                        {
                            var response = JsonConvert.DeserializeObject<GetUserDTO>(result.Content);
                            TempData["Title"] = "User Detail";
                            TempData["msg"] = "Request Successfull";
                            return View(response);
                        }
                        else
                        {
                            TempData["Title"] = "User Detail";
                            TempData["msg"] = "Error!!Invalid Request";
                            return RedirectToAction("Index");
                        }
                    }
                    else
                    {
                        return RedirectToAction("Login", "Account");
                    }
                }
                catch
                {
                    TempData["Title"] = "User Detail";
                    TempData["msg"] = "Error!!Invalid Request";
                    return RedirectToAction("Index");
                }
            }
            else
            {
                TempData["Title"] = "Log In ";
                TempData["msg"] = "Please log in as appopriate User";
                return RedirectToAction("Login", "Account");
            }
        }



        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        public async Task<ActionResult> Create([Bind(Include = "FirstName,LastName,Email,Password,Role")] PostUserDTO user)
        {
            if (HttpContext.User.IsInRole("Admin"))
            {
                try
                {
                    // TODO: Add insert logic here
                    if (ModelState.IsValid)
                    {
                        var request = CreateRequest("Api/Users", Method.POST);
                        request.AddObject(user);
                        var response = await Client.ExecuteTaskAsync(request);
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            TempData["Title"] = "Create ";
                            TempData["msg"] = " Successfully Created";
                            return RedirectToAction("Index");

                        }
                        else
                        {
                            TempData["Title"] = "Create User";
                            TempData["msg"] = "Error!!" + response.StatusCode;
                            return RedirectToAction("Index");

                        }
                    }
                    else
                    {
                        TempData["Title"] = "Create User";
                        TempData["msg"] = "Error!! Invalid Model";
                        return RedirectToAction("Index");
                    }
                }
                catch
                {
                    TempData["Title"] = " Create";
                    TempData["msg"] = "Error!!Invalid Request";
                    return RedirectToAction("Index");
                }
            }
            else
            {
                TempData["Title"] = "Log In ";
                TempData["msg"] = "Please log in as appopriate User";
                return RedirectToAction("Login", "Account");
            }
        }
        [HttpGet]
        // GET: Users/Edit/5
        public async Task<ActionResult> Edit(string URL)
        {
            if (HttpContext.User.IsInRole("Admin"))
            {
                if (!String.IsNullOrEmpty(URL))
                {
                    string newUrl = URL.Substring(URL.Length - 11);
                    var request = CreateRequest(newUrl, Method.GET);
                    var result = await Client.ExecuteTaskAsync(request);
                    if (result.StatusCode == HttpStatusCode.OK)
                    {
                        var respose = JsonConvert.DeserializeObject<GetUserDTO>(result.Content.ToString());
                        return View(respose);
                    }
                    else
                    {
                        TempData["Title"] = "Edit ";
                        TempData["msg"] = "Error!!Invalid Request";
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    TempData["Title"] = "Edit User";
                    TempData["msg"] = "Error!!Invalid Request";
                    return RedirectToAction("Index");
                }
            }
            else
            {
                TempData["Title"] = "Log In ";
                TempData["msg"] = "Please log in as appopriate User";
                return RedirectToAction("Login", "Account");
            }
        }
        // POST: Users/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(GetUserDTO user)
        {
            if (HttpContext.User.IsInRole("Admin"))
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        string newUrl = user.URL.Substring(39);
                        var request = CreateRequest(newUrl, Method.PUT);
                        request.AddObject(user);
                        request.AddParameter("id", user.URL.Substring(49));
                        var result = await Client.ExecuteTaskAsync(request);
                        if (result.StatusCode == HttpStatusCode.OK)
                        {
                            TempData["Title"] = "Edit User";
                            TempData["msg"] = "Tag Successfully Updated";
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            TempData["Title"] = "Edit User";
                            TempData["msg"] = "Error!! " + result.StatusCode;
                            return RedirectToAction("Index");
                        }
                    }
                    // TODO: Add update logic here
                    else
                    {
                        TempData["Title"] = "Edit User";
                        TempData["msg"] = "Error!!Model State Not Valid";
                        return RedirectToAction("Index");
                    }
                }
                catch
                {
                    TempData["Title"] = "Error with Edit ";
                    TempData["msg"] = "Invalid Request";
                    return RedirectToAction("Index");
                }
            }
            else
            {
                TempData["Title"] = "Log In ";
                TempData["msg"] = "Please log in as appopriate User";
                return RedirectToAction("Login", "Account");
            }
        }

        public ActionResult Delete(string URL)
        {
            if (HttpContext.User.IsInRole("Admin"))
            {
                try
                {
                    if (!String.IsNullOrEmpty(URL))
                    {
                        string newUrl = URL.Substring(39);
                        var request = CreateRequest(newUrl, Method.DELETE);
                        var result = Client.Execute(request);
                        if (result.StatusCode == HttpStatusCode.OK)
                        {
                            TempData["Title"] = "Delete User";
                            TempData["msg"] = " Successfully Deleted";
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            TempData["Title"] = "Delete User";
                            TempData["msg"] = "Error!!" + result.StatusCode;
                            return RedirectToAction("Index");
                        }
                    }
                    else
                    {
                        TempData["Title"] = "Delete User ";
                        TempData["msg"] = "Error!!Invalid Request";
                        return RedirectToAction("Index");
                    }
                }
                catch
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            else
            {
                TempData["Title"] = "Log In ";
                TempData["msg"] = "Please log in as appopriate User";
                return RedirectToAction("Login", "Account");
            }

        }
    }
}

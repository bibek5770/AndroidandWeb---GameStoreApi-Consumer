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
    public class TagsController : BaseController
    {
        // GET: Tags
        public async Task<ActionResult> Index()
        
        {
            var request = CreateRequest("Api/Tags", Method.GET);
            var result = await Client.ExecuteTaskAsync(request);
            {
                if (result.StatusCode == HttpStatusCode.OK)
                {
                    var deserializedTags = JsonConvert.DeserializeObject<List<GetTagDTO>>(result.Content);
                    TempData["Title"] = "Get All Tags";
                    TempData["msg"] = "Request Successfull";
                    return View(deserializedTags);

                }
                else
                {
                    TempData["Title"]="Get All Tags";
                    TempData["msg"] = result.StatusCode;
                    return RedirectToAction("Index","Account");
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
                    var response = await Client.ExecuteTaskAsync<GetTagDTO>(request);
                    if (response.StatusCode == HttpStatusCode.OK) {
                        var deserializedTags = JsonConvert.DeserializeObject<GetTagDTO>(response.Content);
                        TempData["Title"] = "Tag Detail";
                        TempData["msg"] = "Request Successfull";
                        return View(deserializedTags);
                    }else{
                        TempData["Title"] = "Tag Detail";
                        TempData["msg"] = "Error!!"+response.StatusCode;
                        return RedirectToAction("Index");
                    }
                    
                }
                else
                {
                    TempData["Title"] = "Tag Detail";
                    TempData["msg"] = "Invalid Request";
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                TempData["Title"] = "Tag Detail";
                TempData["msg"] = "Error!!Invalid Request";
                return RedirectToAction("Index");
            }
        }



        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        public async Task<ActionResult> Create( GetTagDTO tag)
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
                    var request = CreateRequest("Api/Tags/", Method.POST);
                    request.AddObject(tag);
                    var response = await Client.ExecuteTaskAsync(request);
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            TempData["Title"] = "Create Tag";
                            TempData["msg"] = "Tag Successfully Created";
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            TempData["Title"] = "Create Tag";
                            TempData["msg"] = "Error!!" + response.StatusCode;
                            return RedirectToAction("Index");

                        }
                    }
                }

                    else{
                        TempData["Title"] = "Create Tag";
                        TempData["msg"] = "Error!! Invalid Model";
                        return RedirectToAction("Index");
                    }                
            }catch
            {
                TempData["Title"] = "Create Tag Detail";
                TempData["msg"] = "Invalid Request";
                return RedirectToAction("Index");
            }
        }
        [HttpGet]
        // GET: Users/Edit/5
        public async Task<ActionResult> Edit(string URL)
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
                    var request = CreateRequest(newUrl, Method.GET);
                    var result = await Client.ExecuteTaskAsync(request);
                    {
                        if (result.StatusCode == HttpStatusCode.OK)
                        {
                            var respose = JsonConvert.DeserializeObject<GetTagDTO>(result.Content.ToString());
                            TempData["Title"] = "Edit Tag";
                            TempData["msg"] = "Tag Successfully Updated";
                            return View(respose);
                        }
                        else
                        {
                            TempData["Title"] = "Edit Tag";
                            TempData["msg"] = "Error!! " + result.StatusCode;
                            return RedirectToAction("Index");
                        }
                    }
                }
                else
                {
                    TempData["Title"] = "Edit Tag";
                    TempData["msg"] = "Error!!Invalid Request";
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                TempData["Title"] = "Error with Edit Tag";
                TempData["msg"] = "Invalid Request";
                return RedirectToAction("Index");
            }
        }

        // POST: Users/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(GetTagDTO tag)
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
                    string newUrl = tag.URL.Substring(39);
                    var request = CreateRequest(newUrl, Method.PUT);
                    request.AddObject(tag);
                    var result = await Client.ExecuteTaskAsync(request);
                    {
                        if (result.StatusCode == HttpStatusCode.OK)
                        {
                            TempData["Title"] = "Edit Tag";
                            TempData["msg"] = "Tag is Successfully Edited";
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            TempData["Title"] = "Edit Tag";
                            TempData["msg"] = "Error!! " + result.StatusCode;
                            return RedirectToAction("Index");
                        }
                    }
                   
                }
                else
                {
                    TempData["Title"] = "Error while editing Tag ";
                    TempData["msg"] = "Request Model is Not Valid";
                    return RedirectToAction("Login", "Account");
                }
            }
            catch
            {
                TempData["Title"] = "Edit Tag";
                TempData["msg"] = "Error!!Invalid Request";
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
                        TempData["Title"] = "Delete Tag";
                        TempData["msg"] = "Tag Successfully Deleted";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["Title"] = "Delete Tag";
                        TempData["msg"] = "Error!!"+result.StatusCode;
                        return RedirectToAction("Index");
                    }
                
            }
                else{
                    TempData["Title"] = "Error with delete Tag ";
                    TempData["msg"] = "Invalid Request";
                return RedirectToAction("Index");
                }
            }
            catch
            {
                TempData["Title"] = "Error with Delete Tag";
                TempData["msg"] = "Invalid Request";
                return RedirectToAction("Index");
            }
        }
    }
}
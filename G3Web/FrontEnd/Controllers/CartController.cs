using FrontEnd.Models.DTO;
using FrontEnd.Models.ViewModels;
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
    public class CartController : BaseController
    {
        // GET: Cart
        public async Task<ActionResult> Index()
        {
            if (!HttpContext.User.IsInRole("Admin"))
            {
                TempData["Title"] = "Log In ";
                TempData["msg"] = "Please log in as appopriate User";
                return RedirectToAction("Login", "Account");
            }
            var request = CreateRequest("Api/Carts", Method.GET);
            var result = await Client.ExecuteTaskAsync(request);
            {
                if (result.StatusCode == HttpStatusCode.OK)
                {
                    var response = JsonConvert.DeserializeObject<List<GetCartDTO>>(result.Content.ToString());
                    if (TempData["msg"] == null)
                    {
                        TempData["Title"] = "Get All Carts";
                        TempData["msg"] = "Request Successfull";
                    }
                    return View(response);

                }
                else
                {
                    TempData["Title"] = "Get Carts";
                    TempData["msg"] = result.StatusCode;
                    return RedirectToAction("Index", "Account");
                }
            }
        }

        [HttpGet]
        public async Task<ActionResult> SearchByUserId(string id)
        {
            try
            {
                if (!String.IsNullOrEmpty(id))
                {
                    //string newUrl = URL.Substring(URL.Length - 1);
                    var request = CreateRequest("Api/Carts/{id}", Method.GET);
                    request.AddParameter("id", int.Parse(id));
                    var result = await Client.ExecuteTaskAsync(request);
                    {
                        if (result.StatusCode == HttpStatusCode.OK)
                        {
                            TempData["Title"] = " Search Cart By Id";
                            TempData["msg"] = "Request Successfull";
                            var response = JsonConvert.DeserializeObject<GetCartDTO>(result.Content);
                            return View(response);
                        }
                        else
                        {
                            TempData["Title"] = " Search Cart By Id";
                            TempData["msg"] = "Error!!Invalid Request";
                            return RedirectToAction("Index","Account");
                        }
                    }
                }
                else
                {
                    TempData["Title"] = "Get Cart By User Id";
                    TempData["msg"] = "Error!!Invalid Request";
                    return RedirectToAction("Index","Account");
                }
            }
            catch
            {
                TempData["Title"] = " Detail";
                TempData["msg"] = "Error!!Invalid Request";
                return RedirectToAction("Index","Account");
            }
        }

        [HttpGet]
        public async Task<ActionResult> Details(string URL)
        {
            try
            {
                if (!String.IsNullOrEmpty(URL))
                {
                    string id = URL.Substring(33);

                    var request = CreateRequest("Api/Carts", Method.GET);
                    string Url = "";
                    var result = await Client.ExecuteTaskAsync(request);
                    {
                        if (result.StatusCode == HttpStatusCode.OK) {
                        TempData["Title"] = " Detail";
                        TempData["msg"] = "Request Successfull";
                        var response = JsonConvert.DeserializeObject<List<GetCartDTO>>(result.Content);
                        foreach (GetCartDTO cart in response)
                        {
                            Url = cart.URL.Substring(33);
                            if (Url.Equals(id))
                            {
                                return View(cart);
                            }
                        }

                        TempData["Title"] = "Cart Detail";
                            TempData["msg"] = "Error!!Cart Not Found";
                            return RedirectToAction("Index");
                        }else{
                            TempData["Title"] = "Cart Detail";
                            TempData["msg"] = "Error!!Invalid Request";
                            return RedirectToAction("Index");
                        }
                    }
                }
                else
                {
                    TempData["Title"] = "Cart Detail";
                    TempData["msg"] = "Error!!Invalid Request";
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                TempData["Title"] = "Cart Detail";
                TempData["msg"] = "Error!!Invalid Request";
                return RedirectToAction("Index");           
            }


        }
    }
}
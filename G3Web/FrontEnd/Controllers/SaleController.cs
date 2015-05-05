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
    public class SaleController : BaseController
    {
        // GET: Sale
        public async Task<ActionResult> Index()
        {
            try
            {
                if (HttpContext.User.IsInRole("Admin"))
                {
                    var request = CreateRequest("Api/Sales", Method.GET);
                    var result = await Client.ExecuteTaskAsync(request);
                    if (result.StatusCode == HttpStatusCode.OK)
                    {
                        var response = JsonConvert.DeserializeObject<List<GetSalesDTO>>(result.Content);
                        List<GetGameDTO> games = new List<GetGameDTO>();
                        List<SalesViewModel> returnSales = new List<SalesViewModel>();
                        List<SalesViewModel> allsales = await Sales(response);
                        {
                            foreach (var sale in allsales)
                            {
                                returnSales.Add(new SalesViewModel
                                {
                                    URL = sale.URL,
                                    SalesDate = sale.SalesDate,
                                    Total = sale.Total,
                                    EmployeeId = sale.EmployeeId,
                                    user = sale.user
                                });
                            }
                        }
                        TempData["Title"] = "Get All Sales";
                        TempData["msg"] = "Request Successfull";
                        return View(returnSales);
                    }
                    else
                    {
                        TempData["Title"] = "Get All Sales";
                        TempData["msg"] = result.StatusCode;
                        return RedirectToAction("Index", "Account");
                    }
                }
                else if (HttpContext.User.IsInRole("Employee"))
                {
                    var request = CreateRequest("Api/Sales", Method.GET);
                    string UserId = GetUserId();
                    request.AddUrlSegment("empId", UserId);
                    request.AddUrlSegment("userid", UserId);

                    var result = await Client.ExecuteTaskAsync(request);
                    if (result.StatusCode == HttpStatusCode.OK)
                    {
                        var response = JsonConvert.DeserializeObject<List<GetSalesDTO>>(result.Content);
                        List<GetGameDTO> games = new List<GetGameDTO>();
                        List<SalesViewModel> returnSales = new List<SalesViewModel>();
                        List<SalesViewModel> allsales = await Sales(response);
                        {
                            foreach (var sale in allsales)
                            {
                                returnSales.Add(new SalesViewModel
                                {
                                    URL = sale.URL,
                                    SalesDate = sale.SalesDate,
                                    Total = sale.Total,
                                    EmployeeId = sale.EmployeeId,
                                    user = sale.user
                                });
                            }
                        }
                        return View(returnSales);
                    }
                    else
                    {
                        TempData["Title"] = "Get Sales";
                        TempData["msg"] = result.StatusCode;
                        return RedirectToAction("Index", "Account");
                    }

                }
                TempData["Title"] = "Get Sale";
                TempData["msg"] = "Please Log In";
                return RedirectToAction("login", "account");
            }
            catch
            {
                TempData["Title"] = "Get Sale";
                TempData["msg"] = "Internal Error";
                return RedirectToAction("Index", "Account");
            }
        }

        [HttpGet]
        public async Task<ActionResult> Create(string URL)
        {
            try
            {
                if (!String.IsNullOrEmpty(URL))
                {
                    string id = URL.Substring(33);

                    var request = CreateRequest("Api/Carts", Method.GET);
                    string Url = "";
                    //   bool CartFound = false;
                    var result = await Client.ExecuteTaskAsync(request);
                    {
                        if (result.StatusCode == HttpStatusCode.OK)
                        {
                            var response = JsonConvert.DeserializeObject<List<GetCartDTO>>(result.Content);
                            foreach (GetCartDTO cart in response)
                            {
                                Url = cart.URL.Substring(33);
                                if (Url.Equals(id))
                                {
                                    //  CartFound = true;
                                    CreateSaleDTO postSale = new CreateSaleDTO();
                                    postSale.Games = cart.Games;
                                    postSale.User_Id = cart.User_Id;
                                    request = CreateRequest("Api/Sales", Method.POST);
                                    request.AddBody(postSale);
                                    result = await Client.ExecuteTaskAsync(request);
                                    if (result.StatusCode == HttpStatusCode.OK)
                                    {
                                        TempData["Title"] = "Checkout User Cart ";
                                        TempData["msg"] = "user Cart Successfully Checkout Out";
                                        return RedirectToAction("Index");
                                    }
                                    else
                                    {
                                        TempData["Title"] = "Checkout User Cart ";
                                        TempData["msg"] = "Error!!Invalid Request. " + result.StatusCode;
                                        return RedirectToAction("Index");

                                    }
                                    // break;
                                }

                            }

                        }
                        else
                        {
                            TempData["Title"] = "Checkout User Cart ";
                            TempData["msg"] = "Error!!Invalid Request. " + result.StatusCode;
                            return RedirectToAction("Index");
                        }

                    }
                }
                else
                {
                    TempData["Title"] = "Checkout User Cart ";
                    TempData["msg"] = "Error!!Invalid Request";
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                TempData["Title"] = "Checkout User Cart ";
                TempData["msg"] = "Error!!Invalid Request";
                return RedirectToAction("Index");
            }
            TempData["Title"] = "Checkout User Cart ";
            TempData["msg"] = "Error!!Invalid Request";
            return RedirectToAction("Index");
        }


        //public async Task<ActionResult> FindById(String id)
        //{
        //    var request = CreateRequest("Api/sales/{id}", Method.GET);
        //    request.AddUrlSegment("id", URL.ToString());
        //    var result = await Client.ExecuteTaskAsync(request);
        //    {
        //        var response = JsonConvert.DeserializeObject<GetUserDTO>(result.Content.ToString());
        //    }
        //    return View("");
        //}
        [HttpGet]
        public async Task<string> ViewCart(string URL)
        {

            var request = CreateRequest("Api/Users/{id}", Method.GET);
            request.AddUrlSegment("id", URL.ToString());
            var result = await Client.ExecuteTaskAsync(request);
            {
                var response = JsonConvert.DeserializeObject<GetUserDTO>(result.Content.ToString());
                string name = response.FirstName + " " + response.LastName;
                return (name);
            }
        }



        public async Task<List<SalesViewModel>> Sales(List<GetSalesDTO> jsonResult)
        {
            List<SalesViewModel> sales = new List<SalesViewModel>();
            foreach (GetSalesDTO sale in jsonResult)
            {
                sales.Add(new SalesViewModel
                {
                    URL = sale.URL,
                    SalesDate = sale.SalesDate,
                    Total = sale.Total,
                    EmployeeId = sale.Cart.User_Id.ToString(),
                    user = await getEmployer(sale.Cart.User_Id)
                });
            }
            return sales;
        }
        public async Task<string> getEmployer(int id)
        {
            var request = CreateRequest("Api/Users/{id}", Method.GET);
            request.AddUrlSegment("id", id.ToString());
            var result = await Client.ExecuteTaskAsync(request);
            {
                var response = JsonConvert.DeserializeObject<GetUserDTO>(result.Content.ToString());
                string name = response.FirstName + " " + response.LastName;
                return (name);
            }
        }

    }
}
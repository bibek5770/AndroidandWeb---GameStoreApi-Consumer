using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using FrontEnd.Models;
using FrontEnd.Controllers;
using FrontEnd.UserRole;
using System.Threading.Tasks;
using GamesTore.Models;
using FrontEnd.Models.DTO;


namespace FrontEnd.Controllers
{
    public class AccountController : BaseController
    {

        public string msg = string.Empty;
        // GET: User
        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public ActionResult LogIn()
        {
            TempData["Title"] = "";
            TempData["msg"] = "";
            FormsAuthentication.SignOut();
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                    1,
                    "",
                    System.DateTime.Now,
                    System.DateTime.Now,
                    false,
                    "",
                    FormsAuthentication.FormsCookiePath
                    );
            FormsAuthentication.SetAuthCookie("", false);
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> LogIn(string email, string password)
        {


            var request = new RestRequest("Api/Apikey", Method.GET) { RequestFormat = DataFormat.Json };
            request.AddParameter("email", "383@envoc.com");
            request.AddParameter("password", "selu2015");
            string apikeyAdmin = "";
            string idAdmin = "";
            var result = await Client.ExecuteTaskAsync(request);
            {
                string[] arrResult = result.Content.ToString().Split('"');
                string[] userid = arrResult[6].Split(':', '}');
                idAdmin = userid[1];
                apikeyAdmin = arrResult[3];

            }

            request = new RestRequest("Api/Apikey", Method.GET) { RequestFormat = DataFormat.Json };
            request.AddParameter("email", email);
            request.AddParameter("password", password);
            result = await Client.ExecuteTaskAsync(request);
            {

                if (result.StatusCode.ToString().Equals("OK"))
                {

                    string[] arrResult = result.Content.ToString().Split('"');
                    string[] userid = arrResult[6].Split(':', '}');
                    string idCurrentUser = userid[1];
                    string apikeyCurrentUser = arrResult[3];
                    //get roles now
                    var requestRole = new RestRequest("API/Users/" + idCurrentUser, Method.GET);
                    //int idUser = Convert.ToInt32(idCurrentUser);
                    requestRole.Parameters.Clear();
                    requestRole.AddUrlSegment("id", idAdmin);
                    requestRole.AddHeader("xcmps383authenticationid", idAdmin);
                    if (!idCurrentUser.Equals("1"))
                    {
                        requestRole.AddHeader("xcmps383authenticationkey", apikeyAdmin);

                    }
                    else
                    {
                        requestRole.AddHeader("xcmps383authenticationkey", apikeyCurrentUser);
                    }
                    requestRole.AddHeader("Content-Type", "application/json; charset=utf-8");
                    requestRole.AddHeader("Accept", "application/json");
                    var result1 = await Client.ExecuteTaskAsync(requestRole);
                    var a = result1.Content.ToString().Split('"');
                    //string role = "Admin";
                    string role = a[18].ToString();
                    if (role.Equals(":0}")) role = "Admin";
                    else if (role.Equals(":1}")) role = "Employee";
                    else if (role.Equals(":2}")) RedirectToAction("Login");
                        //role = "Customer";

                    string userData = idCurrentUser + " " + apikeyCurrentUser + " " + role;
                    var identity = new UserIdentity(userData, role);
                    var principal = new UserPrincipal(identity);
                    Thread.CurrentPrincipal = principal;
                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                        1,
                        idCurrentUser + " " + apikeyCurrentUser + " " + role,
                        System.DateTime.Now,
                        System.DateTime.Now.AddMinutes(2800),
                        false,
                        role,
                        FormsAuthentication.FormsCookiePath
                        );
                    string encTicket = FormsAuthentication.Encrypt(ticket);
                    System.Web.HttpCookie cookie = new System.Web.HttpCookie(FormsAuthentication.FormsCookieName, encTicket);

                    cookie.HttpOnly = true;
                    Response.Cookies.Add(cookie);
                    FormsAuthentication.SetAuthCookie(userData, false);
                    if (role.Equals("Admin"))
                    {
                        return RedirectToAction("Index");
                    }
                    else if (role.Equals("Employee"))
                    {
                        return RedirectToAction("Index");
                    }

                    Console.WriteLine("whoops!!sth wrong");
                }
            }
            return View();
        }
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();

            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                    1,
                    "",
                    System.DateTime.Now,
                    System.DateTime.Now,
                    false,
                    "",
                    FormsAuthentication.FormsCookiePath
                    );
            FormsAuthentication.SetAuthCookie("", false);
            return RedirectToAction("LogIn", "Account");
        }
    }
}

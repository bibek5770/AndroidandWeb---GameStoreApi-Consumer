using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RestSharp;
using System.Web.Mvc;
using System.Web.Security;

namespace FrontEnd.Controllers
{
    public class BaseController : Controller
    {
     // public RestClient Client = new RestClient("http://localhost:12932/");
        public RestClient Client = new RestClient("http://dev.envocsupport.com/GameStore3/");


        public string GetApiKey()
        {
            System.Web.HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];

            if (authCookie != null)
            {
                FormsAuthenticationTicket tckt = FormsAuthentication.Decrypt(authCookie.Value);
                var data = tckt.Name.Split(' ');
                try
                {
                    return data[1].ToString();
                }
                catch
                {
                    RedirectToAction("Login", "User");
                }
            }
            return null;
        }

        public string GetUserId()
        {
            System.Web.HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];

            if (authCookie != null)
            {
                FormsAuthenticationTicket tckt = FormsAuthentication.Decrypt(authCookie.Value);
                var data = tckt.Name.Split(' ');

                return data[0].ToString();
            }
            return null;
        }

        
        public RestRequest CreateRequest(string url, Method method)
        {
            var request = new RestRequest(url, method);
            request.AddHeader("xcmps383authenticationid", GetUserId());
            request.AddHeader("xcmps383authenticationkey", GetApiKey());
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/json; charset=utf-8");
            request.RequestFormat = DataFormat.Json;
            return request;
        }
    }
}
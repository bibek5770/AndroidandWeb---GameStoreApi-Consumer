using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;

namespace FrontEnd.UserRole
{
    public class UserIdentity : IIdentity
    {
        private static string Userdata = string.Empty; // Stores user "id apikey"
        private static string Roles = string.Empty; // Stores user roles separated by spaces
        private bool authenticated = false;

        #region IIdentity Members
        public string AuthenticationType
        {
            get { return "CUSTOM_SECURITY"; }
        }
        public string Name
        {
            get { return Userdata; }
        }
        public bool IsAuthenticated
        {
            get { return authenticated; }
        }



        internal bool IsInRole(string role)
        {
            return Roles.Contains(role);
        }
        #endregion

        #region Constructor(s)

        public UserIdentity(string userdata, string roles)
        {
            // Parse the user data (id / apikey / roles -- separated by spaces)
            if (!userdata.Equals(""))
            {
                var data = userdata.Split(' ');

                // Set Userdata to string containing user "id apikey"
                Userdata = data[0].ToString() + ' ' + data[1].ToString();

                Roles = string.Empty;

                Roles = roles;
                authenticated = true;
                // Parse the roles
                //var parsedRoles = roles.Split(' ');

                //// Get all roles -- separate by spaces
                //for (int i = 0; i < parsedRoles.Length; i++)
                //{
                //    Roles += parsedRoles[i].ToString() + " ";
                //}

            }
            else
            {
                authenticated = false;
                Roles = string.Empty;
            }
        }
        #endregion
    }
}
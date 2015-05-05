using FrontEnd.UserRole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace FrontEnd.Controllers
{
    public class UserPrincipal : IPrincipal
    {

        #region IPrincipal Members

        private UserIdentity currentIdentity;

        public IIdentity Identity
        {
            get { return currentIdentity; }
        }

        public bool IsInRole(string role)
        {
            return currentIdentity.IsInRole(role);
        }


        //     #endregion

        //#region Constructor(s)

        internal UserPrincipal() { }

        public UserPrincipal(UserIdentity identity)
        {
            AppDomain currentdomain = Thread.GetDomain();
            currentdomain.SetPrincipalPolicy(PrincipalPolicy.UnauthenticatedPrincipal);

            IPrincipal oldPrincipal = Thread.CurrentPrincipal;
            Thread.CurrentPrincipal = this;

            try
            {
                if (oldPrincipal.GetType() != typeof(UserPrincipal))
                    currentdomain.SetThreadPrincipal(this);
            }
            catch
            {
                // failed, but we don't care because there's nothing
                // we can do in this case
            }

            currentIdentity = (UserIdentity)identity;
        }
        #endregion
    }
}
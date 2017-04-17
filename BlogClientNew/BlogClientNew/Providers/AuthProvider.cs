using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace BlogClientNew.Providers
{
    public class AuthProvider:IAuthProvider
    {
        public bool IsLoggedIn {
            get {
                return HttpContext.Current.User.Identity.IsAuthenticated;
            }
        }

        public bool Login(string UserName, string Password) {
            bool result = FormsAuthentication.Authenticate(UserName,Password);
            if (result) {
                FormsAuthentication.SetAuthCookie(UserName,false);
            }
            return result;
        }

        public void Logout() {
            FormsAuthentication.SignOut();
        }
    }
}
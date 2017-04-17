using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogClientNew.Providers
{
    public interface IAuthProvider
    {
        bool IsLoggedIn { get; }
        bool Login(string UserName, string Password);
        void Logout();
    }
}
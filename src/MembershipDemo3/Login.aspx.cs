using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MembershipDemo3
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Login1_Authenticate(object sender, AuthenticateEventArgs e)
        {
            //if (FormsAuthentication.Authenticate(Login1.UserName, Login1.Password))
            //{
            //    FormsAuthentication.SetAuthCookie(Login1.UserName, false);
            //    Response.Redirect(FormsAuthentication.GetRedirectUrl(Login1.UserName, false));
            //}
            //else
            //{
            //    //FormsAuthentication.RedirectToLoginPage();
            //}
        }

        protected void Login1_LoggedIn(object sender, EventArgs e)
        {

        }
    }
}
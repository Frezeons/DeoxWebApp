using DeoxWebApp.Models;
using System;
using System.Reflection.Emit;
using System.Web;
using System.Web.UI;

namespace DeoxWebApp
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SessionBuffer sessionBuffer = new SessionBuffer();

                var user = sessionBuffer.GetUserLoginInfo();
                if (user != null)
                {
                    //deneme.Text = user.UserName;
                    // Do something with the user info
                }
                else
                {
                    //Response.Redirect("\\Login.aspx");
                }
            }
        }
    }
}

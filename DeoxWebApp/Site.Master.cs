using DeoxWebApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace DeoxWebApp
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SessionBuffer sessionBuffer = new SessionBuffer();
                var user = sessionBuffer.GetUserLoginInfo();
                if (user != null)
                {
                    authLinks.Visible = false;
                    if (user.RoleName != "Admin")
                    {
                        ManageUser.Visible = false;
                    }
                    else
                        GetBadgeValue();
                }
                else
                {
                    Response.Redirect("/Login");
                }
            }
        }
        private void GetBadgeValue()
        {
            DataLayer dataLayer = new DataLayer();
            string query = "SELECT COUNT(*) FROM Users u JOIN VerifyRequest vr ON u.ID = vr.UserID WHERE vr.IsEmailVerify = 1 AND vr.IsAdminVerify = 0;";
            DataSet ds = dataLayer.Select(query);
            DataTable dt = ds.Tables[0];

            if (dt.Rows[0][0].ToString() == "0")
                BadgeNumber.Style.Add("display", "none");
            else
                BadgeNumber.InnerText = dt.Rows[0][0].ToString();
        }
        protected void LogoutButton_Click(object sender, EventArgs e)
        {
            // Oturum bilgilerini temizleme
            Session["user"] = null;
            Session.Clear();
            Session.Abandon();
            Response.Redirect("/Login");
        }
    }
}
using DeoxWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DeoxWebApp
{
    public partial class VerifyUser : System.Web.UI.Page
    {
        DataLayer dataLayer = new DataLayer();
        Models.Register register = new Models.Register();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["token"] != null && Request.QueryString["token"] != "null")
                {
                    var token = Request.QueryString["token"];
                    var requestResult = register.ValidateToken(token);
                    var userId = requestResult.userId;
                    var RequestId = requestResult.requestId;
                    HttpContext.Current.Session["userID"] = userId;
                    HttpContext.Current.Session["requestId"] = RequestId;
                    if (RequestAccept(userId) == true)
                    {
                        var parameters = new Dictionary<string, object> { };
                        string query2 = "update VerifyRequest set IsEmailVerify = 1 where ID = " + RequestId + "";
                        dataLayer.Select(query2);
                    }
                    string alertScript = "<script type='text/javascript'>alert('E-posta doğrulamanız yapılmıştır.'); window.location.href = '\\Login';</script>";
                    Response.Write(alertScript);
                    Response.End();
                }

                else
                {

                    string alertScript = "<script type='text/javascript'>alert('Geçersiz İstek.'); window.location.href = '\\Login';</script>";
                    Response.Write(alertScript);
                    Response.End();
                }
            }
        }

        private bool RequestAccept(int UserId)
        {
            string query1 = "SELECT top(1) Created_Time FROM VerifyRequest where UserId = " + UserId + " and IsEmailVerify = 0 ORDER BY Created_Time DESC;";// daha önce basılmamış bir link varsa onun yollandığı tarih
            var checkdate = dataLayer.Select(query1);

            if (checkdate != null && checkdate.Tables.Count > 0 && checkdate.Tables[0].Rows.Count > 0)
            {
                var checkdate1 = checkdate.Tables[0].Rows[0][0];

                if (checkdate1 != null && checkdate1 != DBNull.Value)
                {
                    var datetimenow = DateTime.Now;
                    var tokendate = Convert.ToDateTime(checkdate1);
                    TimeSpan timeDifference = datetimenow - tokendate;

                    if (Math.Abs(timeDifference.TotalMinutes) < 30)
                    {
                        return true;
                    }
                }
            }
            return false;

        }

    }
}
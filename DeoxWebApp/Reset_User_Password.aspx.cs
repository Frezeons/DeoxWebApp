using DeoxWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;
using static SessionBuffer;

namespace DeoxWebApp
{
    public partial class Reset_User_Password : System.Web.UI.Page
    {
        Models.Register register = new Models.Register();
        DataLayer dataLayer = new DataLayer();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["token"] != null && Request.QueryString["token"] != "null")
                {
                    var token = Request.QueryString["token"];
                    DataLayer dataLayer = new DataLayer();
                    Models.Register register = new Models.Register();
                    var requestResult = register.ValidateToken(token);
                    var userId = requestResult.userId;
                    var RequestId = requestResult.requestId;
                    HttpContext.Current.Session["userID"] = userId;
                    HttpContext.Current.Session["requestId"] = RequestId;
                    if (RequestAccept(userId) == true) {
                        var parameters = new Dictionary<string, object>{};
                        string query2 = "update ChangePassRequest set IsDone = 1 where ID = " + RequestId + "";
                        dataLayer.Update(query2,parameters);
                    }
                    }
               
                    else
                    {
                       
                        string alertScript = "<script type='text/javascript'>alert('Geçersiz İstek.'); window.location.href = '\\Login';</script>";
                        Response.Write(alertScript);
                        Response.End();
                    }
                
                
                

                using (var streamReader = new System.IO.StreamReader(Request.InputStream))
                {
                    string szJsonData = streamReader.ReadToEnd();

                    if (!string.IsNullOrEmpty(szJsonData))
                    {
                        ItemProcess(szJsonData);
                    }
                }
            }
        }
        private bool RequestAccept(int UserId)
        {
            string query1 = "SELECT top(1) Created_Date FROM ChangePassRequest where UserId = " + UserId + " and IsDone = 0 ORDER BY Created_Date DESC;";// daha önce basılmamış bir link varsa onun yollandığı tarih
            var checkdate = dataLayer.Select(query1);
            if (checkdate != null)
            {
                var datetimenow = DateTime.Now;
                var checkdate1 = checkdate.Tables[0].Rows[0][0];
                var tokendate = Convert.ToDateTime(checkdate1);
                TimeSpan timeDifference = datetimenow - tokendate;
                if (Math.Abs(timeDifference.TotalMinutes) < 30)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        private void ItemProcess(string szJsonData)
        {
            try
            {
                if (szJsonData.Contains("info"))
                {
                    var str = DetectString(szJsonData);
                    //Response.StatusCode = 200;
                    //var userId = "userId :" + HttpContext.Current.Session["userID"];
                    //var RequestId = "RequestId : " + HttpContext.Current.Session["RequestId"];
                    //Response.Write(userId + "\r" + RequestId + "|");
                }

                else if (szJsonData.Contains("password"))
                {
                    var str = DetectString(szJsonData);
                    var newPassword = str[1];
                    var confirmPassword = str[2];

                    
                    if (register.IsValidPassword(newPassword))
                    {
                        var parameters = new Dictionary<string, object>
                        {
                        };
                        var newhashedpass = register.EncryptMD5Function(newPassword, "password");
                        string query = "update Users set HASH_PASS=" + newhashedpass + " where ID=" + HttpContext.Current.Session["userID"] + "";
                        dataLayer.Update(query, parameters);
                        Response.StatusCode = 200;
                    }
                    else
                    {
                        Response.StatusCode = 345;

                    }


                }
                else
                {
                    Response.StatusCode = 300;
                    Response.Write("İşlem tanımlanamadı.");

                }
            }
            catch (Exception ex)
            {

            }

        }

        private string[] DetectString(string szJsonString)
        {
            // Replace unwanted characters and split by underscore
            string processedString = szJsonString.Replace("\"", "").Replace("\\", "").Replace("{", "").Replace("}", "").Replace("[", "").Replace("]", "").Replace(":", ",").Replace("_", ",");
            return processedString.Split(',');
        }


    }
}
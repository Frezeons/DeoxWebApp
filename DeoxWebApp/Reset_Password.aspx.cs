using DeoxWebApp.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DeoxWebApp
{
    public partial class Reset_Password : System.Web.UI.Page
    {
        MailManagement mailManagement = new MailManagement();
        Models.Register register = new Models.Register();
        DataLayer dataLayer = new DataLayer();
        Auth a = new Auth();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["token"] != null && Request.QueryString["token"] != "null")
                {
                    var iCompanyCode = Request.QueryString["token"];
                }

                using (var streamReader = new System.IO.StreamReader(Request.InputStream))
                {
                    string szJsonData = streamReader.ReadToEnd();

                    if (!string.IsNullOrEmpty(szJsonData))
                    {
                        Actions(szJsonData);
                    }
                }
            }
        }




        private void Actions(string szJsonData)
        {
            try
            {
                if (szJsonData.Contains("mailOrUser"))
                {
                    var str = DetectString(szJsonData);
                    if (str.Length > 1)
                    {


                        var txtUserName = str[1];
                        if (!txtUserName.Contains("@")) //kullanıcı adı
                        {
                            string query = "select ID, Username, EMAIL from users where Username= '" + txtUserName + "'";
                            var parameters = new Dictionary<string, object>
                            {
                            };
                            DataSet ds = dataLayer.Select(query, parameters);
                            DataTable dt = ds.Tables[0];

                            if (dt.Rows.Count > 0)
                            {
                                DataRow row = dt.Rows[0];
                                string userMail = row["EMAIL"].ToString();
                                SendMail(userMail);
                                Response.StatusCode = 200;
                            }
                            else
                            {
                                Response.StatusCode = 405;

                            }
                        }
                        else //mail
                        {
                            string query = "select ID, Username, EMAIL from users where EMAIL= '" + txtUserName + "'";
                            var parameters = new Dictionary<string, object>
                            {
                            };
                            DataSet ds = dataLayer.Select(query, parameters);
                            DataTable dt = ds.Tables[0];

                            if (dt.Rows.Count > 0)
                            {
                                DataRow row = dt.Rows[0];
                                string userMail = row["EMAIL"].ToString();
                                string userName = row["USERNAME"].ToString();
                                SendMail(userName);
                                Response.StatusCode = 200;
                            }
                            else
                            {
                                Response.StatusCode = 404;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception message for debugging
                // Replace 'YourLogger' with your actual logging method
                YourLogger.LogError("An error occurred in ItemProcess: " + ex.Message);
            }

        }

        private void SendMail(string szJsonData)
        {
            Auth a = new Auth();
            var userInfo = a.GetUserInfo(szJsonData);
            if (RequestAccept(userInfo.UserId) == true)
            {
                string query = "INSERT INTO ChangePassRequest (UserId,Created_Date,IsDone) VALUES ( " + userInfo.UserId + ",GETDATE(),0) SELECT TOP(1) ID FROM ChangePassRequest WHERE UserId=" + userInfo.UserId + " order by ID desc";
                DataSet ds = dataLayer.Select(query);
                int iRequestId = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                var token = register.GenerateToken(userInfo.UserId, iRequestId);
                string query2 = "update ChangePassRequest set EncryptValue='" + token + "' where ID = " + iRequestId + "";
                var parameters = new Dictionary<string, object>
                {
                };
                dataLayer.Update(query2, parameters);
                string baseUrl = "https://localhost:44346/Reset_User_Password";
                string urlWithToken = $"{baseUrl}?token={token}";
                string htmlContent = @"
                <div>
                  <p style=""direction:ltr;font-size:0px;padding:40px 10px;text-align:center;"">
                    <span style=""font-family:Poppins, Helvetica, Arial, sans-serif;font-size:20px;font-weight:300;line-height:30px;text-align:left;color:#003366;"">
                      Merhaba " + userInfo.UserName + @",
                    </span>
                  </p>
                  <p style=""direction:ltr;font-size:0px;padding:10px 25px;text-align:left;"">
                    <span style=""font-family:Poppins, Helvetica, Arial, sans-serif;font-size:20px;font-weight:300;line-height:30px;text-align:left;color:#003366;"">
                      Şifre değişikliği için aşağıdaki bağlantıyı 30 dk içerisinde kullanmanız gerekmektedir.
                    </span>
                  </p>
                  <p style=""direction:ltr;font-size:0px;padding:10px 25px;text-align:left;"">
                    <span style=""font-family:Poppins, Helvetica, Arial, sans-serif;font-size:20px;font-weight:300;line-height:30px;text-align:left;color:#003366;"">
                      Değişiklik isteği sizin tarafınızdan yapılmadı ise bizi bilgilendirin : <strong>destek@mahmutcangungor.com</strong>
                    </span>
                     </p>
                    <p style=""direction:ltr;font-size:0px;padding:10px 25px;text-align:left;"">
                    <span style=""font-family:Poppins, Helvetica, Arial, sans-serif;font-size:20px;font-weight:300;line-height:30px;text-align:left;color:#003366;"">
                      For password change, you must use the link below within 30 minutes.
                    </span>
                  </p> 
                    <p style=""direction:ltr;font-size:0px;padding:10px 25px;text-align:left;"">
                    <span style=""font-family:Poppins, Helvetica, Arial, sans-serif;font-size:20px;font-weight:300;line-height:30px;text-align:left;color:#003366;"">
                      Notify us if the change request was not made by you: <strong> destek@mahmutcangungor.com</strong>
                    </span>
                  </p>
                  <p style=""direction:ltr;font-size:0px;padding:10px 25px;text-align:center;"">
                    <a href=""" + urlWithToken + @""" style=""display: inline-block; background: #043768; color: white; font-family: Poppins, Helvetica, Arial, sans-serif; font-size: 18px; font-weight: normal; line-height: 30px; margin: 0; text-decoration: none; text-transform: none; padding: 10px 25px; border-radius: 3px;"" target=""_blank"">Şifreni Sıfırla/Reset your password</a>
                  </ p >
                </ div > ";

                MailManagement mailManagement = new MailManagement();
                mailManagement.SendEmail(userInfo.Mail, "Reset Password", htmlContent);

            }
            else
            {
                string alertScript = "<script type='text/javascript'>alert('Epostanız Gönderilmiş Kontrol Ediniz.');</script>";
                Response.Write(alertScript);
                Response.End();
            }
        }

        private string[] DetectString(string szJsonString)
        {
            // Replace unwanted characters and split by underscore
            string processedString = szJsonString.Replace("\"", "").Replace("\\", "").Replace("{", "").Replace("}", "").Replace("[", "").Replace("]", "").Replace(":", ",").Replace("_", ",");
            return processedString.Split(',');
        }

        public static class YourLogger
        {
            public static void LogError(string message)
            {
                // Implement your logging here, e.g., write to a log file or database
                System.Diagnostics.Debug.WriteLine(message);
            }
        }

        private bool RequestAccept(int UserId)
        {
            string query1 = "SELECT top(1) Created_Date FROM ChangePassRequest where UserId = " + UserId + " and IsDone = 0 ORDER BY Created_Date DESC;";// daha önce basılmamış bir link varsa onun yollandığı tarih
            var checkdate = dataLayer.Select(query1);
            if (checkdate != null && checkdate.Tables.Count > 0 && checkdate.Tables[0].Rows.Count > 0)
            {
                var datetimenow = DateTime.Now;
                var checkdate1 = checkdate.Tables[0].Rows[0][0];
                var tokendate = Convert.ToDateTime(checkdate1);
                TimeSpan timeDifference = datetimenow - tokendate;
                if (Math.Abs(timeDifference.TotalMinutes) > 30)
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
                return true;
            }

        }

        private void RequestCount(int UserId, string szJsonData)
        {
            var userInfo = a.GetUserInfo(szJsonData);
            string query = "SELECT COUNT (Created_Date) FROM ChangePassRequest;";// tüm requestleri çek count at
            var checkcount_temp = dataLayer.Select(query);
            var checkcount = Convert.ToInt32(checkcount_temp);
        }

        private void DailyRequestCount(int UserId,string szJsonData) {
            var userInfo = a.GetUserInfo(szJsonData);
            string query = "SELECT COUNT(Created_Date) FROM ChangePassRequest WHERE CAST(Created_Date AS DATE) = CAST(GETDATE() AS DATE);";// bu günkü requestleri çek count at
            var checkcount_temp = dataLayer.Select(query);
            var checkcount = Convert.ToInt32(checkcount_temp);
        }
    }
}
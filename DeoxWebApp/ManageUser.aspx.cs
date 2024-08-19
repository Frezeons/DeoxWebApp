using DeoxWebApp.Models;
using Microsoft.AspNet.FriendlyUrls;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using static System.Net.Mime.MediaTypeNames;


namespace DeoxWebApp
{
    public partial class ManageUser : System.Web.UI.Page
    {


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SessionBuffer sessionBuffer = new SessionBuffer();
                    var user = sessionBuffer.GetUserLoginInfo();
                    if (user != null)
                    {
                        if (user.RoleId != 3)
                        {
                            Response.Redirect("\\Login.aspx");
                        }
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

            catch (Exception ex)
            {
                // Log the exception message for debugging
                // Replace 'YourLogger' with your actual logging method
                YourLogger.LogError("An error occurred in Page_Load: " + ex.Message);
            }
        }

        private void ItemProcess(string szJsonData)
        {
            try
            {
                if (szJsonData.Contains("edit"))
                {
                    var str = DetectString(szJsonData);
                    if (str.Length > 1)
                    {
                        DataLayer dataLayer = new DataLayer();
                        Models.Register register = new Models.Register();
                        var txtUserID = str[1];
                        var txtUserName = str[2];
                        var txtUserMail = str[4];
                        var txtUserpass = str[3];
                        var cbxRoles = str[5];
                        var hash_pass = register.EncryptMD5Function(txtUserpass, "password");
                        string query = "UPDATE Users SET USERNAME = '" + txtUserName + "',  USERROLE = '" + cbxRoles + "', EMAIL = '" + txtUserMail + "',HASH_PASS='" + hash_pass + "' WHERE ID = '" + txtUserID + "';";
                        var parameters = new Dictionary<string, object>
                        {
                        };
                        dataLayer.Update(query, parameters);
                        Response.StatusCode = 200;

                    }
                    else
                    {
                        Response.StatusCode = 400;
                        Response.Write("İşlem Başarısız.");

                    }
                }

                else if (szJsonData.Contains("getrole"))
                {
                    var str = DetectString(szJsonData);
                    var response = cbxRoles();
                    Response.StatusCode = 200;
                    Response.Write(response + "|");
                }

                else if (szJsonData.Contains("getUserRole"))
                {
                    var str = DetectString(szJsonData);
                    var response = cbxRoles();
                    Response.StatusCode = 200;
                    Response.Write(response + "|");
                }

                else if (szJsonData.Contains("logout"))
                {
                    var str = DetectString(szJsonData);
                    SessionBuffer buf = new SessionBuffer();
                    buf.LogOut();

                    Response.Redirect("/Login");

                }

                else if (szJsonData.Contains("approve"))
                {
                    var str = DetectString(szJsonData);
                    if (str.Length > 1)
                    {
                        DataLayer dataLayer = new DataLayer();
                        Models.Register register = new Models.Register();
                        var txtUserID = str[2]; //// onayla buttonu basıldığı zaman hem useri 1 e çekecek hemde adminverify i 1 e çekecek
                        string queryrole = "update Users set USERROLE=1 where ID=" + txtUserID + "";
                        dataLayer.Select(queryrole);
                        string queryverify = "update VerifyRequest set IsAdminVerify=1 where UserId=" + txtUserID + "";
                        dataLayer.Select(queryverify);
                        Response.StatusCode = 200;
                    }
                    else
                    {
                        Response.StatusCode = 400;
                        Response.Write("İşlem Başarısız.");
                    }
                }

                else if (szJsonData.Contains("cancel"))
                {
                    var str = DetectString(szJsonData);
                    if (str.Length > 1)
                    {
                        DataLayer dataLayer = new DataLayer();
                        Models.Register register = new Models.Register();
                        var txtUserID = str[2]; //// onayla buttonu basıldığı zaman adminverify i 2 e çekecek
                        string queryverify = "update VerifyRequest set IsAdminVerify=2 where UserId=" + txtUserID + "";
                        dataLayer.Select(queryverify);
                        Response.StatusCode = 200;
                    }
                    else
                    {
                        Response.StatusCode = 400;
                        Response.Write("İşlem Başarısız.");
                    }
                }

                else if (szJsonData.Contains("add"))
                {
                    var str = DetectString(szJsonData);
                    if (str.Length > 1)
                    {
                        DataLayer dataLayer = new DataLayer();
                        Models.Register register = new Models.Register();
                        var txtUserName = str[1];
                        var txtUserMail = str[2];
                        var txtUserpass = str[3];
                        var cbxRoles = str[4];
                        var hash_pass = register.EncryptMD5Function(txtUserpass, "password");
                        string query1 = "INSERT INTO Users (USERNAME, USERROLE, HASH_PASS, EMAIL) VALUES ('" + txtUserName + "', " + cbxRoles + ", '" + hash_pass + "', '" + txtUserMail + "')";
                        var parameters = new Dictionary<string, object>
                        {
                        };
                        dataLayer.Insert(query1, parameters);//userid çek
                        string queryID = "select ID from Users where Username='" + txtUserName + "'";
                        DataSet useridtemp = dataLayer.Select(queryID);
                        DataTable dt = useridtemp.Tables[0];
                        var useridtemp1 = dt.Rows[0][0];
                        int UserID = Convert.ToInt32(useridtemp1);

                        string query3 = "INSERT INTO VerifyRequest (UserId, IsEmailVerify, IsAdminVerify) VALUES (" + UserID + ", 1, 1)";
                        dataLayer.Select(query3);
                        Response.StatusCode = 200;
                    }
                    else
                    {
                        Response.StatusCode = 400;
                        Response.Write("İşlem Başarısız.");
                    }
                }

                else if (szJsonData.Contains("delete"))
                {
                    var str = DetectString(szJsonData);
                    if (str.Length > 1)
                    {
                        var id = str[2];
                        DataLayer dataLayer = new DataLayer();
                        string query = $"DELETE FROM Users WHERE ID = @Value";
                        dataLayer.Delete(query, "@Value", id);
                        Response.StatusCode = 200;
                    }
                    else
                    {
                        Response.StatusCode = 400;
                        Response.Write("Geçersiz ID.");
                    }
                }

                else if (szJsonData.Contains("getBadgeCount"))
                {
                    var str = DetectString(szJsonData);
                    DataLayer dataLayer = new DataLayer();
                    string query = "SELECT COUNT(*) FROM Users u JOIN VerifyRequest vr ON u.ID = vr.UserID WHERE vr.IsEmailVerify = 1 AND vr.IsAdminVerify = 0;";
                    DataSet ds = dataLayer.Select(query);
                    DataTable dt = ds.Tables[0];
                    var badgecount = dt.Rows[0][0];
                    int response = Convert.ToInt32(badgecount);
                    Response.StatusCode = 200;
                    Response.Write(response + "|");
                }

                else if (szJsonData.Contains("user"))
                {
                    var str = DetectString(szJsonData);
                    var response = ShowDataStr();
                    Response.StatusCode = 200;
                    Response.Write(response + "|");
                }

                else
                {
                    Response.StatusCode = 300;
                    Response.Write("İşlem tanımlanamadı.");

                }


            }
            catch (Exception ex)
            {
                // Log the exception message for debugging
                // Replace 'YourLogger' with your actual logging method
                YourLogger.LogError("An error occurred in ItemProcess: " + ex.Message);
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



        private string ShowDataStr()
        {
            string szHtmlTableRow = "";
            SessionBuffer buf = new SessionBuffer();
            var user = buf.GetUserLoginInfo();
            DataLayer dataLayer = new DataLayer();
            string query = "SELECT u.ID, u.USERNAME, u.EMAIL,  r.RoleName FROM   Users AS u LEFT JOIN   Roles AS r ON u.USERROLE = r.ROLEID LEFT JOIN  VerifyRequest AS vr ON u.ID = vr.UserId WHERE  (vr.IsAdminVerify IS NULL OR vr.IsAdminVerify <> 2);";
            DataSet ds = dataLayer.Select(query);


            Thread.Sleep(1000);
            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];

                foreach (DataRow row in dt.Rows)
                {
                    szHtmlTableRow += "<tr>";
                    szHtmlTableRow += "<td>" + row["ID"].ToString() + "</td>";
                    szHtmlTableRow += "<td>" + row["Username"].ToString() + "</td>";
                    szHtmlTableRow += "<td>" + row["EMAIL"].ToString() + "</td>";
                    szHtmlTableRow += "<td>" + row["RoleName"].ToString() + "</td>";

                    string roleName = row["RoleName"].ToString();

                    // Edit button for roles other than Admin and Non-Verified User
                    if (roleName != "Admin" && roleName != "Non-Verified User")
                    {
                        szHtmlTableRow += "<td>" +
                            $"<button type='button' class='btn btn-warning' name='Edit' onclick=\"FilltoModal('{row["ID"]}','{row["Username"]}','{row["EMAIL"]}',' ','{row["RoleName"]}'); VisibleButton('edit')\" " +
                            $"data-bs-toggle='modal' data-bs-target='#myModal' id='TableEdit'>Düzenle</button> &nbsp;";

                        szHtmlTableRow +=
                            $"<button type='button' class='btn btn-danger' id='{row["ID"]}' name='Delete' " +
                            $"onclick=\"SendRequest(this.id , 'delete')\" id='TableDel'>Sil</button>";
                    }


                    // Approve and Delete buttons for users with GetUserVerify = 1 and RoleName = "Non-Verified User"
                    if (GetUserVerify(Convert.ToInt32(row["ID"])) == 1 && row["RoleName"].ToString() == "Non-Verified User")
                    {
                        szHtmlTableRow += "<td>" +
                            $"<button type='button' class='btn btn-success' id='{row["ID"]}' name='Onayla' " +
                            $"onclick='VerifyUser(this.id, \"approve\")'>Onayla</button> &nbsp;";

                        szHtmlTableRow +=
                            $"<button type='button' class='btn btn-secondary' id='{row["ID"]}' name='Delete' " +
                            $"onclick='VerifyUser(this.id, \"cancel\")'>Reddet</button>";
                    }
                    else if (GetUserVerify(Convert.ToInt32(row["ID"])) == 0 && row["RoleName"].ToString() == "Non-Verified User")
                    {
                        szHtmlTableRow += "<td>" + $"<span class=\"badge bg-secondary\">Mail Doğrulaması Bekleniyor.</span>";
                    }


                        szHtmlTableRow += "</td></tr>";
                }

                return szHtmlTableRow;

            }
            return "";
        }

        private string cbxRoles()
        {
            string szRoleRow = "";
            DataLayer _dataLayer = new DataLayer();
            string query = "select * from Roles where RoleId=1 or RoleId=2";
            DataSet ds = _dataLayer.Select(query);

            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                int rowIndex = 1;

                foreach (DataRow row in dt.Rows)
                {
                    szRoleRow += "<option value=\"" + row["RoleID"] + "\">" + row["RoleName"].ToString() + "</option>";
                    rowIndex++;
                }

                return szRoleRow;
            }
            return "";
        }

        private int GetUserVerify(int UserID) {
            DataLayer dataLayer = new DataLayer();
            string query = "SELECT * FROM VerifyRequest WHERE UserID = " + UserID + " ORDER BY Created_Time DESC;";
            DataSet emailveriftemp = dataLayer.Select(query);
            DataTable dt1 = emailveriftemp.Tables[0];
            var emailveriftemp1 = dt1.Rows[0][4];
            int IsEmailVerify = Convert.ToInt32(emailveriftemp1);
            return IsEmailVerify;
        }

}
}


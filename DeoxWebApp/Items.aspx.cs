using DeoxWebApp.Models;
using Microsoft.AspNet.FriendlyUrls;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using static System.Net.Mime.MediaTypeNames;


namespace DeoxWebApp
{
    public partial class Items : System.Web.UI.Page
    {


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                   
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
                        var txtItemId = str[1];
                        var txtItemName = str[2];
                        var txtItemCount = str[3];
                        var cbxColor = str[4];
                        DataLayer dataLayer = new DataLayer();
                        string query = "UPDATE Items SET ItemName = '" + txtItemName + "',  Color = @Color, ItemCount = @ItemCount WHERE ID = @ID;";
                        var parameters = new Dictionary<string, object>
                        {
                        {"@ID", txtItemId},
                        {"@ItemName", txtItemName},
                        {"@Color", cbxColor},
                        {"@ItemCount", txtItemCount}
                        };
                        dataLayer.Update(query, parameters);
                        Response.StatusCode = 200;
                        SuccessMessage.Text = "Düzenleme Başarılı.";
                        ErrorMessage.Text = "";
                    }
                    else
                    {
                        Response.StatusCode = 400;
                        Response.Write("İşlem Başarısız.");
                        ErrorMessage.Text = "İşlem Başarısız.";
                        SuccessMessage.Text = "";
                    }
                }

                else if (szJsonData.Contains("role"))
                {
                    SessionBuffer sessionBuffer = new SessionBuffer();
                    var user = sessionBuffer.GetUserLoginInfo();
                    Response.Write(sessionBuffer.GetUserRole(user.UserName) + "|");
                   
                }

                else if (szJsonData.Contains("logout"))
                {
                    var str = DetectString(szJsonData);
                    SessionBuffer buf = new SessionBuffer();
                    buf.LogOut();

                    Response.Redirect("/Login");

                }

                else if (szJsonData.Contains("add"))
                {
                    var str = DetectString(szJsonData);
                    if (str.Length > 1)
                    {
                        var txtItemName = str[1];
                        var txtItemCount = str[2];
                        var cbxColor = str[3];
                        DataLayer dataLayer = new DataLayer();
                        string query = "INSERT INTO Items (ItemName, Color, ItemCount) VALUES (@ItemName, @Color, @ItemCount)";
                        var parameters = new Dictionary<string, object>
                        {
                        {"@ItemName", txtItemName},
                        {"@Color", cbxColor},
                        {"@ItemCount", txtItemCount}
                        };
                        dataLayer.Insert(query, parameters);
                        Response.StatusCode = 200;
                        SuccessMessage.Text = "Ekleme Başarılı.";
                        ErrorMessage.Text = "";
                    }
                    else
                    {
                        Response.StatusCode = 400;
                        Response.Write("İşlem Başarısız.");
                        ErrorMessage.Text = "İşlem Başarısız.";
                        SuccessMessage.Text = "";
                    }
                }

                else if (szJsonData.Contains("delete"))
                {
                    var str = DetectString(szJsonData);
                    if (str.Length > 1)
                    {
                        var id = str[2];
                        DataLayer dataLayer = new DataLayer();
                        string query = $"DELETE FROM Items WHERE ID = @Value";
                        dataLayer.Delete(query, "@Value", id);
                        Response.StatusCode = 200;
                        SuccessMessage.Text = "Delete işlemi başarılı.";
                        ErrorMessage.Text = "";
                    }
                    else
                    {
                        Response.StatusCode = 400;
                        Response.Write("Geçersiz ID.");
                        ErrorMessage.Text = "Geçersiz ID.";
                        SuccessMessage.Text = "";
                    }
                }

                else if (szJsonData.Contains("products"))
                {
                    var str = DetectString(szJsonData);
                    var response = ShowDataStr();
                    Response.StatusCode = 200;
                    Response.Write(response + "|");
                }

                else if (szJsonData.Contains("colors"))
                {
                    var str = DetectString(szJsonData);
                    var response = cbxColor();
                    Response.StatusCode = 200;
                    Response.Write(response + "|");
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

                else
                {
                    Response.StatusCode = 300;
                    Response.Write("İşlem tanımlanamadı.");
                    ErrorMessage.Text = "İşlem tanımlanamadı.";
                    SuccessMessage.Text = "";

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
            string query = "select ID,ItemName,ColorName AS ItemColor,ItemCount from Items , Color where Color.Color_id=Items.Color";
            DataSet ds = dataLayer.Select(query);
            Thread.Sleep(1000);
            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];

                foreach (DataRow row in dt.Rows)
                {
                    szHtmlTableRow += "<tr>";
                    szHtmlTableRow += "<td>" + row["ID"].ToString() + "</td>";
                    szHtmlTableRow += "<td>" + row["ItemName"].ToString() + "</td>";
                    szHtmlTableRow += "<td>" + row["ItemColor"].ToString() + "</td>";
                    szHtmlTableRow += "<td>" + row["ItemCount"].ToString() + "</td>";

                    if (user.RoleId == 2 || user.RoleId == 3)
                    {
                        szHtmlTableRow += "<td>" + $"<button type='button' class='btn btn-warning' name='Edit' " +
                                            $"onclick=\"FilltoModal('{row["ID"]}','{row["ItemName"]}','{row["ItemCount"]}','{row["ItemColor"]}'); VisibleButton('edit')\" " +
                                            $"data-bs-toggle='modal' data-bs-target='#myModal'>Düzenle</button> &nbsp;";
                    }
                    if (user.RoleId == 3)
                    {
                        szHtmlTableRow += $"<button type='button' class='btn btn-danger' name='Delete'" +
                                        $"onclick=\"SendRequest({row["ID"]}, 'delete')\" id='TableDel'>Sil</button>";
                    }

                    szHtmlTableRow += "</td></tr>";
                }


                return szHtmlTableRow;
            }
            return "";
        }

        private string cbxColor()
        {
            string szColorRow = "";
            DataLayer _dataLayer = new DataLayer();
            string query = "SELECT * FROM Color";
            DataSet ds = _dataLayer.Select(query);

            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                int rowIndex = 1;

                foreach (DataRow row in dt.Rows)
                {
                    szColorRow += "<option value=\"" + row["Color_id"] + "\">" + row["ColorName"].ToString() + "</option>";
                    rowIndex++;
                }

                return szColorRow;
            }
            return "";
        }

        private static void DeleteItem(string tableName, string columnName, object value)
        {
            string query = $"DELETE FROM {tableName} WHERE {columnName} = @Value";
            var parameters = new Dictionary<string, object>
            {
                { "@Value", value }
            };
            DataLayer dataLayer = new DataLayer();
            dataLayer.Delete(query, "@Value", value);
        }

    }
}


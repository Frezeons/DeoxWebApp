using DeoxWebApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DeoxWebApp
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SessionBuffer sessionBuffer = new SessionBuffer();

                var user = sessionBuffer.GetUserLoginInfo();
                if (user != null)
                {
                    Response.Redirect("\\Items.aspx");
                }
            }

        }

        public void button_login_Click(object sender, EventArgs e)
        {
            try
            {
                string username = username_text.Text;
                string password = pass_text.Text;

                Auth auth = new Auth();

                if (auth.AuthenticateUser(username, password))
                {
                    if (auth.VerificationUser(username, password))
                    {
                        SessionBuffer buffer = new SessionBuffer();
                        buffer.GetAppUser(username);
                        var buf = buffer.GetUserLoginInfo();
                        Response.Redirect("\\Items.aspx");
                    }
                    else
                    {
                        DataLayer dataLayer = new DataLayer();
                        string queryID = "select ID from Users where Username='" + username + "'";
                        DataSet useridtemp = dataLayer.Select(queryID);
                        DataTable dt = useridtemp.Tables[0];
                        var useridtemp1 = dt.Rows[0][0];
                        int UserID = Convert.ToInt32(useridtemp1);

                        string query = "select IsEmailVerify from VerifyRequest where UserId= " + UserID + "";
                        DataSet emailveriftemp = dataLayer.Select(query);
                        DataTable dt1 = emailveriftemp.Tables[0];
                        var emailveriftemp1 = dt1.Rows[0][0];
                        int IsEmailVerify = Convert.ToInt32(emailveriftemp1);

                        string queryadmin = "select IsAdminVerify from VerifyRequest where UserId= " + UserID + "";
                        DataSet adminveriftemp = dataLayer.Select(queryadmin);
                        DataTable dt2 = adminveriftemp.Tables[0];
                        var adminveriftemp1 = dt2.Rows[0][0];
                        int IsAdminVerify = Convert.ToInt32(adminveriftemp1);

                        if (IsEmailVerify == 0) // üye mailden onaylamamış
                        {
                            if (RequestVerify(UserID)) // yarım saat önce gelipte açmadığı varsa onu kontrol (son gönderimin üstünden yarım saat geçmiş)
                            {
                                Register register = new Register();
                                bool isConfirm = register.SendMail(username); // isConfirm olarak SendMail'in sonucunu kontrol et

                                if (isConfirm)
                                {
                                    string alertScript = "<script type='text/javascript'>alert('Doğrulama E-postanız yeniden gönderilmiştir.'); window.location.href = '\\Login';</script>";
                                    Response.Write(alertScript);
                                    Response.End();
                                }
                                else
                                {
                                    // E-posta gönderimi başarısız olduysa, kullanıcıya bir hata mesajı gösterilebilir
                                    message_login.Text = "E-posta gönderiminde bir hata oluştu.";
                                }
                            }
                            else
                            {
                                string alertScript = "<script type='text/javascript'>alert('Zaten Halihazırda Gönderilmiş Doğrulama E-postanız Vardır Lütfen Kontrol Ediniz.'); window.location.href = '\\Login';</script>";
                                Response.Write(alertScript);
                                Response.End();
                            }


                        }
                        else
                        {
                            if (IsAdminVerify == 0) { 
                                // isemail verify 1 ise
                                string alertScript = "<script type='text/javascript'>alert('Admin onayı bekleniyor.'); window.location.href = '\\Login';</script>";
                                Response.Write(alertScript);
                                Response.End();
                        }
                            else if (IsAdminVerify == 2)
                            {
                                // isemail verify 1 ise
                                string alertScript = "<script type='text/javascript'>alert('Böyle Bir Kullanıcı Bulunmamaktadır.'); window.location.href = '\\Login';</script>";
                                Response.Write(alertScript);
                                Response.End();
                            }
                           
                        }
                    }
                }
                else
                {
                    // Kullanıcı doğrulanamadı, hata mesajı gösterilebilir
                    message_login.Text = "Geçersiz kullanıcı adı veya şifre.";
                }
            }
            catch (Exception ex)
            {
                // Hata yönetimi: Hata loglanabilir veya kullanıcıya hata mesajı gösterilebilir.
                message_login.Text = "Bir hata oluştu: " + ex.Message;
            }
        }

        protected void button_to_register_Click(object sender, EventArgs e)
        {
            Response.Redirect("\\Register.aspx");
        }

        protected void btn_reset_password_Click(object sender, EventArgs e)
        {
            Response.Redirect("\\Reset_Password.aspx");
        }

        private bool RequestVerify(int UserId)
        {
            DataLayer dataLayer = new DataLayer();
            string query1 = "SELECT top(1) Created_Time FROM VerifyRequest where UserId = " + UserId + " and IsEmailVerify = 0 ORDER BY Created_Time DESC;";// daha önce basılmamış bir link varsa onun yollandığı tarih
            var checkdate = dataLayer.Select(query1);
            if (checkdate != null)
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
                return false;
            }

        }
    }
}
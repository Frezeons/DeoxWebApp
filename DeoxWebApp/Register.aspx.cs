using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using DeoxWebApp.Models; // Register sınıfının bulunduğu namespace'i ekleyin.

namespace DeoxWebApp
{
    public partial class Register : System.Web.UI.Page
    {
        MailManagement mailManagement = new MailManagement();
        Models.Register register = new Models.Register();
        DataLayer dataLayer = new DataLayer();
        Auth a = new Auth();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SessionBuffer sessionBuffer = new SessionBuffer();

                var user = sessionBuffer.GetUserLoginInfo();
                if (user != null)
                {
                    Response.Redirect("\\Default.aspx");
                }
            }

        }

        protected void button_register_Click(object sender, EventArgs e)
        {
            string username = username_register_text.Text;
            string email = email_register_text.Text;
            string password = password_register_text.Text;

            Models.Register register = new Models.Register(); // Register sınıfını burada oluşturun.

            if (!register.IsValidUsername(username))
            {
                message_register.Text = "Kullanıcı adı en az 5 karakter olmalıdır.";
                message_register.ForeColor = System.Drawing.Color.Red;
            }
            else if (!register.IsValidPassword(password))
            {
                message_register.Text = "Şifre en az 5 karakter olmalı, bir büyük harf ve bir özel karakter içermelidir.";
                message_register.ForeColor = System.Drawing.Color.Red;
            }
            else if (!register.UsernameEmailControl(username, email))
            {
                message_register.Text = "Kullanıcı adı veya e-posta zaten mevcut.";
                message_register.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                string hashedPassword = register.EncryptMD5Function(password, "password");
                if (register.RegisterUser(username, email, hashedPassword))
                {
                    message_register.Text = "Kayıt başarılı.";
                    message_register.ForeColor = System.Drawing.Color.Green;
                    SendMail(username);
                    string alertScript = "<script type='text/javascript'>alert('Doğrulama E-postanız Gönderilmiştir Lütfen Kontrol Ediniz.'); window.location.href = '\\Login';</script>";
                    Response.Write(alertScript);
                    Response.End();
                }
                else
                {
                    message_register.Text = "Kayıt sırasında bir hata oluştu.";
                    message_register.ForeColor = System.Drawing.Color.Red;
                }
            }
        }

        protected void button_to_login_Click(object sender, EventArgs e)
        {
            Response.Redirect("\\Login.aspx");
        }

        public bool SendMail(string szJsonData)
        {
            try
            {

                var userInfo = a.GetUserInfo(szJsonData);
                string query = "INSERT INTO VerifyRequest (UserId,Created_Time) VALUES ( " + userInfo.UserId + ",GETDATE()) SELECT TOP(1) ID FROM VerifyRequest WHERE UserId=" + userInfo.UserId + " order by ID desc";
                DataSet ds = dataLayer.Select(query);
                int iRequestId = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                var token = register.GenerateToken(userInfo.UserId, iRequestId);
                string query2 = "update VerifyRequest set EncryptValue='" + token + "' where ID = " + iRequestId + "";
                dataLayer.Select(query2);
                string baseUrl = "https://localhost:44346/VerifyUser";
                string urlWithToken = $"{baseUrl}?token={token}";

                string htmlContent =
                @"
            <div>
            <p style=""direction:ltr;font-size:0px;padding:40px 10px;text-align:center;"">
            <span style=""font-family:Poppins, Helvetica, Arial, sans-serif;font-size:20px;font-weight:300;line-height:30px;text-align:left;color:#003366;"">
                Merhaba " + userInfo.UserName + @",
            </span>
            </p>
            <p style=""direction:ltr;font-size:0px;padding:10px 25px;text-align:left;"">
            <span style=""font-family:Poppins, Helvetica, Arial, sans-serif;font-size:20px;font-weight:300;line-height:30px;text-align:left;color:#003366;"">
                Hesabınızı kullanabilmeniz için hesabınızı doğrulamanız lazım.
            </span>
            </p>
            <p style=""direction:ltr;font-size:0px;padding:10px 25px;text-align:left;"">
            <span style=""font-family:Poppins, Helvetica, Arial, sans-serif;font-size:20px;font-weight:300;line-height:30px;text-align:left;color:#003366;"">
                Eğer bir problem yaşarsanız bizi bilgilendirin : <strong>destek@mahmutcangungor.com</strong>
            </span>
            </p>
            <p style=""direction:ltr;font-size:0px;padding:10px 25px;text-align:left;"">
            <span style=""font-family:Poppins, Helvetica, Arial, sans-serif;font-size:20px;font-weight:300;line-height:30px;text-align:left;color:#003366;"">
                You need to verify your account before you can use it.
            </span>
            </p> 
            <p style=""direction:ltr;font-size:0px;padding:10px 25px;text-align:left;"">
            <span style=""font-family:Poppins, Helvetica, Arial, sans-serif;font-size:20px;font-weight:300;line-height:30px;text-align:left;color:#003366;"">
                Let us know if you have any problems: <strong> destek@mahmutcangungor.com</strong>
            </span>
            </p>
            <p style=""direction:ltr;font-size:0px;padding:10px 25px;text-align:center;"">
                <a href=""" + urlWithToken + @""" style=""display: inline-block; background: #043768; color: white; font-family: Poppins, Helvetica, Arial, sans-serif; font-size: 18px; font-weight: normal; line-height: 30px; margin: 0; text-decoration: none; text-transform: none; padding: 10px 25px; border-radius: 3px;"" target=""_blank"">Hesabını Doğrula/Verify Account</a>
            </p>
            </div> ";
                MailManagement mailManagement = new MailManagement();
                mailManagement.SendEmail(userInfo.Mail, "Verify Account", htmlContent);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
    }

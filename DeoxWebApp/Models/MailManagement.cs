using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Web;

namespace DeoxWebApp.Models
{
    public class MailManagement
    {
        #region special
        protected string _mailAdress= "mahocan.gngr@gmail.com", _mailPassword= "yduz tepx yers jxbz";
        #endregion


        public void SendEmail(string toEmailAddress, string subject, string body)
        {
            try
            {
                MailMessage eposta = new MailMessage();
                eposta.From = new MailAddress(_mailAdress);
                eposta.To.Add(new MailAddress(toEmailAddress));
                eposta.Subject = subject;
                eposta.Body = body;
                eposta.IsBodyHtml = true;


                SmtpClient smtp = new SmtpClient();

                smtp.Credentials = new System.Net.NetworkCredential(_mailAdress, _mailPassword);
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                smtp.Send(eposta);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception caught in SendEmail(): {ex.ToString()}");
            }


        }

        private static readonly Byte[] _privateKey = new Byte[] { 0xDE, 0xAD, 0xBE, 0xEF }; // NOTE: You should use a private-key that's a LOT longer than just 4 bytes.
        private static readonly TimeSpan _passwordResetExpiry = TimeSpan.FromMinutes(5);
        private const Byte _version = 1; // Increment this whenever the structure of the message changes.
    }
}
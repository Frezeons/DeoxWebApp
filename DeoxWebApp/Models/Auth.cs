using System;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Web.Services;

namespace DeoxWebApp.Models
{
    public class Auth
    {
        DataLayer _dataLayer = new DataLayer();

        public bool Usernameauth(string username)
        {
            string query = "SELECT COUNT(*) FROM Users WHERE USERNAME = '" + username + "'";

            var user = _dataLayer.Select(query).Tables[0];

            return user.Rows.Count > 0;
        }

        public bool Passwordauth(string username, string password)
        {
            Register register = new Register();
            string query = @"SELECT HASH_PASS FROM Users WHERE USERNAME = '" + username + "'";
            var user = _dataLayer.Select(query).Tables[0];
            if (user != null && user.Rows[0][0].ToString() != "")
            {
                string storedPasswordHash = user.Rows[0][0].ToString();
                string inputPasswordHash = register.EncryptMD5Function(password, "password");
                return storedPasswordHash == inputPasswordHash;
            }
            return false;
        }

        public bool EmailVerifiedAccount(string username)
        {
            string queryID = "select UserID from Users where Username='" + username + "'";
            var UserID = _dataLayer.Select(queryID);
            string queryemail = "select IsEmailVerify from VerifyRequest WHERE UserID=" + UserID + ";";
            DataSet isemailcheckedtemp = _dataLayer.Select(queryemail);
            DataTable dt = isemailcheckedtemp.Tables[0];
            var isemailcheckedtemp1 = dt.Rows[0][0];
            int isemailchecked = Convert.ToInt32(isemailcheckedtemp1);
            if (isemailchecked != 1)
            {
                return false;
            }
            return true;
        }

        public bool AdminVerifiedAccount(string username)
        {
            string queryID = "select ID from Users where Username='" + username + "'";
            DataSet useridtemp = _dataLayer.Select(queryID);
            DataTable dt = useridtemp.Tables[0];
            var useridtemp1 = dt.Rows[0][0];
            int UserID = Convert.ToInt32(useridtemp1);

            string queryrole = "select UserRole from Users where Username='" + username + "'";
            DataSet roletemp = _dataLayer.Select(queryrole);
            DataTable dt2 = roletemp.Tables[0];
            var roletemp1 = dt2.Rows[0][0];
            int UserRole = Convert.ToInt32(roletemp1);

            string queryadmin = "select IsAdminVerify from VerifyRequest WHERE UserID=" + UserID + ";";
            DataSet isadmincheckedtemp = _dataLayer.Select(queryadmin);
            DataTable dt1 = isadmincheckedtemp.Tables[0];
            var isadmincheckedtemp1 = dt1.Rows[0][0];
            int isadminchecked = Convert.ToInt32(isadmincheckedtemp1);

            if (isadminchecked == 1 || UserRole != 0)
            {
                return true;
            }
            return false;

        }

        public bool VerificationUser(string username, string password)
        {   
            bool isUserValid = AuthenticateUser(username, password);
            bool isVerifiedUser = AdminVerifiedAccount(username);

            return isUserValid && isVerifiedUser;
        }

        public bool AuthenticateUser(string username, string password)
        {
            bool isUsernameValid = Usernameauth(username);
            bool isPasswordValid = Passwordauth(username, password);
            
            return isUsernameValid && isPasswordValid;
        }

        public SessionBuffer.AppUser GetUserInfo(string szUserName)
        {
            string query = "select * from users where EMAIL='" + szUserName + "'";
            var info = _dataLayer.Select(query).Tables[0];
            SessionBuffer.AppUser user = new SessionBuffer.AppUser();
            if (info != null && info.Rows.Count > 0)
            {
                DataRow row = info.Rows[0];
                user.Mail = row["EMAIL"].ToString();
                user.UserId = Convert.ToInt32(row["ID"].ToString());
                user.UserName = row["USERNAME"].ToString();

            }

            return user;
        }
    }

}

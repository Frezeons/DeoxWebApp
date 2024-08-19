using DeoxWebApp.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using static SessionBuffer;
using static System.Collections.Specialized.BitVector32;

public class SessionBuffer
{
    private const string _SessionKey = "SessionBuffer";
    DataLayer _dataLayer = null;


    public class AppUser
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string RoleName { get; set; }
        public string Mail { get; set; }
        public int RoleId { get; set; }
    }

    public void GetAppUser(string szUserName)
    {
        ApplicationUSer(szUserName);

    }
    private void ApplicationUSer(string userName)
    {
        _dataLayer = new DataLayer();
        var user = _dataLayer.Select("Select * From Users as u left join Roles as r on u.USERROLE = r.ROLEID where USERNAME = '" + userName + "'").Tables[0];

        if (user != null)
        {
            var appuser = (new AppUser
            {
                UserName = user.Rows[0][2].ToString(),
                RoleName = user.Rows[0][7].ToString(),
                RoleId = Convert.ToInt32(user.Rows[0][6])
            });

            HttpContext.Current.Session[_SessionKey] = appuser;
        }
    }

    public AppUser GetUserLoginInfo()
    {
        return HttpContext.Current.Session[_SessionKey] as AppUser;
    }
    public void LogOut()
    {
        // Oturum bilgilerini temizleme
        HttpContext.Current.Session[_SessionKey] = null;
        HttpContext.Current.Session.Clear();
        HttpContext.Current.Session.Abandon();
    }

    public void UserRole(string CurrrentUsername, bool bIsTrue) // admin 3, moderator 2, user 1
    {
        DataLayer _dataLayer = new DataLayer();
        var Username = HttpContext.Current.Session[_SessionKey];
        var CurrentUser = _dataLayer.Select("select userrole, username from Users where USERNAME='" + Username + "'").Tables[0];

        if (CurrentUser.Rows.Count > 0)
        {
            var userRole = Convert.ToInt32(CurrentUser.Rows[0]["userrole"]);
            string roleName = string.Empty;

            switch (userRole)
            {
                case 3:
                    roleName = "admin";
                    break;
                case 2:
                    roleName = "moderator";
                    break;
                case 1:
                    roleName = "user";
                    break;
                default:
                    roleName = "unknown";
                    break;
            }

        }



    }
    public int GetUserRole(string CurrrentUsername) // admin 3, moderator 2, user 1
    {
        DataLayer _dataLayer = new DataLayer();
        var Username = HttpContext.Current.Session[_SessionKey];
        var CurrentUser = _dataLayer.Select("select userrole, username from Users where USERNAME='" + CurrrentUsername + "';").Tables[0];

        var userRole = Convert.ToInt32(CurrentUser.Rows[0]["userrole"]);
        return userRole;
        //roller tablosuna sorgu atacaksın

    }
}




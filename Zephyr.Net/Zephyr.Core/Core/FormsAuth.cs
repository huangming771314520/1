﻿/*************************************************************************
 * 文件名称 ：FormsAuth.cs                          
 * 描述说明 ：窗体登陆验证
 * 
 * 创建信息 : create by shiruizhi@qq.com on 2012-11-10 (施睿智)
 * 修订信息 : 
 **************************************************************************/

using System;
using System.Web;
using System.Web.Security;
using Newtonsoft.Json;
using Zephyr.Utils;

namespace Zephyr.Core
{
    public static class FormsAuth
    {
        public static void SignIn(string loginName, object userData, int expireMin)
        {
            var data = JsonConvert.SerializeObject(userData);

            //创建一个FormsAuthenticationTicket，它包含登录名以及额外的用户数据。
            var ticket = new FormsAuthenticationTicket(2,
                loginName, DateTime.Now, DateTime.Now.AddDays(1), true, data);

            //加密Ticket，变成一个加密的字符串。
            var cookieValue = FormsAuthentication.Encrypt(ticket);

            //根据加密结果创建登录Cookie
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, cookieValue)
                {
                    HttpOnly = true,
                    Secure = FormsAuthentication.RequireSSL,
                    Domain = FormsAuthentication.CookieDomain,
                    Path = FormsAuthentication.FormsCookiePath
                };
            if (expireMin > 0)
                cookie.Expires = DateTime.Now.AddMinutes(expireMin);

            var context = HttpContext.Current;
            if (context == null)
                throw new InvalidOperationException();

            //写登录Cookie
            context.Response.Cookies.Remove(cookie.Name);
            context.Response.Cookies.Add(cookie);
        }

        public static void SingOut()
        {
            FormsAuthentication.SignOut();
        }

        //public static LoginerBase GetUserData()
        //{
        //    return GetUserData<LoginerBase>();
        //}

        public static LoginerBase GetUserData()
        {
            return GetUserData<LoginerBase>();
        }

        public static T GetUserData<T>() where T : class,new()
        {
            var UserData = new T();
            var context = HttpContext.Current;

            var currentCookies = context.Request.Cookies["SessionKey"];
            if (currentCookies!=null)
            {
                var key = currentCookies.Value;
                if (!string.IsNullOrWhiteSpace(key))
                {
                    dynamic cache = CacheHelper.GetCache(key);
                    if (cache != null)
                    {
                        UserData = JsonConvert.DeserializeObject<LoginerBase>(cache);
                    }
                }
            }
            else
            {
                try
                { 
                    var cookie = context.Request.Cookies[FormsAuthentication.FormsCookieName];
                    var ticket = FormsAuthentication.Decrypt(cookie.Value);
                    UserData = JsonConvert.DeserializeObject<T>(ticket.UserData);
                }
                catch
                { }
            } 
            return UserData;
        }

        //public static T GetUserData<T>() where T : class, new()
        //{
        //    var UserData = new T();

        //    try
        //    {
        //        var context = HttpContext.Current;
        //        var cookie = context.Request.Cookies[FormsAuthentication.FormsCookieName];
        //        var ticket = FormsAuthentication.Decrypt(cookie.Value);
        //        UserData = JsonConvert.DeserializeObject<T>(ticket.UserData);
        //    }
        //    catch
        //    { }
        //    return UserData;
        //}
    }
}

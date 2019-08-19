using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace Zephyr.Areas
{
    public class SSOAuthAttribute : ActionFilterAttribute
    {
        private bool IsCheck = true;
        public SSOAuthAttribute(bool _IsCheck)
        {
            IsCheck = _IsCheck;
        }

        public const string SessionKey = "SessionKey";
        public const string SessionUserCode = "SessionUserCode";
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (IsCheck)
            {
                var cookieSessionkey = "";
                var cookieSessionUserCode = "";

                //SessionKey by QueryString
                if (filterContext.HttpContext.Request.QueryString[SessionKey] != null)
                {
                    cookieSessionkey = filterContext.HttpContext.Request.QueryString[SessionKey];
                    var current_cookie = new HttpCookie(SessionKey, cookieSessionkey);
                    current_cookie.Expires = DateTime.Now.AddMinutes(60);
                    filterContext.HttpContext.Response.Cookies.Add(current_cookie);
                }

                //SessionUserName by QueryString
                if (filterContext.HttpContext.Request.QueryString[SessionUserCode] != null)
                {
                    cookieSessionUserCode = filterContext.HttpContext.Request.QueryString[SessionUserCode];
                    var current_cookie = new HttpCookie(SessionUserCode, cookieSessionUserCode);
                    current_cookie.Expires = DateTime.Now.AddMinutes(60);
                    filterContext.HttpContext.Response.Cookies.Add(current_cookie);
                }

                //从Cookie读取SessionKey
                if (filterContext.HttpContext.Request.Cookies[SessionKey] != null)
                {
                    cookieSessionkey = filterContext.HttpContext.Request.Cookies[SessionKey].Value;
                }

                //从Cookie读取SessionUserName
                if (filterContext.HttpContext.Request.Cookies[SessionUserCode] != null)
                {
                    cookieSessionUserCode = filterContext.HttpContext.Request.Cookies[SessionUserCode].Value;
                }

                if (string.IsNullOrEmpty(cookieSessionkey) || string.IsNullOrEmpty(cookieSessionUserCode))
                {
                    //直接登录
                    filterContext.Result = SsoLoginResult(cookieSessionUserCode);
                }
                else
                {
                    //验证、当前请求的完整 URL。//filterContext.HttpContext.Request.RawUrl
                    //验证memcache缓存是否存在该cookieSessionkey的缓存，不存在或过期则返回false重新登录
                    var result = CheckLogin(cookieSessionkey, filterContext.HttpContext.Request.RawUrl);
                    if (result == false)
                    {
                        //会话丢失，跳转到登录页面
                        filterContext.Result = SsoLoginResult(cookieSessionUserCode);
                    }
                }
            }
            base.OnActionExecuting(filterContext);
        }

        public static bool CheckLogin(string sessionKey, string remark = "")
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(ConfigurationManager.AppSettings["SSOPassport"])
            };

            var requestUri = string.Format("api/Passport?sessionKey={0}&remark={1}", sessionKey, remark);

            try
            {
                var resp = httpClient.GetAsync(requestUri).Result;

                resp.EnsureSuccessStatusCode();

                return resp.Content.ReadAsAsync<bool>().Result;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private static ActionResult SsoLoginResult(string username)
        {
            return new RedirectResult(string.Format("{0}/Passport?appkey={1}&usercode={2}",
                    ConfigurationManager.AppSettings["SSOPassport"],
                    ConfigurationManager.AppSettings["SSOAppKey"],
                    username));
        }
    }
}
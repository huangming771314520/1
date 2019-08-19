using System;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Zephyr.Core;
using Zephyr.Models;
using Zephyr.Utils;
using Zephyr.Areas;
using Zephyr.Areas.Areas.Mms.Common;
using System.Configuration;

namespace Zephyr.Controllers
{
    [AllowAnonymous]
    [MvcMenuFilter(false)]
    public class LoginController : Controller
    {
        public ActionResult Index_Delete()
        {
            return Mms();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Mms()
        {
            ViewBag.CnName = "航鹏化动公司智能制造平台";
            ViewBag.EnName = "Logistics of Information System";
            return View("Index");
        }

        public ActionResult Psi() 
        {
            ViewBag.CnName = "航鹏化动公司智能制造平台";
            ViewBag.EnName = "Purchase-Sales-Inventory Management System";
            ViewBag.EnNameStyle = "left:298px;";
            return View("Index");
        }

        public JsonResult DoAction(JObject request)
        {
            var message = new sys_userService().Login(request);
            return Json(message, JsonRequestBehavior.DenyGet);
        }

        public ActionResult Logout()
        {
            FormsAuth.SingOut();
            //ZCookies.DelCookie("SessionKey");
            //ZCookies.DelCookie("SessionUserCode");
            //return new RedirectResult(string.Format("{0}/Passport?appkey={1}&usercode={2}",
            //        ConfigurationManager.AppSettings["SSOPassport"],
            //        ConfigurationManager.AppSettings["SSOAppKey"],
            //        ""));
            return Redirect("~/Login");
        }
    }
}

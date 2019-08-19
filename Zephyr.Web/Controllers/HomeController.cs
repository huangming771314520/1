using System;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Zephyr.Core;
using Zephyr.Models;
using Zephyr.Areas;

namespace Zephyr.Controllers
{
    [MvcMenuFilter(false)]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var loginer = FormsAuth.GetUserData<LoginerBase>();

            ViewBag.Title = "航鹏化动公司智能制造平台";
            ViewBag.UserCode = loginer.UserCode;
            ViewBag.UserName = loginer.UserName;
            ViewBag.Settings = new sys_userService().GetCurrentUserSettings();
            ViewBag.CurrentDate = DateTime.Now.Year;
            ViewBag.TableName = "";
            return View();
        }

        public ActionResult Error()
        {
            return View();
        }

        public void Download()
        {
            Exporter.Instance().Download();
        }
    }
}

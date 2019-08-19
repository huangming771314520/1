using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using log4net;
using Zephyr.Core;
//using SignalR.Routing;
using Zephyr.Areas.CommonWrap.SendEmail;

namespace Zephyr.Areas
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    public class WebApiApplication : System.Web.HttpApplication
    {
         
        protected void Application_Start()
        {

            //EmailManage.sendMail("测试title", @"E:\MES新框架拓展[20170829]\MES新框架拓展\WebCode\QHD_Framework\Zephyr.Web\Upload\品质明细表.xlsx", "测试内容");
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles); 
            FrameworkConfig.Register();
            GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
        } 
    } 
}

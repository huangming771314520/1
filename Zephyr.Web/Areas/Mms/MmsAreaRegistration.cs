using System.Web.Mvc;
using System.Web.Http;
using Zephyr.Areas;
using System.Web.Routing;

namespace Zephyr.Areas.Mms
{
    public class MmsAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Mms";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            //加在这里 
            //RouteTable.Routes.MapConnection<connection>("echo", "mms/echo/{*operation}");
             

            context.MapRoute(
                this.AreaName + "default",
                this.AreaName + "/{controller}/{action}/{id}",
                new { area = this.AreaName, controller = "Home", action = "Index", id = UrlParameter.Optional },
                new string[] { "Zephyr.Areas."+ this.AreaName + ".Controllers" }
            );

            context.MapRoute(
              this.AreaName + "other", // 路由名称  
             "other/" + this.AreaName + "/{controller}/{action}/{uid}_{token}_{others}", // 带有参数的 URL  
              new { controller = "Home", action = "Index", uid = UrlParameter.Optional, token = UrlParameter.Optional, others = UrlParameter.Optional } // 参数默认值  
            ); 

            GlobalConfiguration.Configuration.Routes.MapHttpRoute(
                this.AreaName + "Api",
                "api/" + this.AreaName + "/{controller}/{action}/{id}",
                new { area = this.AreaName, action = RouteParameter.Optional, id = RouteParameter.Optional, namespaceName = new string[] { string.Format("Zephyr.Areas.{0}.Controllers", this.AreaName) } },
                new { action = new StartWithConstraint() }
            ); 
        }
    }
}

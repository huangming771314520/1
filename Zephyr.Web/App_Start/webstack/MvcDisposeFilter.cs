using System;
using System.Web;
using System.Web.Mvc;

namespace Zephyr.Areas
{
    public class MvcDisposeFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            GC.Collect();
        }
    }
}
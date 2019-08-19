using System.Web;
using System.Web.Mvc;

namespace Zephyr.Areas
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new SSOAuthAttribute(true));
            //filters.Add(new HandleErrorAttribute());
            filters.Add(new MvcHandleErrorAttribute());
            //filters.Add(new AuthorizeAttribute());
            filters.Add(new MvcDisposeFilter());
            filters.Add(new MvcMenuFilter());
        }
    }
}
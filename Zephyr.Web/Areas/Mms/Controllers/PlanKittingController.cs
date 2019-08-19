using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using Zephyr.Areas.Areas.Mms.Common;
using Zephyr.Core;
using Zephyr.Models;

namespace Zephyr.Areas.Mms.Controllers
{
    public class PlanKittingController : Controller
    {
        public ActionResult ConfigPage()
        {
            return View();
        }

        public ActionResult ReportPage()
        {
            return View();
        }

    }

    public class PlanKittingApiController : ApiController
    {




        public dynamic GetConfigPageBomTreeGrid(string PartCode, string VersionCode)
        {
            if (PartCode == "0")
            {
                return null;
            }
            else
            {
                return new PRS_Process_BOMService().GetPlanKitting_ConfigPage_BomTreeGrid(PartCode);
            }
        }

        public dynamic GetUpdatePlanKittingConfig(int id, int assembling, int blanking, int machining, int welding)
        {
            try
            {
                var result = new PlanKittingService().GetUpdatePlanKittingConfig(id, assembling, blanking, machining, welding);

                return new
                {
                    Result = result,
                    Msg = result ? @"设置成功！" : @"设置失败！"
                };
            }
            catch (Exception ex)
            {
                return new
                {
                    Result = false,
                    Msg = ex.Message
                };
            }
        }

        public dynamic GetUpdatePlanKittingConfig(int id, string name, int value)
        {
            try
            {
                var result = new PlanKittingService().GetUpdatePlanKittingConfig(id, name, value);

                return new
                {
                    Result = result,
                    Msg = result ? @"设置成功！" : @"设置失败！"
                };
            }
            catch (Exception ex)
            {
                return new
                {
                    Result = false,
                    Msg = ex.Message
                };
            }
        }

        public dynamic GetReportPageBomTreeGrid(string PartCode, string VersionCode)
        {
            if (PartCode == "0")
            {
                return null;
            }
            else
            {
                return new PRS_Process_BOMService().GetPlanKitting_ReportPage_BomTreeGrid(PartCode);
            }
        }




    }
}
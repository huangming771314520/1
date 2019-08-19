using System;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Dynamic;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using Zephyr.Areas.Areas.Mms.Common;
using Zephyr.Core;
using Zephyr.Models;
using System.Web;
using System.Linq;

namespace Zephyr.Areas.Mms.Controllers
{
    public class MES_OpreatorWorkingLogController : Controller
    {
        //
        // GET: /Mms/MES_OpreatorWorkingLog/

        public ActionResult Index()
        {
            ViewBag.WorkGroupData = new MES_OpreatorWorkingLogService().GetWorkGroupData();

            return View();
        }

    }

    public class MES_OpreatorWorkingLogApiController : ApiController
    {
        public dynamic GetLogsByKeyword(string workGroup, string beginDate, string finishDate)
        {
            DateTime newBeginDate, newFinishDate;

            DateTime.TryParse(beginDate, out newBeginDate);
            DateTime.TryParse(finishDate, out newFinishDate);

            return new MES_OpreatorWorkingLogService().GetProduceLogInfo(workGroup, newBeginDate.Equals(DateTime.MinValue) ? (DateTime?)null : newBeginDate, newFinishDate.Equals(DateTime.MinValue) ? (DateTime?)null : newFinishDate);
        }

        public dynamic PostInspectionReport()
        {
            var strArrPar = HttpContext.Current.Request["arrPar"];

            if (string.IsNullOrEmpty(strArrPar))
            {
                return new ResultModel()
                {
                    Result = false,
                    Msg = @"请选择需要报检的数据！"
                };
            }

            string[] strCodeAndNums = strArrPar.Split(',');

            long n = 0;
            foreach (var item in strCodeAndNums)
            {
                string[] arrCodeAndNum = item.Split('|');

                var result = new MES_OpreatorWorkingLogService().InspectionReport(arrCodeAndNum[0], Convert.ToInt32(arrCodeAndNum[1]));
                if (result.Result)
                {
                    n++;
                }
            }

            return new ResultModel()
            {
                Result = n > 0,
                Msg = n.Equals(0) ? string.Format(@"{0}条数据报检单生成失败！", strCodeAndNums.Length) : null
            };

        }

    }

}

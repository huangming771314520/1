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
    public class BarcodeManagementController : Controller
    {
        //
        // GET: /Mms/BarcodeManagement/

        public ActionResult Index()
        {
            return View();
        }

    }

    public class BarcodeManagementApiController : ApiController
    {
        #region 人员条码
        //人员条码管理页面API

        public dynamic GetDepartmentList()
        {
            return new BarcodeManagementService().GetDepartmentList();
        }

        public dynamic GetUserList(string uName, string dCode)
        {
            int page = Convert.ToInt32(HttpContext.Current.Request["page"]);
            int rows = Convert.ToInt32(HttpContext.Current.Request["rows"]);

            return new BarcodeManagementService().GetUserList(uName, dCode, page, rows);
        }

        public dynamic PostBatchBarcodeToUser()
        {
            string uIDs = HttpContext.Current.Request["uIDs"];

            if (string.IsNullOrEmpty(uIDs))
            {
                return new
                {
                    Result = false,
                    Msg = @"请选择要生成条码的人员"
                };
            }

            try
            {
                string[] strArr = uIDs.Split(',');
                List<int> uIDList = strArr.Select(item =>
                {
                    return Convert.ToInt32(item);
                }).ToList();

                return new BarcodeManagementService().GetUpdateUserBarCodeByUID(uIDList);
            }
            catch
            {
                return new
                {
                    Result = false,
                    Msg = @"参数错误！"
                };
            }
        }

        #endregion

        #region 零件条码
        //零件管理页面API

        public dynamic GetPartByPCodeAndPType(string pName, string pFigureNo)
        {
            int page = Convert.ToInt32(HttpContext.Current.Request["page"]);
            int rows = Convert.ToInt32(HttpContext.Current.Request["rows"]);

            return new BarcodeManagementService().GetPartByPCodeAndPType(pName, pFigureNo, page, rows);
        }

        public dynamic PostBatchBarcodeToPart()
        {
            string pIDs = HttpContext.Current.Request["pIDs"];

            if (string.IsNullOrEmpty(pIDs))
            {
                return new
                {
                    Result = false,
                    Msg = @"请选择要生成条码的零件"
                };
            }

            try
            {
                string[] strArr = pIDs.Split(',');
                List<int> pIDList = strArr.Select(item =>
                {
                    return Convert.ToInt32(item);
                }).ToList();

                return new BarcodeManagementService().GetUpdatePartBarCodeByPID(pIDList);
            }
            catch
            {
                return new
                {
                    Result = false,
                    Msg = @"参数错误！"
                };
            }
        }


        #endregion

    }

}

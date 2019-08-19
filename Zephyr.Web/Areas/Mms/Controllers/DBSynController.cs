using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using Zephyr.Core;
using Zephyr.Models;

namespace Zephyr.Areas.Mms.Controllers
{
    public class DBSynController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }

    public class DBSynApiController : ApiController
    {
        public dynamic GetDBSyn(int type)
        {
            List<int> types = new List<int>() { 1, 2 };

            if (types.Contains(type))
            {
                switch (type)
                {
                    case 1:
                        return new SYS_PartTypeService().NewDeptSyn();
                    case 2:
                        return new SYS_PartTypeService().NewUserSyn();
                    default:
                        throw new Exception("参数错误！");
                }
            }
            else
            {
                return new ResultModel()
                {
                    Result = false,
                    Msg = @"参数错误！"
                };
            }


            //int res = 0;
            //switch (type)
            //{
            //    case "1":
            //        res = new SYS_PartTypeService().UserSyn();
            //        break;
            //    case "2":
            //        res = new SYS_PartTypeService().DepartSyn();
            //        break;
            //    case "3":
            //        res = new SYS_PartTypeService().OrganizeMapSyn();
            //        break;
            //    default:
            //        break;
            //}
            //return res;
        }

    }
}

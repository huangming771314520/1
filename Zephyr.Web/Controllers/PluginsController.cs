using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Zephyr.Core;
using Zephyr.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Collections.Specialized;
using Zephyr.Areas;
using Zephyr.Utils;
using Zephyr.Areas.CommonWrap;
using System.Web.Http;

namespace Zephyr.Controllers
{
    [MvcMenuFilter(false)]
    public class PluginsController : Controller
    {
        //
        // GET: /Plugins/
        public ActionResult Lookup()
        {
            return View();
        }
        public ActionResult GetLookupData(string index)
        {
            var type = Request.QueryString["_lookupType"].Split('.');
            var requestData = new NameValueCollection(Request.QueryString);

            var xmlPath = string.Format("~/Views/Shared/Xml/{0}.xml", type[type.Length - 1]);
            if (type.Length > 1)
                xmlPath = string.Format("~/Areas/{0}/Views/Shared/Xml/{1}.xml", type);

            var das = RequestWrapper.Instance().LoadSettingXml(xmlPath);
            var query = das.SetRequestData(requestData).ToParamQuery();

            var valueField = das["_valueFeild"];
            if (!string.IsNullOrEmpty(valueField))
                query.ClearWhere().AndWhere(das.getFieldName(valueField, true), string.Format("'{0}'", das[valueField].Replace(",", "','")), Cp.In);

            var service = das.GetService();
            var data = service.GetDynamicListWithPaging(query);

            var json = JsonConvert.SerializeObject(data);
            return Content(json, "application/json");
        }

        public ActionResult GetCommonDialogData()
        {
            //获取url中的条件，注意requestData是弹出框中的查询条件，当动态添加的查询条件不在弹出框中时，需要先将requestData中的属性删除
            var requestData = new NameValueCollection(Request.QueryString);
            //获取删选标识字段名
            string PrimaryID = requestData["PrimaryID"];
            //去掉弹出框中不需要的查询条件
            requestData.Remove("xmlName");
            //
            var xmlName = Request.QueryString["xmlName"].ToString();
            //var selectID = Request.QueryString["selectID"].ToString();
            XElement targetXml = CommonSearchDialog.GetSearchXml(xmlName);
            var settingsXml = targetXml.Element("settings");
            var setting = RequestWrapper.Instance().LoadSettingXmlString(settingsXml.ToString());

            if (Request.QueryString["PrimaryID"] != null)
            {
                requestData.Remove("PrimaryID");
            }

            if (!string.IsNullOrWhiteSpace(PrimaryID))
            {
                requestData.Remove(PrimaryID);
            }

            var query = setting.SetRequestData(requestData).ToParamQuery();

            if (!string.IsNullOrWhiteSpace(PrimaryID))
            {
                string NotInList = Request.QueryString[PrimaryID].ToString();
                if (!string.IsNullOrEmpty(NotInList))
                {
                    query.AndWhere(PrimaryID, NotInList, Cp.NotIn);
                }

            }
            var service = setting.GetService();
            var data = service.GetDynamicListWithPaging(query);
            //if (Request.QueryString["selectID"].ToString() != "")
            //{
            //    if (data.Count() > 0)
            //    {
            //        //string sql=string.Format(@"")
            //        data = data.Where(i => !selectID.Contains(i.ID.toString())).ToList();
            //    }
            //}
            var json = JsonConvert.SerializeObject(data);
            return Content(json, "application/json");
        }
        public ActionResult GetCommonDialogData1()
        {
            var requestData = new NameValueCollection(Request.QueryString);
            requestData.Remove("xmlName");
            var selectID = Request.QueryString["selectID"].ToString();
            var xmlName = Request.QueryString["xmlName"].ToString();
            XElement targetXml = CommonSearchDialog.GetSearchXml(xmlName);
            var settingsXml = targetXml.Element("settings");
            var setting = RequestWrapper.Instance().LoadSettingXmlString(settingsXml.ToString());
            var query = setting.SetRequestData(requestData).ToParamQuery();

            var service = setting.GetService();
            var data = service.GetDynamicListWithPaging(query);
            var json = JsonConvert.SerializeObject(data);
            return Content(json, "application/json");
        }
    }

    public class PluginsApiController : ApiController
    {
        
    }
}

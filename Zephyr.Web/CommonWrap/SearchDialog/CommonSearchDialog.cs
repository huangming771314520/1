using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using Zephyr.Core;
using Zephyr.Utils;

namespace Zephyr.Areas.CommonWrap
{
    public class CommonSearchDialog
    {
        public static XElement GetSearchXml(string xmlName)
        {
            List<string> list = ReadExcel.GetDirectoryFiles("~/Views/Shared/SearchDialogXml/");
            XElement targetXml = null;
            foreach (var item in list)
            {
                XElement settingXml = XElement.Load(item);
                List<XElement> excelList = settingXml.Elements("searchDialog").Where(e => e.Attribute("name").Value.Equals(xmlName)).ToList();
                if (excelList.Count > 0)
                {
                    targetXml = excelList.First();
                    break;
                }
            }
            if (targetXml == null)
            {
                throw new Exception("未配置此xml文本！");
            }
            return targetXml;
            //List<XElement> fieldList = targetXml.Element("ColumnList").Elements("column").ToList();
        }

        public static string GetSelectList(string type,XElement element)
        {
            string jsonData = "";
            if (type == "fix")
            {
                var items = element.Elements("item").Select(i => new { value = i.Attribute("value").Value, text = i.Value }).ToList();
                jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(items);
            }
            if (type == "dictionary")
            {
                string CodeType = element.Value;
                List<dynamic> SysCodelist = new List<dynamic>();
                using (var db = Db.Context("Sys"))
                {
                    SysCodelist = db.Sql(string.Format("select Value as value,Text as text,CodeType as codetype from sys_code where CodeType='{0}'", CodeType)).QueryMany<dynamic>();
                }
                jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(SysCodelist);
            }
            if (type == "single")
            {
                var connName = ReadXml.getXmlElementAttr(element, "connName");
                var tableName = ReadXml.getXmlElementAttr(element, "tableName");
                var selectField = ReadXml.getXmlElementAttr(element, "selectField");
                var whereSql = ReadXml.getXmlElementAttr(element, "whereSql", "1=1");
                List<dynamic> SingleList = new List<dynamic>();
                using (var db = Db.Context(connName))
                {
                    string SearchSql = string.Format(@"select {0} from {1} where {2}", selectField, tableName, whereSql);
                    SingleList = db.Sql(SearchSql).QueryMany<dynamic>();
                }
                jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(SingleList);
            }
            return jsonData;
        }
    }
}
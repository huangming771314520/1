using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using Zephyr.Areas.CommonWrap;
using Zephyr.Core;
using Zephyr.Models;
using Zephyr.Utils;

namespace Zephyr.Areas.Controllers
{
    [MvcMenuFilter(false)]
    public class CommonsController : Controller
    {
        /// <summary>
        /// 通用combogrid查询控件API,可供多字段模糊查询，多字段首拼模糊查询
        /// </summary>
        /// <param name="keyword">键入关键字</param>
        /// <param name="tableName">需模糊查询表名</param>
        /// <param name="searchField">需模糊查询表字段名，多字段以","分割</param>
        /// <param name="firstFightField">需首拼模糊查询字段名，多字段以","分割</param>
        /// <param name="whereSql">查询条件设置</param>
        /// <param name="rowCount">查询数据行数</param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetCommonSearchList(string keyword = "", string tableName = "", string searchField = "", string firstFightField = "", string whereSql = "", string connName = "Mms", int rowCount = 0, string orderBy = "")
        {
            var requestData = new NameValueCollection(Request.QueryString);
            List<Dictionary<string, string>> list = new List<Dictionary<string, string>>();
            if (tableName != "" && searchField != "")
            {
                string FieldStr = "";
                string[] FieldList = searchField.Split(',');
                foreach (string item in FieldList)
                {
                    FieldStr += string.Format(" Lower({0}) like '%{1}%' or", item.Replace("distinct", ""), keyword.ToLower());
                }

                if (firstFightField != "")
                {
                    string[] FirstFightList = firstFightField.Split(',');
                    foreach (var item in FirstFightList)
                    {
                        FieldStr += string.Format(" dbo.[fun_getPY](Lower({0})) like '%{1}%' or", item, keyword.ToLower());
                    }
                }
                FieldStr = FieldStr.Substring(0, FieldStr.Length - 2);
                FieldStr = "(" + FieldStr + ")";
                string sqlStr = string.Format("select {0} from {1} where {2}", searchField, tableName, FieldStr);
                if (sqlStr.Contains("distinct"))
                {
                    if (rowCount > 0)
                    {
                        sqlStr = sqlStr.Insert(15, string.Format(@" top {0} ", rowCount));
                    }
                }
                else
                {
                    if (rowCount > 0)
                    {
                        sqlStr = sqlStr.Insert(6, string.Format(@" top {0} ", rowCount));
                    }
                }
                if (whereSql != "")
                {
                    sqlStr = sqlStr + " and " + whereSql;
                }
                if (orderBy != "")
                {
                    sqlStr = sqlStr + " order by " + orderBy;
                }
                using (var db = Db.Context(connName))
                {
                    DataTable SearchDT = db.Sql(sqlStr).QuerySingle<DataTable>();
                    foreach (DataRow dr in SearchDT.Rows)
                    {
                        Dictionary<string, string> dct = new Dictionary<string, string>();
                        foreach (DataColumn dc in SearchDT.Columns)
                        {
                            dct.Add(dc.ColumnName, dr[dc.ColumnName].ToString());
                        }
                        list.Add(dct);
                    }

                    var ExtendDic = new Dictionary<string, string>();
                    foreach (DataColumn dc in SearchDT.Columns)
                    {
                        ExtendDic.Add(dc.ColumnName,"");
                    }
                    list.Insert(0, ExtendDic);
                }
            }
            return Json(new { total = list.Count(), rows = list }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 通用combogrid查询控件API,可供多字段模糊查询，多字段首拼模糊查询，拓展了缓存机制
        /// </summary>
        /// <param name="keyword">键入关键字</param>
        /// <param name="tableName">需模糊查询表名</param>
        /// <param name="searchField">需模糊查询表字段名，多字段以","分割</param>
        /// <param name="firstFightField">需首拼模糊查询字段名，多字段以","分割</param>
        /// <param name="whereSql">查询条件设置</param>
        /// <param name="CacheKey">缓存键名称</param>
        /// <param name="CacheTime">设置缓存时长</param>
        /// <returns>返回模糊搜索json</returns>
        [HttpGet]
        public JsonResult GetCommonSearchList_Cache(string keyword = "", string tableName = "", string searchField = "", string firstFightField = "", string whereSql = "", string CacheKey = "", int CacheTime = 5, string connName = "Mms")
        {
            //ZCache.RemoveCache(CacheKey);
            var cache = ZCache.GetCache(CacheKey);//获取缓存DataTable
            DataTable CacheDataTable = new DataTable();

            if (cache == null)
            {
                DataTable SearchDT = GetComboGridList(tableName, searchField, firstFightField, whereSql, connName);
                ZCache.SetCache(CacheKey, SearchDT, DateTime.Now.AddMinutes(CacheTime), TimeSpan.Zero);//设置缓存DataTable
                CacheDataTable = SearchDT;
            }
            else
            {
                CacheDataTable = (DataTable)cache;
            }

            string SearchSelect = "1=1";
            if (searchField != "" && keyword != "") //如果查询字段不为空 并且 关键字keyword查询不为空 则模糊查询
            {
                SearchSelect = "";
                foreach (var item in searchField.Split(',')) //查询字段的拼接
                {
                    SearchSelect += string.Format(" {0} like '%{1}%' or", item, keyword);
                }
                if (firstFightField != "") //首拼字段的拼接
                {
                    for (int i = 0; i < firstFightField.Split(',').Length; i++)
                    {
                        SearchSelect += string.Format(" {0} like '%{1}%' or", "firstFight_" + i.ToString(), keyword);
                    }
                }
                SearchSelect = SearchSelect.Substring(0, SearchSelect.Length - 2);
            }

            DataTable SearchCacheDataRows = new DataTable();
            if (CacheDataTable.Select(SearchSelect).Count() > 0)
            {
                SearchCacheDataRows = CacheDataTable.Select(SearchSelect).Take(50).CopyToDataTable();
            }

            List<Dictionary<string, string>> combogridList = new List<Dictionary<string, string>>();
            foreach (DataRow dr in SearchCacheDataRows.Rows)
            {
                Dictionary<string, string> dct = new Dictionary<string, string>();
                foreach (DataColumn dc in SearchCacheDataRows.Columns)
                {
                    dct.Add(dc.ColumnName, dr[dc.ColumnName].ToString());
                }
                combogridList.Add(dct);
            }
            return Json(new { total = combogridList.Count(), rows = combogridList }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 通用combogrid查询控件API,可供多字段模糊查询，多字段首拼模糊查询
        /// </summary>
        /// <param name="keyword">键入关键字</param>
        /// <param name="tableName">需模糊查询表名</param>
        /// <param name="searchField">需模糊查询表字段名，多字段以","分割</param>
        /// <param name="firstFightField">需首拼模糊查询字段名，多字段以","分割</param>
        /// <param name="whereSql">查询条件设置</param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetCommonSearchLists(string keyword = "", string tableName = "", string searchField = "", string firstFightField = "", string whereSql = "", string connName = "Mms")
        {
            var requestData = new NameValueCollection(Request.QueryString);
            List<Dictionary<string, string>> list = new List<Dictionary<string, string>>();
            if (tableName != "" && searchField != "")
            {
                string FieldStr = "";
                string[] FieldList = searchField.Split(',');
                foreach (string item in FieldList)
                {
                    FieldStr += string.Format(" Lower({0}) like '%{1}%' or", item.Replace("distinct", ""), keyword.ToLower());
                }

                if (firstFightField != "")
                {
                    string[] FirstFightList = firstFightField.Split(',');
                    foreach (var item in FirstFightList)
                    {
                        FieldStr += string.Format(" dbo.[fun_getPY](Lower({0})) like '%{1}%' or", item, keyword.ToLower());
                    }
                }
                FieldStr = FieldStr.Substring(0, FieldStr.Length - 2);
                FieldStr = "(" + FieldStr + ")";
                string sqlStr = string.Format("select {0} from {1} where {2}", searchField, tableName, FieldStr);
                //if (sqlStr.Contains("distinct"))
                //{
                //    sqlStr = sqlStr.Insert(15, " top 50 ");
                //}
                //else
                //{
                //    sqlStr = sqlStr.Insert(6, " top 50 ");
                //}
                if (whereSql != "")
                {
                    sqlStr = sqlStr + " and " + whereSql;
                }
                using (var db = Db.Context(connName))
                {
                    DataTable SearchDT = db.Sql(sqlStr).QuerySingle<DataTable>();
                    foreach (DataRow dr in SearchDT.Rows)
                    {
                        Dictionary<string, string> dct = new Dictionary<string, string>();
                        foreach (DataColumn dc in SearchDT.Columns)
                        {
                            dct.Add(dc.ColumnName, dr[dc.ColumnName].ToString());
                        }
                        list.Add(dct);
                    }
                }
            }
            return Json(new { total = list.Count(), rows = list }, JsonRequestBehavior.AllowGet);
        }
        public DataTable GetComboGridList(string tableName = "", string searchField = "", string firstFightField = "", string whereSql = "", string connName = "Mms")
        {
            DataTable SearchDT = new DataTable();
            if (tableName != "" && searchField != "")
            {
                if (firstFightField != "")
                {
                    string[] FirstFightList = firstFightField.Split(',');
                    int FirstFightIndex = 0;
                    foreach (var item in FirstFightList)
                    {
                        searchField += string.Format(",dbo.[fun_getPY](Lower({0})) as firstFight_{1}", item, FirstFightIndex);
                        FirstFightIndex++;
                    }
                }
                if (whereSql == "")
                {
                    whereSql = "1=1";
                }
                string sqlStr = string.Format("select {0} from {1} where {2}", searchField, tableName, whereSql);
                using (var db = Db.Context(connName))
                {
                    SearchDT = db.Sql(sqlStr).QuerySingle<DataTable>();
                }
            }
            return SearchDT;
        }


        /// <summary>
        /// 通用combotree查询控件API，可根据节点显示值模糊查询
        /// </summary>
        /// <param name="tableName">无限分级表名</param>
        /// <param name="IdField">无限分级表ID值字段名</param>
        /// <param name="TextField">无限分级表文本值字段名</param>
        /// <param name="ParentIdField">无限分级表父级ID值字段名</param>
        /// <param name="whereSql">sql的where条件拼接</param>
        /// <returns></returns>
        [HttpGet]
        //[OutputCache(Duration = 180, VaryByParam = "tableName;IdField;TextField;ParentIdField;whereSql")]
        public ActionResult GetCommonComboTreeList(string tableName = "", string IdField = "", string TextField = "", string ParentIdField = "", string whereSql = "", string connName = "Mms")
        {
            var requestData = new NameValueCollection(Request.QueryString);
            JTreeNode jTreeNode = new JTreeNode() { id = "0", text = "全部" };
            string sqlStr = string.Format("select {0} as [IdField],{1} as [TextField],{2} as [ParentIdField] from {3}", IdField, TextField, ParentIdField, tableName);
            if (whereSql != "")
            {
                sqlStr += string.Format(" where {0}", whereSql);
            }

            List<TreeModel> treeList = new List<TreeModel>();
            using (var db = Db.Context(connName))
            {
                treeList = db.Sql(sqlStr).QueryMany<TreeModel>();
            }
            JTreeNode.ConstructJTreeNode(ref jTreeNode, treeList);
            var jsonResult = "[" + JTreeNode.ConvertToJson(jTreeNode) + "]";
            return Content(jsonResult);
        }

        public ActionResult CommonImportExcel_Templet(string xmlName)
        {
            var Excel_Templet_Url = "";
            List<string> list = ReadExcel.GetDirectoryFiles("~/Views/Shared/ImportExcelXml/");
            XElement targetXml = null;
            foreach (var item in list)
            {
                XElement settingXml = XElement.Load(item);
                List<XElement> excelList = settingXml.Elements("Excel").Where(e => e.Attribute("name").Value.Equals(xmlName)).ToList();
                if (excelList.Count > 0)
                {
                    targetXml = excelList.First();
                    Excel_Templet_Url = ReadXml.getXmlElementValue(targetXml, "ExcelTempletURL");
                    if (!string.IsNullOrWhiteSpace(Excel_Templet_Url))
                    {
                        Excel_Templet_Url = Excel_Templet_Url.Replace("\n", "").Replace(" ", "").Trim();
                    }
                }
            }
            return Content(Excel_Templet_Url);
        }
        [HttpPost]
        public string GetExcelData()
        {
            string XmlName = Request["xmlName"].ToString();

            ServiceBase service = new ServiceBase("Mms");
            var result = service.GetExcelData(XmlName);
            return JsonConvert.SerializeObject(result);
        }

        /// <summary>
        /// xml动态配置excel导入数据库
        /// </summary>
        [HttpPost]
        public ActionResult CommonImportExcel()
        {
            var result = new
            {
                status = true,
                msg = "导入成功"
            };
            try
            {
                string XmlName = Request["xmlName"].ToString();
                string ClassName = Request["className"].ToString();
                if (ClassName == "")
                {
                    ServiceBase service = new ServiceBase("Mms");
                    service.CommonImportExcel(XmlName);
                }
                else
                {
                    //获取testService类，并实例化该类后，调用方法CommonImportExcel
                    Type TypeModel = Type.GetType("Zephyr.Models." + ClassName);
                    MethodInfo mInfo = TypeModel.GetMethod("CommonImportExcel");
                    object obj = Activator.CreateInstance(TypeModel);
                    mInfo.Invoke(obj, new object[] { XmlName });
                }
            }
            catch (Exception ex)
            {
                result = new
                {
                    status = false,
                    msg = "导入失败：" + ex.InnerException.Message
                };
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [OutputCache(Duration = 300, VaryByParam = "CodeType")]
        public ActionResult GetComboboxList(string CodeType = "")
        {
            List<dynamic> SysCodelist = new List<dynamic>();
            using (var db = Db.Context("Sys"))
            {
                SysCodelist = db.Sql(string.Format("select Value as value,Text as text from sys_code where CodeType in('{0}')", CodeType)).QueryMany<dynamic>();
            }
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(SysCodelist);
            return Content(json);
        }
        public ActionResult GetComboboxListBillType(string ids = "")
        {
            List<dynamic> SysCodelist = new List<dynamic>();
            using (var db = Db.Context("Sys"))
            {
                SysCodelist = db.Sql(string.Format("select Value as value,Text as text from sys_code where CodeType='BillType' and value in({0})", ids)).QueryMany<dynamic>();
            }
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(SysCodelist);
            return Content(json);
        }
        /// <summary>
        /// 通用字典型数据grid格式化 带缓存
        /// </summary>
        /// <param name="CodeType"></param>
        /// <param name="ComboboxValue"></param>
        /// <returns></returns>
        public string GetComboboxFormatter(string CodeType = "", string ComboboxValue = "")
        {
            string result = "";
            string cacheKey = "dictionary_" + CodeType;
            var cache = ZCache.GetCache(cacheKey);//获取缓存DataTable
            if (cache == null)
            {
                List<dynamic> SysCodelist = new List<dynamic>();
                using (var db = Db.Context("Sys"))
                {
                    SysCodelist = db.Sql(string.Format("select Value as value,Text as text from sys_code where CodeType in('{0}')", CodeType)).QueryMany<dynamic>();
                }
                ZCache.SetCache(cacheKey, SysCodelist, DateTime.Now.AddMinutes(5), TimeSpan.Zero);//设置缓存DataTable
                cache = ZCache.GetCache(cacheKey);
            }
            var cacheList = (List<dynamic>)cache;
            if (cacheList.Count() > 0)
            {
                if (cacheList.Where(c => c.value == ComboboxValue).ToList().Count > 0)
                {
                    result = cacheList.Where(c => c.value == ComboboxValue).ToList().First().text;
                }
            }
            return result;
        }
        /// <summary>
        /// 通用单表型数据grid格式化 带缓存
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="IdField"></param>
        /// <param name="TextField"></param>
        /// <param name="ComboboxValue"></param>
        /// <param name="connName"></param>
        /// <returns></returns>
        public string GetComboboxFormatter_Single(string TableName = "", string IdField = "", string TextField = "", string ComboboxValue = "", string connName = "Mms")
        {
            var result = "";
            string cacheKey = TableName + "_" + IdField + "_" + TextField + "_" + connName;
            var cache = ZCache.GetCache(cacheKey);//获取缓存DataTable
            if (cache == null)
            {
                List<dynamic> SysCodelist = new List<dynamic>();
                using (var db = Db.Context(connName))
                {
                    SysCodelist = db.Sql(string.Format("select {0} as value,{1} as text from {2}", IdField, TextField, TableName)).QueryMany<dynamic>();
                }
                ZCache.SetCache(cacheKey, SysCodelist, DateTime.Now.AddMinutes(5), TimeSpan.Zero);//设置缓存DataTable
                cache = ZCache.GetCache(cacheKey);
            }
            var cacheList = (List<dynamic>)cache;
            if (cacheList.Count > 0)
            {
                if (cacheList.Where(c => c.value == ComboboxValue).ToList().Count > 0)
                {
                    result = cacheList.Where(c => c.value == ComboboxValue).ToList().First().text;
                }
            }
            return result;
        }


        public ActionResult GetTreeList()
        {
            NameValueCollection requestData = new NameValueCollection(Request.QueryString);
            string connName = requestData["connName"] ?? "Mms";
            string result = string.Empty;
            string tableName = requestData["tableName"].ToString();
            string IdField = requestData["IdField"].ToString();
            string TextField = requestData["TextField"].ToString();
            string ParentIdField = requestData["ParentIdField"].ToString();
            string ParentValue = requestData["ParentValue"].ToString();
            string fields = IdField + " as id," + TextField + " as text," + ParentIdField + " as pid";
            using (var db = Db.Context(connName))
            {
                string sql = string.Format("select {0} from {1} where {2}='{3}'", fields, tableName, ParentIdField, ParentValue);
                List<dynamic> list = db.Sql(sql).QueryMany<dynamic>();
                if (list.Count > 0)
                {
                    result = Newtonsoft.Json.JsonConvert.SerializeObject(list);
                }
            }
            return Content(result);
        }

        public ActionResult GetTreeLists()
        {
            NameValueCollection requestData = new NameValueCollection(Request.QueryString);
            string connName = requestData["connName"] ?? "Mms";
            string result = string.Empty;
            string tableName = requestData["tableName"].ToString();
            string IdField = requestData["IdField"].ToString();
            string TextField = requestData["TextField"].ToString();
            string ParentIdField = requestData["ParentIdField"].ToString();
            string ParentValue = requestData["ParentValue"].ToString();
            string shortFiled = requestData["shortFiled"];
            string productVerison = requestData["ProductVerison"];
            string verisonText = requestData["VerisonText"];
            string paramsParent = requestData["ParentMateCode"];
            string bomType = requestData["BomType"];

            string fields = IdField + " as id," + TextField + " as text," + ParentIdField + " as pid";
            using (var db = Db.Context(connName))
            {
                string sql;
                if (bomType == "1")
                {
                    sql = string.Format("select {0} from {1} where {2}='{3}' and BomType=1 ", fields, tableName, ParentIdField, ParentValue);
                }
                else if (bomType == "2")
                {
                    sql = string.Format("select {0} from {1} where {2}='{3}' and BomType=2 ", fields, tableName, ParentIdField, ParentValue);
                }
                else
                {
                    sql = string.Format("select {0} from {1} where {2}='{3}'", fields, tableName, ParentIdField, ParentValue);
                }
                sql = (productVerison != null) ? string.Format("select {0} from {1} where {2}='{3}' and {4}='{5}' ", fields, tableName, ParentIdField, ParentValue, verisonText, productVerison.ToString()) : sql;
                sql = sql.Replace("fxm", "+");
                sql = (shortFiled != null) ? string.Format(@"select * from ({0})a {1} ", sql, shortFiled.ToString()) : sql;
                List<dynamic> list = db.Sql(sql).QueryMany<dynamic>();
                if (list.Count > 0)
                {
                    result = Newtonsoft.Json.JsonConvert.SerializeObject(list);
                }
            }
            return Content(result);
        }

        public ActionResult GetTreeNodeList(TreeNodeModel model)
        {
            var list = TreeNodeManage.GetTreeNodeList<dynamic>(
                  TreeNodeManage.Instance()
                  .SetNode(model.NodeField)
                  .SetParentNode(model.ParentNodeField, model.ParentNodeValue)
                  .SetTableName(model.TableName)
                  .SetNodeLevel(model.IsLevel)
                  .SetTreeSetting(model.TreeSetting)
                  .SetWhereSql(model.WhereSql));
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Linq;
using Zephyr.Utils;

namespace Zephyr.Core
{
    public partial class ServiceBase<T> where T : ModelBase, new()
    {
        protected virtual void OnAfterHandleExcel(ref DataTable dtSheet)
        {

        }
        public void CommonImportExcel(string xmlName)
        {
            Logger("通用excel导入数据库", () =>
            {
                HttpPostedFile postFile = HttpContext.Current.Request.Files["ImportExcelFile"];
                string uploadPath = HttpContext.Current.Server.MapPath("~/Upload/ImportExcel/");
                if (!Directory.Exists(uploadPath)) //如果不存在此文件夹，则创建该文件夹
                    Directory.CreateDirectory(uploadPath);
                string fileName = postFile.FileName;
                fileName = "[" + DateTime.Now.ToString("yyyyMMddhhmmss") + "]" + fileName;
                string savePath = string.Format("{0}{1}", uploadPath, fileName);
                HttpContext.Current.Request.Files[0].SaveAs(savePath);
                DataTable dtSheet = ReadExcel.ExcelToDataTable(savePath);//读取的excel的DataTable
                List<string> list = ReadExcel.GetDirectoryFiles("~/Views/Shared/ImportExcelXml/");
                XElement targetXml = null;
                foreach (var item in list)
                {
                    XElement settingXml = XElement.Load(item);
                    List<XElement> excelList = settingXml.Elements("Excel").Where(e => e.Attribute("name").Value.Equals(xmlName)).ToList();
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
                //导入的数据库表名称
                var DataBase_TableName = targetXml.Element("tableName").Value.Replace("\n", "").Replace(" ", "").Trim();
                //转换字段字符串
                var FieldListStr = targetXml.Element("FieldList").Element("ExcelFields").Value.Replace("\n", "").Replace("[", "").Replace("]", "").Replace(" ", "").Trim();
                string[] ExcelFieldList = FieldListStr.Split(',');
                UpdateColumnsName(dtSheet, ExcelFieldList);
                IEnumerable<XElement> ConvertFieldList = targetXml.Element("FieldList").Element("ConvertFields").Elements("setting");
                if (ConvertFieldList.Count() > 0)
                {
                    //从字典表集合中匹配 dbo.sys_code和dbo.sys_codeType
                    List<XElement> dictionarySetting = ConvertFieldList.Where(con => con.Attribute("type").Value.Equals("dictionary")).ToList();
                    //从单表集合中匹配
                    List<XElement> singleSetting = ConvertFieldList.Where(con => con.Attribute("type").Value.Equals("single")).ToList();
                    //从固定写死的集合中匹配
                    List<XElement> fixSetting = ConvertFieldList.Where(con => con.Attribute("type").Value.Equals("fix")).ToList();
                    if (dictionarySetting.Count > 0)
                    {
                        //匹配xml中需要转换字段所有的dictionary集合
                        var dsStr = "'" + string.Join(",", dictionarySetting.Select(ds => ds.Value).ToList().ToArray()).Replace(",", "','") + "'";
                        //List<dynamic> SysCodelist = new sys_codeService().GetAllValueTextList(dsStr);
                        List<dynamic> SysCodelist = new List<dynamic>();
                        using (var db = Db.Context("Sys"))
                        {
                            SysCodelist = db.Sql(string.Format("select Value as value,Text as text,CodeType as codetype from sys_code where CodeType in({0})", dsStr)).QueryMany<dynamic>();
                        }
                        foreach (var dictionarySettingItem in dictionarySetting)
                        {
                            var field = dictionarySettingItem.Attribute("field").Value;
                            var CodeType = dictionarySettingItem.Value;
                            foreach (DataRow dtItem in dtSheet.Rows)
                            {
                                if (dtSheet.Columns.Contains(field))
                                {
                                    dtItem[field] = GetValueTextByType(SysCodelist, CodeType, dtItem[field].ToString());
                                }
                            }
                        }
                    }
                    if (fixSetting.Count > 0)
                    {
                        foreach (var fixSettingItem in fixSetting)
                        {
                            var field = fixSettingItem.Attribute("field").Value; // <setting type='fix' field='[HomeAddress]'>中的[HomeAddress]
                            List<XElement> fixList = fixSettingItem.Elements("item").ToList();
                            Dictionary<string, string> fixDic = new Dictionary<string, string>();
                            fixList.ForEach(fix =>
                            {
                                fixDic.Add(fix.Value, fix.Attribute("value").Value);
                            });
                            foreach (DataRow dtItem in dtSheet.Rows)
                            {
                                if (dtSheet.Columns.Contains(field))
                                {
                                    dtItem[field] = fixDic[dtItem[field].ToString()].ToString();
                                }
                            }
                        }
                    }
                    if (singleSetting.Count > 0)
                    {
                        foreach (var singleSettingItem in singleSetting)
                        {
                            var field = getXmlElementAttr(singleSettingItem, "field");
                            var connName = getXmlElementAttr(singleSettingItem, "connName");
                            var tableName = getXmlElementAttr(singleSettingItem, "tableName");
                            var selectField = getXmlElementAttr(singleSettingItem, "selectField");
                            var whereSql = getXmlElementAttr(singleSettingItem, "whereSql", "1=1");
                            DataTable SearchDT = new DataTable();
                            using (var db = Db.Context(connName))
                            {
                                string SearchSql = string.Format(@"select {0} from {1} where {2}", selectField, tableName, whereSql);
                                SearchDT = db.Sql(SearchSql).QuerySingle<DataTable>();
                            }
                            foreach (DataRow dtItem in dtSheet.Rows)
                            {
                                if (dtSheet.Columns.Contains(field))
                                {
                                    dtItem[field] = SearchDT.Select(string.Format("Text='{0}'", dtItem[field].ToString())).First()["Value"].ToString();
                                }
                            }

                        }
                    }
                }
                this.OnAfterHandleExcel(ref dtSheet);
                DataTableBulkCopy.BulkCopy(DataBase_TableName, dtSheet);
            });
        }

        public DataTable GetExcelData(string xmlName)
        {
            HttpPostedFile postFile = HttpContext.Current.Request.Files["ImportExcelFile"];
            string uploadPath = HttpContext.Current.Server.MapPath("~/Upload/ImportExcel/");
            if (!Directory.Exists(uploadPath)) //如果不存在此文件夹，则创建该文件夹
                Directory.CreateDirectory(uploadPath);
            string fileName = postFile.FileName;
            fileName = "[" + DateTime.Now.ToString("yyyyMMddhhmmss") + "]" + fileName;
            string savePath = string.Format("{0}{1}", uploadPath, fileName);
            HttpContext.Current.Request.Files[0].SaveAs(savePath);
            DataTable dtSheet = ReadExcel.ExcelToDataTable(savePath);//读取的excel的DataTable
            List<string> list = ReadExcel.GetDirectoryFiles("~/Views/Shared/ImportExcelXml/");
            XElement targetXml = null;
            foreach (var item in list)
            {
                XElement settingXml = XElement.Load(item);
                List<XElement> excelList = settingXml.Elements("Excel").Where(e => e.Attribute("name").Value.Equals(xmlName)).ToList();
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
            //导入的数据库表名称
            var DataBase_TableName = targetXml.Element("tableName").Value.Replace("\n", "").Replace(" ", "").Trim();
            //转换字段字符串
            var FieldListStr = targetXml.Element("FieldList").Element("ExcelFields").Value.Replace("\n", "").Replace("[", "").Replace("]", "").Replace(" ", "").Trim();
            string[] ExcelFieldList = FieldListStr.Split(',');
            UpdateColumnsName(dtSheet, ExcelFieldList);
            IEnumerable<XElement> ConvertFieldList = targetXml.Element("FieldList").Element("ConvertFields").Elements("setting");
            if (ConvertFieldList.Count() > 0)
            {
                //从字典表集合中匹配 dbo.sys_code和dbo.sys_codeType
                List<XElement> dictionarySetting = ConvertFieldList.Where(con => con.Attribute("type").Value.Equals("dictionary")).ToList();
                //从单表集合中匹配
                List<XElement> singleSetting = ConvertFieldList.Where(con => con.Attribute("type").Value.Equals("single")).ToList();
                //从固定写死的集合中匹配
                List<XElement> fixSetting = ConvertFieldList.Where(con => con.Attribute("type").Value.Equals("fix")).ToList();
                if (dictionarySetting.Count > 0)
                {
                    //匹配xml中需要转换字段所有的dictionary集合
                    var dsStr = "'" + string.Join(",", dictionarySetting.Select(ds => ds.Value).ToList().ToArray()).Replace(",", "','") + "'";
                    //List<dynamic> SysCodelist = new sys_codeService().GetAllValueTextList(dsStr);
                    List<dynamic> SysCodelist = new List<dynamic>();
                    using (var db = Db.Context("Sys"))
                    {
                        SysCodelist = db.Sql(string.Format("select Value as value,Text as text,CodeType as codetype from sys_code where CodeType in({0})", dsStr)).QueryMany<dynamic>();
                    }
                    foreach (var dictionarySettingItem in dictionarySetting)
                    {
                        var field = dictionarySettingItem.Attribute("field").Value;
                        var CodeType = dictionarySettingItem.Value;
                        foreach (DataRow dtItem in dtSheet.Rows)
                        {
                            if (dtSheet.Columns.Contains(field))
                            {
                                dtItem[field] = GetValueTextByType(SysCodelist, CodeType, dtItem[field].ToString());
                            }
                        }
                    }
                }
                if (fixSetting.Count > 0)
                {
                    foreach (var fixSettingItem in fixSetting)
                    {
                        var field = fixSettingItem.Attribute("field").Value; // <setting type='fix' field='[HomeAddress]'>中的[HomeAddress]
                        List<XElement> fixList = fixSettingItem.Elements("item").ToList();
                        Dictionary<string, string> fixDic = new Dictionary<string, string>();
                        fixList.ForEach(fix =>
                        {
                            fixDic.Add(fix.Value, fix.Attribute("value").Value);
                        });
                        foreach (DataRow dtItem in dtSheet.Rows)
                        {
                            if (dtSheet.Columns.Contains(field))
                            {
                                dtItem[field] = fixDic[dtItem[field].ToString()].ToString();
                            }
                        }
                    }
                }
                if (singleSetting.Count > 0)
                {
                    foreach (var singleSettingItem in singleSetting)
                    {
                        var field = getXmlElementAttr(singleSettingItem, "field");
                        var connName = getXmlElementAttr(singleSettingItem, "connName");
                        var tableName = getXmlElementAttr(singleSettingItem, "tableName");
                        var selectField = getXmlElementAttr(singleSettingItem, "selectField");
                        var whereSql = getXmlElementAttr(singleSettingItem, "whereSql", "1=1");
                        DataTable SearchDT = new DataTable();
                        using (var db = Db.Context(connName))
                        {
                            string SearchSql = string.Format(@"select {0} from {1} where {2}", selectField, tableName, whereSql);
                            SearchDT = db.Sql(SearchSql).QuerySingle<DataTable>();
                        }
                        foreach (DataRow dtItem in dtSheet.Rows)
                        {
                            if (dtSheet.Columns.Contains(field))
                            {
                                dtItem[field] = SearchDT.Select(string.Format("Text='{0}'", dtItem[field].ToString())).First()["Value"].ToString();
                            }
                        }

                    }
                }
            }
            return dtSheet;
        }

        private string getXmlElementValue(XElement element, string name)
        {
            return element.Element(name) == null ? string.Empty : element.Element(name).Value;
        }

        private string getXmlElementAttr(XElement element, string attri, string defaultStr = "")
        {
            return element.Attribute(attri) == null ? defaultStr : element.Attribute(attri).Value;
        }

        public string GetValueTextByType(List<dynamic> SysCodelist, string CodeType, string Text)
        {
            string value = "";
            List<dynamic> newlist = SysCodelist.Where(a => a.codetype == CodeType && a.text == Text).ToList();
            if (newlist.Count() > 0)
            {
                value = newlist.First().value;
            }
            return value;
        }

        /// <summary>
        /// 将excel的datatable字段名称转换为对应的数据库table中的字段名称
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="clist"></param>
        public void UpdateColumnsName(DataTable dt, string[] clist)
        {
            List<int> list = new List<int>();
            for (int i = 0; i < clist.Length; i++)
            {
                if (clist[i] != "" && clist[i].Length > 0)
                {
                    dt.Columns[i].ColumnName = clist[i];
                }
                else
                {
                    list.Add(i);
                }
            }
            list.Reverse();
            foreach (var item in list)
            {
                dt.Columns.RemoveAt(item);
            }
        }
    }
}

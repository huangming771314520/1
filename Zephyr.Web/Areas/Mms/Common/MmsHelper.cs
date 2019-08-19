using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;
using Zephyr.Core;
using Zephyr.Utils;
using System.Web;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Data.OleDb;
using System.Text;
using System.Data.SqlClient;
using System.Net;
using Zephyr.Data;

namespace Zephyr.Areas.Areas.Mms.Common
{
    public class MmsHelper
    {
        public static string GetCookies(string name)
        {
            var cookie = HttpContext.Current.Request.Cookies.Get(name);
            return cookie == null ? null : cookie.Value;
        }

        public static string GetUserName()
        {
            return FormsAuth.GetUserData().UserName;
        }
        public static string GetUserCode()
        {
            return FormsAuth.GetUserData().UserCode;
        }
        public static string GetCurrentProject()
        {
            return GetCookies("CurrentProject");
        }

        public static void ThrowHttpExceptionWhen(bool condition, string message, params object[] param)
        {
            if (condition)
                throw new HttpResponseException(new HttpResponseMessage() { Content = new StringContent(string.Format(message, param)) });
        }
        /// <summary>
        /// 获取单据编号
        /// </summary>
        /// <param name="tableName">生成单据编号的表名</param>
        /// <param name="tableFiled">生成单据编号的字段名</param>
        /// <param name="tableFiled_Pre">生成单据编号前缀</param>
        /// <param name="documentTypeFiled">单据类型字段名</param>
        /// <param name="documentTypeNo">单据类型字段值</param>
        /// <returns></returns>
        public static string GetOrderNumber(string tableName, string tableFiled, string tableFiled_Pre, string documentTypeFiled = "", string documentTypeNo = "")
        {
            var db = Db.Context("Mms");
            string documentNo = "";
            using (db)
            {
                var document = db.StoredProcedure("GetOrderNumber")
                 .ParameterOut("documentNumber", DataTypes.String, 100)
                 .Parameter("tableName", tableName) //生成单据编号的表名
                 .Parameter("tableFiled", tableFiled)//生成单据编号的字段名
                 .Parameter("tableFiled_Pre", tableFiled_Pre)//生成单据编号前缀
                 .Parameter("documentTypeFiled", documentTypeFiled)//单据类型字段名
                 .Parameter("documentTypeNo", documentTypeNo);//单据类型字段值    
                document.Execute();
                documentNo = document.ParameterValue<string>("documentNumber");
            }
            return documentNo;
        }
        /// <summary>
        /// 获取没有时间的单据号
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="tableFiled"></param>
        /// <param name="tableFiled_Pre"></param>
        /// <param name="documentTypeFiled"></param>
        /// <param name="documentTypeNo"></param>
        /// <returns></returns>
        public static string GetLSNumber(string tableName, string tableFiled, string tableFiled_Pre, string documentTypeFiled = "", string documentTypeNo = "")
        {
            var db = Db.Context("Mms");
            string documentNo = "";
            using (db)
            {
                var document = db.StoredProcedure("GetLSNumber")
                 .ParameterOut("documentNumber", DataTypes.String, 100)
                 .Parameter("tableName", tableName) //生成单据编号的表名
                 .Parameter("tableFiled", tableFiled)//生成单据编号的字段名
                 .Parameter("tableFiled_Pre", tableFiled_Pre)//生成单据编号前缀
                 .Parameter("documentTypeFiled", documentTypeFiled)//单据类型字段名
                 .Parameter("documentTypeNo", documentTypeNo);//单据类型字段值    
                document.Execute();
                documentNo = document.ParameterValue<string>("documentNumber");
            }
            return documentNo;
        }

        public static string GetSerialNumber(string tableName, string tableFiled, string tableFiled_Pre, string documentTypeFiled = "", string documentTypeNo = "0",int numberCount=5)
        {
            var db = Db.Context("Mms");
            string documentNo = "";
            using (db)
            {
                var document = db.StoredProcedure("GetSerialNumber")
                 .ParameterOut("documentNumber", DataTypes.String, 100)
                 .Parameter("tableName", tableName) //生成单据编号的表名
                 .Parameter("tableFiled", tableFiled)//生成单据编号的字段名
                 .Parameter("tableFiled_Pre", tableFiled_Pre)//生成单据编号前缀
                 .Parameter("documentTypeFiled", documentTypeFiled)//单据类型字段名
                 .Parameter("documentTypeNo", documentTypeNo)
                 .Parameter("numberCount", numberCount);//单据类型字段值    
                document.Execute();
                documentNo = document.ParameterValue<string>("documentNumber");
            }
            return documentNo;
        }

        public static dynamic GetEditUrls(string controller, object extend = null)
        {
            var expando = (IDictionary<string, object>)new ExpandoObject();
            expando["getdetail"] = string.Format("/api/mms/{0}/getdetail/", controller);
            expando["getmaster"] = string.Format("/api/mms/{0}/geteditmaster/", controller);
            expando["edit"] = string.Format("/api/mms/{0}/edit/", controller);
            expando["audit"] = string.Format("/api/mms/{0}/audit/", controller);
            expando["getrowid"] = string.Format("/api/mms/{0}/getnewrowid/", controller);
            expando["billno"] = string.Format("/api/mms/{0}/GetNewBillNo/", controller);
            expando["upload"] = string.Format("/api/Mms/{0}/PostFile", controller);
            expando["postfile"] = string.Format("/api/mms/{0}/postfile/", controller);
         
            expando["report"] = controller;

            if (extend != null)
                EachHelper.EachObjectProperty(extend, (i, name, value) => { expando[name] = value; });

            return expando;
        }

        public static dynamic GetEditResx(string billName, object extend = null)
        {
            var expando = (IDictionary<string, object>)new ExpandoObject();
            expando["rejected"] = "已撤消修改！";
            expando["editSuccess"] = "保存成功！";
            expando["auditPassed"] = "单据已通过审核！";
            expando["auditReject"] = "单据已取消审核！";

            if (extend != null)
                EachHelper.EachObjectProperty(extend, (i, name, value) => expando[name] = value);

            return expando;
        }

        public static dynamic GetIndexUrls(string controller, object extend = null)
        {
            var expando = (IDictionary<string, object>)new ExpandoObject();
            expando["query"] = string.Format("/api/mms/{0}", controller);
            expando["remove"] = string.Format("/api/mms/{0}/", controller);
            expando["billno"] = string.Format("/api/mms/{0}/getnewbillno", controller);
            expando["audit"] = string.Format("/api/mms/{0}/audit/", controller);
            expando["edit"] = string.Format("/api/mms/{0}/edit/", controller);
            expando["edit1"] = string.Format("mms/{0}/edit/", controller);
            expando["deletespec"] = string.Format("/api/mms/{0}/DeleteSpec/", controller);
            expando["postfile"] = string.Format("/api/mms/{0}/postfile/", controller);
            expando["newkey"] = string.Format("/api/mms/{0}/GetNewKey/", controller);
            expando["downTemplate"] = string.Format("/api/mms/{0}/PostTemplate/", controller);
            if (extend != null)
                EachHelper.EachObjectProperty(extend, (i, name, value) => { expando[name] = value; });

            return expando;
        }

        public static dynamic GetIndexResx(string billName, object extend = null)
        {
            var expando = (IDictionary<string, object>)new ExpandoObject();
            expando["detailTitle"] = billName + "明细";
            expando["noneSelect"] = "请先选择一条" + billName + "！";
            expando["deleteConfirm"] = "确定要删除选中的" + billName + "吗？";
            expando["deleteSuccess"] = "删除成功！";
            expando["auditSuccess"] = "单据已审核！";
            expando["editSuccess"] = "保存成功！";
            if (extend != null)
                EachHelper.EachObjectProperty(extend, (i, name, value) => expando[name] = value);

            return expando;
        }

        #region 判重方法
        public static string queryExist(string data, string columnName, string tableName, object extend = null)
        {

            string existColumn = string.Empty;
            if (data.Length > 0)
            {
                bool someColumnName = true;
                string sqlDup = string.Empty;
                if (columnName.Split(',').Length > 1)
                {
                    sqlDup = string.Format(@"select {0} from {1} where {2} ", columnName, tableName, data.Remove(data.LastIndexOf("or")));
                }
                else
                {
                    someColumnName = false;
                    sqlDup = string.Format(@"select {0} from {1} where {0} in ({2}) ", columnName, tableName, data.TrimEnd(','));
                }

                DataTable dt = new DataTable();

                using (var db = Db.Context("Mms"))
                {
                    dt = db.Sql(sqlDup).QuerySingle<DataTable>();
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (someColumnName)
                            {
                                existColumn += dt.Rows[i][0].ToString() + "  " + dt.Rows[i][1].ToString()+" ";
                            }
                            else
                            {
                                existColumn += dt.Rows[i][0].ToString() + "  ";
                            }
                           
                        }

                    }
                }
            }
            else
            {
                existColumn = "NooneExist";
            }

            if (existColumn.Length > 0)
            {
                return existColumn;
            }
            else
            {
                return "NooneExist";
            }
        }
        #endregion
        public dynamic GetEdit(dynamic data)
        {
            if (data.list.updated.Count > 0)
            {
                for (int i = 0; i < data.list.updated.Count; i++)
                { 
                    data.list.updated[i].Modify_Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    data.list.updated[i].Modify_Person = GetUserName(); 
                }
            }
            return data;
        }

    }
}
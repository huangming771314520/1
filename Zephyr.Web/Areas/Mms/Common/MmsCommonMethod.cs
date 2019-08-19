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
using Zephyr.Models;

namespace Zephyr.Areas.Areas.Mms.Common
{
    public class MmsCommonMethod
    {
        public static string conStr = "";
        public MmsCommonMethod()
        {
            conStr = System.Configuration.ConfigurationManager.ConnectionStrings["Mms"].ToString();
        }

        #region 大批量数据导入
        /// <summary> 
        /// 大批量插入数据(2000每批次) 
        /// 已采用整体事物控制 
        /// </summary> 
        /// <param name="connName">数据库链接字符串</param> 
        /// <param name="tableName">数据库服务器上目标表名</param> 
        /// <param name="dt">含有和目标数据库表结构完全一致(所包含的字段名完全一致即可)的DataTable</param> 
        public static void BulkCopy(string tableName, DataTable dt, string connName = "Mms")
        {
            string connString = System.Configuration.ConfigurationManager.ConnectionStrings[connName].ToString();
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(conn, SqlBulkCopyOptions.Default, transaction))
                    {
                        bulkCopy.BatchSize = 2000;
                        bulkCopy.BulkCopyTimeout = 2000;
                        bulkCopy.DestinationTableName = tableName;
                        try
                        {
                            foreach (DataColumn col in dt.Columns)
                            {
                                bulkCopy.ColumnMappings.Add(col.ColumnName, col.ColumnName);
                            }
                            bulkCopy.WriteToServer(dt);
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw ex;
                        }
                        finally
                        {
                            conn.Close();
                        }
                    }
                }
            }
        }
        #endregion

        public static string GetDocumentNumber(string tableName, string tableFiled, string tableFiled_Pre, string documentTypeFiled = "", string documentTypeNo = "0", int count = 5)
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
                 .Parameter("documentTypeNo", documentTypeNo)//单据类型字段值   
                 .Parameter("numberCount", count);
                document.Execute();
                documentNo = document.ParameterValue<string>("documentNumber");
            }
            return documentNo;
        }

        protected static string UrlEncode(string url)
        {
            byte[] bs = Encoding.GetEncoding("GB2312").GetBytes(url);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < bs.Length; i++)
            {
                if (bs[i] < 128)
                    sb.Append((char)bs[i]);
                else
                {
                    sb.Append("%" + bs[i++].ToString("x").PadLeft(2, '0'));
                    sb.Append("%" + bs[i].ToString("x").PadLeft(2, '0'));
                }
            }
            return sb.ToString();
        }
        /// <summary>
        /// 将CSV文件的数据读取到DataTable中
        /// </summary>
        /// <param name="fileName">CSV文件路径</param>
        /// <returns>返回读取了CSV数据的DataTable</returns>
        public static DataTable OpenCSV(string filePath)
        {

            DataTable dt = new DataTable();
            FileStream fs = new FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);

            StreamReader sr = new StreamReader(fs, Encoding.Default);

            //记录每次读取的一行记录
            string strLine = "";
            //记录每行记录中的各字段内容
            string[] aryLine = null;
            string[] tableHead = null;
            //标示列数
            int columnCount = 0;
            //标示是否是读取的第一行
            bool IsFirst = true;
            //逐行读取CSV中的数据
            while ((strLine = sr.ReadLine()) != null)
            {
                if (IsFirst == true)
                {
                    tableHead = strLine.Split(',');
                    IsFirst = false;
                    columnCount = tableHead.Length;
                    //创建列
                    for (int i = 0; i < columnCount; i++)
                    {
                        DataColumn dc = new DataColumn(tableHead[i]);
                        dt.Columns.Add(dc);
                    }
                }
                else
                {
                    aryLine = strLine.Split(',');
                    DataRow dr = dt.NewRow();
                    for (int j = 0; j < columnCount; j++)
                    {
                        dr[j] = aryLine[j];
                    }
                    dt.Rows.Add(dr);
                }
            }
            if (aryLine != null && aryLine.Length > 0)
            {
                dt.DefaultView.Sort = tableHead[0] + " " + "asc";
            }

            sr.Close();
            fs.Close();
            return dt;
        }
        //循环去除datatable中的空行
        public static DataTable removeEmpty(DataTable dt)
        {
            List<DataRow> removelist = new List<DataRow>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                bool rowdataisnull = true;
                for (int j = 0; j < dt.Columns.Count; j++)
                {

                    if (!string.IsNullOrEmpty(dt.Rows[i][j].ToString().Trim()))
                    {

                        rowdataisnull = false;
                    }

                }
                if (rowdataisnull)
                {
                    removelist.Add(dt.Rows[i]);
                }

            }
            for (int i = 0; i < removelist.Count; i++)
            {
                dt.Rows.Remove(removelist[i]);
            }

            List<string> deleteColumnList = new List<string>();
            foreach (DataColumn dc in dt.Columns)
            {
                if (string.IsNullOrEmpty(dt.Rows[0][dc.ColumnName].ToString()))
                {
                    deleteColumnList.Add(dc.ColumnName);
                }
            }
            foreach (var item in deleteColumnList)
            {
                dt.Columns.Remove(item);
            }

            return dt;
        }
        /// <summary>
        /// 检查是否关键字段是否存在空值
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="keyword"></param>
        /// <param name="result"></param>
        /// <param name="message"></param>
        /// <param name="condition"></param>
        public static void CheckNullByKeyword(DataTable dt, string keyword, ref  int result, ref string message, string condition = "")
        {
            int drs = dt.Select(keyword).Count();
            int dts = dt.Rows.Count;
            if (dts > drs)
            {
                result = 0;
                message = condition + "列存在空值，删除空行重新导入";
            }
        }

        //读取数据
        public static DataTable ReadFile(string filePath)
        {

            DataTable dt = new DataTable();
            string strCon = string.Empty;

            if (filePath.Contains(".xlsx"))
            {
                strCon = "Provider=Microsoft.ACE.OleDb.12.0;" + "data source=" + filePath + ";Extended Properties='Excel 8.0; HDR=NO; IMEX=1'";
            }
            else if (filePath.Contains(".xls"))
            {
                strCon = "Provider=Microsoft.Jet.OleDb.4.0;" + "data source=" + filePath + ";Extended Properties='Excel 8.0; HDR=NO; IMEX=1'";
            }
            else if (filePath.ToLower().Contains(".csv"))
            {

                dt = Zephyr.Areas.Areas.Mms.Common.MmsCommonMethod.OpenCSV(filePath);
            }
            try
            {
                OleDbConnection conn = new OleDbConnection(strCon);
                conn.Open();
                DataTable dta = conn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, null);
                for (int i = 0; i < dta.Rows.Count; i++)
                {
                    string tableName = dta.Rows[i]["Table_Name"].ToString();
                    string strExcel = "select * from " + "[" + tableName + "]";
                    OleDbDataAdapter adp = new OleDbDataAdapter(strExcel, conn);
                    DataSet ds = new DataSet();
                    adp.Fill(ds, "ExcelTable");
                    dt = ds.Tables[0];
                    if (dt.Columns.Count > 1)
                    {
                        break;
                    }
                }

                conn.Close();
            }
            catch (Exception e)
            {
                return dt;
                throw e;
            }
            dt = removeEmpty(dt);
            return dt;
        }

        public static void UpdateColumnsName(DataTable dt, string[] clist)
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
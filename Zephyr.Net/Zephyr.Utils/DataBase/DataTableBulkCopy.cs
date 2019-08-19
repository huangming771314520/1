using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text.RegularExpressions;
using Zephyr.Utils;
using System.Web;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Data.OleDb;
using System.Text;
using System.Data.SqlClient;
using System.Net;

namespace Zephyr.Utils
{
    public partial class DataTableBulkCopy
    {
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
    }
}

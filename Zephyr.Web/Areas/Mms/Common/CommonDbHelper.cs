using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Zephyr.Models
{
    public class CommonDbHelper
    {
        #region 属性
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private string connectionString;
        public string ConntionString
        {
            get
            {
                return connectionString;
            }
            set
            {
                connectionString = value;
            }
        }
       
        #endregion

        #region 构造方法
        /// <summary>
        /// 从配置中自动读取数据库类型及连接字符串
        /// </summary>
        public CommonDbHelper()
        {
            this.connectionString = ConfigurationManager.AppSettings["ConnectingStr"];           
        }
        #endregion

       
        /// <summary>
        /// 执行查询语句
        /// </summary>
        /// <param name="SqlString">查询语句</param>
        /// <returns>DataTable </returns>
        public DataTable ExecuteQuery(string sqlString)
        {
            using (IDbConnection iConn = new SqlConnection(this.ConntionString))
            {
             
                DataSet ds = new DataSet();
                try
                {
                    System.Data.IDataAdapter iAdapter = new SqlDataAdapter(sqlString, (SqlConnection)iConn);
                    iAdapter.Fill(ds);
                }
                catch (System.Exception e)
                {
                    throw new Exception(e.Message);
                }
                finally
                {
                    if (iConn.State != ConnectionState.Closed)
                    {
                        iConn.Close();
                    }
                }
                return ds.Tables[0];
            }
        }
    }
}
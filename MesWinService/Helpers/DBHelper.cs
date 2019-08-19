using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace MesWinService.Helpers
{
    public class DBHelper
    {
        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;
        private static SqlConnection conn;

        public static SqlConnection Conn
        {
            get
            {
                if (conn == null)
                {
                    conn = new SqlConnection(connectionString);
                    conn.Open();
                }
                else if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                else if (conn.State == ConnectionState.Broken)
                {
                    conn.Close();
                    conn.Open();
                }
                return conn;
            }
        }

        public static void CloseConn()
        {
            if (Conn != null && Conn.State == ConnectionState.Open)
            {
                Conn.Close();
            }
        }

        /// <summary>
        /// 执行不带参数的sql语句，并返回受影响的行数
        /// </summary>
        /// <param name="sql">不带参数的sql语句</param>
        /// <returns></returns>
        public static int ExecuteCommand(string sql)
        {
            SqlCommand cmd = new SqlCommand(sql, Conn);
            int n = cmd.ExecuteNonQuery();
            CloseConn();
            return n;
        }

        /// <summary>
        /// 执行不带参数的sql语句List泛型集合（事务处理），并返回布尔类型的执行结果
        /// </summary>
        /// <param name="sqlList"></param>
        /// <returns></returns>
        public static bool ExecuteTranCommand(List<string> sqlList)
        {
            SqlTransaction sqlTran = conn.BeginTransaction();
            try
            {
                foreach (string sql in sqlList)
                {
                    SqlCommand cmd = new SqlCommand(sql, conn, sqlTran);
                    cmd.ExecuteNonQuery();
                }
                sqlTran.Commit();
                return true;
            }
            catch
            {
                sqlTran.Rollback();
                return false;
            }
        }

        /// <summary>
        /// 执行带参数的sql语句，并返回受影响的行数
        /// </summary>
        /// <param name="sql">带参数的sql语句</param>
        /// <param name="values">SqlCommand的参数数组</param>
        /// <returns></returns>
        public static int ExecuteCommand(string sql, SqlParameter[] values)
        {
            SqlCommand cmd = new SqlCommand(sql, Conn);
            cmd.Parameters.AddRange(values);
            int n = cmd.ExecuteNonQuery();
            CloseConn();
            return n;
        }

        /// <summary>
        /// 执行不带参数的sql语句，并返回结果中的第一列第一行，但必须是int型
        /// </summary>
        /// <param name="sql">不带参数的sql语句</param>
        /// <returns></returns>
        public static int GetScalar(string sql)
        {
            SqlCommand cmd = new SqlCommand(sql, Conn);
            int n = Convert.ToInt32(cmd.ExecuteScalar());
            CloseConn();
            return n;
        }

        /// <summary>
        /// 执行带参数的sql语句，并返回结果中的第一列第一行，但必须是int型
        /// </summary>
        /// <param name="sql">带参数的sql语句</param>
        /// <param name="values">SqlCommand的参数数组</param>
        /// <returns></returns>
        public static int GetScalar(string sql, SqlParameter[] values)
        {
            SqlCommand cmd = new SqlCommand(sql, Conn);
            cmd.Parameters.AddRange(values);
            int n = Convert.ToInt32(cmd.ExecuteScalar());
            CloseConn();
            return n;
        }

        /// <summary>
        /// 返回执行不带参数SQL语句后的阅读器 
        /// </summary>
        /// <param name="sql">不带参数的sql语句</param>
        /// <returns></returns>
        public static SqlDataReader GetReader(string sql)
        {
            SqlCommand cmd = new SqlCommand(sql, Conn);
            SqlDataReader sdr = cmd.ExecuteReader();
            return sdr;
        }

        /// <summary>
        /// 返回执行带参数SQL语句后的阅读器 
        /// </summary>
        /// <param name="sql">带参数的sql语句</param>
        /// <param name="values">SqlCommand的参数数组</param>
        /// <returns></returns>
        public static SqlDataReader GetReader(string sql, SqlParameter[] values)
        {
            SqlCommand cmd = new SqlCommand(sql, Conn);
            cmd.Parameters.AddRange(values);
            SqlDataReader sdr = cmd.ExecuteReader();
            return sdr;
        }

        /// <summary>
        /// 返回执行不带参数SQL语句后的结果数据表
        /// </summary>
        /// <param name="sql">不带参数的sql语句</param>
        /// <returns></returns>
        public static DataTable GetDataTable(string sql)
        {
            SqlCommand cmd = new SqlCommand(sql, Conn);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            return dt;
        }

        /// <summary>
        /// 返回执行带参数SQL语句后的结果数据表
        /// </summary>
        /// <param name="sql">带参数的sql语句</param>
        /// <param name="values">SqlCommand的参数数组</param>
        /// <returns></returns>
        public static DataTable GetDataTable(string sql, SqlParameter[] values)
        {
            SqlCommand cmd = new SqlCommand(sql, Conn);
            cmd.Parameters.AddRange(values);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            return dt;
        }

        /// <summary>
        /// 返回执行不带参数SQL语句后的结果数据集
        /// </summary>
        /// <param name="sql">不带参数的sql语句</param>
        /// <returns></returns>
        public static DataSet GetDataSet(string sql)
        {
            SqlCommand cmd = new SqlCommand(sql, Conn);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            sda.Fill(ds);
            return ds;
        }

        /// <summary>
        /// 返回执行带参数SQL语句后的结果数据集
        /// </summary>
        /// <param name="sql">带参数的sql语句</param>
        /// <param name="values">SqlCommand的参数数组</param>
        /// <returns></returns>
        public static DataSet GetDataSet(string sql, SqlParameter[] values)
        {
            SqlCommand cmd = new SqlCommand(sql, Conn);
            cmd.Parameters.AddRange(values);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            sda.Fill(ds);
            return ds;
        }
    }
}

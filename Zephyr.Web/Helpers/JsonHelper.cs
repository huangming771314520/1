using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Text;
using System.Data;


namespace Zephyr.Areas.Helpers
{
    public static class JsonHelper
    {
        public static Dictionary<string, object> jsonToDictionary(string json)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            var obj = jss.DeserializeObject(json);
            Dictionary<string, object> dic = (Dictionary<string, object>)obj;
            //var data = dic["data"];//得到所有行的信息，数组类型，每一个数组是一个Dictionary类型的键值对，即为列  
            //Array rows =(Array)data;//这里从rows 的每一个元素为一个Dictionary类的对象，相当于datatable中的一行的数据 
            return dic;
        }
        /// <summary>
        /// json 转成DataTable
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static DataTable jsonToTable(string json)
        {
            Dictionary<string, object> dic = jsonToDictionary(json);
            DataTable dt = new DataTable();
            dt.TableName = dic["total"].ToString();
            Dictionary<string, object> rowsObj = (Dictionary<string, object>)dic["rows"];
            foreach (var item in rowsObj)
            {
                //Array rows = (Array)item;
                Dictionary<string, object> rows = (Dictionary<string, object>)item.Value;
                //为datatable添加列  
                if (dt.Columns.Count == 0)
                {
                    foreach (string key in rows.Keys)
                    {
                        dt.Columns.Add(key);
                    }
                }
                DataRow dr = dt.NewRow();
                //为行中的每一列列赋值  
                foreach (string keyname in rows.Keys)
                {
                    dr[keyname] = rows[keyname];
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }

        /// <summary>
        /// json 转成DataTable
        /// </summary>
        /// <param name="json"></param>
        /// <param name="type">oneRow 指一行数据，oneTable 整表,more</param>
        /// <returns></returns>
        public static DataTable jsonToTable(string json, string type)
        {
            if (type == "oneRow")
            {
                Dictionary<string, object> dic = jsonToDictionary(json);
                DataTable dt = new DataTable();
                dt.TableName = dic["Main_ID"].ToString();
                //为datatable添加列  
                if (dt.Columns.Count == 0)
                {
                    foreach (string key in dic.Keys)
                    {
                        dt.Columns.Add(key);
                    }
                }
                DataRow dr = dt.NewRow();
                //为行中的每一列列赋值  
                foreach (string keyname in dic.Keys)
                {
                    dr[keyname] = dic[keyname];
                }
                dt.Rows.Add(dr);
                return dt;
            }
            else if(type=="oneTable")
            { 
              return  jsonToTable(json);
            }
            else if (type == "more")
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                var obj = jss.DeserializeObject(json);
                object[] objArray=(object[])obj;
                DataTable dt = new DataTable();
                for (int y = 0; y < objArray.Length; y++)
                {
                    Dictionary<string, object> dic = (Dictionary<string, object>)objArray[y]; 
                    //为datatable添加列  
                    if (dt.Columns.Count == 0)
                    {
                        foreach (string key in dic.Keys)
                        {
                            dt.Columns.Add(key);
                        }
                    }
                    DataRow dr = dt.NewRow();
                    //为行中的每一列列赋值  
                    foreach (string keyname in dic.Keys)
                    {
                        dr[keyname] = dic[keyname];
                    }
                    dt.Rows.Add(dr);
                }
                return dt; 
            }
            else
            {
                return null;
            }
        }
    
    }
}
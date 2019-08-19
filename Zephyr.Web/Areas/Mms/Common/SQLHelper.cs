using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Zephyr.Models
{
    public class SQLHelper<Tentity> where Tentity : class,new()
    {
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns></returns>
        public IList<Tentity> GetData(string statement)
        {           
              
                var dt = new CommonDbHelper().ExecuteQuery(statement);               
                if (dt == null || dt.Rows.Count == 0){
                    return null;
                }                      
                IList<Tentity> list = new List<Tentity>(dt.Rows.Count);
                // 获得此模型的类型  
                Type type = typeof(Tentity);
                string tempName = "";
                foreach (DataRow dr in dt.Rows)
                {
                    Tentity t = new Tentity();
                    // 获得此模型的公共属性     
                    PropertyInfo[] propertys = t.GetType().GetProperties();
                    foreach (PropertyInfo pi in propertys)
                    {
                        // 检查DataTable是否包含此列   
                        tempName = pi.Name;  
                        if (dt.Columns.Contains(tempName))
                        {                             
                            if (!pi.CanWrite) continue;
                            object value = dr[tempName];
                            if (value != DBNull.Value)
                                pi.SetValue(t, value, null);
                        }
                    }
                    list.Add(t);
                }               

                return list;
           
        }
        
    }
}
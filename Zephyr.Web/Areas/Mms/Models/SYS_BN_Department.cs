using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using Zephyr.Core;
using System.Linq;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class SYS_BN_DepartmentService : ServiceBase<SYS_BN_Department>
    {
        public class ModelHelper<T> where T : new()  // 此处一定要加上new()
        {
            public static IList<T> DataTableToModel(DataTable dt)
            {
                IList<T> list = new List<T>();// 定义集合
                Type type = typeof(T); // 获得此模型的类型
                string tempName = "";
                foreach (DataRow dr in dt.Rows)
                {
                    T t = new T();
                    PropertyInfo[] propertys = t.GetType().GetProperties();// 获得此模型的公共属性
                    foreach (PropertyInfo pro in propertys)
                    {
                        tempName = pro.Name;
                        if (dt.Columns.Contains(tempName))
                        {
                            if (!pro.CanWrite) continue;
                            object value = dr[tempName];
                            if (value != DBNull.Value)
                            {
                                if (pro.PropertyType.FullName.Contains("Int"))
                                    pro.SetValue(t, Convert.ToInt32(value), null);
                                else
                                    pro.SetValue(t, value, null);
                            }
                        }
                    }
                    list.Add(t);
                }
                return list;
            }
        }

        protected override void OnAfterHandleExcel(ref DataTable dtSheet)
        {
            var service = new SYS_BN_DepartmentService();

            foreach (DataRow item in dtSheet.Rows)
            {
                string isworkshop = item["IsWorkshop"].ToString();
                if (isworkshop == "是")
                {
                    item["IsWorkshop"] = 1;
                }
                else
                    item["IsWorkshop"] = 0;

                string isenable = item["IsEnable"].ToString();
                if (isenable == "否")
                {
                    item["IsEnable"] = 0;
                }
                else
                    item["IsEnable"] = 1;
            }

            var list = ModelHelper<SYS_BN_Department>.DataTableToModel(dtSheet);
            dtSheet.Columns.Add("ParentCode", typeof(string)); //数据类型为 文本
            foreach (DataRow item in dtSheet.Rows)
            {
                item["ParentCode"] = (from p in list where p.DepartmentName == item["ParentName"].ToString() select p.DepartmentCode).FirstOrDefault();

                var DepartmentCode = item["DepartmentCode"].ToString();
                var Query = ParamDelete.Instance().AndWhere("DepartmentCode", DepartmentCode);
                service.Delete(Query);
            }
            base.OnAfterHandleExcel(ref dtSheet);
        }


        public List<dynamic> GetIDNameList()
        {
            var pQuery = ParamQuery.Instance()
                .Select("ID,DepartmentName");


            return base.GetDynamicList(pQuery);
        }
        public string GetDepartmentCode()
        {
            string sql = " select [DepartmentCode] from [SYS_BN_Department] where [DepartmentCode] = (select max([DepartmentCode]) from [SYS_BN_Department])";
            return db.Sql(sql).QuerySingle<string>();
        }
        //public string GetDepartmentCode()
        //{
        //    string sql = " select [DepartmentCode] from [SYS_BN_Department] where [DepartmentCode] = (select max([DepartmentCode]) from [SYS_BN_Department])";
        //    return db.Sql(sql).QuerySingle<string>();
        //}
        public List<dynamic> GetDepartTree()
        {
            using (var db = Db.Context("Mms"))
            {
                string sql = @"select DepartmentCode as id ,ParentCode as pid,DepartmentCode+' '+DepartmentName as text  from SYS_BN_Department where isEnable=1";
                List<dynamic> list = db.Sql(sql).QueryMany<dynamic>();
                return list;
            }
        }

        public dynamic GetDeptInfoByUCode(string uCode)
        {
            string sql = string.Format(@"SELECT * FROM dbo.SYS_BN_Department WHERE IsEnable = 1 AND DepartmentCode = (SELECT TOP 1 DepartmentCode FROM dbo.SYS_BN_User WHERE UserCode = '{0}')", uCode);
            return db.Sql(sql).QuerySingle<SYS_BN_Department>();
        }

    }

    public class SYS_BN_Department : ModelBase
    {
        [Identity]
        [PrimaryKey]
        public int ID { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentCode { get; set; }
        public string ParentCode { get; set; }
        public string ParentName { get; set; }
        public int? IsWorkshop { get; set; }
        public int? IsEnable { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateTime { get; set; }
        public string ModifyPerson { get; set; }
        public DateTime? ModifyTime { get; set; }
        public string ProcessType { get; set; }
    }
}

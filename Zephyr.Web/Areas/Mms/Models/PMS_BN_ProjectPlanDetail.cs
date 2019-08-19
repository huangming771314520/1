using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class PMS_BN_ProjectPlanDetailService : ServiceBase<PMS_BN_ProjectPlanDetail>
    {
        public List<dynamic> GetColumns()
        {
            List<dynamic> scList = new List<dynamic>();
            using (var sysDB = Db.Context("Sys"))
            {
                scList = sysDB.Sql("select value,text from sys_code where CodeType='planType'").QueryMany<dynamic>();
            }
            return scList;
        }
        public List<dynamic> GetProjectPlan(string contractCode)
        {
            List<dynamic> scList = GetColumns();

            string sql = @"SELECT t1.ContractCode,";
            foreach (var item in scList)
            {
                sql += "CONVERT(varchar(100), t1.[" + item.value + "], 23) as '" + item.text + "考核日期" + "',";
            }
            foreach (var item in scList)
            {
                sql += "CONVERT(varchar(100), t2.[" + item.value + "], 23) as '" + item.text + "完成日期" + "',";
            }
            sql = sql.Remove(sql.Length - 1, 1);
            sql += @",t3.ProductName,t3.ProductType,t3.Model,t3.Specifications FROM (select ContractCode,ProjectDetailID,PlanType,PlanDate from PMS_BN_ProjectPlanDetail) 
AS P
PIVOT 
(
    max(PlanDate) FOR 
    p.PlanType IN (";
            foreach (var item in scList)
            {
                sql += "[" + item.value + "],";
            }
            sql = sql.Remove(sql.Length - 1, 1);
            sql += @") 
) AS T1
inner join
(
SELECT * FROM (select ContractCode,ProjectDetailID,PlanType,FinishDate from PMS_BN_ProjectPlanDetail) 
AS P
PIVOT 
(
    max(FinishDate) FOR 
    p.PlanType IN (";
            foreach (var item in scList)
            {
                sql += "[" + item.value + "],";
            }
            sql = sql.Remove(sql.Length - 1, 1);
            sql += @") 
) AS T ) as T2 on T1.ContractCode=T2.ContractCode
left join PMS_BN_ProjectDetail as t3 on t1.ProjectDetailID=t3.ID ";
            if (!string.IsNullOrEmpty(contractCode))
            {
                sql += "where t1.ContractCode='{0}'";
                sql = string.Format(sql, contractCode);
            }
            var list = db.Sql(sql).QueryMany<dynamic>();
            return list;
        }

        public int IsExsitsPlan(string ContractCode, string ProjectDetailID)
        {
            string sql = string.Format("select count(*) from PMS_BN_ProjectPlanDetail where ContractCode='{0}' and ProjectDetailID='{1}'", ContractCode, ProjectDetailID);
            return db.Sql(sql).QuerySingle<int>();
        }



    }

    public class PMS_BN_ProjectPlanDetail : ModelBase
    {
        [PrimaryKey]
        public int ID { get; set; }
        public int ProjectDetailID { get; set; }
        public int? ProjectID { get; set; }
        public string ContractCode { get; set; }
        public int? PlanType { get; set; }
        public DateTime? PlanDate { get; set; }
        public DateTime? FinishDate { get; set; }
        public int? IsEnable { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateTime { get; set; }
        public string ModifyPerson { get; set; }
        public DateTime? ModifyTime { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Areas.Areas.Mms.Common;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class PMS_BN_ProjectService : ServiceBase<PMS_BN_Project>
    {
        public dynamic GetProjectByCCode(string contractCode)
        {
            string sql = string.Format(@"SELECT * FROM  dbo.PMS_BN_Project WHERE ContractCode = '{0}'", contractCode);

            using (var db = Db.Context("Mms"))
            {
                return db.Sql(sql).QuerySingle<dynamic>();
            }
        }

        public List<string> GetCnotractCode()
        {
            string sql = @"SELECT ContractCode FROM  dbo.PMS_BN_Project where IsEnable=1";

            using (var db = Db.Context("Mms"))
            {
                return db.Sql(sql).QueryMany<string>();
            }
        }
        public int Insert(PMS_BN_Project model)
        {
            var rowsAffected = model.ProjectID = db.Insert<PMS_BN_Project>("PMS_BN_Project", model)
     .AutoMap(x => x.ProjectID)
     .ExecuteReturnLastId<int>();
            return rowsAffected;
        }
        public int Update(PMS_BN_Project model)
        {
            string sql = String.Format(@"select * from PMS_BN_Project where ProjectID='{0}' ", model.ProjectID);
            var single = db.Sql(sql).QuerySingle<PMS_BN_Project>();
            model.CreatePerson = single.CreatePerson;
            model.CreateTime = single.CreateTime;
            model.ModifyPerson = MmsHelper.GetUserCode();
            model.ModifyTime = DateTime.Now;

            var rowsAffected = db.Update<PMS_BN_Project>("PMS_BN_Project", model)
     .AutoMap(x => x.ProjectID)
     .Where(x => x.ProjectID)
     .Execute();
            return rowsAffected;
        }

        public List<dynamic> GetTaskTree(string cCode)
        {
            using (var db = Db.Context("Mms"))
            {
                //                string sql = @"
                //select ContractCode as id ,'0' as pid,ContractCode as text  from PMS_BN_Project where isEnable=1
                //union all
                //select 'A'+CAST(ID AS nvarchar(50)) as id,t2.ContractCode as pid,ProductName as text from PMS_BN_ProjectDetail as t1 
                //inner join PMS_BN_Project as t2 on t1.MainID=t2.ProjectID where t1.isEnable=1
                //union all
                //select  case when TaskType =1 then 'A'+CAST(ID AS nvarchar(50)) when TaskType =2 then 'B'
                //+CAST(ID AS nvarchar(50)) else 'C'+CAST(ID AS nvarchar(50)) end as id 
                //,'A'+ProductID as pid,case when TaskType =1 then '设计任务' when TaskType =2 then '采购任务' else '生产任务' end+'|' +
                //case when TaskStartDate is not null and TaskFinishDate is not null then  CAST(datediff(d,TaskStartDate,TaskFinishDate) AS nvarchar(50)) else CAST(isnull(TaskCycle,'0') AS nvarchar(50))  end +'天' as text 
                //from PMS_ProductTask where isEnable=1
                //union all
                //select CAST(ID AS nvarchar(50)) as id ,'C'+CAST(MainID AS nvarchar(50)) as pid,CAST(ProductQuantity AS nvarchar(50))+'|'+CAST(PlanFinishDate AS nvarchar(50)) from PMS_ProductPlanDetail where isEnable=1 and MainID is not null
                //union all
                //select CAST(ID AS nvarchar(50)) as id ,'A'+CAST(MainID AS nvarchar(50)) as pid,t2.Text+'|'+CONVERT(varchar(100), TaskStartDate, 105)+'|'+CONVERT(varchar(100), TaskFinishDate, 105) from PMS_DesignTaskDetail as t1
                //left join [HBHC_Sys].[dbo].sys_code as t2 on t1.TaskType=t2.Value where t2.CodeType='TaskType' and t1.isEnable=1 and t1.MainID is not null
                //";

                string sql = string.Format(@"
DECLARE @ccode VARCHAR(50) = '{0}';

SELECT 'P'+CAST(ProjectID AS NVARCHAR(50)) AS id,
       '0' AS pid,
       '(' + ContractCode + ')' + ProjectName AS text
FROM PMS_BN_Project
WHERE IsEnable = 1
      AND ContractCode = @ccode
UNION ALL
SELECT 'A' + CAST(ID AS NVARCHAR(50)) AS id,
       'P'+CAST(t2.ProjectID AS NVARCHAR(50)) AS pid,
       ISNULL(ProductName,'')+'/('+ISNULL(Specifications,'')+')'+ISNULL(Model,'') AS text
FROM PMS_BN_ProjectDetail AS t1
    INNER JOIN PMS_BN_Project AS t2
        ON t1.MainID = t2.ProjectID
WHERE t1.IsEnable = 1
      AND t2.ContractCode = @ccode
UNION ALL
select id,pid,text from (
SELECT TOP 100 PERCENT t3.TaskType,CASE
           WHEN t3.TaskType = 1 THEN
               'B' + CAST(t3.ID AS NVARCHAR(50))
           WHEN t3.TaskType = 2 THEN
               'C' + CAST(t3.ID AS NVARCHAR(50))
           ELSE
               'D' + CAST(t3.ID AS NVARCHAR(50))
       END AS id,
       'A' + t3.ProductID AS pid,
       CASE
           WHEN t3.TaskType = 1 THEN
               '设计任务'
           WHEN t3.TaskType = 2 THEN
               '采购任务'
           ELSE
               '生产任务'
       END + '  ' + CASE
                       WHEN t3.TaskCycle IS NULL then ISNULL(CONVERT(VARCHAR(100), t3.TaskStartDate, 111),'') + '~' + ISNULL(CONVERT(VARCHAR(100), t3.TaskFinishDate, 111),'')
                       ELSE
                           CAST(ISNULL(t3.TaskCycle, '0') AS NVARCHAR(50)) + '天'
                   END  AS text
FROM PMS_ProductTask t3 
    INNER JOIN PMS_BN_ProjectDetail AS t1
        ON t3.ProductID = t1.ID
    INNER JOIN PMS_BN_Project AS t2
        ON t1.MainID = t2.ProjectID
WHERE t3.IsEnable = 1
     AND t2.ContractCode = @ccode 
     order by t3.TaskType ) t 
UNION
SELECT CAST(PMS_ProductPlanDetail.ID AS NVARCHAR(50)) AS id,
       'D' + CAST(MainID AS NVARCHAR(50)) AS pid,
       '台数:'+ISNULL(CAST(ProductQuantity AS NVARCHAR(50)),'')  + '  交货日期：' + ISNULL(CAST(PlanFinishDate AS NVARCHAR(50)),'')
	   + '<span style=display:none>&' + PMS_ProductTask.ProductID
       + '&</span>'
FROM PMS_ProductPlanDetail 
INNER JOIN PMS_ProductTask 
        ON PMS_ProductPlanDetail.MainID = PMS_ProductTask.ID
WHERE PMS_ProductPlanDetail. IsEnable = 1
      AND  PMS_ProductPlanDetail.ContractCode=@ccode
UNION  
SELECT CAST(t1.ID AS NVARCHAR(50)) AS id,
       'B' + CAST(t1.MainID AS NVARCHAR(50)) AS pid,
       t2.Text + '  ' + ISNULL(CONVERT(VARCHAR(100), t1.TaskStartDate, 111),'') + '~'
       + ISNULL(CONVERT(VARCHAR(100), t1.TaskFinishDate, 111),'')+'<span style=display:none>&'+t3.ProductID+'&</span>'
FROM PMS_DesignTaskDetail AS t1
    LEFT JOIN [HBHC_Sys].[dbo].sys_code AS t2
        ON t1.TaskType = t2.Value
    INNER JOIN PMS_ProductTask t3
        ON t1.MainID = t3.ID
    INNER JOIN PMS_BN_ProjectDetail AS t4
        ON t3.ProductID = t4.ID
    INNER JOIN PMS_BN_Project AS t5
        ON t4.MainID = t5.ProjectID
WHERE t2.CodeType = 'TaskType'
      AND t1.IsEnable = 1
      AND t1.MainID IS NOT NULL
      AND t5.ContractCode = @ccode;
", cCode);
                List<dynamic> list = db.Sql(sql).QueryMany<dynamic>();
                return list;
            }
        }

        //        public List<dynamic> GetTaskData(string projectName,DateTime? receiveTime)
        //        {
        //            string where = " where 1=1 and";
        //            if (string.IsNullOrEmpty(projectName))
        //            {
        //                projectName="%"+projectName+"%";
        //                where += string.Format("ProdejtName like '{0}'", projectName);
        //            }
        //            if (receiveTime != null)
        //            {
        //                string strBeginData = Convert.ToDateTime(receiveTime.ToString()).ToString("yyyy-MM-dd 00:00:00");
        //                where += string.Format(@" and ContractReceiveTime='{0}'", strBeginData);
        //            }
        //            using (var db = Db.Context("Mms"))
        //            {
        //                string sql =string.Format( @"
        //select tab1.*,
        //tab2.ProjectName,
        //tab2.Is0tSartProduct,
        //tab2.ContractReceiveTime
        // from (
        //select 
        //t1.ContractCode,
        //t1.ProductID,
        //t1.TaskType RWTaskType,
        //t1.TaskStartDate RWTaskStartDate,
        //t1.TaskFinishDate RWTaskFinishDate,
        //t1.TaskCycle,
        //t1.TaskStartDate,
        //t2.TaskType SJTaskType,
        //t2.TaskDescription,
        //t2.TaskStartDate SJTaskStartDate,
        //t2.TaskFinishDate SJTaskFinishDate,
        //t3.ProductQuantity,
        //t3.PlanFinishDate,
        //t3.CompleteQuantity,
        //t4.ProductName,
        //t4.ProductType,
        //t4.Model,
        //t4.Specifications,
        //t4.Quantity,
        //t4.DeliveryTime,
        //t1.IsEnable RWIsEnable,
        //t2.IsEnable SJIsEnable,
        //t3.IsEnable SCIsEnable,
        //t4.IsEnable CPIsEnable
        // from PMS_ProductTask t1
        // left join PMS_DesignTaskDetail t2 on t1.ID=t2.MainID
        // left join PMS_ProductPlanDetail t3 on t3.MainID=t1.ID
        //left join PMS_BN_ProjectDetail t4 on t4.id=t1.ProductID where t1.IsEnable=1  and t4.IsEnable=1
        //) tab1 left join  PMS_BN_Project tab2 
        //on tab1.ContractCode=tab2.ContractCode {0}
        //
        //", where);

        //                List<dynamic> list = db.Sql(sql).QueryMany<dynamic>();
        //                return list;
        //            }
        //        }

    }

    public class PMS_BN_Project : ModelBase
    {
        [Identity]
        [PrimaryKey]
        public int ProjectID { get; set; }
        public string ContractCode { get; set; }
        public string ProjectName { get; set; }
        public string ProjectForShort { get; set; }
        //public DateTime? ContractReceiveTime { get; set; }
        public DateTime? AdvancePaymentDate { get; set; }
        public int? Is0tSartProduct { get; set; }
        public string FileName { get; set; }
        public string FileAddress { get; set; }
        public string DocName { get; set; }
        public int? IsEnable { get; set; }
        public string Remark { get; set; }
        public DateTime? ContractReceiveTime { get; set; }

        public string ProductReport { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateTime { get; set; }
        public string ModifyPerson { get; set; }
        public DateTime? ModifyTime { get; set; }

        public string ProjectState { get; set; }

    }
}

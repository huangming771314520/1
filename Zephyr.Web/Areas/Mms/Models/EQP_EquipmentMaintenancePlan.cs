using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class EQP_EquipmentMaintenancePlanService : ServiceBase<EQP_EquipmentMaintenancePlan>
    {
        public int IsExsitsPlan(string MaintenanceType, int? EquipmentMaintenanceID, string EquipmentCode, DateTime startTime, DateTime endTime)
        {
            string sql = string.Format("select count(*) from EQP_EquipmentMaintenancePlan where EquipmentMaintenanceID='{0}' and EquipmentCode='{1}' and MaintenanceType='{2}' and PlanedStartTime between '{3}' and '{4}' ", EquipmentMaintenanceID, EquipmentCode, MaintenanceType, startTime, endTime);
            return db.Sql(sql).QuerySingle<int>();
        }
        public int IsExsitsWX(string EquipmentCode, int? MaintenanceType, int? Year)
        {
            string sql = string.Format("select count(*) from EQP_EquipmentMaintenancePlan where  EquipmentCode='{0}' and MaintenanceType='{1}' and YEAR(PlanedStartTime)='{2}' ", EquipmentCode, MaintenanceType, Year);
            return db.Sql(sql).QuerySingle<int>();
        }

        public int ChangePlanState(int id, out string msg)
        {
            string sql;
            int rowsAffected = 0;

            db.UseTransaction(true);
            sql = string.Format(@"update EQP_EquipmentMaintenancePlan set MaintenanceState='2' where ID='{0}'", id);
            string selectSql = string.Format(@" select * from dbo.EQP_EquipmentMaintenancePlan a where a.ID='{0}'", id);
            var failureObj = db.Sql(selectSql).QuerySingle<EQP_EquipmentMaintenancePlan>();
            rowsAffected = db.Sql(sql).Execute();
            if (rowsAffected < 0)
            {
                db.Rollback();
                msg = "审核设备维护计划失败";
                return rowsAffected;
            }
            sql = string.Format(@"update SYS_Equipment set EquipmentState='2' where EquipmentCode='{0}'", failureObj.EquipmentCode);
            rowsAffected = db.Sql(sql).Execute();
            if (rowsAffected < 0)
            {
                db.Rollback();
                msg = "更改设备状态失败";
                return rowsAffected;
            }

            msg = "操作成功！";
            db.Commit();
            return rowsAffected;



        }
        public int FinishPlanState(int id, out string msg)
        {
            string sql;
            int rowsAffected = 0;

            db.UseTransaction(true);
            sql = string.Format(@"update EQP_EquipmentMaintenancePlan set MaintenanceState='3' where ID='{0}'", id);
            string selectSql = string.Format(@" select * from dbo.EQP_EquipmentMaintenancePlan a where a.ID='{0}'", id);
            var failureObj = db.Sql(selectSql).QuerySingle<EQP_EquipmentMaintenancePlan>();
            rowsAffected = db.Sql(sql).Execute();
            if (rowsAffected < 0)
            {
                db.Rollback();
                msg = "提交设备维护计划失败";
                return rowsAffected;
            }
            sql = string.Format(@"update SYS_Equipment set EquipmentState='1' where EquipmentCode='{0}'", failureObj.EquipmentCode);
            rowsAffected = db.Sql(sql).Execute();
            if (rowsAffected < 0)
            {
                db.Rollback();
                msg = "更改设备状态失败";
                return rowsAffected;
            }

            msg = "操作成功！";
            db.Commit();
            return rowsAffected;



        }


    }

    public class EQP_EquipmentMaintenancePlan : ModelBase
    {
        [Identity]
        [PrimaryKey]   
        public int ID { get; set; }

        public string MaintenancePlanCode { get; set; }
        public int? EquipmentMaintenanceID { get; set; }
       
        public string EquipmentCode { get; set; }
        public string EquipmentName { get; set; }
        public int? MaintenanceType { get; set; }

        public string MaintenanceName { get; set; }
        public DateTime? PlanedStartTime { get; set; }
        public DateTime? PlanedFinishTime { get; set; }
        public DateTime? ActualStartTime { get; set; }
        public DateTime? ActualFinishTime { get; set; }
        public string PlanedContent { get; set; }
        public string ActualContent { get; set; }
        public string MaintenanceMan { get; set; }
        public int? MaintenanceState { get; set; }
        public int? IsEnable { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateTime { get; set; }
        public string ModifyPerson { get; set; }
        public DateTime? ModifyTime { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class EQP_FailureReportService : ServiceBase<EQP_FailureReport>
    {
        public int ChangeReportState(int id, out string msg)
        {
            string sql;
            int rowsAffected = 0;
           
            db.UseTransaction(true);
            sql = string.Format(@"update EQP_FailureReport set FailReportState='2' where ID='{0}'", id);
            string selectSql = string.Format(@"select * from EQP_FailureReport where ID={0}", id);
            var failureReport = db.Sql(selectSql).QuerySingle<EQP_FailureReport>();
            rowsAffected = db.Sql(sql).Execute();
            if (rowsAffected<0)
            {
                db.Rollback();
                msg = "发送故障报告单失败";
                return rowsAffected;
            }
            sql = string.Format(@"update SYS_Equipment set EquipmentState='2' where ID='{0}'", failureReport.EquipmentID);
            rowsAffected = db.Sql(sql).Execute();
            if (rowsAffected<0)
            {
                db.Rollback();
                msg = "更改设备状态失败";
                return rowsAffected;
            }

            msg = "操作成功！";
            db.Commit();
            return rowsAffected;
            
            
          
        }

        public int AcceptReportState(int id, out string msg)
        {
            string sql;
            int rowsAffected = 0;
            sql = string.Format(@"update EQP_FailureReport set FailReportState='3' where ID='{0}'", id);
            rowsAffected = db.Sql(sql).Execute();
            if(rowsAffected<0)
            {
                msg = "故障报告接受失败";
                return rowsAffected;
            }
            else
            {
                msg = "成功";
                return rowsAffected;

            }
        }
        public List<EQP_FailureReport> GetSelectReport()
        {
            string sql;
            sql="select * from EQP_FailureReport where FailReportState=1  or FailReportState=2;";
            List<EQP_FailureReport> reportList = db.Sql(sql).QueryMany<EQP_FailureReport>();
            return reportList;
        }

       
    }

    public class EQP_FailureReport : ModelBase
    {
        [Identity]
        [PrimaryKey]   
        public int ID { get; set; }
        public string FailureReportCode { get; set; }
        public int? EquipmentID { get; set; }
        public string EquipmentName { get; set; }
        public int? WorkshopID { get; set; }
        public string WorkshopName { get; set; }
        public DateTime? FailTime { get; set; }
        public string FailDescription { get; set; }
        public string ReportPerson { get; set; }
        public int? FailReportState { get; set; }
        public int? IsEnable { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateTime { get; set; }
        public string ModifyPerson { get; set; }
        public DateTime? ModifyTime { get; set; }
    }
}
